namespace FlapperTryCatch;

public class Flapper
    : Generics.Flapper.WithoutReturn<Action, Action<Exception>>
{
    private Flapper(Action action)
        : base(action) { }

    public static Flapper Try(Action action)
    {
        return new Flapper(action);
    }

    new public Flapper Catch<TException>(Action<Exception> action) where TException : Exception
    {
        return (Flapper)base.Catch<TException>(action);
    }

    public override void Execute()
    {
        Execute(InstabilityInjectionPoint);
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
                .AddInstability<Flapper, TType>(instabilityHandlerName);
        }

        public static string RemoveInstability<TType>(string instabilityHandlerName)
            where TType : class
        {
            return Generics.Flapper.TestHelper
                .RemoveInstability<Flapper, TType>(instabilityHandlerName);
        }
    }
}