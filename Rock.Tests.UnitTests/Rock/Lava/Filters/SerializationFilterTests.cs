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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Rock.Lava;

namespace Rock.Tests.UnitTests.Lava
{
    [TestClass]
    public class SerializationFilterTests : LavaUnitTestBase
    {
        /// <summary>
        /// The filter should accept a Person object as input and return a valid JSON string.
        /// </summary>
        [TestMethod]
        public void ToJSON_ForTestPersonObject_ProducesJsonString()
        {
            var person = _helper.GetTestPersonTedDecker();

            var mergeValues = new LavaDictionary { { "CurrentPerson", person } };

            var personJson = person.ToJson( Formatting.Indented );

            _helper.AssertTemplateOutput( personJson, "{{ CurrentPerson | ToJSON }}", mergeValues );
        }

        /// <summary>
        /// The filter should accept a Person object as input and return a valid JSON string.
        /// </summary>
        [TestMethod]
        public void FromJSON_ForTestPersonObject_ProducesJsonObject()
        {
            var person = _helper.GetTestPersonTedDecker();

            var jsonString = person.ToJson();

            var mergeValues = new LavaDictionary { { "JsonString", jsonString } };

            _helper.AssertTemplateOutput( "Ted Decker - North Campus",
                "{% assign jsonObject = JsonString | FromJSON %}{{ jsonObject.NickName }} {{ jsonObject.LastName }} - {{ jsonObject.Campus.Name }}",
                mergeValues );
        }

        /// <summary>
        /// The filter should accept a dictionary object as input and return a valid JSON string.
        /// </summary>
        [TestMethod]
        public void ToJSON_ForDictionary_ProducesJsonString()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "FirstName", "Ted" },
                { "LastName", "Decker" }
            };

            var mergeValues = new LavaDictionary { { "Dictionary", dictionary } };

            var dictionaryJson = dictionary.ToJson( Formatting.Indented );

            _helper.AssertTemplateOutput( dictionaryJson, "{{ Dictionary | ToJSON }}", mergeValues );
        }
    }
}
