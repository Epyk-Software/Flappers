namespace Flappers.Switch;

using Flappers.Core;
using System;

public class SwitchFlapper<TSwitchValueType> : Flapper where TSwitchValueType : notnull
{
    private readonly List<ISwitchCase<TSwitchValueType>> handlers;
    private readonly TSwitchValueType switchOnValue;

    public SwitchFlapper(TSwitchValueType switchOnValue, Action? defaultHandler = null)
        : base(defaultHandler ?? NoOpDefaultHandler)
    {
        handlers = new List<ISwitchCase<TSwitchValueType>>();
        this.switchOnValue = switchOnValue;
    }

    public SwitchFlapper<TSwitchValueType> Case(TSwitchValueType matchValue, Action handler)
    {
        handlers.Add(new SwitchCase<TSwitchValueType>(matchValue, handler));
        return this;
    }

    public SwitchFlapper<TSwitchValueType> Case(Func<TSwitchValueType, bool> matchFunc, Action handler)
    {
        handlers.Add(new FuncSwitchCase<TSwitchValueType>(matchFunc, handler));
        return this;
    }

    public override void Execute()
    {
        if (!TryGetHandler(switchOnValue, out var handler))
        {
            InvokeExecution(base.Execute);
        }

        handler();
    }

    private static void InvokeExecution(Action action)
    {
        action();
    }

    private bool TryGetHandler(TSwitchValueType switchValue, out Action handler)
    {
        var potentialHandler = handlers.FirstOrDefault(h => h.SatisfiesCase(switchValue));
        if (potentialHandler == null)
        {
            handler = default!;
            return false;
        }

        handler = potentialHandler.Handler;
        return true;
    }

    private static void NoOpDefaultHandler() { }
}