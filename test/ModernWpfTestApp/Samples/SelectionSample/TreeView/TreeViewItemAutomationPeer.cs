// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace MUXControlsTestApp.Samples.Selection
{
    public class TreeViewItemAutomationPeer : FrameworkElementAutomationPeer, ISelectionItemProvider
    {
        private TreeViewItem _owner;
        public TreeViewItemAutomationPeer(FrameworkElement owner) : base(owner)
        {
            _owner = (TreeViewItem)owner;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.SelectionItem)
            {
                return this;
            }

            return base.GetPattern(patternInterface);
        }

        public void AddToSelection()
        {
            _owner.Select(true);
        }

        public void RemoveFromSelection()
        {
            _owner.Select(false);
        }

        public void Select()
        {
            _owner.Select(true);
        }

        public bool IsSelected
        {
            get
            {
                return _owner.IsSelected.HasValue && _owner.IsSelected.Value;
            }
        }

        public IRawElementProviderSimple SelectionContainer
        {
            get
            {
                // TODO: could cache this to avoid the walk every time
                IRawElementProviderSimple containerPeer = null;
                FrameworkElement element = _owner;
                while (element != null && !(element is SelectionContainer))
                {
                    element = (FrameworkElement)element.Parent;
                }

                if (element != null)
                {
                    var container = (SelectionContainer)element;
                    containerPeer = ProviderFromPeer(CreatePeerForElement(container));
                }

                return containerPeer;
            }
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ListItem;
        }

        protected override string GetLocalizedControlTypeCore()
        {
            return "Item";
        }
    }
}
