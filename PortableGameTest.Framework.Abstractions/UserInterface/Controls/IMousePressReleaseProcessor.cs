using Nuclectic.Input;

namespace Nuclex.UserInterface.Controls
{
    public interface IMousePressReleaseProcessor
    {
        /// <summary>Called when a mouse button has been pressed down</summary>
        /// <param name="button">Index of the button that has been pressed</param>
        /// <returns>Whether the control has processed the mouse press</returns>
        bool ProcessMousePress(MouseButtons button);

        /// <summary>Called when a mouse button has been released again</summary>
        /// <param name="button">Index of the button that has been released</param>
        void ProcessMouseRelease(MouseButtons button);
    }
}