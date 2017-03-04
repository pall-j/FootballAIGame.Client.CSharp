using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses
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

    }
}
