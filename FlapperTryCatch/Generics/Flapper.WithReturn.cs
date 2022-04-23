namespace FlapperTryCatch.Generics;

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

        protected WithReturn<TDelegate, TCatchHandler, TExecuteResult> Catch<TException>(TCatchHandler handler) where TException : Exception
        {
            catchHandlers.Add(typeof(TException), handler);
            return this;
        }

        public abstract TExecuteResult Execute();

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
            var exType = ex.GetType();
            if (catchHandlers.TryGetValue(exType, out var handler))
            {
                result = (TExecuteResult)handler.DynamicInvoke(Convert.ChangeType(ex, exType))!;
                return true;
            }

            result = default!;
            return false;
        }
    }
}