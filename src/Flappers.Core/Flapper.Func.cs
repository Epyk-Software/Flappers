namespace Flappers.Core;

public class Flapper<TResult>
{
    private readonly Func<TResult> func;

    public Flapper(Func<TResult> func)
    {
        this.func = func ?? throw new ArgumentNullException(nameof(func));
    }

    public virtual TResult Execute()
    {
        return InvokeExecution(func);
    }

    private static TResult InvokeExecution(Func<TResult> func)
    {
        return func.Invoke();
    }

    public static implicit operator Flapper<TResult>(Func<TResult> func)
    {
        return new Flapper<TResult>(func);
    }

    public static implicit operator Func<TResult>(Flapper<TResult> flapper)
    {
        return flapper.Execute;
    }
}