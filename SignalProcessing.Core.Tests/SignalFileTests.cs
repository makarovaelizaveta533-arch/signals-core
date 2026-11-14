using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    namespace SignalProcessing.Core.Tests
    {
        public class SignalFileTests : IDisposable
        {
            private readonly string _filePath;

            public SignalFileTests()
            {
                // Временный файл перед каждым тестом
                _filePath = Path.GetTempFileName();
            }

            public void Dispose()
            {
                // Удаляем после теста
                if (File.Exists(_filePath))
                    File.Delete(_filePath);
            }

            // Тест 15: запись сигнала {0.5, -1.25, 3, 0.1} с частотой 250 Гц и чтение обратно
            [Fact]
            public void WriteAndRead_ValidSignal_ReturnsSameValues()
            {
                var signal = new SingleChannelSignal(250f, new float[] { 0,5f, -1,25f, 3f, 0,1f });

                SingleChannelSignalWriter.SignalWriter(signal, _filePath);
                var readSignal = SingleChannelSignalReader.SignalReader(_filePath);

                Assert.Equal(250f, readSignal.Freq, 1e-5f);
                AssertSignal.Equal(signal.Value, readSignal.Value);
            }

            // Тест 16: файл из одной строки с частотой -> сигнал без отсчетов
            [Fact]
            public void Read_OnlyFrequency_ReturnsEmptySignal()
            {
                File.WriteAllLines(_filePath, new[] { "100" });

                var readSignal = SingleChannelSignalReader.SignalReader(_filePath);

                Assert.Equal(100f, readSignal.Freq, 1e-5f);
                Assert.Empty(readSignal.Value);
            }

            // Тест 17: пустой файл; строка "abc" среди значений; частота "0"
            public static IEnumerable<object[]> InvalidSignalData => new List<object[]>
            {
                new object[] { new string[] { } },                 // пустой файл
                new object[] { new string[] { "100", "abc" } },     // буква вместо числа
                new object[] { new string[] { "0", "1.5" } },       // частота 0
            };

            [Theory]
            [MemberData(nameof(InvalidSignalData))]
            public void Read_InvalidData_ThrowsInvalidDataException(string[] lines)
            {
                File.WriteAllLines(_filePath, lines);

                Assert.Throws<InvalidDataException>(
                    () => SingleChannelSignalReader.SignalReader(_filePath));
            }


        }
    }
}
