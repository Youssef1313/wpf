// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
//
// Description: The source of the input language of the thread.
//
//

using MS.Win32;

namespace System.Windows.Input
{
    //------------------------------------------------------
    //
    //  InputLanguageProfileNotifySink
    //
    //------------------------------------------------------

    /// <summary>
    ///     This is an internal. This is an implementation of ITfLanguageProfileNotifySink.
    /// </summary>
    internal class InputLanguageProfileNotifySink : UnsafeNativeMethods.ITfLanguageProfileNotifySink
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------
 
        internal InputLanguageProfileNotifySink(InputLanguageSource target)
        {
            _target = target;
        }

        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------
 
        /// <summary>
        ///     OnLanguageChange call back of the interface.
        /// </summary>
        public void OnLanguageChange(short langid, out bool accept)
        {
            accept = _target.OnLanguageChange(langid);
        }
        /// <summary>
        ///     OnLanguageChanged call back of the interface.
        /// </summary>

        public void OnLanguageChanged()
        {
            _target.OnLanguageChanged();
        }

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------
 
        // the owner of this sink.
        private InputLanguageSource _target;
    }
}
