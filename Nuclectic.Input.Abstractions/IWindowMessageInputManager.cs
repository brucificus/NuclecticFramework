using Nuclectic.Input.Abstractions.Devices;

namespace Nuclectic.Input.Abstractions
{
	public interface IWindowMessageInputManager : IKeyboardSoloFactory, IMouseFactory
    {
        bool IsWindowMessageInputAvailable { get; }
    }
}
