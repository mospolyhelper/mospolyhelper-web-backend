using System;

namespace Mospolyhelper.Utils
{
    public class Result<T> where T : class
    {
        internal object? Value { get; }

        internal Result(object? value)
        {
            this.Value = value;
        }

        /**
         * Returns `true` if this instance represents a successful outcome.
         * In this case [isFailure] returns `false`.
         */
        public bool IsSuccess => !(Value is Failure) && !(Value is Loading);

        /**
         * Returns `true` if this instance represents a failed outcome.
         * In this case [isSuccess] returns `false`.
         */
        public bool IsFailure => Value is Failure;

        /**
         * Returns `true` if this instance represents a loading outcome.
         * In this case [isSuccess] returns `false`.
         */
        public bool IsLoading => Value is Loading;

        // value & exception retrieval

        /**
         * Returns the encapsulated value if this instance represents [success][Result.isSuccess] or `null`
         * if it is [failure][Result.isFailure].
         *
         * This function is a shorthand for `getOrElse { null }` (see [getOrElse]) or
         * `fold(onSuccess = { it }, onFailure = { null })` (see [fold]).
         */
        public T? GetOrNull()
        {
            if (IsFailure || IsLoading)
                return null;
            else
                return Value as T;
        }

        /**
         * Returns the encapsulated [Throwable] exception if this instance represents [failure][isFailure] or `null`
         * if it is [success][isSuccess].
         *
         * This function is a shorthand for `fold(onSuccess = { null }, onFailure = { it })` (see [fold]).
         */
        public Exception? ExceptionOrNull()
        {
            return (Value as Failure)?.Exception;
        }

        /**
         * Returns a string `Success(v)` if this instance represents [success][Result.isSuccess]
         * where `v` is a string representation of the value or a string `Failure(x)` if
         * it is [failure][isFailure] where `x` is a string representation of the exception.
         */
        public override string ToString()
        {
            switch (Value)
            {
                case Failure qq:
                    return Value.ToString();
                case Loading ss: return "Loading";
                default: return "Success($value)";
            };
        }

        // companion with constructors

        /**
         * Companion object for [Result] class that contains its constructor functions
         * [success] and [failure].
         */
        /**
         * Returns an instance that encapsulates the given [value] as successful value.
         */
        public static Result<T> Success(T? value) => new Result<T>(value);

        /**
         * Returns an instance that encapsulates the given [Throwable] [exception] as failure.
         */
        public static Result<T> Failure(Exception exception) =>
           new Result<T>(ResultExt.CreateFailure<T>(exception));

        /**
         * Returns an instance of loading.
         */
        public static Result<T> Loading() =>
           new Result<T>(ResultExt.GetLoading());
    }

    internal class Failure
    {
        public Exception Exception { get; }

        public Failure(Exception exception)
        {
            Exception = exception;
        }
        public override bool Equals(object? obj)
        {
            return Exception.Equals((obj as Failure)?.Exception);
        }
        public override int GetHashCode()
        {
            return Exception.GetHashCode();
        }
        public override string ToString()
        {
            return $"Failure({Exception})";
        }
    }

    public class Loading
    {
        public static Loading Value = new Loading();
    }
    static class ResultExt
    {
        /**
         * Creates an instance of internal marker [Result.Failure] class to
         * make sure that this class is not exposed in ABI.
         */
        internal static object CreateFailure<T>(Exception exception) =>
            new Failure(exception);

        /**
         * Return an instance of internal marker Loading class to
         * make sure that this class is not exposed in ABI.
         */
        internal static object GetLoading() => Loading.Value;

        ///**
        // * Throws exception if the result is failure. This internal function minimizes
        // * inlined bytecode for [getOrThrow] and makes sure that in the future we can
        // * add some exception-augmenting logic here (if needed).
        // */
        //internal fun Result<*>.throwOnFailure() {
        //    if (value is Result.Failure) throw value.exception
        //}

        ///**
        // * Calls the specified function [block] and returns its encapsulated result if invocation was successful,
        // * catching any [Throwable] exception that was thrown from the [block] function execution and encapsulating it as a failure.
        // */
        //public inline fun<R> runCatching(block: ()->R): Result<R> {
        //    return try
        //    {
        //        Result.success(block())
        //    }
        //    catch (e: Throwable) {
        //        Result.failure(e)
        //    }
        //    }

        //    /**
        //     * Calls the specified function [block] with `this` value as its receiver and returns its encapsulated result if invocation was successful,
        //     * catching any [Throwable] exception that was thrown from the [block] function execution and encapsulating it as a failure.
        //     */
        //public inline fun<T, R> T.runCatching(block: T.()->R): Result<R> {
        //        return try
        //        {
        //            Result.success(block())
        //        }
        //        catch (e: Throwable) {
        //            Result.failure(e)
        //      }
        //        }

        //        // -- extensions ---

        //        /**
        //         * Returns the encapsulated value if this instance represents [success][Result.isSuccess] or throws the encapsulated [Throwable] exception
        //         * if it is [failure][Result.isFailure].
        //         *
        //         * This function is a shorthand for `getOrElse { throw it }` (see [getOrElse]).
        //         */
        //inline fun<T> Result<T>.getOrThrow(): T {
        //            throwOnFailure()
        //    return value as T
        //}

        //        /**
        //         * Returns the encapsulated value if this instance represents [success][Result.isSuccess] or the
        //         * result of [onFailure] function for the encapsulated [Throwable] exception if it is [failure][Result.isFailure].
        //         *
        //         * Note, that this function rethrows any [Throwable] exception thrown by [onFailure] function.
        //         *
        //         * This function is a shorthand for `fold(onSuccess = { it }, onFailure = onFailure)` (see [fold]).
        //         */
        //inline fun<R, T : R > Result<T>.getOrElse(onFailure: (exception: Throwable)->R): R {
        //            contract {
        //                callsInPlace(onFailure, InvocationKind.AT_MOST_ONCE)
        //            }
        //            return when(val exception = exceptionOrNull()) {
        //                null->value as T
        //        else ->onFailure(exception)
        //           }
        //        }

        /**
         * Returns the encapsulated value if this instance represents [success][Result.isSuccess] or the
         * [defaultValue] if it is [failure][Result.isFailure].
         *
         * This function is a shorthand for `getOrElse { defaultValue }` (see [getOrElse]).
         */
        public static R GetOrDefault<T, R>(this Result<T> result, R defaultValue) where T : class, R
        {
            if (result.IsFailure || result.IsLoading) return defaultValue;
            return result.Value as T;
        }

    //        /**
    //         * Returns the result of [onSuccess] for the encapsulated value if this instance represents [success][Result.isSuccess]
    //         * or the result of [onFailure] function for the encapsulated [Throwable] exception if it is [failure][Result.isFailure].
    //         *
    //         * Note, that this function rethrows any [Throwable] exception thrown by [onSuccess] or by [onFailure] function.
    //         */
    //inline fun<R, T> Result<T>.fold(
    //onSuccess: (value: T)->R,
    //    onFailure: (exception: Throwable)->R,
    //    onLoading: ()->R
    //): R {
    //            contract {
    //                callsInPlace(onSuccess, InvocationKind.AT_MOST_ONCE)
    //                callsInPlace(onFailure, InvocationKind.AT_MOST_ONCE)
    //                callsInPlace(onLoading, InvocationKind.AT_MOST_ONCE)
    //            }
    //            val exception = exceptionOrNull()
    //    return when {
    //                exception == null && isLoading->onLoading()
    //        exception == null->onSuccess(value as T)
    //        else ->onFailure(exception)
    //    }
    //        }

    //        // transformation

    //        /**
    //         * Returns the encapsulated result of the given [transform] function applied to the encapsulated value
    //         * if this instance represents [success][Result.isSuccess] or the
    //         * original encapsulated [Throwable] exception if it is [failure][Result.isFailure].
    //         *
    //         * Note, that this function rethrows any [Throwable] exception thrown by [transform] function.
    //         * See [mapCatching] for an alternative that encapsulates exceptions.
    //         */
    //inline fun<R, T> Result<T>.map(transform: (value: T)->R): Result<R> {
    //            contract {
    //                callsInPlace(transform, InvocationKind.AT_MOST_ONCE)
    //            }
    //            return when {
    //                isSuccess->Result.success(transform(value as T))
    //        else ->Result(value)
    //            }
    //        }

    //        /**
    //         * Returns the encapsulated result of the given [transform] function applied to the encapsulated value
    //         * if this instance represents [success][Result.isSuccess] or the
    //         * original encapsulated [Throwable] exception if it is [failure][Result.isFailure].
    //         *
    //         * This function catches any [Throwable] exception thrown by [transform] function and encapsulates it as a failure.
    //         * See [map] for an alternative that rethrows exceptions from `transform` function.
    //         */
    //public inline fun<R, T> Result<T>.mapCatching(transform: (value: T)->R): Result<R> {
    //            return when {
    //                isSuccess->runCatching { transform(value as T) }
    //        else ->Result(value)
    //            }
    //        }

    //        /**
    //         * Returns the encapsulated result of the given [transform] function applied to the encapsulated [Throwable] exception
    //         * if this instance represents [failure][Result.isFailure] or the
    //         * original encapsulated value if it is [success][Result.isSuccess].
    //         *
    //         * Note, that this function rethrows any [Throwable] exception thrown by [transform] function.
    //         * See [recoverCatching] for an alternative that encapsulates exceptions.
    //         */
    //inline fun<R, T : R > Result<T>.recover(transform: (exception: Throwable)->R): Result<R> {
    //            contract {
    //                callsInPlace(transform, InvocationKind.AT_MOST_ONCE)
    //            }
    //            return when(val exception = exceptionOrNull()) {
    //                null-> this
    //        else ->Result.success(transform(exception))
    //           }
    //        }

    //        /**
    //         * Returns the encapsulated result of the given [transform] function applied to the encapsulated [Throwable] exception
    //         * if this instance represents [failure][Result.isFailure] or the
    //         * original encapsulated value if it is [success][Result.isSuccess].
    //         *
    //         * This function catches any [Throwable] exception thrown by [transform] function and encapsulates it as a failure.
    //         * See [recover] for an alternative that rethrows exceptions.
    //         */
    //public inline fun<R, T : R > Result<T>.recoverCatching(transform: (exception: Throwable)->R): Result<R> {
    //            val value = value // workaround for inline classes BE bug
    //    return when(val exception = exceptionOrNull()) {
    //                null-> this
    //        else ->runCatching { transform(exception) }
    //            }
    //        }

    //        // "peek" onto value/exception and pipe

    /**
     * Performs the given [action] on the encapsulated [Throwable] exception if this instance represents [failure][Result.isFailure].
     * Returns the original `Result` unchanged.
     */
    public static Result<T> OnFailure<T>(this Result<T> result, Action<Exception> action) where T : class
        {
            var e = result.ExceptionOrNull();
            if (e != null)
            {
                action.Invoke(e);
            }
            return result;
        }

        /**
         * Performs the given [action] on the encapsulated value if this instance represents [success][Result.isSuccess].
         * Returns the original `Result` unchanged.
         */
        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action) where T : class
        {
            if (result.IsSuccess) action.Invoke(result.Value as T);
            return result;
        }

        /**
         * Performs the given [action] if this instance represents [loading][Result.isLoading].
         * Returns the original `Result` unchanged.
         */
        public static Result<T> OnLoading<T>(this Result<T> result, Action action) where T : class
        {
            if (result.IsLoading) action();
            return result;
        }
    }
}
