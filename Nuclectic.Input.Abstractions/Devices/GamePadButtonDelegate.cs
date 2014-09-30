using Microsoft.Xna.Framework.Input;

namespace Nuclectic.Input.Abstractions.Devices
{
    /// <summary>Delegate use to report presses and releases of game pad buttons</summary>
    /// <param name="buttons">Button or buttons that have been pressed or released</param>
    public delegate void GamePadButtonDelegate(Buttons buttons);
}