using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace SignalProcessing.Core.Tests
{
    public class SingleChannelSignalTests
    {
        [Fact]
        public void Constructor_NullData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("value",
                () => new SingleChannelSignal(100f, null));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_NonPositiveFrequency_ThrowsArgumentOutOfRangeException(float frequency)
        {
            float[] data = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>("freq",
                () => new SingleChannelSignal(frequency, data));
        }
    }
}
