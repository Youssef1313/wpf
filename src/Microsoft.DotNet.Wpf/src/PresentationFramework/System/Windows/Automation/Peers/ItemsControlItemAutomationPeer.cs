// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Automation.Peers
{
    // work around (ItemsControl.GroupStyle doesn't show items in groups in the UIAutomation tree)
    // this class should be public
    internal class ItemsControlItemAutomationPeer : ItemAutomationPeer
    {
        public ItemsControlItemAutomationPeer(object item, ItemsControlWrapperAutomationPeer parent)
            : base(item, parent)
        { }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.DataItem;
        }

        protected override string GetClassNameCore()
        {
            return "ItemsControlItem";
        }
    }
}
