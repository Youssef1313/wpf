// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace MS.Internal.Ink
{
    internal static class Native
    {
        /// <summary>
        /// Initialize the constants
        /// </summary>
        static Native()
        {
            // NOTICE-2005/10/14-WAYNEZEN,
            // Make sure those lengths are indepentent from the 32bit or 64bit platform. Otherwise it could 
            // break the ISF format.
            SizeOfInt = sizeof(int);
            SizeOfUInt = sizeof(uint);
            SizeOfUShort = sizeof(ushort);
            SizeOfByte = sizeof(byte);
            SizeOfFloat = sizeof(float);
            SizeOfDouble = sizeof(double);
            SizeOfGuid = (uint)Unsafe.SizeOf<Guid>();
            SizeOfDecimal = sizeof(decimal);
        }

        internal static readonly uint SizeOfInt;      // Size of an int
        internal static readonly uint SizeOfUInt;     // Size of an unsigned int
        internal static readonly uint SizeOfUShort;   // Size of an unsigned short
        internal static readonly uint SizeOfByte;     // Size of a byte
        internal static readonly uint SizeOfFloat;    // Size of a float
        internal static readonly uint SizeOfDouble;   // Size of a double
        internal static readonly uint SizeOfGuid;    // Size of a GUID
        internal static readonly uint SizeOfDecimal; // Size of a VB-style Decimal

        internal const int BitsPerByte = 8;    // number of bits in a byte
        internal const int BitsPerShort = 16;    // number of bits in one short - 2 bytes
        internal const int BitsPerInt = 32;    // number of bits in one integer - 4 bytes
        internal const int BitsPerLong = 64;    // number of bits in one long - 8 bytes
        

        // since casting from floats have mantisaa components,
        //      casts from float to int are not constrained by
        //      Int32.MaxValue, but by the maximum float value
        //      whose mantissa component is still within range
        //      of an integer. Anything larger will cause an overflow.
        internal const int MaxFloatToIntValue = 2147483584 - 1; // 2.14748e+009
    }
}
