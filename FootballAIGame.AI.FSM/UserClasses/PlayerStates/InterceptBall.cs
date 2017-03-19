using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;
using FootballAIGame.AI.FSM.UserClasses.TeamStates;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class InterceptBall : PlayerState
    {
        private Pursuit BallPursuit { get; set; }

        public InterceptBall(Player player, FootballAI footballAI) : base(player, footballAI)
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
