namespace Nuclectic.Input.Devices
{
	public class XnaMouseFactory
		: IMouseFactory
	{
		public IMouse GetMouse() { return new XnaMouse(); }
	}
}