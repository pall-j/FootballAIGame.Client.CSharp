using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class MoveToHomeRegion<TPlayer> : State<TPlayer> where TPlayer : Player
    {
        private MoveToHomeRegion() { }

        private static MoveToHomeRegion<TPlayer> _instance;

        public static MoveToHomeRegion<TPlayer> Instance
        {
            get { return _instance ?? (_instance = new MoveToHomeRegion<TPlayer>()); }
        }

        public override void Enter(TPlayer player)
        {
            player.SteeringBehaviours.Target = player.HomeRegion.Center;
            player.SteeringBehaviours.SeekOn = true;
        }

        public override void Run(TPlayer player)
        {
            player.SteeringBehaviours.Target = player.HomeRegion.Center;
        }

        public override void Exit(TPlayer player)
        {
            player.SteeringBehaviours.SeekOn = false;
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }
    }
}
