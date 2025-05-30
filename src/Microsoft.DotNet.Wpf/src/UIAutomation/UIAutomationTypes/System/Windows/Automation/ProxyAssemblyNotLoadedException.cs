// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Description: Exception thrown when proxy assembly is requried but not yet loaded


//using System.Globalization;
//using System.Collections;
//using System.Runtime.InteropServices;
//using System.Diagnostics;
using System.Runtime.Serialization;

namespace System.Windows.Automation
{
    /// <summary>
    /// This exception is thrown when there is a problem laoading a proxy assembly.  This can happen
    /// In reponse to Automation.RegisterProxyAssembly or when loading the default proxies when the 
    /// first hwnd base AutomationElement is encountered.
    /// </summary>  
    [Serializable]
#if (INTERNAL_COMPILE)
    internal class ProxyAssemblyNotLoadedException : Exception
#else
    public class ProxyAssemblyNotLoadedException : Exception
#endif
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProxyAssemblyNotLoadedException() {}
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        public ProxyAssemblyNotLoadedException(String message) : base(message) {}
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public  ProxyAssemblyNotLoadedException(string message, Exception innerException) : base(message, innerException) {}
        
        /// <internalonly>
        /// Constructor for serialization
        /// </internalonly>
#pragma warning disable SYSLIB0051 // Type or member is obsolete
        protected ProxyAssemblyNotLoadedException(SerializationInfo info, StreamingContext context) : base(info, context) {}
#pragma warning restore SYSLIB0051 // Type or member is obsolete

        /// <summary>
        /// Populates a SerializationInfo with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination for this serialization.</param>
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable SYSLIB0051 // Type or member is obsolete
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
#pragma warning restore SYSLIB0051 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member

    }
}
