namespace FlapperTryCatch;

public class Flapper<TExecutionResult>
    : Generics.Flapper.WithReturn<Func<TExecutionResult>, Action<Exception>, TExecutionResult>
{
    private Flapper(Func<TExecutionResult> action)
        : base(action) { }

    public static Flapper<TExecutionResult> Try(Func<TExecutionResult> action)
    {
        return new Flapper<TExecutionResult>(action);
    }

    public Flapper<TExecutionResult> Catch<TException>(Action<TException> action) where TException : Exception
    {
        return (Flapper<TExecutionResult>) base.Catch<TException>(ex => action((TException)ex));
    }

    public override TExecutionResult Execute()
    {
        return Execute(InstabilityInjectionPoint);
    }

    private static void InstabilityInjectionPoint()
    {
    }

    public static new class TestHelper
    {
        public static string AddInstability<TType>(string instabilityHandlerName)
            where TType : class
        {
            return Generics.Flapper.TestHelper
                .AddInstability<Flapper<TExecutionResult>, TType>(instabilityHandlerName);
        }

        public static string RemoveInstability<TType>(string instabilityHandlerName)
            where TType : class
        {
            return Generics.Flapper.TestHelper
                .RemoveInstability<Flapper<TExecutionResult>, TType>(instabilityHandlerName);
        }
    }
}