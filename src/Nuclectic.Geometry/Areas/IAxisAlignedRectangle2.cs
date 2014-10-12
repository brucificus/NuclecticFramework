namespace Nuclectic.Geometry.Areas
{
	public interface IAxisAlignedRectangle2 : IArea2
	{
		/// <summary>The width of the rectangle</summary>
		float Width { get; }

		/// <summary>The height of the rectangle</summary>
		float Height { get; }
	}
}