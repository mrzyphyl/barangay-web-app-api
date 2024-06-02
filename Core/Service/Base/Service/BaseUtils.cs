using System.Reflection;

namespace Core.Service.Base.Service
{
    public static class LinqExtensionMethods
    {
        public static IEnumerable<T> IfThenElse<T>(
        this IEnumerable<T> elements,
        Func<bool> condition,
        Func<IEnumerable<T>, IEnumerable<T>> thenPath,
        Func<IEnumerable<T>, IEnumerable<T>> elsePath = null)
        {
            if (elsePath != null)
                return condition() ? thenPath(elements) : elsePath(elements);
            else
                return condition() ? thenPath(elements) : elements;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> elements, string propertyName, OrderBySort sort)
        {

            try
            {
                var productProperty = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (productProperty == null)
                    return elements;

                if (sort == OrderBySort.Ascending)
                    elements = elements.OrderBy(a => productProperty.GetValue(a, null));
                else
                    elements = elements.OrderByDescending(a => productProperty.GetValue(a, null));

            }
            catch
            {

            }

            return elements;
        }


        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> elements, OrderBySort sort, IComparer<T> comparer)
        {

            try
            {
                if (sort == OrderBySort.Ascending)
                    elements = elements.OrderBy(a => a, comparer);
                else
                    elements = elements.OrderByDescending(a => a, comparer);

            }
            catch
            {

            }

            return elements;
        }



    }

    public enum OrderBySort
    {
        Ascending,
        Descending
    }

}
