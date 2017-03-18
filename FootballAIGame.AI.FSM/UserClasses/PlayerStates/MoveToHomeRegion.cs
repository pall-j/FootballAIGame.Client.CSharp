using System;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class MoveToHomeRegion : PlayerState
    {
        private Arrive MoveToHomeRegionArrive { get; set; }

        public MoveToHomeRegion(Player player, Ai ai) : base(player, ai)
        {
        }

        public override void Enter()
        {
            MoveToHomeRegionArrive = new Arrive(Player, 3, 1, Player.HomeRegion.Center);
            Player.SteeringBehaviorsManager.AddBehavior(MoveToHomeRegionArrive);
        }

        public override void Run()
        {
            MoveToHomeRegionArrive.Target = Player.HomeRegion.Center;
            if (Player.IsAtHomeRegion && Math.Abs(Player.CurrentSpeed) < 0.00001)
                Player.StateMachine.ChangeState(new Default(Player, Ai));

        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(MoveToHomeRegionArrive);
        }

    }
}
