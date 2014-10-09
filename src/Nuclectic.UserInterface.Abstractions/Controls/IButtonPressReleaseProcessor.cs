using Microsoft.Xna.Framework.Input;

namespace Nuclectic.UserInterface.Controls
{
	public interface IButtonPressReleaseProcessor
	{
		/// <summary>Called when a button on the game pad has been pressed</summary>
		/// <param name="button">Button that has been pressed</param>
		/// <returns>
		///   True if the button press was processed by the control and future game pad
		///   input belongs to the control until all buttons are released again
		/// </returns>
		bool ProcessButtonPress(Buttons button);

		/// <summary>Called when a button on the game pad has been released</summary>
		/// <param name="button">Button that has been released</param>
		void ProcessButtonRelease(Buttons button);
	}
}