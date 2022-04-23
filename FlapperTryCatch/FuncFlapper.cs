using FlapperTryCatch.Generics;

namespace FlapperTryCatch
{
    public class FuncFlapper<TExecutionResult> : Flapper.WithReturn<Action, Action<Exception>, TExecutionResult>
    {
        private FuncFlapper(Action action)
            : base(action)
        {
        }

        public static FuncFlapper<TExecutionResult> Try(Action action)
        {
            return new FuncFlapper<TExecutionResult>(action);
        }

        new public FuncFlapper<TExecutionResult> Catch<TException>(Action<Exception> action) where TException : Exception
        {
            return (FuncFlapper<TExecutionResult>) base.Catch<TException>(action);
        }

        new public TExecutionResult Execute()
        {
            return Execute(InstabilityInjectionPoint);
        }

        private static void InstabilityInjectionPoint()
        {
        }
    }
}