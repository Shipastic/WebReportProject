
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DAPManSWebReports.API.Services.Sorting
{
    public class SortingService : ISortingService
    {
        public IQueryable<T> ApplySorting<T>(IQueryable<T> source, string sortColumn, string sortType)
        {
            if (string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortType) || sortColumn.Equals("undefined") || sortType.Equals("undefined"))
            {
                return source;
            }

            // Лямбда-выражение для доступа к значению словаря по ключу
            var parameter = Expression.Parameter(typeof(Dictionary<string, object>), "x");
            var propertyAccess = Expression.Property(parameter, "Item", Expression.Constant(sortColumn));

            // Лямбда-выражение (x => x[sortColumn])
            var orderByExp = Expression.Lambda<Func<Dictionary<string, object>, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);

            // Метод OrderBy или OrderByDescending
            var orderByMethod = sortType.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";

            // Вызов метода OrderBy или OrderByDescending
            var resultExp = Expression.Call(
                typeof(Queryable),
                orderByMethod,
                new Type[] { typeof(Dictionary<string, object>), typeof(object) },
                source.Expression,
                Expression.Quote(orderByExp));

            return (IQueryable<T>)source.Provider.CreateQuery<Dictionary<string, object>>(resultExp);
        }
    }
}
