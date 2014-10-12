namespace Nuclectic.UserInterface.Controls
{
	public interface IMouseMoveLeaveProcessor
	{
		/// <summary>
		///     Called when the mouse has left the control and is no longer hovering over it
		/// </summary>
		void ProcessMouseLeave();

		/// <summary>Processes mouse movement notifications</summary>
		/// <param name="containerWidth">Absolute width of the control's container</param>
		/// <param name="containerHeight">Absolute height of the control's container</param>
		/// <param name="x">Absolute X position of the mouse within the container</param>
		/// <param name="y">Absolute Y position of the mouse within the container</param>
		void ProcessMouseMove(
			float containerWidth, float containerHeight, float x, float y
			);
	}
}