using Microsoft.Xna.Framework.Input;

namespace Nuclectic.Input.Devices
{
	public class XnaKeyboardSolo
		: XnaKeyboard
	{
		public XnaKeyboardSolo() { }

		/// <summary>Human-readable name of the input device</summary>
		public override string Name { get { return "Keyboard"; } }

		/// <summary>Whether the input device is connected to the system</summary>
		public override bool IsAttached { get { return true; } }

		protected override KeyboardState queryKeyboardState() { return Keyboard.GetState(); }
	}
}