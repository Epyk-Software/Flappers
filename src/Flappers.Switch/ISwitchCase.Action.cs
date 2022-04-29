namespace Flappers.Switch;

using System;

internal interface ISwitchCase<TSwitchValueType> where TSwitchValueType : notnull
{
    bool SatisfiesCase(TSwitchValueType switchValue);
    Action Handler { get; }
}