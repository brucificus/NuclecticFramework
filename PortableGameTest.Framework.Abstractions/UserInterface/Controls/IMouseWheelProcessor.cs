namespace Nuclex.UserInterface.Controls
{
    public interface IMouseWheelProcessor
    {
        /// <summary>Called when the mouse wheel has been rotated</summary>
        /// <param name="ticks">Number of ticks that the mouse wheel has been rotated</param>
        void ProcessMouseWheel(float ticks);
    }
}