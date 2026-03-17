using System.Linq.Dynamic.Core;

namespace SurveyBasket.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string? searchValue, Func<T, string> searchSelector)
    {
        if (string.IsNullOrEmpty(searchValue))
            return query;

        return query.Where(x => searchSelector(x).Contains(searchValue));
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortColumn, string? sortDirection = "ASC")
    {
        if (string.IsNullOrEmpty(sortColumn))
            return query;

        var direction = string.IsNullOrEmpty(sortDirection) || sortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase)
            ? "ASC"
            : "DESC";

        return query.OrderBy($"{sortColumn} {direction}");
    }
}
