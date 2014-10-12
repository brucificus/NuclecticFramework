namespace Nuclectic.Input.Devices
{
	public class NoKeyboardSoloFactory : IKeyboardSoloFactory
	{
		public IKeyboard GetKeyboard() { return new NoKeyboard(); }
	}
}