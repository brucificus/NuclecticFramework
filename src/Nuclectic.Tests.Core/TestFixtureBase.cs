
namespace Nuclectic.Tests
{
	/// <summary>
	/// Base class for test classes.
	/// </summary>
    public class TestFixtureBase
    {
		/// <summary>
		/// Creates a strict AutoMock context
		/// </summary>
		protected AutoMockWrapper CreateAutoMock()
        {
            return AutoMockWrapper.CreateStrict();
        }

		/// <summary>
		/// Creates a loose AutoMock context
		/// </summary>
		protected AutoMockWrapper CreateAutoMockLoose()
        {
            return AutoMockWrapper.CreateLoose();
        }
    }
}
