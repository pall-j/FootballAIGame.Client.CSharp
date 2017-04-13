using System;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    class MoveToHomeRegion : PlayerState
    {
        private Arrive MoveToHomeRegionArrive { get; set; }

        public MoveToHomeRegion(Player player, FsmAI footballAI) : base(player, footballAI)
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
                Player.StateMachine.ChangeState(new Default(Player, AI));

        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(MoveToHomeRegionArrive);
        }

    }
}
