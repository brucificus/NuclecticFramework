using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public class MarshalOffsetOfByFieldOffsetAttribute
		: IMarshalOffsetOf
	{
		public IntPtr? OffsetOf(Type t, string fieldName)
		{
			var fieldInfo = t.GetField(fieldName);
			if (fieldInfo == null)
				return null;

			var fieldOffsetAttribute = fieldInfo.GetCustomAttributes(typeof (FieldOffsetAttribute), false).FirstOrDefault() as FieldOffsetAttribute;
			if (fieldOffsetAttribute == null)
				return null;

			return (IntPtr?)fieldOffsetAttribute.Value;
		}
	}
}