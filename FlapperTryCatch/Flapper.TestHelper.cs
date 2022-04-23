using HarmonyLib;
using System.Reflection;

namespace FlapperTryCatch;

public partial class Flapper
{
    public static class FlapperTestHelper
    {
        private const string InjectionPoint = "InjectInstability";
        private static readonly BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

        public static string AddInstability<TType>(string instabilityHandlerName)
            where TType : class
        {
            var original = typeof(Flapper).GetMethod(InjectionPoint, bindingFlags);
            var instabilityHandler = typeof(TType).GetMethod(instabilityHandlerName, bindingFlags);

            string result = Memory.DetourMethod(original, instabilityHandler);
            return result;
        }
    }
}