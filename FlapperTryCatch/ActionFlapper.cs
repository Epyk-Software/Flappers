using FlapperTryCatch.Generics;

namespace FlapperTryCatch
{
    public class ActionFlapper : Flapper.WithoutReturn<Action, Action<Exception>>
    {
        private ActionFlapper(Action action)
            :base(action)
        {
        }

        public static ActionFlapper Try(Action action)
        {
            return new ActionFlapper(action);
        }

        new public ActionFlapper Catch<TException>(Action<Exception> action) where TException : Exception
        {
            return (ActionFlapper) base.Catch<TException>(action);
        }

        public new void Execute()
        {
            Execute(InstabilityInjectionPoint);
        }

        private static void InstabilityInjectionPoint()
        {
        }
    }
}