using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.AIs.Fsm.TeamStates;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    class InterceptBall : PlayerState
    {
        private Pursuit BallPursuit { get; set; }

        public InterceptBall(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        public override void Enter()
        {
            BallPursuit = new Pursuit(Player, 1, 1.0, AI.Ball);
            Player.SteeringBehaviorsManager.AddBehavior(BallPursuit);
        }

        public override void Run()
        {
            if (AI.MyTeam.StateMachine.CurrentState is Attacking ||
                Vector.DistanceBetween(Player.Position, AI.MyTeam.GoalCenter) >
                Parameters.GoalKeeperInterceptRange)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player, AI));
            }
        }

        public override void Exit()
        {
           Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
