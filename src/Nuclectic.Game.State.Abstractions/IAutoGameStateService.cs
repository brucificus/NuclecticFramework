namespace Nuclectic.Game.State
{
	public interface IAutoGameStateService : IGameStateService
	{
		/// <summary>Pushes the specified state onto the state stack</summary>
		/// <param name="modality">
		///   Behavior of the game state in relation to the state(s) below it on the stack
		/// </param>
		void Push<TGameState>(GameStateModality modality)
			where TGameState : IGameState;

		/// <summary>Pushes the specified state onto the state stack</summary>
		/// <param name="modality">
		///   Behavior of the game state in relation to the state(s) below it on the stack
		/// </param>
		void Push<TGameState, TParameter1>(GameStateModality modality, TParameter1 parameter1)
			where TGameState : IGameState;

		/// <summary>Takes the currently active game state from the stack</summary>
		void Pop();
	}
}