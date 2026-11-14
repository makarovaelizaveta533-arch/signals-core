using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    internal class SignalOperationsTests
    {
        [Fact]
        public void Sum_TwoSameLengthSignals_ReturnsElementwiseSum()
        {
            var a = new SingleChannelSignal(100f, new float[] { 1, 2, 3 });
            var b = new SingleChannelSignal(100f, new float[] { 10, 20, 30 });

            SingleChannelSignal result = Core.SignalOperations.Sum(new[] { a, b });

            Assert.Equal(100f, result.Freq, 1e-5f);
            AssertSignal.Equal(new float[] { 11, 22, 33 }, result.Value);
        }

        [Fact]
        public void Sum_DifferentLengths_ShortTreatedAsZero()
        {
            var a = new SingleChannelSignal(100f, new float[] { 1, 2, 3, 4 });
            var b = new SingleChannelSignal(100f, new float[] { 10, 20 });

            SingleChannelSignal result = Core.SignalOperations.Sum(new [] { a, b });

            Assert.Equal(4, result.Value.Length);
            AssertSignal.Equal(new float[] { 11, 22, 3, 4 }, result.Value);
        }

    }
}
