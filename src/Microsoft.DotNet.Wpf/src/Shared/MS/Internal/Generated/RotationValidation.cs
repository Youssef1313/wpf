// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//


namespace System.Windows.Media.Imaging
{
    internal static partial class ValidateEnums
    {
        /// <summary>
        ///     Returns whether or not an enumeration instance a valid value.
        ///     This method is designed to be used with ValidateValueCallback, and thus
        ///     matches it's prototype.
        /// </summary>
        /// <param name="valueObject">
        ///     Enumeration value to validate.
        /// </param>    
        /// <returns> 'true' if the enumeration contains a valid value, 'false' otherwise. </returns>
        public static bool IsRotationValid(object valueObject)
        {
            Rotation value = (Rotation) valueObject;

            return (value == Rotation.Rotate0) || 
                   (value == Rotation.Rotate90) || 
                   (value == Rotation.Rotate180) || 
                   (value == Rotation.Rotate270);
        }
    }
}
