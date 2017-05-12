using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's dribble state. Player in this state pushes the ball in front of him 
    /// towards the opponent goal. After each push the state is changed to <see cref="PursueBall"/>.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class Dribble : PlayerState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dribble"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public Dribble(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }


        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            AI.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            var target = new Vector(90, Player.Position.Y);
            if (!AI.MyTeam.IsOnLeft)
                target.X = 20;

            if (Player.Position.X > 89 && AI.MyTeam.IsOnLeft)
                target = new Vector(100, GameClient.FieldHeight / 2.0 + (FsmAI.Random.NextDouble() - 0.5) * 7.32);

            if (Player.Position.X < 21 && !AI.MyTeam.IsOnLeft)
                target = new Vector(10, GameClient.FieldHeight / 2.0 + (FsmAI.Random.NextDouble() - 0.5) * 7.32);

            var kickDirection = Vector.GetDifference(target, Player.Position);
            var playerFutureMovement = Vector.GetSum(Player.Movement, kickDirection.GetResized(Player.MaxAcceleration)).GetTruncated(Player.MaxSpeed);
            var futureSpeedInKickDirection = Vector.GetDotProduct(playerFutureMovement, kickDirection)/kickDirection.Length;

            Player.KickBall(AI.Ball, target, futureSpeedInKickDirection);

            Player.StateMachine.ChangeState(new PursueBall(Player, AI));
        }
    }
}
