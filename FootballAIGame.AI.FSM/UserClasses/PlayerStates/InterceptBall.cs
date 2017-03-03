using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;
using FootballAIGame.AI.FSM.UserClasses.TeamStates;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class InterceptBall : PlayerState
    {
        private Pursuit BallPursuit { get; set; }

        public InterceptBall(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            BallPursuit = new Pursuit(Player, 1, 1.0, Ai.Instance.Ball);
            Player.SteeringBehaviorsManager.AddBehavior(BallPursuit);
        }

        public override void Run()
        {
            if (Ai.Instance.MyTeam.StateMachine.CurrentState is Attacking ||
                Vector.DistanceBetween(Player.Position, Ai.Instance.MyTeam.GoalCenter) >
                Parameters.GoalKeeperInterceptRange)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player));
            }
        }

        public override void Exit()
        {
           Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
