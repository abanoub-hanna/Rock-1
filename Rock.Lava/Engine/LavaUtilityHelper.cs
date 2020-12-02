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

namespace Rock.Lava
{
    public static class LavaUtilityHelper
    {
        public static string GetLiquidElementNameFromShortcodeName( string shortcodeName )
        {
            var internalName = shortcodeName.Trim().ToLower() + LavaEngine.ShortcodeInternalNameSuffix;

            return internalName;
        }

        public static string GetShortcodeNameFromLiquidElementName( string shortcodeName )
        {            
            if ( shortcodeName != null
                 && shortcodeName.EndsWith( LavaEngine.ShortcodeInternalNameSuffix ) )
            {
                return shortcodeName.Substring( 0, shortcodeName.Length - 1 );
            }

            return string.Empty;
        }
    }
}