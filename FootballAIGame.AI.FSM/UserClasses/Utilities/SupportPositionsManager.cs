using System;
using System.Collections.Generic;
using System.Linq;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;

namespace FootballAIGame.AI.FSM.UserClasses.Utilities
{
    class SupportPositionsManager
    {

        private FootballAI AI { get; set; }

        private List<SupportPosition> LeftSupportPositions { get; set; }

        private List<SupportPosition> RightSupportPositions { get; set; }

        private List<SupportPosition> SupportPositions { get; set; }

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

        public SupportPositionsManager(FootballAI footballAI)
        {
            AI = footballAI;
            CreateSupportPositions();
        }

        public void Update()
        {
            foreach (var supportPosition in SupportPositions)
                UpdatePosition(supportPosition);
        }

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

        private bool IsShotOnGoalPossible(Vector position)
        {
            // we expect the lowest possible max kicking power of the player

            var artificialPlayer = new FootballPlayer(0) {Position = position};
            var artificialBall = new FootballBall() {Position = position};

            Vector shotTarget;
            return (AI.MyTeam.TryGetShotOnGoal(artificialPlayer, out shotTarget, artificialBall));
        }

        private double GetDistanceFromControllingScore(Vector position)
        {
            double distance = Vector.DistanceBetween(position, AI.MyTeam.ControllingPlayer.Position);

            var diff = Math.Abs(distance - Parameters.OptimalDistanceFromControlling);

            if (diff <= Parameters.MaxValuedDifferenceFromOptimal)
                return (Parameters.MaxValuedDifferenceFromOptimal - diff)/Parameters.MaxValuedDifferenceFromOptimal;

            return 0;
        }

        private void CreateSupportPositions()
        {
            LeftSupportPositions = new List<SupportPosition>();
            RightSupportPositions = new List<SupportPosition>();
            SupportPositions = new List<SupportPosition>();

            var dx = GameClient.FieldWidth/15.0;
            var dy = GameClient.FieldHeight/9.0;

            for (int i = 1; i < 7; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    LeftSupportPositions.Add(new SupportPosition(new Vector(i * dx, j *dy), 0));
                    RightSupportPositions.Add(new SupportPosition(new Vector(i*dx + 6*dx, j*dy), 0));
                }
            }

            SupportPositions.AddRange(LeftSupportPositions);
            SupportPositions.AddRange(RightSupportPositions);
        }

        private class SupportPosition
        {
            public Vector Position { get; set; }

            public double Score { get; set; }

            public double DistanceScore { get; set; }

            public double ShootScore { get; set; }

            public double PassScore { get; set; }

            public SupportPosition(Vector position, double score)
            {
                Position = position;
                Score = score;
            }
        }
    }

    
}
