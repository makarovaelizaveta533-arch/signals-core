using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalOperationsTests
    {
        // ТЕСТ 18: Sum: {1, 2, 3} и {10, 20} -> {11, 22, 3}, частота исходных сигналов
        [Fact]
        public void Sum_DifferentLengths_ShortTreatedAsZero()
        {
            var a = new SingleChannelSignal(100f, new float[] { 1, 2, 3 });
            var b = new SingleChannelSignal(100f, new float[] { 10, 20 });

            // Передаем как массив
            SingleChannelSignal result = SignalOperations.Sum(new SingleChannelSignal[] { a, b });

            Assert.Equal(100f, result.Freq, 1e-5f);
            AssertSignal.Equal(new float[] { 11, 22, 3 }, result.Value);
        }

        // ТЕСТ 19: Concat: {1, 2} и {3} -> {1, 2, 3}
        [Fact]
        public void Concat_TwoSignals_ReturnsConcatenated()
        {
            var a = new SingleChannelSignal(100f, new float[] { 1, 2 });
            var b = new SingleChannelSignal(100f, new float[] { 3 });

            SingleChannelSignal result = SignalOperations.Concat(new SingleChannelSignal[] { a, b });

            Assert.Equal(100f, result.Freq, 1e-5f);
            AssertSignal.Equal(new float[] { 1, 2, 3 }, result.Value);
        }

        // ТЕСТ 20: Sum и Concat для сигналов с частотами 100 и 200 Гц -> ArgumentException
        // (В задании сказано проверить и Sum, и Concat, поэтому пишем два теста)

        [Fact]
        public void Sum_DifferentFrequencies_ThrowsArgumentException()
        {
            var a = new SingleChannelSignal(100f, new float[] { 1, 2 });
            var b = new SingleChannelSignal(200f, new float[] { 3, 4 });

            var ex = Assert.Throws<ArgumentException>(() =>
            SignalOperations.Sum(new SingleChannelSignal[] { a, b }));

            // Проверяем, что в сообщении об ошибке есть обе частоты
            Assert.Contains("100", ex.Message);
            Assert.Contains("200", ex.Message);
        }

        [Fact]
        public void Concat_DifferentFrequencies_ThrowsArgumentException()
        {
            var a = new SingleChannelSignal(100f, new float[] { 1 });
            var b = new SingleChannelSignal(200f, new float[] { 2 });

            var ex = Assert.Throws<ArgumentException>(() =>
            SignalOperations.Concat(new SingleChannelSignal[] { a, b }));

            Assert.Contains("100", ex.Message);
            Assert.Contains("200", ex.Message);
        }
    }
}
