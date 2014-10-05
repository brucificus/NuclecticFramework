using System;
using PortableGameTest.Core;

namespace PortableGameTest_WinRT
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var factory = new MonoGame.Framework.GameFrameworkViewSource<Game<GameWinRTPlatform>>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
        }
    }
}
