using Microsoft.Xna.Framework.Input.Touch;

namespace Nuclectic.Input.Devices
{
	public interface ITouchState
	{
		/// <summary>Whether the touch panel is connected</summary>
		/// <remarks>
		///   If the touch panel is not connected, all data in the state will
		///   be neutral
		/// </remarks>
		bool IsAttached { get; }

		/// <summary>Touch events that occured since the last update</summary>
		TouchCollection Touches { get; }
	}
}