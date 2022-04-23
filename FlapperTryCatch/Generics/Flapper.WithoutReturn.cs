namespace FlapperTryCatch.Generics;

public abstract partial class Flapper
{
    public abstract class WithoutReturn<TDelegate, TCatchHandler> : Flapper
        where TDelegate : Delegate
        where TCatchHandler : Delegate
    {
        protected readonly Dictionary<Type, TCatchHandler> catchHandlers;
        protected TDelegate execute;

        protected WithoutReturn(TDelegate execute)
        {
            catchHandlers = new();
            this.execute = execute;
        }

        //public static Flapper<TDelegate, TCatchHandler> Try(TDelegate @delegate)
        //{
        //    return new Flapper<TDelegate, TCatchHandler>(@delegate);
        //}

        public WithoutReturn<TDelegate, TCatchHandler> Catch<TException>(TCatchHandler handler) where TException : Exception
        {
            catchHandlers.Add(typeof(TException), handler);
            return this;
        }

        public virtual void Execute()
            => Execute(InjectInstability);

        protected void Execute(Action instabilityInjectionPoint)
        {
            try
            {
                instabilityInjectionPoint();
                execute.DynamicInvoke();
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

            handler.DynamicInvoke(ex);
            return true;
        }

        private static void InjectInstability()
        {
        }

        public static new class TestHelper
        {
            public static string AddInstability<TType>(string instabilityHandlerName)
                where TType : class
            {
                return Flapper.TestHelper
                    .AddInstability<WithoutReturn<TDelegate, TCatchHandler>, TType>(instabilityHandlerName);
            }
        }
    }
}