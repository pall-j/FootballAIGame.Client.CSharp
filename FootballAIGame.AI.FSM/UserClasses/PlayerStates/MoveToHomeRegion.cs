using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class MoveToHomeRegion : PlayerState
    {
        private Arrive MoveToHomeRegionArrive { get; set; }

        public MoveToHomeRegion(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            MoveToHomeRegionArrive = new Arrive(Player, 1, 1, Player.HomeRegion.Center);
            //MoveToHomeRegionArrive = new Wander(Player, 1, 1, 0.5, 1, 2);
            Player.SteeringBehaviorsManager.AddBehavior(MoveToHomeRegionArrive);
        }

        public override void Run()
        {
            MoveToHomeRegionArrive.Target = Player.HomeRegion.Center;
            if (Player.IsAtHomeRegion && Math.Abs(Player.CurrentSpeed) < 0.00001)
                Player.StateMachine.ChangeState(new Default(Player));
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(MoveToHomeRegionArrive);
        }

    }
}
