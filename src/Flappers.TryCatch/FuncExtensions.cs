namespace Flappers.TryCatch;

public static class FuncExtensions
{
    public static TryCatchFlapper<TResult> Catch<TException, TResult>(this Func<TResult> func, Func<TException, TResult> handler)
         where TException : Exception
    {
        TryCatchFlapper<TResult> flapper = func;
        return flapper.Catch(handler);
    }
}