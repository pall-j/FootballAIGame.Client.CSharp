using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class ForwardGlobalState : PlayerState
    {
        private FieldPlayerGlobalState FieldPlayerGlobalState { get; set; }

        public ForwardGlobalState(Player player) : base(player)
        {
            FieldPlayerGlobalState = new FieldPlayerGlobalState(player);
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
