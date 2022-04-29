namespace Flappers.Switch;

using System;

internal interface ISwitchCase<TSwitchValueType, TResult> where TSwitchValueType : notnull
{
    bool SatisfiesCase(TSwitchValueType switchValue);
    Func<TResult> Handler { get; }
}