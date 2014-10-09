namespace Nuclectic.Input.Devices
{
	public class NoMouseFactory : IMouseFactory
	{
		public IMouse GetMouse() { return new NoMouse(); }
	}
}