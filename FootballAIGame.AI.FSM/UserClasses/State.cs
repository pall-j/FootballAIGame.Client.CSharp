using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses
{
    abstract class State<TEntity>
    {
        public virtual void Enter(TEntity entity) { }

        public abstract void Run(TEntity entity);

        public virtual void Exit(TEntity entity) { }

        public abstract bool ProcessMessage(Message message);
    }
}
