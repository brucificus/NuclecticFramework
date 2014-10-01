using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Nuclex.Graphics
{
    public interface ICamera
    {
        /// <summary>Turns the camera so it is facing the point provided</summary>
        /// <param name="lookAtPosition">Position the camera should be pointing to</param>
        void LookAt(Vector3 lookAtPosition);

        /// <summary>Moves the camera to the specified location</summary>
        /// <param name="position">Location the camera will be moved to</param>
        void MoveTo(Vector3 position);

        /// <summary>The camera's current position</summary>
        Vector3 Position { get; set; }

        /// <summary>The camera's forward vector</summary>
        Vector3 Forward { get; }

        /// <summary>The camera's right vector</summary>
        Vector3 Right { get; }

        /// <summary>The camera's up vector</summary>
        Vector3 Up { get; }

		Matrix View { get; }
		Matrix Projection { get; }

        /// <summary>
        ///   Debugging aid that allows the camera to be moved around by the keyboard
        ///   or the game pad
        /// </summary>
        /// <param name="gameTime">Game time to use for scaling the movements</param>
        /// <param name="keyboardState">Current state of the keyboard</param>
        /// <param name="gamepadState">Current state of the gamepad</param>
        /// <remarks>
        ///   <para>
        ///     This is only intended as a debugging aid and should not be used for the actual
        ///     player controls. As long as you don't rebuild the camera matrix each frame
        ///     (which is not a good idea anyway) this will allow you to control the camera
        ///     in the style of the old "Descent" game series.
        ///   </para>
        ///   <para>
        ///     To enable the camera controls, simply call this method from your main loop!
        ///   </para>
        /// </remarks>
        void HandleControls(
            GameTime gameTime, KeyboardState keyboardState, GamePadState gamepadState
            );
    }
}