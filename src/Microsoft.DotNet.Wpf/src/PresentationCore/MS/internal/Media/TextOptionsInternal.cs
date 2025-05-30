// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// 
//
// Description: TextOptions groups attached properties that affect the way 
//              WPF displays text such as TextFormattingMode 
//              and TextRenderingMode.
//

using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Media
{
    /// <summary>
    /// Provide access to text options of element in syntax of TextOptions.xxx = yyy;
    /// Actual data is stored in the owner.
    /// </summary>
    internal static class TextOptionsInternal
    {
        #region Dependency Properties

        /// <summary> Text hinting property </summary>
        internal static readonly DependencyProperty TextHintingModeProperty = 
                DependencyProperty.RegisterAttached(
                        "TextHintingMode",
                        typeof(TextHintingMode),
                        typeof(TextOptionsInternal),
                        new UIPropertyMetadata(TextHintingMode.Auto),
                        new ValidateValueCallback(System.Windows.Media.ValidateEnums.IsTextHintingModeValid));                        

        #endregion Dependency Properties

        
        #region Attached Properties Setters

        public static void SetTextHintingMode(DependencyObject element, TextHintingMode value)
        {
            ArgumentNullException.ThrowIfNull(element);

            element.SetValue(TextHintingModeProperty, value);
        }

        public static TextHintingMode GetTextHintingMode(DependencyObject element)
        {
            ArgumentNullException.ThrowIfNull(element);

            return (TextHintingMode)element.GetValue(TextHintingModeProperty);
        }

        #endregion Attached Groperties Getters and Setters
    }
}
