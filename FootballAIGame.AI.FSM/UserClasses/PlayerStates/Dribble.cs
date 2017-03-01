using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class Dribble : PlayerState
    {
        public Dribble(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            Ai.Instance.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        public override void Run()
        {
            var target = new Vector(110, Player.Position.Y);
            if (!Ai.Instance.MyTeam.IsOnLeft)
                target.X = 0;

            var kickDirection = Vector.Difference(target, Player.Position);
            var playerFutureMovement = Vector.Sum(Player.Movement, kickDirection.Resized(Player.MaxAcceleration)).Truncated(Player.MaxSpeed);
            var futureSpeedInKickDirection = Vector.DotProduct(playerFutureMovement, kickDirection)/kickDirection.Length + 0.1;

            Player.KickBall(Ai.Instance.Ball, target, futureSpeedInKickDirection);

            if (Player is GoalKeeper)
                Console.WriteLine("State change: Dribble -> PursueBall");
            Player.StateMachine.ChangeState(new PursueBall(Player));
        }

        public override void Exit()
        {
            if (Ai.Instance.MyTeam.ControllingPlayer == Player)
                Ai.Instance.MyTeam.ControllingPlayer = null;
        }
    }
}
