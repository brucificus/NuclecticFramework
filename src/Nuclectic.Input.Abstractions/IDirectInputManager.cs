using Nuclectic.Input.Devices;

namespace Nuclectic.Input
{
	public interface IDirectInputManager
	{
		bool IsDirectInputAvailable { get; }

		/// <summary>Creates game pad wrappers for all DirectInput game pads</summary>
		/// <returns>An array with wrappers for all DirectInput game pads</returns>
		IGamePad[] CreateGamePads();
	}
}