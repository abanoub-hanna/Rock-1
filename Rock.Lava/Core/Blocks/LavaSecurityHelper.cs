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
using System.Linq;

namespace Rock.Lava
{
    public static class LavaSecurityHelper
    {
        /// <summary>
        /// Determines whether the specified command is authorized within the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The command.</param>
        /// <returns>
        ///   <c>true</c> if the specified command is authorized; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAuthorized( ILavaContext context, string command )
        {
            if ( command.IsNullOrWhiteSpace() )
            {
                return false;
            }

            if ( context.EnabledCommands.Any() )
            {
                if ( context.EnabledCommands.Contains( "All" ) || context.EnabledCommands.Contains( command ) )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
