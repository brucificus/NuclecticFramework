using Microsoft.Xna.Framework.Input;

namespace Nuclex.Input.Devices
{
    public interface IExtendedGamePadState
    {
        /// <summary>Retrieves the state of the specified button</summary>
        /// <param name="buttonIndex">
        ///   Index of the button whose state will be retrieved
        /// </param>
        /// <returns>The state of the queried button</returns>
        ButtonState GetButton(int buttonIndex);

        /// <summary>Determines whether the specified button is pressed down</summary>
        /// <param name="buttonIndex">Button which will be checked</param>
        /// <returns>True if the specified button is pressed down</returns>
        bool IsButtonDown(int buttonIndex);

        /// <summary>Determines whether the specified button is up</summary>
        /// <param name="buttonIndex">Button which will be checked</param>
        /// <returns>True if the specified button is up</returns>
        bool IsButtonUp(int buttonIndex);

        /// <summary>Number of available axes in this state</summary>
        int AxisCount { get; }

        /// <summary>Number of available sliders in this state</summary>
        int SliderCount { get; }

        /// <summary>Retrieves the state of the specified axis</summary>
        /// <param name="axis">Axis whose state will be retrieved</param>
        /// <returns>The state of the specified axis</returns>
        float GetAxis(ExtendedAxes axis);

        /// <summary>Retrieves the state of the specified slider</summary>
        /// <param name="slider">Slider whose state will be retrieved</param>
        /// <returns>The state of the specified slider</returns>
        float GetSlider(ExtendedSliders slider);

        /// <summary>Retrieves the PoV controller of the specified index</summary>
        /// <param name="index">Index of the PoV controller that will be retrieved</param>
        /// <returns>The state of the PoV controller with the specified index</returns>
        int GetPov(int index);
    }
}