namespace Flappers.Switch;

using Flappers.Core;
using System;

public class SwitchFlapper<TSwitchValueType, TResult> : Flapper<TResult> where TSwitchValueType : notnull
{
    private readonly List<ISwitchCase<TSwitchValueType, TResult>> cases;
    private readonly TSwitchValueType switchOnValue;

    public SwitchFlapper(TSwitchValueType switchOnValue, Func<TResult>? defaultHandler = null)
        : base(defaultHandler ?? NoOpDefaultHandler)
    {
        cases = new List<ISwitchCase<TSwitchValueType, TResult>>();
        this.switchOnValue = switchOnValue;
    }

    public SwitchFlapper<TSwitchValueType, TResult> Case(TSwitchValueType matchValue, Func<TResult> handler)
    {
        cases.Add(new SwitchCase<TSwitchValueType, TResult>(matchValue, handler));
        return this;
    }

    public SwitchFlapper<TSwitchValueType, TResult> Case(Func<TSwitchValueType, bool> matchFunc, Func<TResult> handler)
    {
        cases.Add(new FuncSwitchCase<TSwitchValueType, TResult>(matchFunc, handler));
        return this;
    }

    public override TResult Execute()
    {
        if (!TryGetHandler(switchOnValue, out var handler))
        {
            return InvokeExecution(base.Execute);
        }

        return handler();
    }

    private static TResult InvokeExecution(Func<TResult> action)
    {
        return action();
    }

    private bool TryGetHandler(TSwitchValueType switchValue, out Func<TResult> handler)
    {
        var @case = cases.FirstOrDefault(h => h.SatisfiesCase(switchValue));
        if (@case == null)
        {
            handler = default!;
            return false;
        }

        handler = @case.Handler;
        return true;
    }

    private static TResult NoOpDefaultHandler() { return default!; }
}