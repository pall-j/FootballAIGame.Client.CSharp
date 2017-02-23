using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class ForwardGlobalState : State<Player>
    {
        private FieldPlayerGlobalState FieldPlayerGlobalState { get; set; }

        public ForwardGlobalState(Player entity) : base(entity)
        {
            FieldPlayerGlobalState = new FieldPlayerGlobalState(entity);
        }

        public override void Run()
        {
            FieldPlayerGlobalState.Run();
        }

        public override bool ProcessMessage(Message message)
        {
            return FieldPlayerGlobalState.ProcessMessage(message);
        }
    }
}
