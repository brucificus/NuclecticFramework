namespace PortableGameTest.Framework.Input.Devices
{
    public interface IJoystickState
    {
        int[] GetPointOfViewControllers();
        int[] GetSliders();
        int[] GetVelocitySliders();
        int[] GetAccelerationSliders();
        int[] GetForceSliders();
        bool[] GetButtons();
        bool IsPressed(int button);
        bool IsReleased(int button);
        int TorqueZ { get; }
        int TorqueY { get; }
        int TorqueX { get; }
        int ForceZ { get; }
        int ForceY { get; }
        int ForceX { get; }
        int AngularAccelerationZ { get; }
        int AngularAccelerationY { get; }
        int AngularAccelerationX { get; }
        int AccelerationZ { get; }
        int AccelerationY { get; }
        int AccelerationX { get; }
        int AngularVelocityZ { get; }
        int AngularVelocityY { get; }
        int AngularVelocityX { get; }
        int VelocityZ { get; }
        int VelocityY { get; }
        int VelocityX { get; }
        int RotationZ { get; }
        int RotationY { get; }
        int RotationX { get; }
        int Z { get; }
        int Y { get; }
        int X { get; }
    }
}