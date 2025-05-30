// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace System.Xaml.Schema
{
    internal class MemberReflector : Reflector
    {
        // VisibilityInvalid indicates the value hasn't been looked up yet
        private const DesignerSerializationVisibility VisibilityInvalid = (DesignerSerializationVisibility)int.MaxValue;
        // VisibilityNone indicates the value was looked up, and wasn't present
        private const DesignerSerializationVisibility VisibilityNone = (DesignerSerializationVisibility)(int.MaxValue - 1);
        private static MemberReflector s_UnknownReflector;

        // Lazy init: check NullableReference.IsSet to determine if these fields have been initialized
        private NullableReference<string> _constructorArgument;
        private NullableReference<XamlValueConverter<XamlDeferringLoader>> _deferringLoader;
        private NullableReference<object> _defaultValue;

        private NullableReference<MethodInfo> _getter;

        private NullableReference<MethodInfo> _setter;

        private NullableReference<XamlValueConverter<TypeConverter>> _typeConverter;
        private NullableReference<XamlValueConverter<ValueSerializer>> _valueSerializer;

        // Lazy init: set to VisibilityInvalid when uninitialized
        private DesignerSerializationVisibility _designerSerializationVisibility;

        // Thread safety: see notes regarding bitflags in TypeReflector.cs
        private int _memberBits;

        internal MemberReflector()
        {
            _designerSerializationVisibility = VisibilityInvalid;
        }

        internal MemberReflector(bool isEvent)
            :this()
        {
            if (isEvent)
            {
                _memberBits = (int)BoolMemberBits.Event;
            }

            _memberBits |= GetValidMask((int)BoolMemberBits.Event);
        }

        internal MemberReflector(MethodInfo getter, MethodInfo setter, bool isEvent)
            : this(isEvent)
        {
            _getter.Value = getter;
            _setter.Value = setter;
        }

        ///<summary>Ctor used by directives</summary>
        internal MemberReflector(XamlType type, XamlValueConverter<TypeConverter> typeConverter)
        {
            Type = type;
            _typeConverter.Value = typeConverter;
            _designerSerializationVisibility = DesignerSerializationVisibility.Visible;
            _memberBits = (int)BoolMemberBits.Directive | (int)BoolMemberBits.AllValid;

            // Explicitly set all the nullable references so that IsSet is true
            _deferringLoader.Value = null;
            _getter.Value = null;
            _setter.Value = null;
            _valueSerializer.Value = null;
        }

        internal static MemberReflector UnknownReflector
        {
            get
            {
                if (s_UnknownReflector is null)
                {
                    s_UnknownReflector = new MemberReflector
                    {
                        _designerSerializationVisibility = DesignerSerializationVisibility.Visible,
                        _memberBits = (int)BoolMemberBits.Default |
                        (int)BoolMemberBits.Unknown | (int)BoolMemberBits.AllValid
                    };

                    // Explicitly set all the nullable references so that IsSet is true
                    s_UnknownReflector._deferringLoader.Value = null;
                    s_UnknownReflector._getter.Value = null;
                    s_UnknownReflector._setter.Value = null;
                    s_UnknownReflector._typeConverter.Value = null;
                    s_UnknownReflector._valueSerializer.Value = null;

                    s_UnknownReflector.DependsOn = ReadOnlyCollection<XamlMember>.Empty;
                    s_UnknownReflector.Invoker = XamlMemberInvoker.UnknownInvoker;
                    s_UnknownReflector.Type = XamlLanguage.Object;
                }

                return s_UnknownReflector;
            }
        }

        // Lazy init and thread safety note: all implicit properties (i.e properties without backing
        // fields) are idempotent, and null if uninitialized

        internal string ConstructorArgument
        {
            get { return _constructorArgument.Value; }
            set { _constructorArgument.Value = value; }
        }

        internal bool ConstructorArgumentIsSet
        {
            get { return _constructorArgument.IsSet; }
        }

        internal IReadOnlyDictionary<char, char> MarkupExtensionBracketCharactersArgument { get; set; }

        internal bool MarkupExtensionBracketCharactersArgumentIsSet { get; set; }

        internal object DefaultValue
        {
            get { return _defaultValue.IsNotPresent ? null : _defaultValue.Value; }
            set { _defaultValue.Value = value; }
        }

        internal bool DefaultValueIsNotPresent
        {
            get { return _defaultValue.IsNotPresent; }
            set { _defaultValue.IsNotPresent = value; }
        }

        internal bool DefaultValueIsSet
        {
            get { return _defaultValue.IsSet; }
        }

        internal XamlValueConverter<XamlDeferringLoader> DeferringLoader
        {
            get { return _deferringLoader.Value; }
            set { _deferringLoader.Value = value; }
        }

        internal bool DeferringLoaderIsSet { get { return _deferringLoader.IsSet; } }

        internal IList<XamlMember> DependsOn { get; set; }

        internal DesignerSerializationVisibility? SerializationVisibility
        {
            get { return (_designerSerializationVisibility != VisibilityNone) ? _designerSerializationVisibility : (DesignerSerializationVisibility?)null; }
            set { _designerSerializationVisibility = value.GetValueOrDefault(VisibilityNone); }
        }

        internal bool DesignerSerializationVisibilityIsSet
        {
            get { return _designerSerializationVisibility != VisibilityInvalid; }
        }

        internal MethodInfo Getter
        {
            get { return _getter.Value; }
            set { _getter.SetIfNull(value); }
        }

        internal bool GetterIsSet
        {
            get { return _getter.IsSet; }
        }

        internal XamlMemberInvoker Invoker { get; set; }

        // No need to check valid flag, this is set at creation
        internal bool IsUnknown { get { return (_memberBits & (int)BoolMemberBits.Unknown) != 0; } }

        internal MethodInfo Setter
        {
            get { return _setter.Value; }
            set { _setter.SetIfNull(value); }
        }

        internal bool SetterIsSet
        {
            get { return _setter.IsSet; }
        }

        internal XamlType Type { get; set; }

        internal XamlType TargetType { get; set; }

        internal XamlValueConverter<TypeConverter> TypeConverter
        {
            get { return _typeConverter.Value; }
            set { _typeConverter.Value = value; }
        }

        internal bool TypeConverterIsSet { get { return _typeConverter.IsSet; } }

        internal MemberInfo UnderlyingMember { get; set; }

        internal XamlValueConverter<ValueSerializer> ValueSerializer
        {
            get { return _valueSerializer.Value; }
            set { _valueSerializer.Value = value; }
        }

        internal bool ValueSerializerIsSet { get { return _valueSerializer.IsSet; } }

        internal bool? GetFlag(BoolMemberBits flag)
        {
            return GetFlag(_memberBits, (int)flag);
        }

        internal void SetFlag(BoolMemberBits flag, bool value)
        {
            SetFlag(ref _memberBits, (int)flag, value);
        }

        // Assumes declaring type is visible and is either public or internal
        // Assumes method type args (if any) are visible
        internal static bool IsInternalVisibleTo(MethodInfo method, Assembly accessingAssembly, XamlSchemaContext schemaContext)
        {
            if (accessingAssembly is null)
            {
                return false;
            }

            if (method.IsAssembly || method.IsFamilyOrAssembly)
            {
                if (TypeReflector.IsInternal(method.DeclaringType))
                {
                    // We've already done an internals visibility check for the declaring type
                    return true;
                }

                return schemaContext.AreInternalsVisibleTo(
                    method.DeclaringType.Assembly, accessingAssembly);
            }

            return false;
        }

        // Assumes declaring type is visible and is either public or internal
        // Assumes method type args (if any) are visible
        internal static bool IsProtectedVisibleTo(MethodInfo method, Type derivedType, XamlSchemaContext schemaContext)
        {
            if (derivedType is null)
            {
                return false;
            }

            // Note: this doesn't handle the case of method.IsAssembly, because callers should use
            // IsInternalVisibleTo for those cases.
            if (!derivedType.Equals(method.DeclaringType) && !derivedType.IsSubclassOf(method.DeclaringType))
            {
                return false;
            }

            if (method.IsFamily || method.IsFamilyOrAssembly)
            {
                return true;
            }

            if (method.IsFamilyAndAssembly)
            {
                if (TypeReflector.IsInternal(method.DeclaringType))
                {
                    // We've already done an internals visibility check for the declaring type
                    return true;
                }

                return schemaContext.AreInternalsVisibleTo(
                    method.DeclaringType.Assembly, derivedType.Assembly);
            }

            return false;
        }

        internal static bool GenericArgumentsAreVisibleTo(MethodInfo method, Assembly accessingAssembly, XamlSchemaContext schemaContext)
        {
            if (method.IsGenericMethod)
            {
                foreach (Type typeArg in method.GetGenericArguments())
                {
                    if (!TypeReflector.IsVisibleTo(typeArg, accessingAssembly, schemaContext))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Used by Reflector for attribute lookups
        protected override MemberInfo Member
        {
            get { return UnderlyingMember; }
        }
    }
}
