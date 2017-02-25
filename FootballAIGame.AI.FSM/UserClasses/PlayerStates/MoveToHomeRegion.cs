using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class MoveToHomeRegion : PlayerState
    {
        private SteeringBehavior MoveToHomeRegionArrive { get; set; }

        public MoveToHomeRegion(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            if (Player.Id != 0)
                MoveToHomeRegionArrive = new Arrive(1, 1, Player.HomeRegion.Center);
            else
            {
                MoveToHomeRegionArrive = new Pursuit(1, 1, Ai.Instance.OpponentTeam.Forwards[0]);
            }
            Player.SteeringBehaviorsManager.AddBehavior(MoveToHomeRegionArrive);
        }

        public override void Run()
        {
            //MoveToHomeRegionArrive.Target = Player.HomeRegion.Center;
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(MoveToHomeRegionArrive);
        }

    }
}
