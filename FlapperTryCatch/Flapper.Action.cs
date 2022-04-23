using HarmonyLib;
using System.Reflection;

namespace FlapperTryCatch
{
    public class Flapper
    {
        protected readonly Dictionary<Type, Action<Exception>> catchHandlers;
        protected Action execute;

        protected Flapper(Action execute)
        {
            catchHandlers = new();
            this.execute = execute;
        }

        public static Flapper Try(Action execute)
        {
            return new Flapper(execute);
        }

        public Flapper Catch<TException>(Action<TException> handler) where TException : Exception
        {
            catchHandlers.Add(typeof(TException), (ex) => handler((TException)ex));
            return this;
        }

        public virtual void Execute()
            => Execute(InjectInstability);

        protected void Execute(Action instabilityInjectionPoint)
        {
            try
            {
                instabilityInjectionPoint();
                execute();
            }
            catch (Exception ex)
            {
                if (!TryHandle(ex))
                    throw;
            }
        }

        protected bool TryHandle(Exception ex)
        {
            if (!catchHandlers.TryGetValue(ex.GetType(), out var handler))
            {
                return false;
            }

            handler(ex);
            return true;
        }

        private static void InjectInstability()
        {
        }

        public static class TestHelper
        {
            private static readonly Type flapperType = typeof(Flapper);
            public static string AddInstability<TType>(string instabilityHandlerName)
                where TType : class
            {
                return FlapperTestHelper.AddInstability(flapperType, typeof(TType), instabilityHandlerName);
            }
            public static string RemoveInstability<TType>(string instabilityHandlerName)
                where TType : class
            {
                return FlapperTestHelper.RemoveInstability(flapperType, typeof(TType), instabilityHandlerName);
            }
        }
    }
}