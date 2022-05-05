namespace Flappers.Pipeline;

using Flappers.Core;

public class PipelineFlapper : Flapper
{
    private readonly List<Action<Action>> operationStack;
    private readonly Action terminate;

    public PipelineFlapper(Action execute)
        : base(execute)
    {
        operationStack = new List<Action<Action>>();
        terminate = () => InvokeExecution(base.Execute);
    }

    public PipelineFlapper AddStage(Action<Action> stage)
    {
        ArgumentNullException.ThrowIfNull(stage);
        operationStack.Add(stage);
        return this;
    }

    public override void Execute()
    {
        List<Action> actions = new() { terminate };
        foreach (Action<Action> opteration in operationStack.Reverse<Action<Action>>())
        {
            var lastAction = actions[^1];
            var operationAction = () => InvokeOperation(opteration, lastAction);
            actions.Add(operationAction);
        }
        actions[^1]();
    }

    private void InvokeOperation(Action<Action> operation, Action next)
    {
        operation(next);
    }

    private static void InvokeExecution(Action action)
    {
        action();
    }

    public static implicit operator PipelineFlapper(Action action)
    {
        return new PipelineFlapper(action);
    }

    public static implicit operator Action(PipelineFlapper flapper)
    {
        return flapper.Execute;
    }
}