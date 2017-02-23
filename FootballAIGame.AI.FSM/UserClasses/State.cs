using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses
{
    abstract class State<TEntity>
    {
        protected TEntity Entity { get; set; }

        protected State(TEntity entity)
        {
            Entity = entity;
        }

        public virtual void Enter() { }

        public abstract void Run();

        public virtual void Exit() { }

        public virtual bool ProcessMessage(Message message)
        {
            return false;
        }
    }
}
