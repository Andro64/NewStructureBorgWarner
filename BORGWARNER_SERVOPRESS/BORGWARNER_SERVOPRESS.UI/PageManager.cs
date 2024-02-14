using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BORGWARNER_SERVOPRESS.UI
{
    public class PageManager
    {
        private DependencyObject parent;

        public PageManager(DependencyObject parent)
        {
            this.parent = parent;
        }
        public void CleanControls(List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if (control is ComboBox)
                {                    
                    ((ComboBox)control).SelectedIndex = -1;
                }
            }
        }
        public void IsReadOnlyControls(List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control is TextBox)
                {
                    ((TextBox)control).IsReadOnly = true;
                }
                else if (control is RichTextBox)
                {
                    ((RichTextBox)control).IsReadOnly = true;
                }               
                else if (control is ComboBox)
                {
                    ((ComboBox)control).IsReadOnly = true;
                }
            }
        }
        public void IsNotReadOnlyControls(List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control is TextBox)
                {
                    ((TextBox)control).IsReadOnly = false;
                }
                else if (control is RichTextBox)
                {
                    ((RichTextBox)control).IsReadOnly = false;
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).IsReadOnly = false;
                }
            }
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

        public void ChangeBackgroundColor(SolidColorBrush backgroundColor, List<string> controlNames)
        {
            foreach (var controlName in controlNames)
            {
                UIElement control = GetControl(controlName);

                if (control != null)
                {
                    if (control is Control)
                    {
                        ((Control)control).Background = backgroundColor;
                    }
                    else if (control is Panel)
                    {
                        ((Panel)control).Background = backgroundColor;
                    }
                    else if (control is Ellipse)
                    {
                        Ellipse ellipse = (Ellipse)control;
                        ellipse.Fill = backgroundColor;
                    }
                    // Puedes agregar más casos aquí para otros tipos de controles
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
