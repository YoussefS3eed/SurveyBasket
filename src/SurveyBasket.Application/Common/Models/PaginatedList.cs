using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace SurveyBasket.Application.Common.Models;

public class PaginatedList<T>
{
    public PaginatedList() { }

    [JsonConstructor]
    public PaginatedList(List<T> items, int pageNumber, int totalPages, bool hasPreviousPage, bool hasNextPage)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = totalPages;
        HasPreviousPage = hasPreviousPage;
        HasNextPage = hasNextPage;
    }

    public PaginatedList(List<T> items, int pageNumber, int count, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        HasPreviousPage = PageNumber > 1;
        HasNextPage = PageNumber < TotalPages;
    }

    public List<T> Items { get; set; } = default!;
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, pageNumber, count, pageSize);
    }
}
