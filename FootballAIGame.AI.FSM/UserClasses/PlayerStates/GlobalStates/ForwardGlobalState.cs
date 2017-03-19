using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class ForwardGlobalState : PlayerState
    {
        private FieldPlayerGlobalState FieldPlayerGlobalState { get; set; }

        public ForwardGlobalState(Player player, FootballAI footballAI) : base(player, footballAI)
        {
            FieldPlayerGlobalState = new FieldPlayerGlobalState(player, footballAI);
        }

        public override void Run()
        {
            FieldPlayerGlobalState.Run();
        }

        public override bool ProcessMessage(IMessage message)
        {
            return FieldPlayerGlobalState.ProcessMessage(message);
        }
    }
}
