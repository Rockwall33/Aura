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

namespace AuraAPI
{
    /// <summary>
    /// Collection of extension functions for FrustumParametersEnum objects
    /// </summary>
    public static class FrustumParametersEnumExtensions
    {
        /// <summary>
        /// Tells if specified flags are checked
        /// </summary>
        /// <param name="comparisonFlags">The flags to check</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static bool HasFlags(this FrustumParametersEnum referenceFlags, FrustumParametersEnum comparisonFlags)
        {
            return (referenceFlags & comparisonFlags) == comparisonFlags;
        }

        /// <summary>
        /// Sets specified flags on
        /// </summary>
        /// <param name="addedFlags">The flags to set on</param>
        /// <returns></returns>
        public static FrustumParametersEnum SetFlags(this FrustumParametersEnum referenceFlags, FrustumParametersEnum addedFlags)
        {
            return referenceFlags | addedFlags;
        }

        /// <summary>
        /// Sets specified flags off
        /// </summary>
        /// <param name="removedFlags">The flags to set off</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static FrustumParametersEnum RemoveFlags(this FrustumParametersEnum referenceFlags, FrustumParametersEnum removedFlags)
        {
            return referenceFlags & ~removedFlags;
        }

        /// <summary>
        /// Toggles the specified flags
        /// </summary>
        /// <param name="togglingFlags">the flags to toggle</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static FrustumParametersEnum ToggleFlags(this FrustumParametersEnum referenceFlags, FrustumParametersEnum togglingFlags)
        {
            return referenceFlags ^ togglingFlags;
        }

        /// <summary>
        /// Forces the specified flags to on/off
        /// </summary>
        /// <param name="replacingFlags">The flags to replace</param>
        /// <param name="value">The forced value</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static FrustumParametersEnum ReplaceFlags(this FrustumParametersEnum referenceFlags, FrustumParametersEnum replacingFlags, bool value)
        {
            FrustumParametersEnum newFlags = referenceFlags;

            if(value && !referenceFlags.HasFlags(replacingFlags) || !value && referenceFlags.HasFlags(replacingFlags))
            {
                newFlags = referenceFlags.ToggleFlags(replacingFlags);
            }

            return newFlags;
        }
    }
}
