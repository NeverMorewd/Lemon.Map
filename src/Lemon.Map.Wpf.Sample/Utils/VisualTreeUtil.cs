using System.Windows;
using System.Windows.Media;

namespace Lemon.Map.Wpf.Utils
{
    public static class VisualTreeUtil
    {
        public static List<T>? FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            List<T> TList = [];
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child == null)
                {
                    continue;
                }
                if (child is T t)
                {
                    TList.Add(t);
                    List<T>? childOfChildren = FindVisualChildren<T>(child);
                    if (childOfChildren !=null)
                    {
                        TList.AddRange(childOfChildren);
                    }
                }
                else
                {
                    List<T>? childOfChildren = FindVisualChildren<T>(child);
                    if (childOfChildren != null)
                    {
                        TList.AddRange(childOfChildren);
                    }
                }
            }
            return TList;
        }

        public static T? FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            var parents = FindVisualParents<T>(obj);
            return parents.FirstOrDefault();
        }

        public static List<T> FindVisualParents<T>(DependencyObject obj) where T : DependencyObject
        {
            List<T> TList = [];
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            if (parent != null)
            {
                if (parent is T t)
                {
                    TList.Add(t);
                    List<T> parentOfParent = FindVisualParents<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
                else
                {
                    List<T> parentOfParent = FindVisualParents<T>(parent);
                    if (parentOfParent != null)
                    {
                        TList.AddRange(parentOfParent);
                    }
                }
            }
            return TList;
        }
    }
}
