using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.TeamStates;

namespace FootballAIGame.AI.FSM.UserClasses
{
    
    class FiniteStateMachine<TEntity>
    {
        public State<TEntity> CurrentState { get; private set; }

        public State<TEntity> GlobalState { get; private set; }

        public TEntity Owner { get; set; }
        
        public FiniteStateMachine(TEntity owner, State<TEntity> startState, State<TEntity> globalState)
        {
            this.Owner = owner;
            this.CurrentState = startState;
            this.GlobalState = globalState;
        }

        public void ChangeState(State<TEntity> newState)
        {
            if (CurrentState != null)
                CurrentState.Exit(Owner);

            CurrentState = newState;
            CurrentState.Enter(Owner);
        }

        public void Update()
        {
            if (GlobalState != null)
                GlobalState.Run(Owner);
            if (CurrentState != null)
                CurrentState.Run(Owner);
        }

    }
}
