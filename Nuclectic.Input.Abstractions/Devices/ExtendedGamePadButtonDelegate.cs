namespace Nuclectic.Input.Abstractions.Devices
{
    /// <summary>Delegate use to report presses and releases of game pad buttons</summary>
    /// <param name="buttons1">Button or buttons that have been pressed or released</param>
    /// <param name="buttons2">Button or buttons that have been pressed or released</param>
    public delegate void ExtendedGamePadButtonDelegate(ulong buttons1, ulong buttons2);
}