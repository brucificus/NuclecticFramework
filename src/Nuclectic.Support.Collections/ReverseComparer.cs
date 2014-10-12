#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2013 Nuclex Development Labs

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

using System.Collections.Generic;

namespace Nuclectic.Support.Collections
{
	/// <summary>
	///     Compares two values in reverse or reverses the output of another comparer
	/// </summary>
	/// <typeparam name="TCompared">Type of values to be compared</typeparam>
	public class ReverseComparer<TCompared> : IComparer<TCompared>
	{
		/// <summary>Initializes a new reverse comparer</summary>
		public ReverseComparer()
			: this(Comparer<TCompared>.Default) { }

		/// <summary>
		///     Initializes the comparer to provide the inverse results of another comparer
		/// </summary>
		/// <param name="comparerToReverse">Comparer whose results will be inversed</param>
		public ReverseComparer(IComparer<TCompared> comparerToReverse) { this.comparer = comparerToReverse; }

		/// <summary>Compares the left value to the right value</summary>
		/// <param name="left">Value on the left side</param>
		/// <param name="right">Value on the right side</param>
		/// <returns>The relationship of the two values</returns>
		public int Compare(TCompared left, TCompared right) { return this.comparer.Compare(right, left); // intentionally reversed 
		}

		/// <summary>The default comparer from the .NET framework</summary>
		private IComparer<TCompared> comparer;
	}
} // namespace Nuclex.Support.Collections