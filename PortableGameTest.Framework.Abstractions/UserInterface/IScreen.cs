using System;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Input;

namespace Nuclex.UserInterface
{
    public interface IScreen : IInputReceiver
    {
        /// <summary>Triggered when the control in focus changes</summary>
        event EventHandler<ControlEventArgs> FocusChanged;

        /// <summary>Width of the screen in pixels</summary>
        float Width { get; set; }

        /// <summary>Height of the screen in pixels</summary>
        float Height { get; set; }

        /// <summary>Control responsible for hosting the GUI's top-level controls</summary>
        Control Desktop { get; }

        /// <summary>Whether the GUI has currently captured the input devices</summary>
        /// <remarks>
        ///   <para>
        ///     When you mix GUIs and gameplay (for example, in a strategy game where the GUI
        ///     manages the build menu and the remainder of the screen belongs to the game),
        ///     it is important to keep control of who currently owns the input devices.
        ///   </para>
        ///   <para>
        ///     Assume the player is drawing a selection rectangle around some units using
        ///     the mouse. He will press the mouse button outside any GUI elements, keep
        ///     holding it down and possibly drag over the GUI. Until the player lets go
        ///     of the mouse button, input exclusively belongs to the game. The same goes
        ///     vice versa, of course.
        ///   </para>
        ///   <para>
        ///     This property tells whether the GUI currently thinks that all input belongs
        ///     to it. If it is true, the game should not process any input. The GUI will
        ///     implement the input model as described here and respect the game's ownership
        ///     of the input devices if a mouse button is pressed outside of the GUI. To
        ///     correctly handle input device ownership, send all input to the GUI
        ///     regardless of this property's value, then check this property and if it
        ///     returns false let your game process the input.
        ///   </para>
        /// </remarks>
        bool IsInputCaptured { get; }

        /// <summary>True if the mouse is currently hovering over any GUI elements</summary>
        /// <remarks>
        ///   Useful if you mix gameplay with a GUI and use different mouse cursors
        ///   depending on the location of the mouse. As long as input is not captured
        ///   (see <see cref="IsInputCaptured" />) you can use this property to know
        ///   whether you should use the standard GUI mouse cursor or let your game
        ///   decide which cursor to use.
        /// </remarks>
        bool IsMouseOverGui { get; }

        /// <summary>Child control that currently has the input focus</summary>
        Control FocusedControl { get; set; }
    }
}