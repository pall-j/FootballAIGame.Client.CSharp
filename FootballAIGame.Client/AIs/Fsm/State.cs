using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm
{
    abstract class State<TEntity>
    {
        protected TEntity Entity { get; set; }

        protected FsmAI AI { get; set; }

        protected State(TEntity entity, FsmAI footballAI)
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
