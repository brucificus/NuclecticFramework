using Microsoft.Xna.Framework;

namespace Nuclectic.Input.Abstractions.Devices
{
    /// <summary>Delegate used to report touch actions</summary>
    /// <param name="id">ID of the distinct touch</param>
    /// <param name="position">Position the action occurred at</param>
    public delegate void TouchDelegate(int id, Vector2 position);
}