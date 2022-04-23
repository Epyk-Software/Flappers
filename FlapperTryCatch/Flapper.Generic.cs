using HarmonyLib;
using System.Reflection;

namespace FlapperTryCatch
{
    public partial class Flapper<TResult>
    {
        private readonly Dictionary<Type, Func<Exception, TResult>> catchHandlers;
        private Func<TResult> func;

        internal Flapper(Func<TResult> func)
        {
            catchHandlers = new();
            this.func = func;
        }
        public static Flapper<TResult> Try(Func<TResult> func)
        {
            return new Flapper<TResult>(func);
        }

        public Flapper<TResult> Catch<TException>(Func<TException, TResult> func) where TException : Exception
        {
            catchHandlers.Add(typeof(TException), ex => func((TException)ex));
            return this;
        }

        public TResult Execute()
        {
            try
            {
                InjectInstability();
                return func();
            }
            catch (Exception ex)
            {
                if (!TryHandle(ex, out TResult result))
                    throw;

                return result;
            }
        }

        private bool TryHandle(Exception ex, out TResult result)
        {
            if (!catchHandlers.TryGetValue(ex.GetType(), out var handler))
            {
                result = default;
                return false;
            }

            result = handler(ex);
            return true;
        }

        private static void InjectInstability()
        {
        }
    }
}