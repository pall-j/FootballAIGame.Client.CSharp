using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    abstract class PlayerState : State<Player>
    {
        protected Player Player { get; set; }

        protected PlayerState(Player player, Ai ai) : base(player, ai)
        {
            Player = player;
        }
    }
}
