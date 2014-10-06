"Nuclectic Framework" is a <a href="http://www.monogame.net/">MonoGame</a>/<a href="http://www.hanselman.com/blog/CrossPlatformPortableClassLibrariesWithNETAreHappening.aspx">PCL</a> port of the "<a href="http://nuclexframework.codeplex.com/">Nuclex Framework</a>" originally for <a href="http://en.wikipedia.org/wiki/Microsoft_XNA">XNA</a>.
It contains a set of fast and elegant components that take care of the grunt work required to implement certain features in your MonoGame game. 
All components are designed with minimal (yet flexible) dependencies, so you can just pick what you really need, or extend and experiment.

Available Packages
------------------

The following functionality is available via NuGet pre-release packages:

- <b>Nuclectic.Game.State</b> - Provides game state management.
- <b>Nuclectic.Geometry</b> - Extends MonoGame with additional geometry operations. Extensions for Vector*, Point, &amp; Matrix. Also primitives for 1- and 2-dimensional shapes, (within 2-dimensional space), with simplistic collision detection.
- <b>Nuclectic.Input</b> - Provides an InputManager that can poll for input from keyboard, gamepads, mouse, and touchpanels.
- <b>Nuclectic.Game.Packing</b> - Provides rectangle packing algorithms useful for building sprite sheets.

Known Issues
------------

- I can't install a Nuclectic package because of the target framework: Make sure your project targets .NET 4.5 and try again.
- When I install a Nuclectic package my build breaks: Remove the MonoGame reference and re-add the correct one.
- This package list seems short: Nuclectic Framework currently consists of 26 published pre-release NuGet packages. Some of these packages aren't worth mentioning yet, and others are intermediary packages to help keep Nuclectic as decoupled as possible.
- Vector font support is missing: The content importer for vector fonts needs to be ported to the MonoGame Content Pipeline before publishing the runtime code makes sense.
- The InputManager seems crippled: As part of decoupling and PCLing the framework, InputManager functionality is now spread out across multiple packages.
- Constructors are more complicated: See above. Dependency injection will likely be a stronger requirement with Nuclectic, more to come on this later.
- The UserInterface library is missing: This is another of the things that are too broken to publish yet.

<h2>Why This Isn't The Framework You Remember</h2>

The original Nuclex Framework was a glorious thing. It offered a lot of helpful functionality to hobbiests that wanted to tinker with XNA without constantly re-inventing the wheel. As XNA expanded across multiple platforms, the Nuclex Framework and it's build system became more and more complex. Then, as XNA died, <a href="http://www.codeplex.com/site/users/view/Cygon">Cygon</a>'s <a href="http://nuclexframework.codeplex.com/releases/">latest release</a> of Nuclex in March 2011 became the <i>last</i> release.

Cue the advent of both MonoGame and PCLs. While PCLs are useful for making universal code libraries, a lot of Nuclex's original functionality (and a lot of game development) is still platform specific. Combined with the fact that MonoGame target platforms do not coincide with .NET PCL platforms, this creates a need for a vastly different library and object structure than what Nuclex originally had.

The Nuclectic Framework, as under development at this time, is the result of multiple iterations at attempting to port Nuclex to MonoGame. It is not meant to be something that is "done", but something that is <i>beginning</i>. And the goal is not simply to be Nuclex for MonoGame. The future holds bright possibilities.

So, given all that, we accept pull requests.