using System;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.SimulationEntities
{
    /// <summary>
    /// Represents the state of the football match.
    /// </summary>
    class GameState
    {
        /// <summary>
        /// Gets or sets the football players' array consisting of 22 players, where first 11
        /// players are from the player's team and the other 11 players are from the opponent's team.
        /// </summary>
        /// <value>
        /// The array of football players.
        /// </value>
        public FootballPlayer[] FootballPlayers { get; set; }

        /// <summary>
        /// Gets or sets the football ball.
        /// </summary>
        /// <value>
        /// The football ball.
        /// </value>
        public FootballBall Ball { get; set; }

        /// <summary>
        /// The simulation step number specifying to which simulation step this instance belongs.
        /// </summary>
        /// <value>
        /// The simulation step.
        /// </value>
        public int Step { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a kickoff is happening.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a kickoff is happening; otherwise, <c>false</c>.
        /// </value>
        public bool KickOff { get; set; }

        /// <summary>
        /// Parses the specified binary representation of the game state.
        /// </summary>
        /// <param name="data">The binary representation of the game state.</param>
        /// <returns>
        /// The parsed <see cref="GameState"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if an error has occurred while parsing the game state.</exception>
        public static GameState Parse(byte[] data)
        {
            var floatData = new float[92];
            var stepData = new int[1];

            if (data.Length != floatData.Length * 4 + 4 + 1)
                throw new ArgumentException("Invalid game state data.");

            Buffer.BlockCopy(data, 0, stepData, 0, 4);
            Buffer.BlockCopy(data, 5, floatData, 0, (data.Length - 5));

            var players = new FootballPlayer[22];
            for (var i = 0; i < 22; i++)
            {
                players[i] = new FootballPlayer(i);
            }

            var ball = new FootballBall()
            {
                Position = new Vector(floatData[0], floatData[1]),
                Movement = new Vector(floatData[2], floatData[3])
            };

            for (var i = 0; i < 22; i++)
            {
                players[i].Position = new Vector(floatData[4 + 4*i], floatData[4 + 4*i + 1]);
                players[i].Movement = new Vector(floatData[4 + 4*i + 2], floatData[4 + 4*i + 3]);
            }

            return new GameState()
            {
                Ball = ball,
                FootballPlayers = players,
                Step = stepData[0],
                KickOff = data[4] == 1
            };
        }

    }
    
}
