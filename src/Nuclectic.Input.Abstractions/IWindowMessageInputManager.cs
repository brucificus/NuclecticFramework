using Nuclectic.Input.Devices;

namespace Nuclectic.Input
{
	public interface IWindowMessageInputManager : IKeyboardSoloFactory, IMouseFactory
	{
		bool IsWindowMessageInputAvailable { get; }
	}
}