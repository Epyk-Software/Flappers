namespace Flappers.Pipeline;

using Flappers.Core;

public class PipelineFlapper<TResult> : Flapper<TResult>
{
    private readonly List<Func<Func<TResult>, TResult>> operationStack;
    private readonly Func<TResult> terminate;

    public PipelineFlapper(Func<TResult> func)
        : base(func)
    {
        operationStack = new List<Func<Func<TResult>, TResult>>();
        terminate = () => InvokeExecution(base.Execute);
    }

    public PipelineFlapper<TResult> AddStage(Func<Func<TResult>, TResult> stage)
    {
        ArgumentNullException.ThrowIfNull(stage);
        operationStack.Add(stage);
        return this;
    }

    public override TResult Execute()
    {
        List<Func<TResult>> actions = new() { terminate };
        foreach (var opteration in operationStack.Reverse<Func<Func<TResult>, TResult>>())
        {
            var lastAction = actions[^1];
            Func<TResult> operationAction = () => InvokeOperation(opteration, lastAction);
            actions.Add(operationAction);
        }
        return actions[^1]();
    }

    private TResult InvokeOperation(Func<Func<TResult>, TResult> operation, Func<TResult> next)
    {
        return operation(next);
    }

    private static TResult InvokeExecution(Func<TResult> func)
    {
        return func();
    }

    public static implicit operator PipelineFlapper<TResult>(Func<TResult> func)
    {
        return new PipelineFlapper<TResult>(func);
    }

    public static implicit operator Func<TResult>(PipelineFlapper<TResult> flapper)
    {
        return flapper.Execute;
    }
}