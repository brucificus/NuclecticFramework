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

#if !NO_DIRECTINPUT

using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;

using SlimDX.DirectInput;

using Nuclex.Input.Devices;

namespace Nuclex.Input {

  /// <summary>Checks whether the specified device is attached</summary>
  /// <param name="device">Device that will be checked</param>
  /// <returns>True if the device is attached, false otherwise</returns>
  internal delegate bool CheckAttachedDelegate(Device device);

  /// <summary>Manages DirectInput devices</summary>
  internal class DirectInputManager : IDisposable {

    /// <summary>
    ///   Determines whether DirectInput is available on the current system
    /// </summary>
    public static bool IsDirectInputAvailable {
      get {
        IntPtr handle = UnsafeNativeMethods.LoadLibrary("dinput8.dll");
        bool isAvailable = (handle != IntPtr.Zero);

        if (isAvailable) {
          UnsafeNativeMethods.FreeLibrary(handle);
        }

        return isAvailable;
      }
    }

    /// <summary>Initializes a new DirectInput manager</summary>
    /// <param name="windowHandle">Handle of the game's main window</param>
    public DirectInputManager(IntPtr windowHandle) {
      this.directInput = new DirectInput();
    }

    /// <summary>Immediately releases all resources owned by the instance</summary>
    public void Dispose() {
      if (this.directInput != null) {
        this.directInput.Dispose();
        this.directInput = null;
      }
    }

    /// <summary>Whether the device is currently attached</summary>
    /// <param name="device">Device that will be checked</param>
    /// <returns>True if the device is attached, false otherwise</returns>
    public bool IsDeviceAttached(Device device) {
      return this.directInput.IsDeviceAttached(
        device.Information.InstanceGuid
      );
    }

    /// <summary>Creates game pad wrappers for all DirectInput game pads</summary>
    /// <returns>An array with wrappers for all DirectInput game pads</returns>
    public DirectInputGamePad[] CreateGamePads() {
      IList<DeviceInstance> devices = this.directInput.GetDevices(
        DeviceClass.GameController, DeviceEnumerationFlags.AllDevices
      );

      var checkAttachedDelegate = new CheckAttachedDelegate(IsDeviceAttached);
      var gamePads = new List<DirectInputGamePad>();

      for (int index = 0; index < Math.Min(devices.Count, 4); ++index) {

        // Do not enumerate XINPUT-enabled controllers. There are handled by
        // XNA (which is based on XINPUT) itself.
        if (!isXInputDevice(devices[index])) {
          var joystick = new Joystick(this.directInput, devices[index].InstanceGuid);
          gamePads.Add(new DirectInputGamePad(joystick, checkAttachedDelegate));
        }

      }

      return gamePads.ToArray();
    }

    /// <summary>
    ///   Determines whether the specified DirectInput device is handled by XINPUT
    /// </summary>
    /// <param name="deviceInstance">
    ///   The DirectInput device instance that will be checked
    /// </param>
    /// <returns>True if this is a device that is handled by XINPUT</returns>
    /// <remarks>
    ///   <para>
    ///     XINPUT devices are accessable through both DirectInput and XINPUT.
    ///     Since we're already using XINPUT (through XNA), we need to filter out
    ///     any DirectInput devices that we already access through XINPUT, otherwise,
    ///     each XINPUT game controller would appear twice.
    ///   </para>
    ///   <para>
    ///     This method is based on the code from the ZMan's article on detecting
    ///     which DirectInput devices are also XInput devices:
    ///     http://www.thezbuffer.com/articles/351.aspx
    ///   </para>
    /// </remarks>
    private static bool isXInputDevice(DeviceInstance deviceInstance) {

      // For XINPUT controllers, the first 8 characters of the device GUID
      // contain the VID and PID of the USB device
      string productGuid = deviceInstance.ProductGuid.ToString();
      int searchedVid = Convert.ToInt32(productGuid.Substring(4, 4), 16);
      int searchedPid = Convert.ToInt32(productGuid.Substring(0, 4), 16);

      // Query WMI for any plug and play devices with IG_ in their device name,
      // this identifies XInput devices
      var searcher = new ManagementObjectSearcher(
        "select DeviceID from Win32_PNPEntity where DeviceID like '%IG[_]%' "
      );

      // Look for the queried game pad's VID and PID appear the list of
      // XInput devices
      foreach (ManagementObject managementObject in searcher.Get()) {
        object usbId = managementObject["DeviceID"];
        if (usbId == null) {
          continue;
        }

        int vid, pid;
        if (!tryExtractVidPid(usbId.ToString(), out vid, out pid)) {
          continue;
        }

        // If the the queried game pad's VID and PID match, it was in the list
        // of XInput devices, ergo it is an XInput device
        if ((vid == searchedVid) && (pid == searchedPid)) {
          return true;
        }
      }

      // Not found, game pad is not an XInput device
      return false;

    }

    /// <summary>the USB VID and PID from a management object's DeviceID</summary>
    /// <param name="deviceId">DeviceID the USB IDs will be extracted from</param>
    /// <param name="vid">Output parameter that receives the VID</param>
    /// <param name="pid">Output parameter that receives the PID</param>
    /// <returns>True if the VID and PID were successfully extracted</returns>
    private static bool tryExtractVidPid(string deviceId, out int vid, out int pid) {
      int vidIndex = deviceId.IndexOf("VID_");
      if ((vidIndex != -1) && (vidIndex <= deviceId.Length - 8)) {

        int pidIndex = deviceId.IndexOf("PID_");
        if ((pidIndex != -1) && (pidIndex <= deviceId.Length - 8)) {

          vid = Convert.ToInt32(deviceId.Substring(vidIndex + 4, 4), 16);
          pid = Convert.ToInt32(deviceId.Substring(pidIndex + 4, 4), 16);
          return true;

        }
      }

      vid = 0;
      pid = 0;
      return false;
    }

    /// <summary>DirectInput instance used by the manager to query input devices</summary>
    private DirectInput directInput;

  }

} // namespace Nuclex.Input

#endif // !NO_DIRECTINPUT
