using Homework.Enverus.InternationalRigCountImport.Core.Extensions;

namespace Homework.Enverus.InternationalRigCountImport.Test.Unit.Utilities
{
    public  class EnumerableExtensionTest
    {
        [Xunit.Theory]
        [InlineData(new byte[]{})]
        [InlineData(null)]
        public void IsNullOrEmpty_IsNullOrEmpty(byte[]? input)
        {
            Assert.True(input.IsNullOrEmpty());
        }
        [Xunit.Theory]
        [InlineData(new byte[] {22,14 })]
        public void IsNullOrEmpty_NotNullOrEmpty(byte[]? input)
        {
            Assert.False(input.IsNullOrEmpty());
        }
    }
}
