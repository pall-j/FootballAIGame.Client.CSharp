using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    /// <summary>
    /// Extends the <see cref="FootballBall"/>. Adds method for loading game state.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.SimulationEntities.FootballBall" />
    class Ball : FootballBall
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ball"/> class.
        /// </summary>
        /// <param name="ball">The football ball.</param>
        public Ball(FootballBall ball)
        {
            Position = ball.Position;
            Movement = ball.Movement;
        }

        /// <summary>
        /// Loads the state. Updates position and movement vector accordingly.
        /// </summary>
        /// <param name="gameState">The state of the game.</param>
        public void LoadState(GameState gameState)
        {
            Position.X = gameState.Ball.Position.X;
            Position.Y = gameState.Ball.Position.Y;
            Movement.X = gameState.Ball.Movement.X;
            Movement.Y = gameState.Ball.Movement.Y;
        }
    }
}
