using System;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Support.Helpers.InteropServices.Marshal;

namespace Nuclectic.Graphics.Helpers
{
	public class VertexDefinitionBuilder
	{
		private readonly IMarshalOffsetOf _offsetOfProvider;

		public VertexDefinitionBuilder(IMarshalOffsetOf offsetOfProvider)
		{
			_offsetOfProvider = offsetOfProvider;
		}

		/// <summary>
		///   Builds a vertex element list that can be used to construct a vertex declaration
		///   from a vertex structure that has the vertex element attributes applied to it
		/// </summary>
		/// <typeparam name="VertexType">
		///   Vertex structure with vertex element attributes applied to it
		/// </typeparam>
		/// <returns>
		///   A vertex element list that can be used to create a new vertex declaration matching
		///   the provided vertex structure
		/// </returns>
		public VertexElement[] BuildElementList<VertexType>() where VertexType : struct
		{
			FieldInfo[] fields = getFields<VertexType>();

			int fieldOffset = 0;
			int elementCount = 0;

			// Set up an array for the vertex elements and fill it with the data we
			// gather directly from the vertex structure
			VertexElement[] elements = new VertexElement[fields.Length];
			for (int index = 0; index < fields.Length; ++index)
			{

				// Find out whether this field is used by the vertex shader. If so, add
				// it to the elements list so it ends up in the vertex shader.
				VertexElementAttribute attribute = getVertexElementAttribute(fields[index]);
				if (attribute != null)
				{
					buildVertexElement(fields[index], attribute, ref elements[elementCount]);


#if !(XBOX360 || WINDOWS_PHONE)
					fieldOffset = _offsetOfProvider.OffsetOf(typeof(VertexType), fields[index].Name).Value.ToInt32();
#endif
					elements[elementCount].Offset = (short)fieldOffset;

					++elementCount;
				}

#if XBOX360
        fieldOffset += Marshal.SizeOf(fields[index].FieldType);
#endif
			}

			// If there isn't a single vertex element, this type would be completely useless
			// as a vertex. Probably the user forgot to add the VertexElementAttribute.
			if (elementCount == 0)
			{
				throw new InvalidOperationException(
					"No fields had the VertexElementAttribute assigned to them."
					);
			}

			Array.Resize(ref elements, elementCount);
			return elements;
		}

		/// <summary>Retrieves the fields declared in a vertex structure</summary>
		/// <typeparam name="VertexType">Type the fields will be retrieved from</typeparam>
		/// <returns>The list of fields declared in the provided structure</returns>
		private static FieldInfo[] getFields<VertexType>() where VertexType : struct
		{
			Type vertexType = typeof(VertexType);

			// Obtain a list of all the fields (object member variables) in the vertex type
			FieldInfo[] fields = vertexType.GetFields().Where(f => f.IsStatic == false).ToArray();
			if (fields.Length == 0)
			{
				throw new InvalidOperationException("Specified vertex type has no fields");
			}

			return fields;
		}

		/// <summary>Builds a vertex element from an attributed field in a structure</summary>
		/// <param name="fieldInfo">
		///   Reflected data on the field for which a vertex element will be built
		/// </param>
		/// <param name="attribute">Vertex eelement attribute assigned to the field</param>
		/// <param name="element">
		///   Output parameter the newly built vertex element is stored in
		/// </param>
		private static void buildVertexElement(
			FieldInfo fieldInfo, VertexElementAttribute attribute, ref VertexElement element
			)
		{
			element.VertexElementUsage = attribute.Usage;
			element.UsageIndex = attribute.UsageIndex;

			// Was an explicit data type provided for this field?
			if (attribute.FormatProvided == true)
			{
				element.VertexElementFormat = attribute.Format;
			}
			else
			{ // Nope, try to auto-detect the data type
				if (fieldInfo.FieldType == typeof(Vector2))
				{
					element.VertexElementFormat = VertexElementFormat.Vector2;
				}
				else if (fieldInfo.FieldType == typeof(Vector3))
				{
					element.VertexElementFormat = VertexElementFormat.Vector3;
				}
				else if (fieldInfo.FieldType == typeof(Vector4))
				{
					element.VertexElementFormat = VertexElementFormat.Vector4;
				}
				else if (fieldInfo.FieldType == typeof(Color))
				{
					element.VertexElementFormat = VertexElementFormat.Color;
				}
				else if (fieldInfo.FieldType == typeof(float))
				{
					element.VertexElementFormat = VertexElementFormat.Single;
				}
				else if (fieldInfo.FieldType == typeof(int))
				{
					element.VertexElementFormat = VertexElementFormat.Short4;
				}
				else if (fieldInfo.FieldType == typeof(short))
				{
					element.VertexElementFormat = VertexElementFormat.Short2;
				}
				else
				{ // No success in auto-detection, give up
					throw new InvalidOperationException(
						"Unrecognized field type, please specify vertex format explicitly"
						);
				}
			}
		}


		/// <summary>
		///   Retrieves the vertex element attribute assigned to a field in a structure
		/// </summary>
		/// <param name="fieldInfo">
		///   Informations about the vertex element field the attribute is retrieved for
		/// </param>
		/// <returns>The vertex element attribute of the requested field</returns>
		private static VertexElementAttribute getVertexElementAttribute(FieldInfo fieldInfo)
		{
			var attributes = fieldInfo.GetCustomAttributes(
				typeof(VertexElementAttribute), false
				).ToArray();

			// The docs state that if the requested attribute has not been applied to the field,
			// an array of length 0 will be returned.
			if (attributes.Length == 0)
			{
				return null;
			}

			return (VertexElementAttribute)attributes[0];
		}
	}
}