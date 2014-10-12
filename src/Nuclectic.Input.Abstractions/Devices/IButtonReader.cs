namespace Nuclectic.Input.Devices
{
	/// <summary>Reads the state of a button</summary>
	public interface IButtonReader
	{
		/// <summary>Determines whether the specified button is pressed</summary>
		/// <param name="buttons">Array containing the state of all buttons</param>
		/// <returns>True if the specified button was pressed</returns>
		bool IsPressed(bool[] buttons);

		/// <summary>
		///   Determines whether the state of the specified button has changed
		///   between two states
		/// </summary>
		/// <param name="previous">Previous state of the buttons</param>
		/// <param name="current">Current state of the buttons</param>
		/// <returns>True if the state of the button has changed</returns>
		bool HasChanged(bool[] previous, bool[] current);
	}
}