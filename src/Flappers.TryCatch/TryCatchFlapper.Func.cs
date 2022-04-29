namespace Flappers.TryCatch;

using Flappers.Core;

public class TryCatchFlapper<TResult> : Flapper<TResult>
{
    private readonly Dictionary<Type, Func<Exception, TResult>> catchHandlers;

    public TryCatchFlapper(Func<TResult> func)
        : base(func)
    {
        catchHandlers = new Dictionary<Type, Func<Exception, TResult>>();
    }

    public TryCatchFlapper<TResult> Catch<TException>(Func<TException, TResult> handler) where TException : Exception
    {
        catchHandlers.Add(typeof(TException), (ex) => handler((TException)ex));
        return this;
    }

    public override TResult Execute()
    {
        try
        {
            return InvokeExecution(base.Execute);
        }
        catch (Exception ex)
        {
            if (!TryGetHandler(ex, out var handler))
                throw;

            return InvokeHandler(ex, handler);
        }
    }

    private static TResult InvokeExecution(Func<TResult> func)
    {
        return func();
    }

    private static TResult InvokeHandler(Exception ex, Func<Exception, TResult> handler)
    {
      return handler(ex);
    }

    private bool TryGetHandler(Exception ex, out Func<Exception, TResult> handler)
    {
        return catchHandlers.TryGetValue(ex.GetType(), out handler!);
    }

    public static implicit operator TryCatchFlapper<TResult>(Func<TResult> func)
    {
        return new TryCatchFlapper<TResult>(func);
    }

    public static implicit operator Func<TResult>(TryCatchFlapper<TResult> flapper)
    {
        return flapper.Execute;
    }
}