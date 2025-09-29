using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.Paging;

public static class IQueryablePaginateExtensions
{
    public static async Task<IPaginate<T>> ToPaginateAsync<T>
    (
        this IQueryable<T> source,
        int index,
        int size,
        CancellationToken cancellationToken = default
    )
    {
        int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
        List<T> items = await source.Skip(index * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);
        Paginate<T> list = new()
        {
            Index = index,
            Size = size,
            Count = count,
            Pages = (int)Math.Ceiling(count / (double)size),
            Items = items
        };
        return list;
    }

    public static IPaginate<T> ToPaginate<T>
    (
        this IQueryable<T> source,
        int index,
        int size
    )
    {
        int count = source.Count();
        List<T> items = source.Skip(index * size).Take(size).ToList();
        Paginate<T> list = new()
        {
            Index = index,
            Size = size,
            Count = count,
            Pages = (int)Math.Ceiling(count / (double)size),
            Items = items
        };
        return list;
    }
}