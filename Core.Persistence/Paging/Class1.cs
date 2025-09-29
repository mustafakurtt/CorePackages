using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.Paging;

public interface IPaginate<T>
{
    int Index { get; }
    int Size { get; }
    int Count { get; }
    int Pages { get; }
    IList<T> Items { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }
}


public class Paginate<T> : IPaginate<T>
{
    public Paginate()
    {
        Items = Array.Empty<T>();
    }

    public int Size { get; set; }
    public int Index { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public IList<T> Items { get; set; }
    public bool HasPrevious => Index > 0;
    public bool HasNext => Index + 1 < Pages;
}

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