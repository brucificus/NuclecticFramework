using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input.Devices
{
	public class XnaKeyboardGamePad
		: XnaKeyboard
	{
		/// <summary>Index of the player this device represents</summary>
		private PlayerIndex playerIndex;

		/// <summary>Game pad the chat pad is attached to</summary>
		private IGamePad gamePad;

		/// <summary>Initializes a new XNA-based keyboard device</summary>
		/// <param name="playerIndex">Index of the player whose chat pad will be queried</param>
		/// <param name="gamePad">Game pad the chat pad is attached to</param>
		public XnaKeyboardGamePad(PlayerIndex playerIndex, IGamePad gamePad)
			: base()
		{
		  this.playerIndex = playerIndex;
		  this.gamePad = gamePad;
		}

		/// <summary>Human-readable name of the input device</summary>
		public override string Name
		{
			get { return "Xbox chat pad"; }
		}

		/// <summary>Whether the input device is connected to the system</summary>
		public override bool IsAttached
		{
			get { return this.gamePad.IsAttached; }
		}

		protected override KeyboardState queryKeyboardState()
		{
			if (this.gamePad.IsAttached)
			{
				return Keyboard.GetState(this.playerIndex);
			}
			else
			{
				return new KeyboardState();
			}
		}
	}
}
