#nullable enable
using System;

namespace Microsoft.Azure.Databricks.Client;

public static class NullableExt
{
    /// <summary>
    /// Apply the given `action` to the nullable's value, if it is not null. Otherwise, do nothing.
    /// </summary>
    public static void Iter<T>(this T? source, Action<T> action) where T : struct
    {
        if (source != null)
        {
            action(source.Value);
        }
    }

    /// <summary>
    /// Apply the given `action` to the nullable's value, if it is not null. Otherwise, do nothing.
    /// </summary>
    public static void Iter<T>(this T? source, Action<T> action) where T : class
    {
        if (source != null)
        {
            action(source);
        }
    }

    /// <summary>
    /// Returns a nullable containing the result of applying `func` to this nullable's value if this nullable is nonempty.
    /// </summary>
    public static TOut? Map<TIn, TOut>(this TIn? source, Func<TIn, TOut> func) 
        where TIn : class 
        where TOut : class
    {
        return source switch
        {
            null => null,
            var it => func(it)
        };
    }

    /// <summary>
    /// Returns a nullable containing the result of applying `func` to this nullable's value if this nullable is nonempty.
    /// </summary>
    public static TOut? Map<TIn, TOut>(this TIn? source, Func<TIn, TOut> func)
        where TIn : struct
        where TOut: class
    {
        return source == null ? null : func(source.Value);
    }

    /// <summary>
    /// Returns the nullable's value if the nullable is not null, otherwise return the result of evaluating default.
    /// </summary>
    public static TOut GetOrElse<TIn, TOut>(this TIn? source, Func<TOut> @default) where TIn : TOut where TOut : class
    {
        return source switch
        {
            null => @default(),
            var it => it
        };
    }

    /// <summary>
    /// Returns true if this nullable is not null and the predicate `p` returns true when applied to this nullable's value. Otherwise, returns false.
    /// </summary>
    public static bool Exists<T>(this T? source, Predicate<T> p) where T : class
    {
        return source switch
        {
            null => false,
            var it => p(it)
        };
    }

    /// <summary>
    /// Returns true if this nullable is empty or the predicate `p` returns true when applied to this nullable's value.
    /// </summary>
    public static bool ForAll<T>(this T? source, Predicate<T> p) where T : class
    {
        return source switch
        {
            null => true,
            var it => p(it)
        };
    }
}