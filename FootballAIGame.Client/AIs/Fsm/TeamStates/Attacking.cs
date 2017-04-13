using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    class Attacking : TeamState
    {
        public Attacking(Team team, FsmAI footballAI) : base(team, footballAI)
        {
        }

        public override void Enter()
        {
            SetHomeRegions();
        }

        public override void Run()
        {
            if (Team.PlayerInBallRange == null &&
                AI.OpponentTeam.PlayerInBallRange != null)
            {
                Team.StateMachine.ChangeState(new Defending(Team, AI));
            }
            
            if (Team.SupportingPlayers.Count == 0 && Team.ControllingPlayer != null)
            {
                var bestPos = AI.SupportPositionsManager.BestSupportPosition;
                var bestSupporter = Team.GetNearestPlayerToPosition(bestPos, Team.ControllingPlayer);
                MessageDispatcher.Instance.SendMessage(new SupportControllingMessage(), bestSupporter);
            }
        }

        public override void SetHomeRegions()
        {
            Team.GoalKeeper.HomeRegion = Region.GetRegion(7, 4);

            Team.Defenders[0].HomeRegion = Region.GetRegion(6, 4);
            Team.Defenders[1].HomeRegion = Region.GetRegion(5, 3);
            Team.Defenders[2].HomeRegion = Region.GetRegion(4, 2);
            Team.Defenders[3].HomeRegion = Region.GetRegion(4, 6);

            Team.Midfielders[0].HomeRegion = Region.GetRegion(3, 4);
            Team.Midfielders[1].HomeRegion = Region.GetRegion(2, 2);
            Team.Midfielders[2].HomeRegion = Region.GetRegion(2, 4);
            Team.Midfielders[3].HomeRegion = Region.GetRegion(2, 6);

            Team.Forwards[0].HomeRegion = Region.GetRegion(1, 2);
            Team.Forwards[1].HomeRegion = Region.GetRegion(1, 6);

            if (!Team.IsOnLeft) return;

            // team is on the left side -> mirror image
            foreach (var player in Team.Players)
            {
                player.HomeRegion = Region.GetRegion(
                    (Region.NumberOfColumns - 1) - player.HomeRegion.X, player.HomeRegion.Y);
            }
        }

    }
}
