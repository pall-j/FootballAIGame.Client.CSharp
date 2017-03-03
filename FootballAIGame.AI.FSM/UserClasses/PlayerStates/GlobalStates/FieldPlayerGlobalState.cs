using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class FieldPlayerGlobalState : PlayerState
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public FieldPlayerGlobalState(Player player) : base(player)
        {
            PlayerGlobalState = new PlayerGlobalState(player);
        }

        public override void Run()
        {
            var team = Ai.Instance.MyTeam;

            if (team.NearestPlayerToBall == Player &&
                     team.PassReceiver == null)
            {
                //if (Player is GoalKeeper)
                //    Console.WriteLine("State change: Default -> PursueBall");
                Player.StateMachine.ChangeState(new PursueBall(Player));
            }


            PlayerGlobalState.Run();   
        }

        public override bool ProcessMessage(Message message)
        {
            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
