namespace FlapperTryCatch
{
    public class Flapper<TExecuteResult>
    {
        protected readonly Dictionary<Type, Func<Exception, TExecuteResult>> catchHandlers;
        protected Func<TExecuteResult> execute;

        private Flapper(Func<TExecuteResult> execute)
        {
            catchHandlers = new();
            this.execute = execute;
        }
        public static Flapper<TExecuteResult> Try(Func<TExecuteResult> execute)
        {
            return new Flapper<TExecuteResult>(execute);
        }

        public Flapper<TExecuteResult> Catch<TException>(Func<TException, TExecuteResult> handler) where TException : Exception
        {
            catchHandlers.Add(typeof(TException), ex => handler((TException)ex));
            return this;
        }

        public TExecuteResult Execute()
            => Execute(InjectInstability);

        protected TExecuteResult Execute(Action instabilityInjectionPoint)
        {
            try
            {
                instabilityInjectionPoint();
                return execute();
            }
            catch (Exception ex)
            {
                if (TryHandle(ex, out TExecuteResult result))
                    return result;

                throw;
            }
        }

        private bool TryHandle(Exception ex, out TExecuteResult result)
        {
            if (catchHandlers.TryGetValue(ex.GetType(), out var handler))
            {
                result = handler(ex);
                return true;
            }

            result = default!;
            return false;
        }

        private static void InjectInstability()
        {
        }

        public static class TestHelper
        {
            private static readonly Type flapperType = typeof(Flapper<TExecuteResult>);
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
