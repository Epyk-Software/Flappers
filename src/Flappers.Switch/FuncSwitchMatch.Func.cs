namespace Flappers.Switch;

using System;

internal class FuncSwitchCase<TSwitchValueType, TResult> : ISwitchCase<TSwitchValueType, TResult> where TSwitchValueType : notnull
{
    private readonly Func<TSwitchValueType, bool> matchFunc;

    public FuncSwitchCase(Func<TSwitchValueType, bool> matchFunc, Func<TResult> handler)
    {
        this.matchFunc = matchFunc;
        Handler = handler;
    }

    public bool SatisfiesCase(TSwitchValueType switchValue)
    {
        return matchFunc(switchValue);
    }

    public Func<TResult> Handler { get; }
}