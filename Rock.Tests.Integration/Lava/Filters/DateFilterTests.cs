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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rock.Tests.Integration.Lava
{
    [TestClass]
    public class DateFilterTests
    {
        private static LavaTestHelper _helper;

        #region Constructors

        [ClassInitialize]
        public static void Initialize( TestContext context )
        {
            _helper = LavaTestHelper.NewForDotLiquidProcessor();
        }

        #endregion

        #region Filter Tests: SundayDate

        /// <summary>
        /// Applying the filter to a Friday returns the previous Sunday if the week starts on a Sunday.
        /// </summary>
        [TestMethod]
        public void SundayDate_InputDateIsFriday_YieldsPreviousSunday()
        {
            // The filter returns the Sunday associated with the current week.
            // If Sunday is considered to be the first day of the week, any other day should return a prior date.
            _helper.AssertTemplateOutputDate( "26-Apr-2020",
                                      "{{ '1-May-2020' | SundayDate }}" );
        }

        /// <summary>
        /// Applying the filter to a Sunday returns the same day.
        /// </summary>
        [TestMethod]
        public void SundayDate_InputDateIsSunday_YieldsSameDay()
        {
            _helper.AssertTemplateOutputDate( "3-May-2020",
                                      "{{ '3-May-2020' | SundayDate }}" );
        }

        /// <summary>
        /// Applying the filter to the 'Now' keyword yields the Sunday of the current week.
        /// </summary>
        [TestMethod]
        public void SundayDate_InputParameterIsNow_YieldsNextSunday()
        {
            var nextSunday = RockDateTime.Now.SundayDate();

            _helper.AssertTemplateOutputDate( nextSunday,
                                      "{{ 'Now' | SundayDate }}" );
        }

        #endregion
    }
}
