namespace Flappers.Switch;

using System;

internal class FuncSwitchCase<TSwitchValueType> : ISwitchCase<TSwitchValueType> where TSwitchValueType : notnull
{
    private readonly Func<TSwitchValueType, bool> matchFunc;

    public FuncSwitchCase(Func<TSwitchValueType, bool> matchFunc, Action handler)
    {
        this.matchFunc = matchFunc;
        Handler = handler;
    }

    public bool SatisfiesCase(TSwitchValueType switchValue)
    {
        return matchFunc(switchValue);
    }

    public Action Handler { get; }
}