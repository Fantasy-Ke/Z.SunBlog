using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Text;
using Microsoft.EntityFrameworkCore;
using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.EntityFrameworkCore.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> PageBy<T>(
        this IQueryable<T> query,
        int skipCount,
        int maxResultCount)
    {
        return query.Skip(skipCount).Take(maxResultCount);
    }

    public static TQueryable PageBy<T, TQueryable>(
        this TQueryable query,
        int skipCount,
        int maxResultCount)
        where TQueryable : IQueryable<T>
    {
        return (TQueryable)query.Skip(skipCount).Take(maxResultCount);
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return !condition ? query : query.Where(predicate);
    }

    public static TQueryable WhereIf<T, TQueryable>(
        this TQueryable query,
        bool condition,
        Expression<Func<T, bool>> predicate)
        where TQueryable : IQueryable<T>
    {
        return !condition ? query : (TQueryable)query.Where<T>(predicate);
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, int, bool>> predicate)
    {
        return !condition ? query : query.Where(predicate);
    }

    public static TQueryable WhereIf<T, TQueryable>(
        this TQueryable query,
        bool condition,
        Expression<Func<T, int, bool>> predicate)
        where TQueryable : IQueryable<T>
    {
        return !condition ? query : (TQueryable)query.Where<T>(predicate);
    }

    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static async Task<PageResult<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, IPagination input)
    {
        var items =await  queryable.Skip((input.PageNo - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
        var totalCount = await queryable.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)input.PageSize);
        return new PageResult<T>()
        {
            PageNo = input.PageNo,
            PageSize = input.PageSize,
            Rows = items,
            Total = (int)totalCount,
            Pages = totalPages,
        };

    }

}
