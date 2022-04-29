namespace Flappers.Switch;

using System;

internal class SwitchCase<TSwitchValueType, TResult> : ISwitchCase<TSwitchValueType, TResult> where TSwitchValueType : notnull
{
    private readonly TSwitchValueType matchValue;

    public SwitchCase(TSwitchValueType matchValue, Func<TResult> handler)
    {
        this.matchValue = matchValue;
        Handler = handler;
    }

    public bool SatisfiesCase(TSwitchValueType switchValue)
    {
        return switchValue.Equals(matchValue);
    }

    public Func<TResult> Handler { get; }
}