using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace PortableGameTest.Framework.Input.Devices
{
    public class JoystickStateAdapter : IJoystickState
    {
        private readonly JoystickState _joystickState;

        public JoystickStateAdapter(JoystickState joystickState)
        {
            _joystickState = joystickState;
        }

        public int[] GetPointOfViewControllers()
        {
            return _joystickState.GetPointOfViewControllers();
        }

        public int[] GetSliders()
        {
            return _joystickState.GetSliders();
        }

        public int[] GetVelocitySliders()
        {
            return _joystickState.GetVelocitySliders();
        }

        public int[] GetAccelerationSliders()
        {
            return _joystickState.GetAccelerationSliders();
        }

        public int[] GetForceSliders()
        {
            return _joystickState.GetForceSliders();
        }

        public bool[] GetButtons()
        {
            return _joystickState.GetButtons();
        }

        public bool IsPressed(int button)
        {
            return _joystickState.IsPressed(button);
        }

        public bool IsReleased(int button)
        {
            return _joystickState.IsReleased(button);
        }

        public int TorqueZ
        {
            get { return _joystickState.TorqueZ; }
        }

        public int TorqueY
        {
            get { return _joystickState.TorqueY; }
        }

        public int TorqueX
        {
            get { return _joystickState.TorqueX; }
        }

        public int ForceZ
        {
            get { return _joystickState.ForceZ; }
        }

        public int ForceY
        {
            get { return _joystickState.ForceY; }
        }

        public int ForceX
        {
            get { return _joystickState.ForceX; }
        }

        public int AngularAccelerationZ
        {
            get { return _joystickState.AngularAccelerationZ; }
        }

        public int AngularAccelerationY
        {
            get { return _joystickState.AngularAccelerationY; }
        }

        public int AngularAccelerationX
        {
            get { return _joystickState.AngularAccelerationX; }
        }

        public int AccelerationZ
        {
            get { return _joystickState.AccelerationZ; }
        }

        public int AccelerationY
        {
            get { return _joystickState.AccelerationY; }
        }

        public int AccelerationX
        {
            get { return _joystickState.AccelerationX; }
        }

        public int AngularVelocityZ
        {
            get { return _joystickState.AngularVelocityZ; }
        }

        public int AngularVelocityY
        {
            get { return _joystickState.AngularVelocityY; }
        }

        public int AngularVelocityX
        {
            get { return _joystickState.AngularVelocityX; }
        }

        public int VelocityZ
        {
            get { return _joystickState.VelocityZ; }
        }

        public int VelocityY
        {
            get { return _joystickState.VelocityY; }
        }

        public int VelocityX
        {
            get { return _joystickState.VelocityX; }
        }

        public int RotationZ
        {
            get { return _joystickState.RotationZ; }
        }

        public int RotationY
        {
            get { return _joystickState.RotationY; }
        }

        public int RotationX
        {
            get { return _joystickState.RotationX; }
        }

        public int Z
        {
            get { return _joystickState.Z; }
        }

        public int Y
        {
            get { return _joystickState.Y; }
        }

        public int X
        {
            get { return _joystickState.X; }
        }
    }
}
