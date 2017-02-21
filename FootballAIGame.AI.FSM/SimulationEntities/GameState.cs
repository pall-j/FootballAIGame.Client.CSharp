using System;
using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    /// <summary>
    /// Represents the state of the football game.
    /// </summary>
    class GameState
    {
        /// <summary>
        /// Gets or sets the football players array consisting of 22 players, where first 11
        /// players are from the player's team and the rest 11 players are from the opponent's team.
        /// </summary>
        public FootballPlayer[] FootballPlayers { get; set; }

        /// <summary>
        /// Gets or sets the football ball.
        /// </summary>
        public FootballBall Ball { get; set; }

        /// <summary>
        /// The simulation step number specifying to which simulation step this instance belongs.
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Parses the specified binary representation of the game state.
        /// </summary>
        /// <param name="data">The binary representation of the game state.</param>
        /// <returns>The parsed game state.</returns>
        public static GameState Parse(byte[] data)
        {
            var floatData = new float[92];
            var stepData = new int[1];

            Buffer.BlockCopy(data, 0, stepData, 0, 4);
            Buffer.BlockCopy(data, 4, floatData, 0, (data.Length - 4));

            //Buffer.BlockCopy(data, 0, floatData, 0, data.Length);

            var players = new FootballPlayer[22];
            for (var i = 0; i < 22; i++)
            {
                players[i] = new FootballPlayer();
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
                Step = stepData[0]
            };
        }
    }

    
}
