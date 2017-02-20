using FootballAIGameClient.SimulationEntities;

namespace FootballAIGameClient
{
    interface IFootballAi
    {
        /// <summary>
        /// Called every time the new match simulation with the AI starts.<para />
        /// Called before <see cref="GetParameters"/>.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the <see cref="GameAction"/> for the specified <see cref="GameState"/>.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        /// <returns>The <see cref="GameAction" /> for the specified <see cref="GameState" />.</returns>
        GameAction GetAction(GameState gameState);

        /// <summary>
        /// Gets the player parameters. Position and moving vector properties are ignored.
        /// </summary>
        /// <returns>The array of football players with their parameters set.</returns>
        FootballPlayer[] GetParameters();
    }
}
