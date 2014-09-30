using Microsoft.Xna.Framework;
using Nuclex.Game.States;

namespace PortableGameTest.Framework.Game.States
{
	public interface IGameStateService : IUpdateable, IDrawable
	{
		/// <summary>The currently active game state. Can be null.</summary>
		IGameState ActiveState { get; }

		/// <summary>Pauses the currently active state</summary>
		void Pause();

		/// <summary>Resumes the currently active state</summary>
		void Resume();
	}
}