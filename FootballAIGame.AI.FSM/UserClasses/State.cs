using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses
{
    abstract class State<TEntity>
    {
        protected TEntity Entity { get; set; }

        protected FootballAI AI { get; set; }

        protected State(TEntity entity, FootballAI footballAI)
        {
            Entity = entity;
            AI = footballAI;
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
