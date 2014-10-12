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

using System;

namespace Nuclectic.Support.Helpers
{
	/// <summary>Helper methods for enumerations</summary>
	public static class EnumHelper
	{
		/// <summary>Returns the highest value encountered in an enumeration</summary>
		/// <typeparam name="TEnumeration">
		///   Enumeration of which the highest value will be returned
		/// </typeparam>
		/// <returns>The highest value in the enumeration</returns>
		public static TEnumeration GetHighestValue<TEnumeration>()
			where TEnumeration : IComparable
		{
			TEnumeration[] values = GetValues<TEnumeration>();

			// If the enumeration is empty, return nothing
			if (values.Length == 0)
			{
				return default(TEnumeration);
			}

			// Look for the highest value in the enumeration. We initialize the highest value
			// to the first enumeration value so we don't have to use some arbitrary starting
			// value which might actually appear in the enumeration.
			TEnumeration highestValue = values[0];
			for (int index = 1; index < values.Length; ++index)
			{
				if (values[index].CompareTo(highestValue) > 0)
				{
					highestValue = values[index];
				}
			}

			return highestValue;
		}

		/// <summary>Returns the lowest value encountered in an enumeration</summary>
		/// <typeparam name="TEnumeration">
		///   Enumeration of which the lowest value will be returned
		/// </typeparam>
		/// <returns>The lowest value in the enumeration</returns>
		public static TEnumeration GetLowestValue<TEnumeration>()
			where TEnumeration : IComparable
		{
			TEnumeration[] values = GetValues<TEnumeration>();

			// If the enumeration is empty, return nothing
			if (values.Length == 0)
			{
				return default(TEnumeration);
			}

			// Look for the lowest value in the enumeration. We initialize the lowest value
			// to the first enumeration value so we don't have to use some arbitrary starting
			// value which might actually appear in the enumeration.
			TEnumeration lowestValue = values[0];
			for (int index = 1; index < values.Length; ++index)
			{
				if (values[index].CompareTo(lowestValue) < 0)
				{
					lowestValue = values[index];
				}
			}

			return lowestValue;
		}

		/// <summary>Retrieves a list of all values contained in an enumeration</summary>
		/// <typeparam name="TEnum">
		///   Type of the enumeration whose values will be returned
		/// </typeparam>
		/// <returns>All values contained in the specified enumeration</returns>
		/// <remarks>
		///   This method produces collectable garbage so it's best to only call it once
		///   and cache the result.
		/// </remarks>
		public static TEnum[] GetValues<TEnum>() { return (TEnum[])Enum.GetValues(typeof (TEnum)); }
	}
} // namespace Nuclex.Support