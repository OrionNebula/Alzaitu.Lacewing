using System;
using System.Collections.Generic;
using System.Text;

namespace Alzaitu.Lacewing.Server.StateMachine
{
    public abstract class State<TContext>
    {
        public abstract State<TContext> StepForward(TContext context);
    }
}
