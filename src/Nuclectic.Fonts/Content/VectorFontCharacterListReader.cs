using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Nuclectic.Fonts.Content
{
	/// <summary>
	///     Content reader for list of vector font characters
	/// </summary>
	/// <remarks>Exists as a workaround to MonoGame ContentTypeReaderManager not handling open-generics</remarks>
	public class VectorFontCharacterListReader : ListReader<VectorFontCharacter>
	{
	}
}