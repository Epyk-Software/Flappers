namespace Flappers.Pipeline.Tests;

public class PipelineFlapperTests
{
    [Fact]
    public void PipelineFlapper_Throws_WhenExecuteThrows()
    {
        // given
        Action execute = () => throw new NotImplementedException();
        PipelineFlapper flapper = new PipelineFlapper(execute);

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.IsType<NotImplementedException>(exception);
    }

    [Fact]
    public void PipelineFlapper_AddingNullStage_ThrowsArgumentNullException()
    {
        // given
        Action execute = () => { };
        PipelineFlapper flapper = new PipelineFlapper(execute);
        Action<Action> stage = null;

        // when

        // then
        Assert.Throws<ArgumentNullException>(() => flapper.AddStage(stage));
    }

    [Fact]
    public void PipelineFlapper_Execute_CallsOriginalExecute()
    {
        // given
        bool executeCalled = false;
        Action execute = () => { executeCalled = true; };

        PipelineFlapper flapper = new PipelineFlapper(execute);

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
        Action execute = () => { executeCalled = true; };
        bool stageCalled = false;
        Action<Action> stage = (next) => { stageCalled = true; next(); };

        PipelineFlapper flapper = new PipelineFlapper(execute);
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

        PipelineFlapper flapper = new PipelineFlapper(() => actualStagesExecuted.Add(1));
        flapper.AddStage((next) => { actualStagesExecuted.Add(3); next(); });
        flapper.AddStage((next) => { actualStagesExecuted.Add(2); next(); });
        flapper.AddStage((next) => { actualStagesExecuted.Add(5); next(); });
        flapper.AddStage((next) => { actualStagesExecuted.Add(4); next(); });

        // when
        flapper.Execute();

        // then
        Assert.Equal(expectedStagesExecuted, actualStagesExecuted);
    }
}