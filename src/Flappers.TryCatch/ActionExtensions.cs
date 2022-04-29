namespace Flappers.TryCatch;

public static class ActionExtensions
{
    public static TryCatchFlapper Catch<TException>(this Action action, Action<TException> handler)
         where TException : Exception
    {
        TryCatchFlapper flapper = action;
        return flapper.Catch(handler);
    }
}