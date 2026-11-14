using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OnlineFilterTests
    {
        [Fact]
        public void Filter_Signal_OneCall_ReturnsExpected()
        {
            var filter = new OnlineFilter(3);
            float[] result = filter.Filter(new float[] { 1, 2, 3, 4, 5 });

            AssertSignal.Equal(new float[] { 1, 4f / 3, 2, 3, 4 }, result);
        }

        [Fact]
        public void Filter_TwoCalls_EqualsOneCall()
        {
            float[] expected = new OnlineFilter(3).Filter(new float[] { 1, 2, 3, 4, 5 });

            var filter = new OnlineFilter(3);
            var result = new List<float>();
            result.AddRange(filter.Filter(new float[] { 1, 2 }));
            result.AddRange(filter.Filter(new float[] { 3, 4, 5 }));


        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(100)]
        public void Filter_RandomSignal_ResultIndependentOfChunking(int chunkSize)
        {
            var random = new Random(42);
            float[] data = new float[1000];
            for (int i = 0; i < data.Length; i++)
                data[i] = (float)(random.NextDouble() * 2 - 1);

            float[] expected = new OnlineFilter(7).Filter(data);

            var filter = new OnlineFilter(7);
            var actual = new List<float>();
            for (int i = 0; i < data.Length; i += chunkSize)
            {
                int n = Math.Min(chunkSize, data.Length - i);
                float[] chunk = new float[n];
                Array.Copy(data, i, chunk, 0, n);
                actual.AddRange(filter.Filter(chunk));
            }

            AssertSignal.Equal(expected, actual.ToArray());
        }

        [Fact]
        public void Filter_RandomSignal_MatchesOfflineWithDelay()
        {
            var random = new Random(42);
            float[] data = new float[1000];
            for (int i = 0; i < data.Length; i++)
                data[i] = (float)(random.NextDouble() * 2 - 1);

            int windowSize = 7;
            int m = (windowSize - 1) / 2;

            float[] online = new OnlineFilter(windowSize).Filter(data);
            float[] offline = OfflineFilter.Filter(data, windowSize);

            for (int i = m; i < data.Length; i++)
                Assert.Equal(offline[i - m], online[i], 1e-5f);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidWindowSize_ThrowsArgumentOutOfRangeException(int windowSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>("windowSize",
                () => new OnlineFilter(windowSize));
        }
    }
}