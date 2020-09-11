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
using System.Globalization;

using Humanizer;

namespace Rock.Lava.Filters
{
    public static partial class TemplateFilters
    {
        /// <summary>
        /// Converts the input to a Boolean(true/false) value.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="resultIfNullOrEmpty"></param>
        /// <returns></returns>
        public static bool AsBoolean( object input, bool resultIfNullOrEmpty = false )
        {
            return ExtensionMethods.AsBoolean( input.ToStringSafe(), resultIfNullOrEmpty );
        }

        /// <summary>
        /// Converts the input to a decimal value, or 0 if the conversion is unsuccessful.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal AsDecimal( object input )
        {
            return ExtensionMethods.AsDecimal( input.ToStringSafe() );
        }

        /// <summary>
        /// Converts the input to a double-precision value, or 0 if the conversion is unsuccessful.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double AsDouble( object input )
        {
            return ExtensionMethods.AsDouble( input.ToStringSafe() );
        }

        /// <summary>
        /// Converts the input to an integer value, or 0 if the conversion is unsuccessful.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int AsInteger( object input )
        {
            return ExtensionMethods.AsInteger( input.ToStringSafe() );
        }

        /// <summary>
        /// Formats the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string Format( object input, string format )
        {
            if ( input == null )
            {
                return null;
            }

            var inputString = input.ToString();
            
            if ( string.IsNullOrWhiteSpace( format ) )
            {
                return inputString;
            }

            var decimalValue = inputString.AsDecimalOrNull();

            if ( decimalValue == null )
            {
                return string.Format( "{0:" + format + "}", inputString );
            }
            else
            {
                return string.Format( "{0:" + format + "}", decimalValue );
            }
        }

        /// <summary>
        /// Formats the specified input as currency using the specified CurrencySymbol.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="currencySymbol">The currency symbol.</param>
        /// <returns></returns>
        public static string FormatAsCurrency( object input, string currencySymbol = null )
        {
            if ( input == null )
            {
                return null;
            }

            currencySymbol = currencySymbol ?? CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;

            // if the input an integer, decimal, double or anything else that can be parsed as a decimal, format that
            decimal? inputAsDecimal = input.ToString().AsDecimalOrNull();

            if ( inputAsDecimal.HasValue )
            {
                return string.Format( "{0}{1:N}", currencySymbol, inputAsDecimal );
            }
            else
            {
                if ( input is string )
                {
                    // if the input is a string, just append the currency symbol to the front, even if it can't be converted to a number
                    return string.Format( "{0}{1}", currencySymbol, input );
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// takes 1, 2 and returns 1st, 2nd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToOrdinal( string input )
        {
            if ( input == null )
            {
                return input;
            }

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.Ordinalize();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// takes 1,2 and returns one, two
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToWords( string input )
        {
            if ( input == null )
            {
                return input;
            }

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.ToWords();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// takes 1,2 and returns first, second
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToOrdinalWords( string input )
        {
            if ( input == null )
            {
                return input;
            }

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.ToOrdinalWords();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// takes 1,2 and returns I, II, IV
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NumberToRomanNumerals( string input )
        {
            if ( input == null )
            {
                return input;
            }

            int number;

            if ( int.TryParse( input, out number ) )
            {
                return number.ToRoman();
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// formats string to be appropriate for a quantity
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns></returns>
        public static string ToQuantity( string input, object quantity )
        {
            int numericQuantity;
            if ( quantity is string )
            {
                numericQuantity = ( int ) ( ( quantity as string ).AsDecimal() );
            }
            else
            {
                numericQuantity = Convert.ToInt32( quantity );
            }

            return input == null
                ? input
                : input.ToQuantity( numericQuantity );
        }
    }
}
