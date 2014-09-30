namespace Nuclectic.Input.Devices
{
	public class XnaKeyboardSoloFactory : IKeyboardSoloFactory
	{
		public IKeyboard GetKeyboard()
		{
			return new XnaKeyboardSolo();
		}
	}
}
