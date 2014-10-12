using Nuclectic.UserInterface.Input;

namespace Nuclectic.UserInterface.Controls
{
	public interface ICommandProcessor
	{
		bool ProcessCommand(Command command);
	}
}