using HarmonyLib;
using System.Reflection;

namespace FlapperTryCatch
{
    public partial class Flapper
    {
        private readonly Dictionary<Type, Action> catchHandlers;
        private readonly Action action;

        private Flapper(Action action)
        {
            catchHandlers = new();
            this.action = action;
        }

        public static Flapper Try(Action action)
        {
            return new Flapper(action);
        }

        public Flapper Catch<TException>(TException exception, Action action) where TException : Exception
        {
            catchHandlers.Add(typeof(TException), action);
            return this;
        }

        public void Execute()
        {
            try
            {
                InstabilityInjectionPoint();
                action();
            }
            catch (Exception ex)
            {
                if (!TryHandle(ex))
                    throw;
            }
        }

        private bool TryHandle(Exception ex)
        {
            if (!catchHandlers.TryGetValue(ex.GetType(), out var handler))
            {
                return false;
            }

            handler();
            return true;
        }

        private static void InstabilityInjectionPoint()
        {
        }
    }
}