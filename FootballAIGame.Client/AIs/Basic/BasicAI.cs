using System;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Basic
{
    /// <summary>
    /// Represents the basic random AI.
    /// </summary>
    class BasicAI : IFootballAI
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Random" /> used for generating random numbers.
        /// </summary>
        /// <value>
        /// The <see cref="Random"/> instance.
        /// </value>
        public static Random Random { get; set; }

        /// <summary>
        /// Gets or sets the football players with their parameters set.
        /// Set after GetParameters is called. Used to know players' parameters at every <see cref="GetAction"/> call.
        /// </summary>
        private FootballPlayer[] Players { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the AI's football team currently holds the left goal post.
        /// </summary>
        /// <value>
        /// <c>true</c> if the AI's football team currently holds the left goal post; otherwise, <c>false</c>.
        /// </value>
        private bool IsOnLeft { get; set; }

        /// <summary>
        /// Called every time the new match simulation with the AI starts.<para />
        /// Called before <see cref="GetParameters" />.
        /// </summary>
        public void Initialize()
        {
            if (Random == null)
                Random = new Random();
        }

        /// <summary>
        /// Gets the <see cref="AIAction" /> for the specified <see cref="GameState" />.
        /// </summary>
        /// <param name="gameState">The state of the game.</param>
        /// <returns>The <see cref="AIAction" /> for the specified <see cref="GameState" />.</returns>
        public AIAction GetAction(GameState gameState)
        {
            if (gameState.Step == 0)
                IsOnLeft = gameState.FootballPlayers[0].Position.X < 55;

            if (gameState.Step == 750) // switch
                IsOnLeft = !IsOnLeft;

            var action = new AIAction { PlayerActions = new PlayerAction[11], Step = gameState.Step };

            for (var i = 0; i < 11; i++)
            {
                var player = gameState.FootballPlayers[i];
                var playerAction = new PlayerAction();
                action.PlayerActions[i] = playerAction;

                playerAction.Movement.X = Random.NextDouble() - 0.5;
                playerAction.Movement.Y = Random.NextDouble() - 0.5;

                if ((player.Position.X > 110 && playerAction.Movement.X > 0) || (player.Position.X <= 110.01 && player.Position.X + playerAction.Movement.X > 110))
                    playerAction.Movement.X *= -1;

                if ((player.Position.Y > 75 && playerAction.Movement.Y > 0) || (player.Position.Y < 75.01 && player.Position.Y + playerAction.Movement.Y > 75))
                    playerAction.Movement.Y *= -1;

                if ((player.Position.X < 0 && playerAction.Movement.X < 0) || (player.Position.X >= 0 && player.Position.X + playerAction.Movement.X < 0))
                    playerAction.Movement.X *= -1;

                if ((player.Position.Y < 0 && playerAction.Movement.Y < 0) || (player.Position.Y >= 0 && player.Position.Y + playerAction.Movement.Y < 0))
                    playerAction.Movement.Y *= -1;

                if (IsOnLeft)
                {
                    playerAction.Kick.X = 110 - player.Position.X;
                    playerAction.Kick.Y = (75 / 2f) - player.Position.Y;
                }
                else
                {
                    playerAction.Kick.X = -player.Position.X;
                    playerAction.Kick.Y = (75 / 2f) - player.Position.Y;
                }

                // acceleration correction
                var toNewMovement = Vector.GetDifference(playerAction.Movement, player.Movement);
                toNewMovement.Truncate(player.MaxAcceleration);
                playerAction.Movement = Vector.GetSum(player.Movement, toNewMovement);

                // speed correction
                playerAction.Movement.Truncate(player.MaxSpeed);

                // kick correction
                playerAction.Kick.Truncate(player.MaxKickSpeed);
            }

            return action;
        }

        /// <summary>
        /// Gets the players' parameters.
        /// </summary>
        /// <returns>
        /// The array of football players with their parameters set.
        /// </returns>
        public FootballPlayer[] GetParameters()
        {
            if (Players != null) return Players;

            Players = new FootballPlayer[11];

            for (var i = 0; i < 11; i++)
            {
                Players[i] = new FootballPlayer(i)
                {
                    Speed = 0.4f,
                    KickPower = 0.2f,
                    Possession = 0.2f,
                    Precision = 0.2f
                };
            }

            return Players;
        }
    }
}
