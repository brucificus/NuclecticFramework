using Nuclex.UserInterface;

namespace PortableGameTest.Framework.UserInterface.Controls
{
    public interface IHasSettableScreen
    {
        void SetScreen(IScreen screen);
    }

    public interface IHasScreen
    {
        IScreen Screen { get; }
    }
}
