using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.AIs.Fsm.TeamStates;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    class DefendGoal : PlayerState
    {
        private Interpose Interpose { get; set; }

        public DefendGoal(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        public override void Enter()
        {
            var goalCenter = new Vector(0, GameClient.FieldHeight / 2);
            if (!AI.MyTeam.IsOnLeft)
                goalCenter.X = GameClient.FieldWidth;

            Interpose = new Interpose(Player, 1, 1.0, AI.Ball, goalCenter)
            {
                PreferredDistanceFromSecond = Parameters.DefendGoalDistance
            };

            Player.SteeringBehaviorsManager.AddBehavior(Interpose);
        }

        public override void Run()
        {
            if (AI.MyTeam.StateMachine.CurrentState is Defending &&
                Vector.DistanceBetween(AI.Ball.Position, AI.MyTeam.GoalCenter) < Parameters.GoalKeeperInterceptRange)
            {
                Player.StateMachine.ChangeState(new InterceptBall(Player, AI));
            }
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Interpose);
        }
    }
}
