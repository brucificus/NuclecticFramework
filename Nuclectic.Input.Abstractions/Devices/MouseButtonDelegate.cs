namespace Nuclectic.Input.Devices
{
    /// <summary>
    ///   Delegate used to report a press or released of one or more mouse buttons
    /// </summary>
    /// <param name="buttons">Button or buttons that have been pressed or released</param>
    public delegate void MouseButtonDelegate(MouseButtons buttons);
}