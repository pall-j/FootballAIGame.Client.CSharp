using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;
using FootballAIGame.AI.FSM.UserClasses.Utilities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class Support : PlayerState
    {
        private Arrive Arrive { get; set; }

        public Support(Player player) : base(player)
        {
        }

        public override void Enter()
        {
           Arrive = new Arrive(Player, 1, 1.0, SupportPositionsManager.Instance.BestSupportPosition); 
           Player.SteeringBehaviorsManager.AddBehavior(Arrive);
        }

        public override void Run()
        {
            Arrive.Target = SupportPositionsManager.Instance.BestSupportPosition;
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Arrive);
        }
    }
}
