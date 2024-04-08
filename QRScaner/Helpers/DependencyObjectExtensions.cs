using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace System.Windows
{
    static class DependencyObjectExtensions
    {
        public static DependencyObject FindVisualRoot(this DependencyObject obj)
        {
            do
            {
                var parent = VisualTreeHelper.GetParent(obj);
                if (parent is null) return obj;
                obj = parent;
            }
            while (true);
        }
        public static DependencyObject FindLogicalRoot(this DependencyObject obj)
        {
            do
            {
                var parent = LogicalTreeHelper.GetParent(obj);
                if (parent is null) return obj;
                obj = parent;
            }
            while (true);
        }

        public static T FindVisualParent<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            if (obj is null) return null;
            var target = obj;
            do
            {
                target = VisualTreeHelper.GetParent(target);
            }
            while (target != null && !(target is T));

            return target as T;
        }

        public static T FindLogicalParent<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            if (obj is null) return null;
            var target = obj;
            do
            {
                target = LogicalTreeHelper.GetParent(target);
            }
            while (target != null && !(target is T));

            return target as T;
        }
        //public static DependencyObject FindVisualRoot(DependencyObject element)
        //{
        //    // Allows element to be ContentElement
        //    bool includeContentElements = true;
        //    while (element != null)
        //    {
        //        DependencyObject parent = GetVisualParent(element, includeContentElements);
        //        if (parent == null)
        //        {
        //            return element;
        //        }
        //        element = parent;
        //        includeContentElements = false;
        //    }
        //    return null;
        //}
        //private static DependencyObject GetVisualParent(DependencyObject element, bool includeContentElements)
        //{
        //    if (includeContentElements)
        //    {
        //        ContentElement? ce = element as ContentElement;
        //        if (ce != null)
        //        {
        //            return LogicalTreeHelper.GetParent(ce);
        //        }
        //    }
        //    return VisualTreeHelper.GetParent(element);
        //}

    }
}
