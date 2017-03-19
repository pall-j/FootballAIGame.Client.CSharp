using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class FieldPlayerGlobalState : PlayerState
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public FieldPlayerGlobalState(Player player, FootballAI footballAI) : base(player, footballAI)
        {
            PlayerGlobalState = new PlayerGlobalState(player, footballAI);
        }

        public override void Run()
        {
            var team = AI.MyTeam;

            if (Player.CanKickBall(AI.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player, AI));
            }
            else if (team.NearestPlayerToBall == Player &&
                     team.PassReceiver == null)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player, AI));
            }


            PlayerGlobalState.Run();   
        }

        public override bool ProcessMessage(IMessage message)
        {
            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
