namespace Flappers.Switch;

using System;

internal class SwitchCase<TSwitchValueType> : ISwitchCase<TSwitchValueType> where TSwitchValueType : notnull
{
    private readonly TSwitchValueType matchValue;

    public SwitchCase(TSwitchValueType matchValue, Action handler)
    {
        this.matchValue = matchValue;
        Handler = handler;
    }

    public bool SatisfiesCase(TSwitchValueType switchValue)
    {
        return switchValue.Equals(matchValue);
    }

    public Action Handler { get; }
}