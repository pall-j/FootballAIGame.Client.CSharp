using System;
using System.Collections.Generic;
using System.Linq;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Utilities
{
    /// <summary>
    /// Provides functionality to evaluate positions by certain criteria and
    /// find the best supporting positions accordingly.
    /// </summary>
    class SupportPositionsManager
    {
        /// <summary>
        /// Gets the best support position.
        /// </summary>
        /// <value>
        /// The best support position.
        /// </value>
        public Vector BestSupportPosition
        {
            get
            {
                var bestPosition = SupportPositions.First();
                foreach (var supportPosition in SupportPositions)
                {
                    if (supportPosition.Score > bestPosition.Score)
                        bestPosition = supportPosition;
                }

                //Console.WriteLine(string.Format("Score {0}, DistanceS {1}, SafePassS {2}, ShotScore {3}", bestPosition.Score, bestPosition.DistanceScore, bestPosition.PassScore, bestPosition.ShootScore));
                return bestPosition.Position;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="FsmAI"/> instance to which this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="FsmAI"/> instance to which this instance belongs.
        /// </value>
        private FsmAI AI { get; set; }

        /// <summary>
        /// Gets or sets the support positions in the left (x less than 55) half of the field.
        /// </summary>
        /// <value>
        /// The support positions in the left half of the field.
        /// </value>
        private List<SupportPosition> LeftSupportPositions { get; set; }

        /// <summary>
        /// Gets or sets the support positions in the right (x greater than 55) half of the field.
        /// </summary>
        /// <value>
        /// The support positions in the right half of the field.
        /// </value>
        private List<SupportPosition> RightSupportPositions { get; set; }

        /// <summary>
        /// Gets or sets the support positions.
        /// </summary>
        /// <value>
        /// The support positions.
        /// </value>
        private List<SupportPosition> SupportPositions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportPositionsManager"/> class.
        /// </summary>
        /// <param name="footballAI">The <see cref="FsmAI"/> instance to which this instance belongs.</param>
        public SupportPositionsManager(FsmAI footballAI)
        {
            AI = footballAI;
            CreateSupportPositions();
        }

        /// <summary>
        /// Updates the support positions in accordance with the current state.
        /// </summary>
        public void Update()
        {
            foreach (var supportPosition in SupportPositions)
                UpdatePosition(supportPosition);
        }

        /// <summary>
        /// Updates the support position in accordance with the current state.
        /// </summary>
        /// <param name="supportPosition">The support position.</param>
        private void UpdatePosition(SupportPosition supportPosition)
        {
            supportPosition.Score = 0;

            // these other scores may be used for debugging
            supportPosition.ShootScore = 0;
            supportPosition.DistanceScore = 0;
            supportPosition.PassScore = 0;

            var controlling = AI.MyTeam.ControllingPlayer;
            if (controlling != null)
            {
                if (AI.MyTeam.IsPassFromControllingSafe(supportPosition.Position))
                {
                    supportPosition.Score += Parameters.PassSafeFromControllingPlayerWeight;
                    supportPosition.PassScore += Parameters.PassSafeFromControllingPlayerWeight;
                }

                supportPosition.Score += Parameters.DistanceFromControllingPlayerWeight *
                    GetDistanceFromControllingScore(supportPosition.Position);
                supportPosition.DistanceScore += Parameters.DistanceFromControllingPlayerWeight *
                    GetDistanceFromControllingScore(supportPosition.Position);
            }

            if (IsShotOnGoalPossible(supportPosition.Position))
            {
                supportPosition.Score += Parameters.ShotOnGoalPossibleWeight;
                supportPosition.ShootScore += Parameters.ShotOnGoalPossibleWeight;
            }

            // distance from opponent
        }

        /// <summary>
        /// Determines whether the shot on goal from the specified position is possible.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>
        ///   <c>true</c> if a shot on goal from the specified position is possible; otherwise, <c>false</c>.
        /// </returns>
        private bool IsShotOnGoalPossible(Vector position)
        {
            // we expect the lowest possible max kicking power of the player

            var artificialPlayer = new FootballPlayer(0) { Position = position };
            var artificialBall = new FootballBall() { Position = position };

            Vector shotTarget;
            return (AI.MyTeam.TryGetShotOnGoal(artificialPlayer, out shotTarget, artificialBall));
        }

        /// <summary>
        /// Gets the distance from controlling player score of the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The distance from controlling player score of the specified position.</returns>
        private double GetDistanceFromControllingScore(Vector position)
        {
            double distance = Vector.GetDistanceBetween(position, AI.MyTeam.ControllingPlayer.Position);

            var diff = Math.Abs(distance - Parameters.OptimalDistanceFromControlling);

            if (diff <= Parameters.MaxValuedDifferenceFromOptimal)
                return (Parameters.MaxValuedDifferenceFromOptimal - diff) / Parameters.MaxValuedDifferenceFromOptimal;

            return 0;
        }

        /// <summary>
        /// Creates the support positions.
        /// </summary>
        private void CreateSupportPositions()
        {
            LeftSupportPositions = new List<SupportPosition>();
            RightSupportPositions = new List<SupportPosition>();
            SupportPositions = new List<SupportPosition>();

            var dx = GameClient.FieldWidth / 15.0;
            var dy = GameClient.FieldHeight / 9.0;

            for (int i = 1; i < 7; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    LeftSupportPositions.Add(new SupportPosition(new Vector(i * dx, j * dy), 0));
                    RightSupportPositions.Add(new SupportPosition(new Vector(i * dx + 6 * dx, j * dy), 0));
                }
            }

            SupportPositions.AddRange(LeftSupportPositions);
            SupportPositions.AddRange(RightSupportPositions);
        }

        /// <summary>
        /// Represents the support position with its scores.
        /// </summary>
        private class SupportPosition
        {
            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>
            /// The position.
            /// </value>
            public Vector Position { get; set; }

            /// <summary>
            /// Gets or sets the score.
            /// </summary>
            /// <value>
            /// The score.
            /// </value>
            public double Score { get; set; }

            /// <summary>
            /// Gets or sets the distance score. This score is higher if the position is
            /// nearer controlling player.
            /// </summary>
            /// <value>
            /// The distance score.
            /// </value>
            public double DistanceScore { get; set; }

            /// <summary>
            /// Gets or sets the shoot score. This score is higher if a shot on goal is
            /// possible from this position.
            /// </summary>
            /// <value>
            /// The shoot score.
            /// </value>
            public double ShootScore { get; set; }

            /// <summary>
            /// Gets or sets the pass score. This score is higher if the pass from controlling player
            /// to this position is possible.
            /// </summary>
            /// <value>
            /// The pass score.
            /// </value>
            public double PassScore { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SupportPosition"/> class.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="score">The score.</param>
            public SupportPosition(Vector position, double score)
            {
                Position = position;
                Score = score;
            }
        }
    }


}
