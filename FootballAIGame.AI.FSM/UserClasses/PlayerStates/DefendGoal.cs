using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

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
                DistanceFromSecond = Parameters.DefendGoalDistance
            };

            Player.SteeringBehaviorsManager.AddBehavior(Interpose);
        }

        public override void Run()
        {
            
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Interpose);
        }
    }
}
