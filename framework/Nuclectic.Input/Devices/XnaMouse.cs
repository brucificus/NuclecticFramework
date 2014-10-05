using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Nuclectic.Input.Devices
{
	public class XnaMouse : IMouse
	{
		public XnaMouse()
		{
			this.states = new Queue<MouseState>();
			this.current = new MouseState();
		}

		public bool IsAttached
		{
			get
			{
				return true;
			}
		}
		public string Name
		{
			get
			{
				return "XNA Mouse";
			}
		}
		/// <summary>Updates the state of the input device</summary>
		/// <remarks>
		///   <para>
		///     If this method is called with no snapshots in the queue, it will take
		///     an immediate snapshot and make it the current state. This way, you
		///     can use the input devices without caring for the snapshot system if
		///     you wish.
		///   </para>
		///   <para>
		///     If this method is called while one or more snapshots are waiting in
		///     the queue, this method takes the next snapshot from the queue and makes
		///     it the current state.
		///   </para>
		/// </remarks>
		public void Update()
		{
			MouseState previous = this.current;

			if (this.states.Count == 0)
			{
				this.current = queryMouseState();
			}
			else
			{
				this.current = this.states.Dequeue();
			}

			generateEvents(ref previous, ref this.current);
		}

		/// <summary>Takes a snapshot of the current state of the input device</summary>
		/// <remarks>
		///   This snapshot will be queued until the user calls the Update() method,
		///   where the next polled snapshot will be taken from the queue and provided
		///   to the user.
		/// </remarks>
		public void TakeSnapshot()
		{
			this.states.Enqueue(queryMouseState());
		}

		public event MouseMoveDelegate MouseMoved;
		public event MouseButtonDelegate MouseButtonPressed;
		public event MouseButtonDelegate MouseButtonReleased;
		public event MouseWheelDelegate MouseWheelRotated;

		public MouseState GetState()
		{
			return this.current;
		}

		public void MoveTo(float x, float y)
		{
			Mouse.SetPosition((int)x, (int)y);
		}

		private MouseState queryMouseState()
		{
			return Mouse.GetState();
		}

		private void generateEvents(ref MouseState previous, ref MouseState current)
		{
			// No subscribers? Don't waste time!
			if ((MouseMoved == null) && (MouseButtonPressed == null) && (MouseButtonReleased == null) && (MouseWheelRotated != null))
			{
				return;
			}

			generateButtonEvents(ref previous, ref current, (ms)=>ms.LeftButton, MouseButtons.Left);
			generateButtonEvents(ref previous, ref current, (ms)=>ms.MiddleButton, MouseButtons.Middle);
			generateButtonEvents(ref previous, ref current, (ms)=>ms.RightButton, MouseButtons.Right);
			generateButtonEvents(ref previous, ref current, (ms)=>ms.XButton1, MouseButtons.X1);
			generateButtonEvents(ref previous, ref current, (ms)=>ms.XButton2, MouseButtons.X2);

			if (previous.Position != current.Position)
				OnMouseMoved(current.X, current.Y);

			if (previous.ScrollWheelValue != current.ScrollWheelValue)
				OnMouseWheelRotated(current.ScrollWheelValue - previous.ScrollWheelValue);
		}

		private void OnMouseWheelRotated(int ticks)
		{
			if (this.MouseWheelRotated != null)
				this.MouseWheelRotated(ticks);
		}

		private void OnMouseMoved(int x, int y)
		{
			if (this.MouseMoved != null)
				this.MouseMoved(x, y);
		}

		private void generateButtonEvents(ref MouseState previous, ref MouseState current, Func<MouseState, ButtonState> getButtonState, MouseButtons mouseButton)
		{
			var previousButtonState = getButtonState(previous);
			var currentButtonState = getButtonState(current);

			if (previousButtonState != currentButtonState)
			{
				if (previousButtonState == ButtonState.Released)
					OnMouseButtonPressed(mouseButton);
				else
					OnMouseButtonReleased(mouseButton);
			}
		}

		private void OnMouseButtonReleased(MouseButtons mouseButton)
		{
			if (this.MouseButtonReleased != null)
				this.MouseButtonReleased(mouseButton);
		}

		private void OnMouseButtonPressed(MouseButtons mouseButton)
		{
			if (this.MouseButtonPressed != null)
				this.MouseButtonPressed(mouseButton);
		}

		/// <summary>Snapshots of the mouse state waiting to be processed</summary>
		private Queue<MouseState> states;

		/// <summary>Currently published mouse state</summary>
		protected MouseState current;
	}
}
