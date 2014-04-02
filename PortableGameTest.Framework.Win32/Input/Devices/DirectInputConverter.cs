#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PortableGameTest.Framework.Input.Devices;
using Joystick = SlimDX.DirectInput.Joystick;
using System;
using System.Collections.Generic;
using SlimDX.DirectInput;

using XnaGamePadButtons = Microsoft.Xna.Framework.Input.GamePadButtons;

namespace Nuclex.Input.Devices {

  /// <summary>Converts DirectInput joystick states into usable data</summary>
  internal partial class DirectInputConverter : IDirectInputConverter
  {

    /// <summary>Initializes a new DirectInput state converter</summary>
    /// <param name="joystick">Joystick for which the converter will be used</param>
    internal DirectInputConverter(Joystick joystick) {
      this.sliderReaders = new ISliderReader[8];
      this.axisReaders = new IAxisReader[24];

      this.buttonCount = joystick.Capabilities.ButtonCount;
      this.povCount = joystick.Capabilities.PovCount;

      mapAxes(joystick);
      mapSliders(joystick);
    }

    /// <summary>Converts a joystick state into an XNA game pad state</summary>
    /// <param name="joystickState">Joystick state that will be converted</param>
    /// <returns>The equivalent XNA game pad state</returns>
    public GamePadState Convert(IJoystickState joystickState) {

      // Read the current states of both thumb sticks
      Vector2 leftThumbstick, rightThumbstick;
      {
        if (this.axisReaders[0] != null) {
          leftThumbstick.X = this.axisReaders[0].GetValue(joystickState);
        } else {
          leftThumbstick.X = 0.0f;
        }
        if (this.axisReaders[1] != null) {
          leftThumbstick.Y = -this.axisReaders[1].GetValue(joystickState);
        } else {
          leftThumbstick.Y = 0.0f;
        }

        if (this.axisReaders[12] != null) {
          rightThumbstick.X = this.axisReaders[12].GetValue(joystickState);
        } else {
          rightThumbstick.X = 0.0f;
        }
        if (this.axisReaders[13] != null) {
          rightThumbstick.Y = -this.axisReaders[13].GetValue(joystickState);
        } else {
          rightThumbstick.Y = 0.0f;
        }
      }

      // Read the current states of the triggers
      float leftTrigger, rightTrigger;
      {
        if (this.sliderReaders[0] != null) {
          leftTrigger = this.sliderReaders[0].GetValue(joystickState);
        } else {
          leftTrigger = 0.0f;
        }
        if (this.sliderReaders[1] != null) {
          rightTrigger = this.sliderReaders[1].GetValue(joystickState);
        } else {
          rightTrigger = 0.0f;
        }
      }

      // Convert the button states
      Buttons buttons = 0;
      {
        Buttons[] buttonOrder = ExtendedGamePadState.ButtonOrder;
        bool[] buttonPressed = joystickState.GetButtons();

        int count = Math.Min(buttonOrder.Length, this.buttonCount);
        for (int index = 0; index < count; ++index) {
          if (buttonPressed[index]) {
            buttons |= buttonOrder[index];
          }
        }
      }

      // Convert the first PoV controller into a directional pad
      GamePadDPad dpad;
      {
        if (this.povCount > 0) {
          int[] povs = joystickState.GetPointOfViewControllers();
          dpad = ExtendedGamePadState.DpadFromPov(povs[0]);
        } else {
          dpad = new GamePadDPad();
        }
      }

      // All informations gathered, construct the game pad state
      return new GamePadState(
        new GamePadThumbSticks(leftThumbstick, rightThumbstick),
        new GamePadTriggers(leftTrigger, rightTrigger),
        new XnaGamePadButtons(buttons),
        dpad
      );
    }

    /// <summary>Bit mask of the axes available on the joystick</summary>
    public ExtendedAxes AvailableAxes {
      get { return this.availableAxes; }
    }

    /// <summary>Bit mask of the sliders available on the joystick</summary>
    public ExtendedSliders AvailableSliders {
      get { return this.availableSliders; }
    }

    /// <summary>Readers for the sliders in the order of the enumeration</summary>
    public ISliderReader[] SliderReaders {
      get { return this.sliderReaders; }
    }

    /// <summary>Readers for the axes in the order of the enumeration</summary>
    public IAxisReader[] AxisReaders {
      get { return this.axisReaders; }
    }

    /// <summary>Number of buttons on the device</summary>
    public int ButtonCount {
      get { return this.buttonCount; }
    }

    /// <summary>Number of PoV controllers on the device</summary>
    public int PovCount {
      get { return this.povCount; }
    }

    /// <summary>Maps the axes of the joystick</summary>
    /// <param name="joystick">Joystick for which axes are mapped</param>
    private void mapAxes(Joystick joystick) {
      IList<DeviceObjectInstance> axes = joystick.GetObjects(
        ObjectDeviceType.AbsoluteAxis
      );
      var unmappedReaders = new Queue<IAxisReader>();

      for (int index = 0; index < axes.Count; ++index) {
        if (axes[index].ObjectTypeGuid != ObjectGuid.Slider) {
          ExtendedAxes axis = identifyAxis(
            axes[index].Aspect, axes[index].ObjectTypeGuid
          );

          // If this axis could not be identified but we're still missing one of
          // the standard axes (X, Y, Rx, Ry), remember it so we can later map it
          // to the next unassigned standard axes in case some are left unassigned.
          if (axis == 0) {
            if (unmappedReaders.Count < 4) {
              unmappedReaders.Enqueue(createAxisReader(joystick, axis, axes[index]));
            }
          } else { // Axis identified, build reader and store it
            this.availableAxes |= axis;
            this.axisReaders[indexFromAxis(axis)] = createAxisReader(
              joystick, axis, axes[index]
            );
          }
        } // if
      } // for

      // If the four standard axes are still not completely provided, use
      // the unidentified axes we remembered earlier as a fallback solution.
      if ((this.axisReaders[0] == null) && (unmappedReaders.Count > 0)) {
        this.availableAxes |= ExtendedAxes.X;
        this.axisReaders[0] = unmappedReaders.Dequeue();
      }
      if ((this.axisReaders[1] == null) && (unmappedReaders.Count > 0)) {
        this.availableAxes |= ExtendedAxes.Y;
        this.axisReaders[1] = unmappedReaders.Dequeue();
      }
      if ((this.axisReaders[12] == null) && (unmappedReaders.Count > 0)) {
        this.availableAxes |= ExtendedAxes.RotationX;
        this.axisReaders[12] = unmappedReaders.Dequeue();
      }
      if ((this.axisReaders[13] == null) && (unmappedReaders.Count > 0)) {
        this.availableAxes |= ExtendedAxes.RotationY;
        this.axisReaders[13] = unmappedReaders.Dequeue();
      }
    }

    /// <summary>Maps the sliders of the joystick</summary>
    /// <param name="joystick">Joystick for which sliders are mapped</param>
    private void mapSliders(Joystick joystick) {
      IList<DeviceObjectInstance> sliders = joystick.GetObjects(
        ObjectDeviceType.AbsoluteAxis
      );
      var unmappedReaders = new Queue<ISliderReader>();

      for (int index = 0; index < sliders.Count; ++index) {
        if (sliders[index].ObjectTypeGuid == ObjectGuid.Slider) {
          ExtendedSliders slider = identifySlider(
            sliders[index].Aspect, sliders[index].ObjectTypeGuid
          );

          // If this slider could not be identified but were still missing one
          // of the two standard sliders, remember it so we can use it in case
          // no exact matches are found for sliders 1 and 2.
          if (slider == 0) {
            if (unmappedReaders.Count < 2) {
              unmappedReaders.Enqueue(createSliderReader(joystick, slider, sliders[index]));
            }
          } else { // Slider was identified, assign it to the next free slot
            if ((this.availableSliders & slider) != 0) {
              ++slider;
              if ((this.availableSliders & slider) != 0) {
                slider = 0;
              }
            }
          }

          // If we identified this slider (exactly or by fallback), build a reader
          // for it and store it in the appropriate slot.
          if (slider != 0) {
            this.availableSliders |= slider;
            this.sliderReaders[indexFromSlider(slider)] = createSliderReader(
              joystick, slider, sliders[index]
            );
          }
        } // if
      } // for

      // If sliders 1 and/or 2 are still not provided, use the unidentified
      // sliders we remembered earlier as a fallback solution. These could very
      // well be null, too, in which case the sliders simply aren't there.
      if ((this.sliderReaders[0] == null) && (unmappedReaders.Count > 0)) {
        this.availableSliders |= ExtendedSliders.Slider1;
        this.sliderReaders[0] = unmappedReaders.Dequeue();
      }
      if ((this.sliderReaders[1] == null) && (unmappedReaders.Count > 0)) {
        this.availableSliders |= ExtendedSliders.Slider2;
        this.sliderReaders[1] = unmappedReaders.Dequeue();
      }
    }

    /// <summary>Readers for the sliders in the order of thea axis enumeration</summary>
    private ISliderReader[] sliderReaders;
    /// <summary>Readers for the axes in the order of the slider enumeration</summary>
    private IAxisReader[] axisReaders;
    /// <summary>Axes available on the joystick</summary>
    private ExtendedAxes availableAxes;
    /// <summary>Sliders available on the joystick</summary>
    private ExtendedSliders availableSliders;
    /// <summary>Number of buttons on the device</summary>
    private int buttonCount;
    /// <summary>Number of PoV controllers on the device</summary>
    private int povCount;

  }

} // namespace Nuclex.Input.Devices
