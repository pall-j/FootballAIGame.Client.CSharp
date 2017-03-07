using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;
using FootballAIGame.AI.FSM.UserClasses.TeamStates;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class DefendGoal : PlayerState
    {
        private Interpose Interpose { get; set; }

        public DefendGoal(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            var goalCenter = new Vector(0, GameClient.FieldHeight / 2);
            if (!Ai.Instance.MyTeam.IsOnLeft)
                goalCenter.X = GameClient.FieldWidth;

            Interpose = new Interpose(Player, 1, 1.0, Ai.Instance.Ball, goalCenter)
            {
                PreferredDistanceFromSecond = Parameters.DefendGoalDistance
            };

            Player.SteeringBehaviorsManager.AddBehavior(Interpose);
        }

        public override void Run()
        {
            if (Ai.Instance.MyTeam.StateMachine.CurrentState is Defending &&
                Vector.DistanceBetween(Ai.Instance.Ball.Position, Ai.Instance.MyTeam.GoalCenter) < Parameters.GoalKeeperInterceptRange)
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player));
            }
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Interpose);
        }
    }
}
