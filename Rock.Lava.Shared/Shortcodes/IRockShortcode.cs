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
using System.Collections.Generic;
using System.IO;

namespace Rock.Lava
{

    /// <summary>
    /// Determines the type of tag (inline or block). Block type requires an end tag.
    /// </summary>
    public enum LavaElementTypeSpecifier
    {
        /// <summary>
        /// A language element that consists of a single Tag.
        /// </summary>
        Inline = 1,

        /// <summary>
        /// A language element that consists of an opening Tag and a corresponding closing Tag.
        /// </summary>
        Block = 2
    }

    /// <summary>
    /// Represents a Lava shortcode definition.
    /// </summary>
    public interface IRockShortcode
    {
        string Name
        {
            get;
        }

        void Initialize( string tagName, string markup, IEnumerable<string> tokens );

        void Render( ILavaContext context, TextWriter result );

        LavaElementTypeSpecifier ElementType { get; }
    }

    //public class RockShortcodeDefinition
    //{

    //}
}
