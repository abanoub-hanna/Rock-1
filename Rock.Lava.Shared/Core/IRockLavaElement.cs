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
    /// Interface that classes can implement to be included when searching assemblies for custom Lava Commands.
    /// </summary>
    public interface IRockLavaElement
    {
        /// <summary>
        /// The name by which this element is identified in a Lava document.
        /// </summary>
        string SourceElementName { get; }

        /// <summary>
        /// The source text that defines this element in a Lava document.
        /// </summary>
        string SourceText { get; }

        void OnInitialize( string elementName, string attributesMarkup, List<string> tokens );

        void OnRender( ILavaContext context, TextWriter result );

        void OnParse( List<string> tokens, out List<object> nodes );

        /// <summary>
        /// Executed when the element is first loaded during application startup.
        /// </summary>
        void OnStartup();
    }
}
