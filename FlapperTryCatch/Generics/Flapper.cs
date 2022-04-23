namespace FlapperTryCatch.Generics;
using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;

public abstract partial class Flapper
{
    private static void InstabilityInjectionPoint()
    {
    }

    public static class TestHelper
    {
        private const string InjectionPoint = "InstabilityInjectionPoint";
        private static readonly BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddInstability(Type flapperType, Type replacingType, string instabilityHandlerName)
        {
            var original = flapperType.GetMethod(InjectionPoint, bindingFlags);
            var instabilityHandler = replacingType.GetMethod(instabilityHandlerName, bindingFlags);

            return Memory.DetourMethod(original, instabilityHandler);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddInstability<TFlapper, TReplacingType>(string instabilityHandlerName)
        {
            return AddInstability(typeof(TFlapper), typeof(TReplacingType), instabilityHandlerName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveInstability(Type flapperType, Type replacingType, string instabilityHandlerName)
        {
            var original = flapperType.GetMethod(InjectionPoint, bindingFlags);
            var instabilityHandler = replacingType.GetMethod(instabilityHandlerName, bindingFlags);

            return Memory.DetourMethod(instabilityHandler, original);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveInstability<TFlapper, TReplacingType>(string instabilityHandlerName)
        {
            return RemoveInstability(typeof(TFlapper), typeof(TReplacingType), instabilityHandlerName);
        }
    }
}