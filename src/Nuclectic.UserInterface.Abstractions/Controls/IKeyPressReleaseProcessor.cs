using Microsoft.Xna.Framework.Input;

namespace Nuclectic.UserInterface.Controls
{
	public interface IKeyPressReleaseProcessor
	{
		/// <summary>Called when a key on the keyboard has been pressed down</summary>
		/// <param name="keyCode">Code of the key that was pressed</param>
		/// <param name="repetition">
		///   Whether the key press is due to the user holding down a key
		/// </param>
		bool ProcessKeyPress(Keys keyCode, bool repetition);

		/// <summary>Called when a key on the keyboard has been released again</summary>
		/// <param name="keyCode">Code of the key that was released</param>
		void ProcessKeyRelease(Keys keyCode);
	}
}