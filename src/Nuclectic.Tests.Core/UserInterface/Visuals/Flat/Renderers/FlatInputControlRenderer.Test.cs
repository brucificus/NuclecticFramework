#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2010 Nuclex Development Labs

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

#if UNITTEST

using System;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using NUnit.Framework;
using NMock;

using Nuclex.Input;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Input;

using Is = NMock.Is;

namespace Nuclex.UserInterface.Visuals.Flat.Renderers {

  /// <summary>Unit Test for the flat input control renderer</summary>
  [TestFixture]
  internal class FlatInputControlRendererTest : ControlRendererTest {

    /// <summary>Called before each test is run</summary>
    [SetUp]
    public override void Setup() {
      base.Setup();

      this.input = new InputControl();
      this.input.Text =
        "This is a very long text that will surely extend beyond the width of " +
        "the input box and require clipping to not draw over its borders";
      this.input.Bounds = new UniRectangle(10, 10, 100, 100);
      Screen.Desktop.Children.Add(this.input);

      this.renderer = new FlatInputControlRenderer();
    }

    /// <summary>Verifies that the renderer is able to render the input control</summary>
    [Test, Ignore("Fails for unknown reasons. Investigate.")]
    public void TestRenderNormal() {

      // Make sure the renderer draws at least the input control's text
      MockedGraphics.Expects.AtLeast(0).Method(
        m => m.MeasureString(null, RectangleF.Empty, null)
      ).WithAnyArguments().WillReturn(new RectangleF(0.0f, 0.0f, 200.0f, 10.0f));
      MockedGraphics.Expects.AtLeast(0).Method(
        m => m.SetClipRegion(RectangleF.Empty)
      ).WithAnyArguments();
      MockedGraphics.Expects.AtLeast(0).Method(
        m => m.DrawCaret(null, RectangleF.Empty, null, 0)
      ).WithAnyArguments();

      MockedGraphics.Expects.One.Method(
        m => m.DrawElement(null, RectangleF.Empty)
      ).WithAnyArguments();
      MockedGraphics.Expects.One.Method(
        m => m.DrawString(null, RectangleF.Empty, null)
      ).WithAnyArguments();

      // Let the renderer draw the input control into the mocked graphics interface
      renderer.Render(this.input, MockedGraphics.MockObject);

      // And verify the expected drawing commands were issues
      Mockery.VerifyAllExpectationsHaveBeenMet();

    }

    /// <summary>
    ///   Verifies that the renderer is able to render the input control when it has
    ///   the input focus
    /// </summary>
    [Test, Ignore("Fails for unknown reasons. Investigate.")]
    public void TestRenderFocused() {

      Screen.FocusedControl = this.input;
      Screen.InjectKeyPress(Keys.End); // Inject key to make caret visible

      // Make sure the renderer draws at least the input control's text
      MockedGraphics.Expects.AtLeast(0).Method(
        m => m.MeasureString(null, RectangleF.Empty, null)
      ).WithAnyArguments().WillReturn(new RectangleF(0.0f, 0.0f, 200.0f, 10.0f));
      MockedGraphics.Expects.AtLeast(0).Method(
        m => m.SetClipRegion(RectangleF.Empty)
      ).WithAnyArguments();
      MockedGraphics.Expects.AtLeast(0).Method(
        m => m.DrawCaret(null, RectangleF.Empty, null, 0)
      ).WithAnyArguments();

      MockedGraphics.Expects.One.Method(
        m => m.DrawElement(null, RectangleF.Empty)
      ).WithAnyArguments();
      MockedGraphics.Expects.AtLeastOne.Method(
        m => m.DrawString(null, RectangleF.Empty, null)
      ).WithAnyArguments();

      // Let the renderer draw the input control into the mocked graphics interface
      renderer.Render(this.input, MockedGraphics.MockObject);

      // And verify the expected drawing commands were issues
      Mockery.VerifyAllExpectationsHaveBeenMet();

    }

    /// <summary>
    ///   Verifies that the renderer can provide the control with the text opening
    ///   the user has clicked into to position the caret.
    /// </summary>
    [Test, Ignore("Fails for unknown reasons. Investigate.")]
    public void TestOpeningLocator() {
      TestRenderNormal();
    
      Screen.InjectMouseMove(25.0f, 25.0f);

      MockedGraphics.Expects.AtLeastOne.Method(
        m => m.GetClosestOpening(null, RectangleF.Empty, null, Vector2.Zero)
      ).With(Is.Anything, Is.Anything, this.input.Text, Is.Anything).WillReturn(12);

      Screen.InjectMousePress(MouseButtons.Left);
      Screen.InjectMouseRelease(MouseButtons.Left);
    }

    /// <summary>Input control renderer being tested</summary>
    private FlatInputControlRenderer renderer;
    /// <summary>Input control used to test the input control renderer</summary>
    private InputControl input;

  }

} // namespace Nuclex.UserInterface.Visuals.Flat.Renderers

#endif // UNITTEST
