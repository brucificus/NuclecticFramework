namespace Nuclectic.Input.Abstractions.Devices
{
    /// <summary>Reads the state of a slider or compares states</summary>
    public interface ISliderReader {

        /// <summary>Retrieves the current value of the slider</summary>
        /// <param name="state">Joystick state the slider is taken from</param>
        /// <returns>The value of the slider in the joystick state</returns>
        float GetValue(IJoystickState state);

    }
}