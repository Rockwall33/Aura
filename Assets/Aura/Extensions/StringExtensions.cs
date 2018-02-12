///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                                                                                                                                             ///
///     MIT License                                                                                                                                             ///
///                                                                                                                                                             ///
///     Copyright (c) 2016 Raphaël Ernaelsten (@RaphErnaelsten)                                                                                                 ///
///                                                                                                                                                             ///
///     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),      ///
///     to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute,                  ///
///     and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:              ///
///                                                                                                                                                             ///
///     The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                          ///
///                                                                                                                                                             ///
///     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,     ///
///     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER      ///
///     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS    ///
///     IN THE SOFTWARE.                                                                                                                                        ///
///                                                                                                                                                             ///
///     PLEASE CONSIDER CREDITING AURA IN YOUR PROJECTS. IF RELEVANT, USE THE UNMODIFIED LOGO PROVIDED IN THE "LICENSE" FOLDER.                                 ///
///                                                                                                                                                             ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Text;

namespace AuraAPI
{
    /// <summary>
    /// Static class containing extension for string type
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Insert a string before all upper case letters
        /// </summary>
        /// <param name="insertedString">String that will be inserted before the upper case letter</param>
        /// <param name="ignoreFirstLetter">Should the first letter of the string be ignored? Default = true</param>
        /// <param name="ignoreSpaces">Should insertion be ignored if a space is in front of the upper case letter? Default = true</param>
        /// <returns>The modified string</returns>
        public static string InsertStringBeforeUpperCaseLetters(this string sourceString, string insertedString, bool ignoreFirstLetter = true, bool ignoreSpaces = true)
        {
            if(string.IsNullOrEmpty(sourceString))
            {
                return "";
            }

            StringBuilder newText = new StringBuilder(sourceString.Length * 2);
            if(ignoreFirstLetter)
            {
                newText.Append(sourceString[0]);
            }

            for(int i = 1; i < sourceString.Length; i++)
            {
                if(char.IsUpper(sourceString[i]) && (sourceString[i - 1] != ' ' || !ignoreSpaces))
                {
                    newText.Append(insertedString);
                }
                newText.Append(sourceString[i]);
            }

            return newText.ToString();
        }

        /// <summary>
        /// Insert a char before all upper case letters
        /// </summary>
        /// <param name="insertedCharacter">Char that will be inserted before the upper case letter</param>
        /// <param name="ignoreFirstLetter">Should the first letter of the string be ignored? Default = true</param>
        /// <param name="ignoreSpaces">Should insertion be ignored if a space is in front of the upper case letter? Default = true</param>
        /// <returns>The modified string</returns>
        public static string InsertCharacterBeforeUpperCaseLetters(this string sourceString, char insertedCharacter, bool ignoreFirstLetter = true, bool ignoreSpaces = true)
        {
            return sourceString.InsertStringBeforeUpperCaseLetters(insertedCharacter.ToString(), ignoreFirstLetter, ignoreSpaces);
        }
    }
}
