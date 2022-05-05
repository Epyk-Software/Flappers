namespace Flappers.Pipeline;

public static class FuncExtensions
{
    public static PipelineFlapper<TResult> AddStage<TResult>(this Func<TResult> func, Func<Func<TResult>, TResult> stage)
    {
        PipelineFlapper<TResult> flapper = func;
        return flapper.AddStage(stage);
    }
}