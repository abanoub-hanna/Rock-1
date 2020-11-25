﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotLiquid;
using Rock.Lava.Filters;

namespace Rock.Lava.DotLiquid
{
    /// <summary>
    /// Initialization class for the DotLiquid Templating Engine.
    /// </summary>
    public partial class DotLiquidEngine : LavaEngineBase
    {
        public override string EngineName
        {
            get
            {
                return "DotLiquid";
            }
        }

        public override LavaEngineTypeSpecifier EngineType
        {
            get
            {
                return LavaEngineTypeSpecifier.DotLiquid;
            }
        }

        public override ILavaContext NewContext()
        {
            var dotLiquidContext = new global::DotLiquid.Context();

            // Set the transformation function for converting a Lava Context to a DotLiquid Context.
            // This is needed to allow the Lava context parameter in a filter function to be identified and 
            // injected in a way that is framework-agnostic.
            dotLiquidContext.FilterContextParameterType = Template.FilterContextParameterType;
            dotLiquidContext.FilterContextParameterTransformer = Template.FilterContextParameterTransformer;

            return new DotLiquidLavaContext( dotLiquidContext );
        }

        /// <summary>
        /// Initializes Rock's Lava system (which uses DotLiquid)
        /// Doing this in startup will force the static Liquid class to get instantiated
        /// so that the standard filters are loaded prior to the custom RockFilter.
        /// This is to allow the custom 'Date' filter to replace the standard Date filter.
        /// </summary>
        public override void Initialize( ILavaFileSystem fileSystem, IList<Type> filterImplementationTypes = null )
        {
            // DotLiquid uses a RubyDateFormat by default,
            // but since we aren't using Ruby, we want to disable that
            Liquid.UseRubyDateFormat = false;

            /* 2020-05-20 MDP (actually this comment was here a long time ago)
                NOTE: This means that all the built in template filters,
                and the RockFilters, will use CSharpNamingConvention.

                For example the dotliquid documentation says to do this for formatting dates: 
                {{ some_date_value | date:"MMM dd, yyyy" }}

                However, if CSharpNamingConvention is enabled, it needs to be: 
                {{ some_date_value | Date:"MMM dd, yyyy" }}
            */

            Template.NamingConvention = new global::DotLiquid.NamingConventions.CSharpNamingConvention();

            if ( fileSystem == null )
            {
                fileSystem = new DotLiquidFileSystem( new NullFileSystem() );
            }

            Template.FileSystem = new DotLiquidFileSystem( fileSystem );

            Template.RegisterSafeType( typeof( Enum ), o => o.ToString() );
            Template.RegisterSafeType( typeof( DBNull ), o => null );

            Template.RegisterFilter( typeof( TemplateFilters ) );
            Template.RegisterFilter( typeof( DotLiquidFilters ) );

            Template.FilterContextParameterType = typeof( ILavaContext );
            Template.FilterContextParameterTransformer = ( context ) =>
            {
                // Wrap the DotLiquid context in a framework-agnostic Lava context.
                return new DotLiquidLavaContext( context );
            };

            // Register custom filters last, so they can override built-in filters of the same name.
            if ( filterImplementationTypes != null )
            {
                foreach ( var filterImplementationType in filterImplementationTypes )
                {
                    Template.RegisterFilter( filterImplementationType );
                }
            }

            // Register all Types that implement ILavaDataObject as safe to render.
            RegisterSafeType( typeof( Rock.Lava.ILavaDataObject ) );
        }

        public class LiquidizableObjectProxy : ILiquidizable
        {
            public object this[object key] => throw new NotImplementedException();

            public List<string> AvailableKeys => throw new NotImplementedException();

            public bool ContainsKey( object key )
            {
                throw new NotImplementedException();
            }

            public object ToLiquid()
            {
                throw new NotImplementedException();
            }
        }

        public override Type GetShortcodeType( string name )
        {
            throw new NotImplementedException();
        }

        public override void RegisterSafeType( Type type, string[] allowedMembers = null )
        {
            if ( type is Rock.Lava.ILavaDataObjectSource )
            {
                Template.RegisterSafeType( type, ( x ) => { return ( (Rock.Lava.ILavaDataObjectSource)x ).GetLavaDataObject(); } );
            }
            else if ( type is Rock.Lava.ILavaDataObject )
            {
                Template.RegisterSafeType( typeof( Rock.Lava.ILavaDataObject ),
                x =>
                {
                    return new DropProxy( x, ( (ILavaDataObject)x ).AvailableKeys.ToArray() );
                } );
            }
            else
            {
                // Wrap the object in a RockDynamic proxy, and a DotLiquid compatible proxy.
                Template.RegisterSafeType( type, ( x ) =>
                {
                    var dynamicObject = new RockDynamic( x );
                    var wrapper = new DotLiquidLavaDataObjectProxy( dynamicObject );
                    return wrapper;
                } );
            }
        }

        public override void RegisterTag( string name, Func<string, IRockLavaTag> factoryMethod )
        {
            if ( name == null )
            {
                throw new ArgumentException( "Name must be specified." );
            }

            name = name.Trim().ToLower();

            base.RegisterTag( name, factoryMethod );

            DotLiquidTagProxy.RegisterFactory( name, factoryMethod );

            // Register the proxy for the specified tag name.
            Template.RegisterTag<DotLiquidTagProxy>( name );
        }

        public override void RegisterBlock( string name, Func<string, IRockLavaBlock> factoryMethod )
        {
            if ( name == null )
            {
                throw new ArgumentException( "Name must be specified." );
            }

            name = name.Trim().ToLower();

            base.RegisterBlock( name, factoryMethod );

            DotLiquidBlockProxy.RegisterFactory( name, factoryMethod );

            // DotLiquid regards a Block as a special type of Tag.
            Template.RegisterTag<DotLiquidBlockProxy>( name );
        }

        /// <summary>
        /// Convert a LavaContext to a DotLiquid RenderParameters object. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private RenderParameters GetDotLiquidRenderParametersFromLavaContext( ILavaContext context )
        {
            var renderSettings = new RenderParameters();

            var dotLiquidContext = ( (DotLiquidLavaContext)context ).DotLiquidContext;

            // Set the transformation function for converting a DotLiquid Context to a Lava Context.
            // This tranformation allows the context to be injected into a filter in a way that is independent
            // of the underlying rendering framework.
            dotLiquidContext.FilterContextParameterType = Template.FilterContextParameterType;
            dotLiquidContext.FilterContextParameterTransformer = Template.FilterContextParameterTransformer;

            renderSettings.Context = dotLiquidContext;

            // Store the EnabledCommands setting in the DotLiquid Registers.
            renderSettings.Context.Registers["EnabledCommands"] = string.Join( ",", context.GetEnabledCommands() );

            return renderSettings;
        }

        public override bool TryRender( string inputTemplate, out string output, ILavaContext context )
        {
            try
            {
                var template = CreateNewDotLiquidTemplate( inputTemplate );

                var renderSettings = GetDotLiquidRenderParametersFromLavaContext( context );

                output = template.Render( renderSettings );

                return true;
            }
            catch ( Exception ex )
            {
                ProcessException( ex );

                output = null;
                return false;
            }
        }

        public override void UnregisterShortcode( string name )
        {
            throw new NotImplementedException();
        }

        private Template CreateNewDotLiquidTemplate( string inputTemplate )
        {
            var formattedInput = ReplaceTemplateShortcodes( inputTemplate );

            var template = Template.Parse( formattedInput );

            /* 
             * 2/19/2020 - JPH
             * The DotLiquid library's Template object was not originally designed to be thread safe, but a PR has since
             * been merged into that repository to add this functionality (https://github.com/dotliquid/dotliquid/pull/220).
             * We have cherry-picked the PR's changes into our DotLiquid project, allowing the Template to operate safely
             * in a multithreaded context, which can happen often with our cached Template instances.
             *
             * Reason: Rock Issue #4084, Weird Behavior with Lava Includes
             */
            template.MakeThreadSafe();

            return template;
        }

        public override ILavaTemplate ParseTemplate( string inputTemplate )
        {
            var lavaTemplate = new DotLiquidTemplateProxy( CreateNewDotLiquidTemplate( inputTemplate ) );

            return lavaTemplate;
        }

        public override bool AreEqualValue( object left, object right )
        {
            var condition = global::DotLiquid.Condition.Operators["=="];

            return condition( left, right );
        }

        internal static readonly Regex FullShortCodeToken = new Regex( @"{\[\s*(\w+)\s*([^\]}]*)?\]}", RegexOptions.Compiled );

        public static string ShortcodeNameSuffix = "_sc";

        private string ReplaceTemplateShortcodes( string inputTemplate )
        {
            /* Lava shortcode syntax is not recognized as a valid document element by Liquid parsers.
             * Replace the shortcode token "{[ ]}" with the Liquid tag token "{% %}",
             * and add a prefix to avoid naming collisions with existing registered tags.
             * The shortcode can then be processed as a regular custom tag by the Liquid templating engine.
             */
            var newBlockName = "{% $1<suffix> $2 %}".Replace( "<suffix>", ShortcodeNameSuffix );

            inputTemplate = FullShortCodeToken.Replace( inputTemplate, newBlockName );

            return inputTemplate;
        }
    }
}
