#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/
#endregion

using PortableGameTest.Framework.Input.Devices;
#if !NO_DIRECTINPUT
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

using SlimDX;
using SlimDX.DirectInput;
using Microsoft.Xna.Framework;

namespace Nuclex.Input.Devices {

  /// <summary>Interfaces with a game pad-like controller through DirectInput</summary>
  internal class DirectInputGamePad : GamePad, IDisposable {

    /// <summary>Initializes a new DirectInput-based game pad</summary>
    /// <param name="joystick">The DirectInput joystick this instance will query</param>
    /// <param name="checkAttachedDelegate">
    ///   Delegate through which the instance can check if the device is attached
    /// </param>
    public DirectInputGamePad(
      Joystick joystick, CheckAttachedDelegate checkAttachedDelegate
    ) {
      this.joystick = joystick;
      this.checkAttachedDelegate = checkAttachedDelegate;
      this.states = new LeakyQueue<JoystickState>();
      this.converter = new DirectInputConverter(this.joystick);

      // Ensure the leaky queue has created its array
      ensureSlotAvailable();
    }

    /// <summary>Immediately releases all resources owned by the instance</summary>
    public void Dispose() {
      if (this.joystick != null) {
        this.joystick.Unacquire();
        this.joystick.Dispose();
      }
    }

    /// <summary>Retrieves the current state of the game pad</summary>
    /// <returns>The current state of the game pad</returns>
    public override GamePadState GetState() {
      return this.converter.Convert(new JoystickStateAdapter(this.states.Items[this.states.HeadIndex]));
    }

    /// <summary>Retrieves the current DirectInput joystick state</summary>
    /// <returns>The current state of the DirectInput joystick</returns>
    public override IExtendedGamePadState GetExtendedState() {
      return new ExtendedGamePadState(
        this.converter, new JoystickStateAdapter(this.states.Items[this.states.HeadIndex])
      );
    }

    /// <summary>Whether the input device is connected to the system</summary>
    public override bool IsAttached {
      get { return this.checkAttachedDelegate(this.joystick); }
    }

    /// <summary>Human-readable name of the input device</summary>
    public override string Name {
      get { return this.joystick.Information.InstanceName; }
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
    public override void Update() {
      int previousIndex = this.states.HeadIndex;

      // If no states have been captured as snapshots, query a new state.
      // We also advance the queue so that we have a previous state we
      // can compare against to generate events.
      if (this.states.Count == 0) {
        this.states.AdvanceTail();
        ensureSlotAvailable();
        queryState();
      }

      this.states.AdvanceHead();

      generateEvents(
        this.states.Items[previousIndex],
        this.states.Items[this.states.HeadIndex]
      );
    }

    /// <summary>Takes a snapshot of the current state of the input device</summary>
    /// <remarks>
    ///   This snapshot will be queued until the user calls the Update() method,
    ///   where the next polled snapshot will be taken from the queue and provided
    ///   to the user.
    /// </remarks>
    public override void TakeSnapshot() {
      ensureSlotAvailable();
      queryState();
      this.states.AdvanceTail();
    }

    /// <summary>Tries to retrieve the current state of the input device</summary>
    /// <returns>True if the state was successfully retrieved</returns>
    private bool queryState() {
      Result result;

      // DirectInput devices should be acquired before data can be obtained from
      // them. However, they can unacquire themselves when the user Alt+Tabs out,
      // after which all input retrieval methods return DIERR_INPUTLOST. If we got
      // a DIERR_INPUTLOST (or never acquired the device before), we will start
      // by attempting to acquire the device. If it fails, we'll try again in
      // the next update cycle.
      if (!this.currentlyAcquired) {
        result = this.joystick.Acquire();
        if (result == ResultCode.InputLost) {
          return false;
        }

        this.currentlyAcquired = true;
      }

      // Some devices which do not generate events need to be polled. According
      // to the docs, calling this method when the device doesn't need to be polled
      // is very fast and causes no damage.
      result = this.joystick.Poll();
      if (result == ResultCode.InputLost) {
        this.currentlyAcquired = false;
        return false;
      }

      // Finally, take the current state from the device. Using the ref overload
      // of SlimDX, this can be done without producing a single byte of garbage.
      if (this.states.Items[this.states.TailIndex] == null) {
        this.states.Items[this.states.TailIndex] = new JoystickState();
      }
      result = this.joystick.GetCurrentState(ref this.states.Items[this.states.TailIndex]);
      if (result == ResultCode.InputLost) {
        this.currentlyAcquired = false;
        return false;
      }

      return true;
    }

    /// <summary>
    ///   Ensures that another slot if available in the joystick state queue
    /// </summary>
    private void ensureSlotAvailable() {
      this.states.EnsureSlotAvailable();

      JoystickState[] items = this.states.Items;
      int tailIndex = this.states.TailIndex;
      if (items[tailIndex] == null) {
        items[tailIndex] = new JoystickState();
      }
    }

    /// <summary>Generates events for the changes between two states</summary>
    /// <param name="previous">Previous state for the comparison</param>
    /// <param name="current">Current state for the comparison</param>
    private void generateEvents(JoystickState previous, JoystickState current) {
      bool haveExtendedEventSubscribers = HaveExtendedEventSubscribers;
      if (!HaveEventSubscribers && !haveExtendedEventSubscribers) {
        return;
      }

      bool[] previousButtons = previous.GetButtons();
      bool[] currentButtons = current.GetButtons();

      if (haveExtendedEventSubscribers) {
        generateAllEvents(previousButtons, currentButtons);
      } else {
        generateStandardEventsOnly(previousButtons, currentButtons);
      }
    }

    /// <summary>Generates events for all 128 possible buttons</summary>
    /// <param name="previousButtons">Previous state of all buttons</param>
    /// <param name="currentButtons">Current state of all buttons</param>
    private void generateAllEvents(bool[] previousButtons, bool[] currentButtons) {

      // Collect changes to the first 64 buttons
      ulong pressed1 = 0, released1 = 0;
      {
        int count = Math.Min(this.converter.ButtonCount, 64);
        for (int index = 0; index < count; ++index) {
          bool currentState = currentButtons[index];
          if (previousButtons[index] != currentState) {
            if (currentState) {
              pressed1 |= 1UL << index;
            } else {
              released1 |= 1UL << index;
            }
          }
        }
      }

      // Collect changes to the second 64 buttons
      ulong pressed2 = 0, released2 = 0;
      {
        int count = Math.Min(this.converter.ButtonCount, 128);
        for (int index = 64; index < count; ++index) {
          bool currentState = currentButtons[index];
          if (previousButtons[index] != currentState) {
            if (currentState) {
              pressed2 |= 1UL << (index - 64);
            } else {
              released2 |= 1UL << (index - 64);
            }
          }
        }
      }

      // If there are subscribers to the standard events, notify them
      if (HaveEventSubscribers) {
        Buttons released = ExtendedGamePadState.ButtonsFromExtendedButtons(released1);
        Buttons pressed = ExtendedGamePadState.ButtonsFromExtendedButtons(pressed1);

        // If any buttons have been pressed or released, fire the corresponding events
        if (released != 0) {
          OnButtonReleased(released);
        }
        if (pressed != 0) {
          OnButtonPressed(pressed);
        }
      }

      // If any changes were detected, fire the corresponding events
      if ((released1 != 0) || (released2 != 0)) {
        OnExtendedButtonReleased(released1, released2);
      }
      if ((pressed1 != 0) || (pressed2 != 0)) {
        OnExtendedButtonPressed(pressed1, pressed2);
      }

    }

    /// <summary>Generates events only for the standard XNA buttons</summary>
    /// <param name="previousButtons">Previous state of all buttons</param>
    /// <param name="currentButtons">Current state of all buttons</param>
    private void generateStandardEventsOnly(bool[] previousButtons, bool[] currentButtons) {
      int count = Math.Min(
        this.converter.ButtonCount, ExtendedGamePadState.ButtonOrder.Length
      );

      Buttons pressed = 0, released = 0;
      for (int index = 0; index < count; ++index) {
        bool currentState = currentButtons[index];
        if (previousButtons[index] != currentState) {
          if (currentState) {
            pressed |= ExtendedGamePadState.ButtonOrder[index];
          } else {
            released |= ExtendedGamePadState.ButtonOrder[index];
          }
        }
      }

      // If any buttons have been pressed or released, fire the corresponding events
      if (released != 0) {
        OnButtonReleased(released);
      }
      if (pressed != 0) {
        OnButtonPressed(pressed);
      }
    }

    /// <summary>The DirectInput joystick wrapped by this instance</summary>
    private Joystick joystick;

    /// <summary>
    ///   Delegate through which the instance can check if the device is attached
    /// </summary>
    private CheckAttachedDelegate checkAttachedDelegate;

    /// <summary>Whether the device should currently be in the acquired state</summary>
    private bool currentlyAcquired;

    /// <summary>The joystick states as provided by DirectInput</summary>
    private LeakyQueue<JoystickState> states;

    /// <summary>Converts joystick states into game pad states</summary>
    private DirectInputConverter converter;

  }

} // namespace Nuclex.Input.Devices

#endif // !NO_DIRECTINPUT
