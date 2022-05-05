namespace Flappers.Pipeline.Tests;

public class PipelineFlapperFuncTests
{
    [Fact]
    public void PipelineFlapper_Throws_WhenExecuteThrows()
    {
        // given
        Func<object> execute = () => throw new NotImplementedException();
        PipelineFlapper<object> flapper = new PipelineFlapper<object>(execute);

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.IsType<NotImplementedException>(exception);
    }

    [Fact]
    public void PipelineFlapper_AddingNullStage_ThrowsArgumentNullException()
    {
        // given
        Func<object> execute = () => new { };
        PipelineFlapper<object> flapper = new PipelineFlapper<object>(execute);
        Func<Func<object>, object> stage = null;

        // when

        // then
        Assert.Throws<ArgumentNullException>(() => flapper.AddStage(stage));
    }

    [Fact]
    public void PipelineFlapper_Execute_CallsOriginalExecute()
    {
        // given
        bool executeCalled = false;
        Func<object> execute = () => { executeCalled = true; return new { }; };

        PipelineFlapper<object> flapper = new PipelineFlapper<object>(execute);

        // when
        flapper.Execute();

        // then
        Assert.True(executeCalled);
    }

    [Fact]
    public void PipelineFlapper_Execute_CallsPipelineStageThenOriginalExecute()
    {
        // given
        bool executeCalled = false;
        Func<object> execute = () => { executeCalled = true; ; return new { }; };
        bool stageCalled = false;
        Func<Func<object>, object> stage = (next) => { stageCalled = true; return next(); };

        PipelineFlapper<object> flapper = new PipelineFlapper<object>(execute);
        flapper.AddStage(stage);

        // when
        flapper.Execute();

        // then
        Assert.True(stageCalled);
        Assert.True(executeCalled);
    }

    [Fact]
    public void PipelineFlapper_Execute_CallsRegisteredPipelineStagesAndOriginalExecute()
    {
        // given
        List<int> expectedStagesExecuted = new List<int>() { 3, 2, 5, 4, 1 };
        List<int> actualStagesExecuted = new List<int>();

        PipelineFlapper<object> flapper = new PipelineFlapper<object>(() => { actualStagesExecuted.Add(1); return new { }; });
        flapper.AddStage((next) => { actualStagesExecuted.Add(3); return next(); });
        flapper.AddStage((next) => { actualStagesExecuted.Add(2); return next(); });
        flapper.AddStage((next) => { actualStagesExecuted.Add(5); return next(); });
        flapper.AddStage((next) => { actualStagesExecuted.Add(4); return next(); });

        // when
        flapper.Execute();

        // then
        Assert.Equal(expectedStagesExecuted, actualStagesExecuted);
    }
}