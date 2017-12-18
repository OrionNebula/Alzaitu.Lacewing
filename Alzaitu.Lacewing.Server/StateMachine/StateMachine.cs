namespace Alzaitu.Lacewing.Server.StateMachine
{
    internal class StateMachine<TContext>
    {
        public State<TContext> CurrentState { get; set; }

        public TContext Context { get; }

        public StateMachine(TContext context)
        {
            Context = context;
        }

        public void StepForward()
        {
            CurrentState = CurrentState.StepForward(Context);
        }

        public void Run(int? steps = null)
        {
            if(steps == null)
                while(CurrentState != null)
                    StepForward();

            for(var i = 0; i < steps && CurrentState != null; i++)
                StepForward();
        }
    }
}
