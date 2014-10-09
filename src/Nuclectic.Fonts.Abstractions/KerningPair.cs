namespace Nuclectic.Fonts
{
	/// <summary>Pair of characters for kerning informations</summary>
	public struct KerningPair
	{
		/// <summary>Initializes a new kerning pair</summary>
		/// <param name="left">Left character of the kerning pair</param>
		/// <param name="right">Right character of the kerning pair</param>
		public KerningPair(char left, char right)
		{
			this.Left = left;
			this.Right = right;
		}

		/// <summary>The left character in the kerning pair</summary>
		public char Left;

		/// <summary>The right character in the kerning pair</summary>
		public char Right;

		/// <summary>Returns a hash code for the kerning pair</summary>
		/// <returns>A hash code for the kerning pair</returns>
		public override int GetHashCode() { return ((int)this.Left) * 65536 + ((int)this.Right); }

		/// <summary>Compares this object to another object</summary>
		/// <param name="other">Object to compare to</param>
		/// <returns>True if both objects are identical</returns>
		public override bool Equals(object other)
		{
			if (!(other is KerningPair))
				return false;

			KerningPair kerningPair = (KerningPair)other;

			return
				(kerningPair.Left == this.Left) &&
				(kerningPair.Right == this.Right);
		}
	}
}