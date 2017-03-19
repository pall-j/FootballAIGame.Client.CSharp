using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    abstract class PlayerState : State<Player>
    {
        protected Player Player { get; set; }

        protected PlayerState(Player player, FootballAI footballAI) : base(player, footballAI)
        {
            Player = player;
        }
    }
}
