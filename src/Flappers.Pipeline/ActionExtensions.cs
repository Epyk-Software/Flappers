namespace Flappers.Pipeline;

public static class ActionExtensions
{
    public static PipelineFlapper AddStage<TException>(this Action action, Action<Action> stage)
    {
        PipelineFlapper flapper = action;
        return flapper.AddStage(stage);
    }
}