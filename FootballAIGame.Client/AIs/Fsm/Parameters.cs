using FootballAIGame.Client.AIs.Fsm.PlayerStates;

namespace FootballAIGame.Client.AIs.Fsm
{
    /// <summary>
    /// Contains parameters of the FSM AI that are accessed from all its parts.
    /// </summary>
    static class Parameters
    {
        /// <summary>
        /// The maximum distance of player from his home region for him to be at home.
        /// Important for <see cref="MoveToHomeRegion"/> player state.
        /// </summary>
        public const double PlayerInHomeRegionRange = 8;

        /// <summary>
        /// The maximum distance from the ball for player to think that he can kick it.
        /// By using lower values the player moves closer to the ball before he tries
        /// to kick it.
        /// </summary>
        public const double BallReceivingRange = 2;

        /// <summary>
        /// The maximum distance from the ball for player to be in the ball range.
        /// Used by team states to toggle between attacking and defending strategy.
        /// </summary>
        public const double BallRange = 1.5;

        /// <summary>
        /// The goalkeeper's preferred distance from the goal. 
        /// </summary>
        public const double DefendGoalDistance = 6;

        /// <summary>
        /// The goalkeeper's maximum distance from goal the that he will go
        /// to intercept the ball.
        /// </summary>
        public const double GoalKeeperInterceptRange = 20;

        /// <summary>
        /// The number of random generated shot targets that are
        /// used for finding the safe shot on goal.
        /// </summary>
        public const int NumberOfGeneratedShotTargets = 10;

        /// <summary>
        /// If there is an opponent nearer than this value to a
        /// player, then that player is considered to be in danger.
        /// </summary>
        public const int DangerRange = 6;

        /// <summary>
        /// The maximum distance of goalkeeper from his goal that he will
        /// go to support the controlling player.
        /// </summary>
        public const double MaxGoalkeeperSupportingDistance = 10;


        /* 
         * Support position evaluation parameters. Used by SupportPositionManager.
         * The weight specify how important the property of the position is. 
         */

        public const double PassSafeFromControllingPlayerWeight = 3.0;
        public const double DistanceFromControllingPlayerWeight = 0.5;
        public const double ShotOnGoalPossibleWeight = 2.0;
        public const double DistanceFromOpponentGoalWeight = 0.5;

        public const double OptimalDistanceFromControlling = 20;
        public const double MaxValuedDifferenceFromOptimal = 50;
    }
}
