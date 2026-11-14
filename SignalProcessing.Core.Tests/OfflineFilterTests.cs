using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OfflineFilterTests
    {
        [Fact]
        public void Filter_ConstantSignal_ReturnsSameConstant()
        {
            // Arrange
            float[] data = { 5, 5, 5, 5, 5 };
            // Act
            float[] result = OfflineFilter.Filter(data, 3);
            // Assert
            Assert.Equal(data.Length, result.Length);
            foreach (float value in result)
                Assert.Equal(5f, value, 1e-5f);
        }

        [Fact]
        public void Filter_LinearSignal_Window3_ReturnsExpected()
        {
            float[] data = { 1, 2, 3, 4, 5 };
            float[] result = OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(new float[] { 4f / 3, 2, 3, 4, 14f / 3 }, result);
        }

        [Fact]
        public void Filter_Impulse_Window3_ReturnsExpected()
        {
            float[] data = { 0, 0, 0, 1, 0, 0, 0 };
            float[] result = OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(
                new float[] { 0, 0, 1f / 3, 1f / 3, 1f / 3, 0, 0 },
                result);
        }

        [Fact]
        public void Filter_WindowSize1_ReturnsCopyNotSameObject()
        {
            float[] data = { 1, 2, 3, 4, 5 };
            float[] result = OfflineFilter.Filter(data, 1);

            Assert.NotSame(data, result);
            AssertSignal.Equal(data, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void Filter_InvalidWindowSize_ThrowsArgumentOutOfRangeException(int windowSize)
        {
            float[] data = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>("windowSize",
                () => OfflineFilter.Filter(data, windowSize));
        }

        [Fact]
        public void Filter_NullData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("data",
                () => OfflineFilter.Filter(null, 3));
        }

        [Fact]
        public void Filter_EmptyData_ReturnsEmptyArray()
        {
            float[] result = OfflineFilter.Filter(new float[0], 3);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}



