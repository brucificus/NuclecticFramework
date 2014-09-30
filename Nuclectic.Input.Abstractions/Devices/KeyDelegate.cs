using Microsoft.Xna.Framework.Input;

namespace Nuclectic.Input.Abstractions.Devices
{
    /// <summary>Delegate used to report key presses and releases</summary>
    /// <param name="key">Key that has been pressed or released</param>
    public delegate void KeyDelegate(Keys key);
}