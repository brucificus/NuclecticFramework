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

using PortableGameTest.Framework.Input.Devices;
using Joystick = SlimDX.DirectInput.Joystick;
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using SlimDX.DirectInput;

using XnaGamePadButtons = Microsoft.Xna.Framework.Input.GamePadButtons;

namespace Nuclex.Input.Devices {

  partial class DirectInputConverter {

    /// <summary>Returns the slider index from a value in the slider enumeration</summary>
    /// <param name="slider">Slider enumeration values whose index will be returned</param>
    /// <returns>The index of the specified slider enumeration value</returns>
    private static int indexFromSlider(ExtendedSliders slider) {
      switch (slider) {
        case ExtendedSliders.Slider1: { return 0; }
        case ExtendedSliders.Slider2: { return 1; }
        case ExtendedSliders.Velocity1: { return 2; }
        case ExtendedSliders.Velocity2: { return 3; }
        case ExtendedSliders.Acceleration1: { return 4; }
        case ExtendedSliders.Acceleration2: { return 5; }
        case ExtendedSliders.Force1: { return 6; }
        case ExtendedSliders.Force2: { return 7; }
        default: { return -1; }
      }
    }

    /// <summary>Returns the axis index from a value in the axis enumeration</summary>
    /// <param name="axis">Axis enumeration values whose index will be returned</param>
    /// <returns>The index of the specified axis enumeration value</returns>
    private static int indexFromAxis(ExtendedAxes axis) {
      switch (axis) {
        case ExtendedAxes.X: { return 0; }
        case ExtendedAxes.Y: { return 1; }
        case ExtendedAxes.Z: { return 2; }
        case ExtendedAxes.VelocityX: { return 3; }
        case ExtendedAxes.VelocityY: { return 4; }
        case ExtendedAxes.VelocityZ: { return 5; }
        case ExtendedAxes.AccelerationX: { return 6; }
        case ExtendedAxes.AccelerationY: { return 7; }
        case ExtendedAxes.AccelerationZ: { return 8; }
        case ExtendedAxes.ForceX: { return 9; }
        case ExtendedAxes.ForceY: { return 10; }
        case ExtendedAxes.ForceZ: { return 11; }
        case ExtendedAxes.RotationX: { return 12; }
        case ExtendedAxes.RotationY: { return 13; }
        case ExtendedAxes.RotationZ: { return 14; }
        case ExtendedAxes.AngularVelocityX: { return 15; }
        case ExtendedAxes.AngularVelocityY: { return 16; }
        case ExtendedAxes.AngularVelocityZ: { return 17; }
        case ExtendedAxes.AngularAccelerationX: { return 18; }
        case ExtendedAxes.AngularAccelerationY: { return 19; }
        case ExtendedAxes.AngularAccelerationZ: { return 20; }
        case ExtendedAxes.TorqueX: { return 21; }
        case ExtendedAxes.TorqueY: { return 22; }
        case ExtendedAxes.TorqueZ: { return 23; }
        default: { return -1; }
      }
    }

    /// <summary>Creates an axis reader for the specified object</summary>
    /// <param name="joystick">Joystick providing the control object</param>
    /// <param name="axis">Axis a reader will be created for</param>
    /// <param name="control">Control description for the axis</param>
    /// <returns>A new axis reader for the specified axis</returns>
    private static IAxisReader createAxisReader(
      Joystick joystick, ExtendedAxes axis, DeviceObjectInstance control
    ) {
      int id = (int)control.ObjectType;
      ObjectProperties properties = joystick.GetObjectPropertiesById(id);

      int min = properties.LowerRange;
      int max = properties.UpperRange;

      switch (axis) {
        case ExtendedAxes.X: {
          return new XAxisReader(min, max);
        }
        case ExtendedAxes.Y: {
          return new YAxisReader(min, max);
        }
        case ExtendedAxes.Z: {
          return new ZAxisReader(min, max);
        }
        case ExtendedAxes.VelocityX: {
          return new VelocityXAxisReader(min, max);
        }
        case ExtendedAxes.VelocityY: {
          return new VelocityYAxisReader(min, max);
        }
        case ExtendedAxes.VelocityZ: {
          return new VelocityZAxisReader(min, max);
        }
        case ExtendedAxes.AccelerationX: {
          return new AccelerationXAxisReader(min, max);
        }
        case ExtendedAxes.AccelerationY: {
          return new AccelerationYAxisReader(min, max);
        }
        case ExtendedAxes.AccelerationZ: {
          return new AccelerationZAxisReader(min, max);
        }
        case ExtendedAxes.ForceX: {
          return new ForceXAxisReader(min, max);
        }
        case ExtendedAxes.ForceY: {
          return new ForceYAxisReader(min, max);
        }
        case ExtendedAxes.ForceZ: {
          return new ForceZAxisReader(min, max);
        }
        case ExtendedAxes.RotationX: {
          return new RotationXAxisReader(min, max);
        }
        case ExtendedAxes.RotationY: {
          return new RotationYAxisReader(min, max);
        }
        case ExtendedAxes.RotationZ: {
          return new RotationZAxisReader(min, max);
        }
        case ExtendedAxes.AngularVelocityX: {
          return new AngularVelocityXAxisReader(min, max);
        }
        case ExtendedAxes.AngularVelocityY: {
          return new AngularVelocityYAxisReader(min, max);
        }
        case ExtendedAxes.AngularVelocityZ: {
          return new AngularVelocityZAxisReader(min, max);
        }
        case ExtendedAxes.AngularAccelerationX: {
          return new AngularAccelerationXAxisReader(min, max);
        }
        case ExtendedAxes.AngularAccelerationY: {
          return new AngularAccelerationYAxisReader(min, max);
        }
        case ExtendedAxes.AngularAccelerationZ: {
          return new AngularAccelerationZAxisReader(min, max);
        }
        case ExtendedAxes.TorqueX: {
          return new TorqueXAxisReader(min, max);
        }
        case ExtendedAxes.TorqueY: {
          return new TorqueYAxisReader(min, max);
        }
        case ExtendedAxes.TorqueZ: {
          return new TorqueZAxisReader(min, max);
        }
        default: {
          return null;
        }
      }
    }

    /// <summary>Creates a slider reader for the specified object</summary>
    /// <param name="joystick">Joystick providing the control object</param>
    /// <param name="slider">Slider a reader will be created for</param>
    /// <param name="control">Control description for the axis</param>
    /// <returns>A new slider reader for the specified axis</returns>
    private static ISliderReader createSliderReader(
      Joystick joystick, ExtendedSliders slider, DeviceObjectInstance control
    ) {
      int id = (int)control.ObjectType;
      ObjectProperties properties = joystick.GetObjectPropertiesById(id);

      int min = properties.LowerRange;
      int max = properties.UpperRange;

      switch (slider) {
        case ExtendedSliders.Slider1: {
          return new SliderReader(0, min, max);
        }
        case ExtendedSliders.Slider2: {
          return new SliderReader(1, min, max);
        }
        case ExtendedSliders.Velocity1: {
          return new VelocitySliderReader(0, min, max);
        }
        case ExtendedSliders.Velocity2: {
          return new VelocitySliderReader(1, min, max);
        }
        case ExtendedSliders.Acceleration1: {
          return new AccelerationSliderReader(0, min, max);
        }
        case ExtendedSliders.Acceleration2: {
          return new AccelerationSliderReader(1, min, max);
        }
        case ExtendedSliders.Force1: {
          return new ForceSliderReader(0, min, max);
        }
        case ExtendedSliders.Force2: {
          return new ForceSliderReader(1, min, max);
        }
        default: {
          return null;
        }
      }
    }

    /// <summary>Identifies the specified axis in the ExtendedAxes enumeration</summary>
    /// <param name="aspect">Aspect describing the order of the control</param>
    /// <param name="typeGuid">GUID describing the type of control</param>
    /// <returns>The equivalent entry in the ExtendedAxes enumeration or 0</returns>
    private static ExtendedAxes identifyAxis(ObjectAspect aspect, Guid typeGuid) {
      ExtendedAxes axis;

      if (typeGuid == ObjectGuid.XAxis) {
        axis = ExtendedAxes.X;
      } else if (typeGuid == ObjectGuid.YAxis) {
        axis = ExtendedAxes.Y;
      } else if (typeGuid == ObjectGuid.ZAxis) {
        axis = ExtendedAxes.Z;
      } else if (typeGuid == ObjectGuid.RotationalXAxis) {
        axis = ExtendedAxes.RotationX;
      } else if (typeGuid == ObjectGuid.RotationalYAxis) {
        axis = ExtendedAxes.RotationY;
      } else if (typeGuid == ObjectGuid.RotationalZAxis) {
        axis = ExtendedAxes.RotationZ;
      } else {
        return 0;
      }

      if ((aspect & ObjectAspect.Acceleration) == ObjectAspect.Acceleration) {
        if (axis == ExtendedAxes.X) { return ExtendedAxes.AccelerationX; }
        if (axis == ExtendedAxes.Y) { return ExtendedAxes.AccelerationY; }
        if (axis == ExtendedAxes.Z) { return ExtendedAxes.AccelerationZ; }
        if (axis == ExtendedAxes.RotationX) { return ExtendedAxes.AngularAccelerationX; }
        if (axis == ExtendedAxes.RotationY) { return ExtendedAxes.AngularAccelerationY; }
        if (axis == ExtendedAxes.RotationZ) { return ExtendedAxes.AngularAccelerationZ; }
      } else if ((aspect & ObjectAspect.Velocity) == ObjectAspect.Velocity) {
        if (axis == ExtendedAxes.X) { return ExtendedAxes.VelocityX; }
        if (axis == ExtendedAxes.Y) { return ExtendedAxes.VelocityY; }
        if (axis == ExtendedAxes.Z) { return ExtendedAxes.VelocityZ; }
        if (axis == ExtendedAxes.RotationX) { return ExtendedAxes.AngularVelocityX; }
        if (axis == ExtendedAxes.RotationY) { return ExtendedAxes.AngularVelocityY; }
        if (axis == ExtendedAxes.RotationZ) { return ExtendedAxes.AngularVelocityZ; }
      } else if ((aspect & ObjectAspect.Force) == ObjectAspect.Force) {
        if (axis == ExtendedAxes.X) { return ExtendedAxes.ForceX; }
        if (axis == ExtendedAxes.Y) { return ExtendedAxes.ForceY; }
        if (axis == ExtendedAxes.Z) { return ExtendedAxes.ForceZ; }
        if (axis == ExtendedAxes.RotationX) { return ExtendedAxes.TorqueX; }
        if (axis == ExtendedAxes.RotationY) { return ExtendedAxes.TorqueY; }
        if (axis == ExtendedAxes.RotationZ) { return ExtendedAxes.TorqueZ; }
      } else if ((aspect & ObjectAspect.Position) == ObjectAspect.Position) {
        return axis;
      }

      return 0;
    }

    /// <summary>Identifies the specified slider in the ExtendedSliders enumeration</summary>
    /// <param name="aspect">Aspect describing the order of the control</param>
    /// <param name="typeGuid">GUID describing the type of control</param>
    /// <returns>The equivalent entry in the ExtendedSliders enumeration or 0</returns>
    private static ExtendedSliders identifySlider(ObjectAspect aspect, Guid typeGuid) {
      if (typeGuid == ObjectGuid.Slider) {
        if ((aspect & ObjectAspect.Acceleration) == ObjectAspect.Acceleration) {
          return ExtendedSliders.Acceleration1;
        } else if ((aspect & ObjectAspect.Velocity) == ObjectAspect.Velocity) {
          return ExtendedSliders.Velocity1;
        } else if ((aspect & ObjectAspect.Force) == ObjectAspect.Force) {
          return ExtendedSliders.Force1;
        } else if ((aspect & ObjectAspect.Position) == ObjectAspect.Position) {
          return ExtendedSliders.Slider1;
        }
      }

      return 0;
    }

  }

} // namespace Nuclex.Input.Devices
