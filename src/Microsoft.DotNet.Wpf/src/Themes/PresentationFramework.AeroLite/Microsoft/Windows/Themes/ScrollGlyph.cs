// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Windows.Themes
{
    /// <summary>
    ///     Types and orientations of ScrollBar glyphs.
    /// </summary>
    public enum ScrollGlyph
    {
        /// <summary>
        ///     No glyph
        /// </summary>
        None,

        /// <summary>
        ///     Vertical gripper.
        /// </summary>
        VerticalGripper,

        /// <summary>
        ///     Horizontal gripper.
        /// </summary>
        HorizontalGripper,

        // NOTE: if you add or remove any values in this enum, be sure to update ScrollChrome.IsValidScrollGlyph()    
    }
}
