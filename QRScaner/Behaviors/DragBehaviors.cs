using Microsoft.Xaml.Behaviors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace QRScaner.Behaviors
{
    internal class DragBehaviors: Behavior<Grid>
    {

        protected override void OnAttached() => AssociatedObject.MouseLeftButtonDown += OnButtonClick;

        private void OnButtonClick(object sender, MouseButtonEventArgs e)
        {
            var a = FindVisualRoot(AssociatedObject);

            if (a is MainWindow window)
            {
                if (e.ChangedButton == MouseButton.Left)
                    window.DragMove();
                
            }
           
        }

        public static DependencyObject? FindVisualRoot(DependencyObject element)
        {
            // Allows element to be ContentElement
            bool includeContentElements = true;
            while (element != null)
            {
                DependencyObject parent = GetVisualParent(element, includeContentElements);
                if (parent == null)
                {
                    return element;
                }
                element = parent;
                includeContentElements = false;
            }
            return null;
        }
        private static DependencyObject GetVisualParent(DependencyObject element, bool includeContentElements)
        {
            if (includeContentElements)
            {
                ContentElement? ce = element as ContentElement;
                if (ce != null)
                {
                    return LogicalTreeHelper.GetParent(ce);
                }
            }
            return VisualTreeHelper.GetParent(element);
        }
    }
}
