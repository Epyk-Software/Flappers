namespace FlapperTryCatch.Generics
{
    public abstract partial class Flapper
    {
        public abstract class WithReturn<TDelegate, TCatchHandler, TExecuteResult> : Flapper
            where TDelegate : Delegate
            where TCatchHandler : Delegate

        {
            protected readonly Dictionary<Type, TCatchHandler> catchHandlers;
            protected TDelegate execute;

            protected WithReturn(TDelegate execute)
            {
                catchHandlers = new();
                this.execute = execute;
            }

            protected new WithReturn<TDelegate, TCatchHandler, TExecuteResult> Catch<TException>(TCatchHandler handler) where TException : Exception
            {
                catchHandlers.Add(typeof(TException), handler);
                return this;
            }

            public TExecuteResult Execute()
                => Execute(InjectInstability);

            protected TExecuteResult Execute(Action instabilityInjectionPoint)
            {
                try
                {
                    instabilityInjectionPoint();
                    return (TExecuteResult)execute.DynamicInvoke()!;
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
                    result = (TExecuteResult)handler.DynamicInvoke(ex)!;
                    return true;
                }

                result = default!;
                return false;
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
                        .AddInstability<WithReturn<TDelegate, TCatchHandler, TExecuteResult>, TType>(instabilityHandlerName);
                }
            }
        }
    }
}