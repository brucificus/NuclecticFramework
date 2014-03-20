namespace PortableGameTest.Framework.Input.Devices
{
    /// <summary>Reads the state of an axis or compares states</summary>
    public interface IAxisReader {

        /// <summary>Retrieves the current value of the axis</summary>
        /// <param name="state">Joystick state the axis is taken from</param>
        /// <returns>The value of the axis in the joystick state</returns>
        float GetValue(IJoystickState state);

    }
}