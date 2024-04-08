using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QRScaner.Behaviors
{
    internal class CloseQRScanerBehaviors : Behavior<System.Windows.Controls.Button>
    {
        protected override void OnAttached() => AssociatedObject.Click += OnButtonClick;

        public void OnButtonClick(object sender, RoutedEventArgs e)
        {
            //var a = FindVisualRoot(AssociatedObject);

            if (AssociatedObject.FindVisualRoot() is MainWindow window)
            {
                window.Close();

            }
        }
    }
}
