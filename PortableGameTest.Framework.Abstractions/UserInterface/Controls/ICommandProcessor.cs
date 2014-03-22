using Nuclex.UserInterface.Input;

namespace PortableGameTest.Framework.UserInterface.Controls
{
    public interface ICommandProcessor
    {
        bool ProcessCommand(Command command);
    }
}