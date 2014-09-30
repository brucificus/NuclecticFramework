using Microsoft.Xna.Framework.Input;

namespace Nuclectic.Input.Abstractions.Devices
{
    public interface IDirectInputConverter
    {
        /// <summary>Converts a joystick state into an XNA game pad state</summary>
        /// <param name="joystickState">Joystick state that will be converted</param>
        /// <returns>The equivalent XNA game pad state</returns>
        GamePadState Convert(IJoystickState joystickState);

        /// <summary>Bit mask of the axes available on the joystick</summary>
        ExtendedAxes AvailableAxes { get; }

        /// <summary>Bit mask of the sliders available on the joystick</summary>
        ExtendedSliders AvailableSliders { get; }

        /// <summary>Readers for the sliders in the order of the enumeration</summary>
        ISliderReader[] SliderReaders { get; }

        /// <summary>Readers for the axes in the order of the enumeration</summary>
        IAxisReader[] AxisReaders { get; }

        /// <summary>Number of buttons on the device</summary>
        int ButtonCount { get; }

        /// <summary>Number of PoV controllers on the device</summary>
        int PovCount { get; }
    }
}