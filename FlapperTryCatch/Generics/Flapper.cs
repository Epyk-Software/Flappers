using HarmonyLib;
using System.Reflection;

namespace FlapperTryCatch.Generics;

public abstract partial class Flapper
{
    public static class TestHelper
    {
        private const string InjectionPoint = "InjectInstability";
        private static readonly BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

        public static string AddInstability<TFlapper, TType>(string instabilityHandlerName)
            where TType : class
            where TFlapper : Flapper
        {
            var original = typeof(TFlapper).GetMethod(InjectionPoint, bindingFlags);
            var instabilityHandler = typeof(TType).GetMethod(instabilityHandlerName, bindingFlags);

            return Memory.DetourMethod(original, instabilityHandler);
        }
    }
}