using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballAIGame.AI.FSM.UserClasses
{
    
    class FiniteStateMachine<T>
    {
        public State<T> CurrentState { get; set; }

        public State<T> GlobalState { get; set; }

        public void Update()
        {
            
        }

    }
}
