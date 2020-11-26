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
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;
using Rock.Lava.DotLiquid;

namespace Rock.Lava
{
    /// <summary>
    /// Wraps a LavaFileSystem component so that it can be used as a file provider for the DotLiquid framework.
    /// </summary>
    internal class DotLiquidFileSystem : ILavaFileSystem, IFileSystem
    {
        private ILavaFileSystem _lavaFileSystem;

        public DotLiquidFileSystem( ILavaFileSystem lavaFileSystem )
        {
            _lavaFileSystem = lavaFileSystem;
        }

        //public string ReadTemplateFile( ILavaContext context, string templateName )
        //{
        //    throw new System.NotImplementedException();
        //}

        #region IFileSystem implementation.

        string IFileSystem.ReadTemplateFile( Context context, string templateName )
        {
            // Trim delimiters from the template name.
            templateName = templateName ?? string.Empty;
            templateName = templateName.Trim( @"'""".ToCharArray() );

            try
            {
                var lavaContext = new DotLiquidLavaContext( context );

                return _lavaFileSystem.ReadTemplateFile( lavaContext, templateName );
            }
            catch
            {
                throw new LavaException( "LavaFileSystem ReadTemplate failed. The template \"{0}\" could not loaded.", templateName );
            }

        }

        #endregion

        /// <summary>
        /// Called by Liquid to retrieve a template file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        /// <exception cref="FileSystemException">LavaFileSystem Template Not Found</exception>
        public string ReadTemplateFile( ILavaContext context, string templateName )
        {
            return _lavaFileSystem.ReadTemplateFile( context, templateName );
        }

        public bool FileExists( string filePath )
        {
            return _lavaFileSystem.FileExists( filePath );
        }
    }
}