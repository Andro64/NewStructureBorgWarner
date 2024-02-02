using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BORGWARNER_SERVOPRESS.UI
{
    public class PageManager
    {
        private DependencyObject parent;

        public PageManager(DependencyObject parent)
        {
            this.parent = parent;
        }

        public void DisableControls(List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control != null)
                {
                    control.IsEnabled = false;
                }
            }
        }
        public void EnableControls(List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control != null)
                {
                    control.IsEnabled = true;
                }
            }
        }
        public void VisibleControls(List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control != null)
                {
                    control.Visibility = Visibility.Visible;
                }
            }
        }
        public void HideControls(List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control != null)
                {
                    control.Visibility = Visibility.Collapsed;
                }
            }
        }

        private UIElement GetControl(string controlName)
        {
            return FindControlInTree(parent, controlName);
        }

        private UIElement FindControlInTree(DependencyObject element, string controlName)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(element))
            {
                if (child is FrameworkElement && ((FrameworkElement)child).Name == controlName)
                {
                    return (FrameworkElement)child;
                }

                if (child is DependencyObject && LogicalTreeHelper.GetChildren(child as DependencyObject) != null)
                {
                    UIElement controlInChild = FindControlInTree(child as DependencyObject, controlName);

                    if (controlInChild != null)
                    {
                        return controlInChild;
                    }
                }
            }

            return null;
        }
    }
}
