namespace Flappers.Core;

public class Flapper
{
    private readonly Action action;

    public Flapper(Action action)
    {
        this.action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public virtual void Execute()
    {
        InvokeExecution(action);
    }

    private static void InvokeExecution(Action action)
    {
        action.Invoke();
    }

    public static implicit operator Flapper(Action action)
    {
        return new Flapper(action);
    }

    public static implicit operator Action(Flapper flapper)
    {
        return flapper.Execute;
    }
}