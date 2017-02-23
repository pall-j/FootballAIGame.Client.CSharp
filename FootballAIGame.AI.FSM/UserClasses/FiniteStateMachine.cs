﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
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

            this.GlobalState.Enter();
        }

        public void ChangeState(State<TEntity> newState)
        {
            if (CurrentState != null)
                CurrentState.Exit();

            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Update()
        {
            if (GlobalState != null)
                GlobalState.Run();
            if (CurrentState != null)
                CurrentState.Run();
        }

        public void ProcessMessage(TEntity entity, Message message)
        {
            if (CurrentState != null && CurrentState.ProcessMessage(message))
                return;

            if (GlobalState != null)
                GlobalState.ProcessMessage(message);
        }
    }
}
