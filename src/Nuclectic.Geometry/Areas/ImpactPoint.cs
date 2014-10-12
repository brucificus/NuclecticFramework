using Microsoft.Xna.Framework;

namespace Nuclectic.Geometry.Areas
{
	/// <summary>Informations about the point of first contact between two bodies</summary>
	public struct ImpactPoint
	{
		/// <summary>Whether a contact could be found at all</summary>
		public bool Found;

		/// <summary>The absolute location where the contact occurs</summary>
		public Vector2 AbsoluteLocation;

		/// <summary>The time at which the contact occurs</summary>
		public float Time;
	}
}