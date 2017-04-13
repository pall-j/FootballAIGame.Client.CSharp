using FootballAIGame.Client.AIs.Fsm.Entities;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    abstract class PlayerState : State<Player>
    {
        protected Player Player { get; set; }

        protected PlayerState(Player player, FsmAI footballAI) : base(player, footballAI)
        {
            Player = player;
        }
    }
}
