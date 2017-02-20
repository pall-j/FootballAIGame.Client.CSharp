using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGameClient.CustomDataTypes;
using FootballAIGameClient.SimulationEntities;

namespace FootballAIGameClient.UserClasses
{
    /// <summary>
    /// The main AI class where the AI behavior is defined.
    /// </summary>
    class Ai : IFootballAi
    {
        /// <summary>
        /// Gets or sets the football players with their parameters set.
        /// Set after GetParameters is called. Used to know players parameters at every <see cref="GetAction"/> call.
        /// </summary>
        private FootballPlayer[] Players { get; set; }

        private static Random Random { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the AI football team holds currently the left goal post.
        /// </summary>
        private bool IsOnleft { get; set; }

        /// <summary>
        /// Called every time the new match simulation with this AI starts.<para />
        /// Called before <see cref="GetParameters" />.
        /// </summary>
        public void Initialize()
        {
            if (Random == null)
                Random = new Random();
        }

        /// <summary>
        /// Gets the <see cref="GameAction" /> for the specified <see cref="GameState" />.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        /// <returns>The <see cref="GameAction" /> for the specified <see cref="GameState" />.</returns>
        public GameAction GetAction(GameState gameState)
        {
            if (gameState.Step == 0)
                IsOnleft = gameState.FootballPlayers[0].Position.X < 55;

            if (gameState.Step == 750) // switch
                IsOnleft = !IsOnleft;

            var action = new GameAction { PlayerActions = new PlayerAction[11], Step = gameState.Step};

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

                if (IsOnleft)
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
                var toNewMovement = Vector.Difference(playerAction.Movement, player.Movement);
                if (toNewMovement.Length > player.MaxAcceleration)
                {
                    toNewMovement.Resize(player.MaxAcceleration);
                    playerAction.Movement = Vector.Sum(player.Movement, toNewMovement);
                }

                // speed correction
                if (playerAction.Movement.Length > player.MaxSpeed)
                    playerAction.Movement.Resize(player.MaxSpeed);

                // kick correction
                if (playerAction.Kick.Length > player.MaxKickSpeed)
                    playerAction.Kick.Resize(player.MaxKickSpeed);
            }

            return action;
        }

        /// <summary>
        /// Gets the player parameters. Position and moving vector properties are ignored.
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
                Players[i] = new FootballPlayer
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
