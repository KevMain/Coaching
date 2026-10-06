using Xunit;
using Run_coaching;

namespace Paceframe.Web.Tests
{
    public class BrandTests
    {
        [Fact]
        public void PrimaryColor_IsExpectedTeal()
        {
            Assert.Equal("#0B6E62", Brand.PrimaryColor);
        }
    }
}
