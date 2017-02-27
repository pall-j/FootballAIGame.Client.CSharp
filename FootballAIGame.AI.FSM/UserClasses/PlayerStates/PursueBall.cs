using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class PursueBall : PlayerState
    {
        private Pursuit BallPursuit { get; set; }

        public PursueBall(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            BallPursuit = new Pursuit(Player, 1, 1.0, Ai.Instance.Ball);
            Player.SteeringBehaviorsManager.AddBehavior(BallPursuit);
        }

        public override void Run()
        {
            if (Player.CanKickBall(Ai.Instance.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player));
            }

            var nearestToBall = Ai.Instance.MyTeam.NearestPlayerToBall;
            if (Player != nearestToBall)
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player));
                MessageDispatcher.Instance.SendMessage(new PursueBallMessage(), nearestToBall);
            }
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
