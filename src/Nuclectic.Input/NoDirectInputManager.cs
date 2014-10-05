using System;
using Nuclectic.Input.Devices;

namespace Nuclectic.Input
{
    public class NoDirectInputManager
        : IDirectInputManager
    {
        public bool IsDirectInputAvailable { get { return false; } }
        public IGamePad[] CreateGamePads()
        {
            throw new NotSupportedException();
        }
    }
}
