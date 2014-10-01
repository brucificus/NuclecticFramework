namespace Nuclectic.UserInterface.Controls
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
