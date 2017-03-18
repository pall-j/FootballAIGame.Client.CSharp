using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses
{
    abstract class State<TEntity>
    {
        protected TEntity Entity { get; set; }

        protected Ai Ai { get; set; }

        protected State(TEntity entity, Ai ai)
        {
            Entity = entity;
            Ai = ai;
        }

        public virtual void Enter() { }

        public abstract void Run();

        public virtual void Exit() { }

        public virtual bool ProcessMessage(IMessage message)
        {
            return false;
        }
    }
}
