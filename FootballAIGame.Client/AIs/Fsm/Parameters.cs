namespace FootballAIGame.Client.AIs.Fsm
{
    static class Parameters
    {
        public const double PlayerInHomeRegionRange = 8;
        public const double BallReceivingRange = 2;
        public const double BallRange = 1.5;
        public const double DefendGoalDistance = 6;
        public const double GoalKeeperInterceptRange = 20;
        public const int NumberOfGeneratedShotTargets = 10;
        public const int DangerRange = 6;
        public const double MaxGoalkeeperSupportingDistance = 10;

        /* Support position evaluation parameters */

        public const double PassSafeFromControllingPlayerWeight = 3.0;
        public const double DistanceFromControllingPlayerWeight = 0.5;
        public const double ShotOnGoalPossibleWeight = 2.0;
        public const double DistanceFromOpponentGoalWeight = 0.5;

        public const double OptimalDistanceFromControlling = 20;
        public const double MaxValuedDifferenceFromOptimal = 50;
    }
}
