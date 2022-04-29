namespace Flappers.TryCatch;

using Flappers.Core;

public class TryCatchFlapper : Flapper
{
    private readonly Dictionary<Type, Action<Exception>> catchHandlers;

    public TryCatchFlapper(Action execute)
        : base(execute)
    {
        catchHandlers = new Dictionary<Type, Action<Exception>>();
    }

    public TryCatchFlapper Catch<TException>(Action<TException> handler) where TException : Exception
    {
        catchHandlers.Add(typeof(TException), (ex) => handler((TException)ex));
        return this;
    }

    public override void Execute()
    {
        try
        {
            InvokeExecution(base.Execute);
        }
        catch (Exception ex)
        {
            if (!TryGetHandler(ex, out var handler))
                throw;

            InvokeHandler(ex, handler);
        }
    }

    private static void InvokeExecution(Action action)
    {
        action();
    }

    private static void InvokeHandler(Exception ex, Action<Exception> handler)
    {
        handler(ex);
    }

    private bool TryGetHandler(Exception ex, out Action<Exception> handler)
    {
        return catchHandlers.TryGetValue(ex.GetType(), out handler!);
    }

    public static implicit operator TryCatchFlapper(Action action)
    {
        return new TryCatchFlapper(action);
    }

    public static implicit operator Action(TryCatchFlapper flapper)
    {
        return flapper.Execute;
    }
}