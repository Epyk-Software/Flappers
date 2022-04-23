namespace FlapperTryCatch;

using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;

internal static class FlapperTestHelper
{
    private const string InstabilityInjectionPointMethodName = "InjectInstability";
    private static readonly BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string AddInstability(Type flapperType, Type instabilityHandlerType, string instabilityMethodName)
    {
        var original = flapperType.GetMethod(InstabilityInjectionPointMethodName, bindingFlags);
        var instabilityHandler = instabilityHandlerType.GetMethod(instabilityMethodName, bindingFlags);

        return Memory.DetourMethod(original, instabilityHandler);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string RemoveInstability(Type flapperType, Type instabilityHandlerType, string instabilityMethodName)
    {
        var original = flapperType.GetMethod(InstabilityInjectionPointMethodName, bindingFlags);
        var instabilityHandler = instabilityHandlerType.GetMethod(instabilityMethodName, bindingFlags);

        return Memory.DetourMethod(instabilityHandler, original);
    }
}