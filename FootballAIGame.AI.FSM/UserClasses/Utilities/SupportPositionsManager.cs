using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.Utilities
{
    class SupportPositionsManager
    {
        public const double PassSafeFromControllingPlayerWeight = 3.0;
        public const double DistanceFromControllingPlayerWeight = 1.0;
        public const double ShotOnGoalPossibleWeight = 1.0;
        public const double DistanceFromOpponentGoal = 0.5;

        public const double OptimalDistanceFromControlling = 20;
        public const double MaxValuedDifferenceFromOptimal = 50;

        private SupportPositionsManager()
        {
            CreateSupportPositions();
        }

        private static SupportPositionsManager _instance;

        public static SupportPositionsManager Instance
        {
            get { return _instance ?? (_instance = new SupportPositionsManager()); }
        }

        private List<SupportPosition> LeftSupportPositions { get; set; }

        private List<SupportPosition> RightSupportPositions { get; set; }

        private List<SupportPosition> SupportPositions { get; set; }

        public void Update()
        {
            foreach (var supportPosition in SupportPositions)
                UpdatePosition(supportPosition);
        }

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

        private void UpdatePosition(SupportPosition supportPosition)
        {
            supportPosition.Score = 0;
            supportPosition.ShootScore = 0; // todo those other scores are for debugging only
            supportPosition.DistanceScore = 0;
            supportPosition.PassScore = 0;

            var controlling = Ai.Instance.MyTeam.ControllingPlayer;
            if (controlling != null)
            {
                if (IsPassSafe(supportPosition.Position))
                {
                    supportPosition.Score += PassSafeFromControllingPlayerWeight;
                    supportPosition.PassScore += PassSafeFromControllingPlayerWeight;
                }

                supportPosition.Score += DistanceFromControllingPlayerWeight *
                    GetDistanceFromControllingScore(supportPosition.Position);
                supportPosition.DistanceScore += DistanceFromControllingPlayerWeight *
                    GetDistanceFromControllingScore(supportPosition.Position);
            }

            if (IsShotOnGoalPossible(supportPosition.Position))
            {
                supportPosition.Score += ShotOnGoalPossibleWeight;
                supportPosition.ShootScore += ShotOnGoalPossibleWeight;
            }

        }

        private bool IsShotOnGoalPossible(Vector position)
        {
            // we expect the lowest possible max kicking power of the player

            var target = Ai.Instance.MyTeam.IsOnLeft ? 
                new Vector(GameClient.FieldHeight/2.0, GameClient.FieldWidth) : 
                new Vector(GameClient.FieldHeight/2.0, 0);

            var distance = Vector.DistanceBetween(target, position);

            var ball = new FootballBall() { Position = position };

            return !double.IsInfinity(ball.TimeToCoverDistance(distance, new FootballPlayer(0).MaxKickSpeed));
        }

        private bool IsPassSafe(Vector position)
        {
            var controlling = Ai.Instance.MyTeam.ControllingPlayer;
            var ball = Ai.Instance.CurrentState.Ball;

            var toControlling = Vector.Difference(controlling.Position, position);

            foreach (var opponent in Ai.Instance.OpponentTeam.Players)
            {
                var toOpponent = Vector.Difference(opponent.Position, position);

                var k = Vector.DotProduct(toControlling, toOpponent) /  toControlling.Length;
                var interposeTarget = Vector.Sum(position, toControlling.Resized(k));

                if (k > toControlling.Length || k <= 0)
                    continue; // safe

                var ballToInterpose = Vector.DistanceBetween(ball.Position, interposeTarget);

                var t1 = ball.TimeToCoverDistance(ballToInterpose, controlling.MaxKickSpeed);
                var t2 = opponent.TimeToGetToTarget(interposeTarget);

                if (t2 < t1)
                    return false;
            }

            return true;
        }

        private double GetDistanceFromControllingScore(Vector position)
        {
            double distance = Vector.DistanceBetween(position, Ai.Instance.MyTeam.ControllingPlayer.Position);

            var diff = Math.Abs(distance - OptimalDistanceFromControlling);

            if (diff <= MaxValuedDifferenceFromOptimal)
                return (MaxValuedDifferenceFromOptimal - diff)/MaxValuedDifferenceFromOptimal;

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
