namespace Flappers.Core.TestHelpers;

using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;

public static class FlappersTestHelper
{
    public const string DefaultInstabilityMethodName = "InvokeExecution";

    private static readonly BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string AddInstability(Type flapperType, Type instabilityHandlerType, string instabilityMethodName = DefaultInstabilityMethodName)
    {
        var original = flapperType.GetMethod(instabilityMethodName, bindingFlags);
        var instabilityHandler = instabilityHandlerType.GetMethod(instabilityMethodName, bindingFlags);

        return Memory.DetourMethod(original, instabilityHandler);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string RemoveInstability(Type flapperType, Type instabilityHandlerType, string instabilityMethodName = DefaultInstabilityMethodName)
    {
        var original = flapperType.GetMethod(instabilityMethodName, bindingFlags);
        var instabilityHandler = instabilityHandlerType.GetMethod(instabilityMethodName, bindingFlags);

        return Memory.DetourMethod(instabilityHandler, original);
    }
}