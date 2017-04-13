using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates.GlobalStates
{
    class MidfielderGlobalState : PlayerState
    {
        private FieldPlayerGlobalState FieldPlayerGlobalState { get; set; }

        public MidfielderGlobalState(Player player, FsmAI footballAI) : base(player, footballAI)
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
