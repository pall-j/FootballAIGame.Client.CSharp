using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;
using FootballAIGame.AI.FSM.UserClasses.TeamStates;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class InterceptBall : PlayerState
    {
        private Pursuit BallPursuit { get; set; }

        public InterceptBall(Player player, Ai ai) : base(player, ai)
        {
        }

        public override void Enter()
        {
            BallPursuit = new Pursuit(Player, 1, 1.0, Ai.Ball);
            Player.SteeringBehaviorsManager.AddBehavior(BallPursuit);
        }

        public override void Run()
        {
            if (Ai.MyTeam.StateMachine.CurrentState is Attacking ||
                Vector.DistanceBetween(Player.Position, Ai.MyTeam.GoalCenter) >
                Parameters.GoalKeeperInterceptRange)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player, Ai));
            }
        }

        public override void Exit()
        {
           Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
