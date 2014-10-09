using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nuclectic.Fonts
{
	public interface ITextBatch
	{
		/// <summary>Current concatenated view * projection matrix of the scene</summary>
		Matrix ViewProjection { get; set; }

		/// <summary>Draws a line of text to the screen</summary>
		/// <param name="text">Pregenerated text mesh to draw to the screen</param>
		/// <param name="color">Color and opacity with which to draw the text</param>
		void DrawText(IText text, Color color);

		/// <summary>Draws a line of text to the screen</summary>
		/// <param name="text">Pregenerated text mesh to draw to the screen</param>
		/// <param name="color">Color and opacity with which to draw the text</param>
		/// <param name="transform">Transformation matrix to apply to the text</param>
		void DrawText(IText text, Matrix transform, Color color);

		/// <summary>Draws a line of text to the screen</summary>
		/// <param name="text">Pregenerated text mesh to draw to the screen</param>
		/// <param name="effect">Custom effect with which to draw the text</param>
		void DrawText(IText text, Effect effect);

		/// <summary>Begins a new text rendering batch</summary>
		/// <remarks>
		///   Call this before drawing text with the DrawText() method. For optimal
		///   performance, try to put all your text drawing commands inside as few
		///   Begin()..End() pairs as you can manage.
		/// </remarks>
		void Begin();

		/// <summary>Ends the current text rendering batch</summary>
		/// <remarks>
		///   This method needs to be called each time you call the Begin() method
		///   after all text drawing commands have taken place.
		/// </remarks>
		void End();
	}
}