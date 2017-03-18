using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class Dribble : PlayerState
    {
        public Dribble(Player player, Ai ai) : base(player, ai)
        {
        }

        public override void Enter()
        {
            Ai.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        public override void Run()
        {
            var target = new Vector(90, Player.Position.Y);
            if (!Ai.MyTeam.IsOnLeft)
                target.X = 20;

            if (Player.Position.X > 89 && Ai.MyTeam.IsOnLeft)
                target = new Vector(100, GameClient.FieldHeight / 2.0 + (Ai.Random.NextDouble() - 0.5) * 7.32);

            if (Player.Position.X < 21 && !Ai.MyTeam.IsOnLeft)
                target = new Vector(10, GameClient.FieldHeight / 2.0 + (Ai.Random.NextDouble() - 0.5) * 7.32);

            var kickDirection = Vector.Difference(target, Player.Position);
            var playerFutureMovement = Vector.Sum(Player.Movement, kickDirection.Resized(Player.MaxAcceleration)).Truncated(Player.MaxSpeed);
            var futureSpeedInKickDirection = Vector.DotProduct(playerFutureMovement, kickDirection)/kickDirection.Length;

            Player.KickBall(Ai.Ball, target, futureSpeedInKickDirection);

            Player.StateMachine.ChangeState(new PursueBall(Player, Ai));
        }

    }
}
