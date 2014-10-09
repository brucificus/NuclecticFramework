namespace Nuclectic.Geometry
{
	/// <summary>Sides of a plane in 3D space</summary>
	public enum Side
	{
		/// <summary>Negative half space (away from the plane's normal vector)</summary>
		Negative = -1,

		/// <summary>Positive half space (same side as the plane's normal vector)</summary>
		Positive = +1,
	}
}