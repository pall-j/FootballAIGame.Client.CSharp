using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class MoveToHomeRegion : State<Player>
    {
        private MoveToHomeRegion() { }

        private static MoveToHomeRegion _instance;

        public static MoveToHomeRegion Instance
        {
            get { return _instance ?? (_instance = new MoveToHomeRegion()); }
        }

        public override void Enter(Player player)
        {
            player.SteeringBehaviours.Target = player.HomeRegion.Center;
            player.SteeringBehaviours.FleeOn = true;
        }

        public override void Run(Player player)
        {
            player.SteeringBehaviours.Target = player.HomeRegion.Center;
        }

        public override void Exit(Player player)
        {
            player.SteeringBehaviours.FleeOn = false;
        }
    }
}
