using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
        public static class SignalOperations
        {

            /// Поэлементная сумма сигналов. Длина результата равна наибольшей длине;
            /// короткий сигнал за пределами своей длины считается нулевым.
            /// signals равен null или содержит null.
            /// Список пуст или частоты различаются более чем на 1e-5 Гц.
            public static SingleChannelSignal Sum(IReadOnlyList<SingleChannelSignal> signals)
            {
                if (signals == null)
                    throw new ArgumentNullException(nameof(signals));
                if (signals.Count == 0)
                    throw new ArgumentException("Список сигналов пуст", nameof(signals));

                float freq = CheckSignalsAndGetFrequency(signals);

                int maxLength = 0;
                foreach (var s in signals)
                    if (s.Value.Length > maxLength)
                        maxLength = s.Value.Length;

                var result = new float[maxLength];
                foreach (var s in signals)
                    for (int i = 0; i < s.Value.Length; i++)
                        result[i] += s.Value[i];

                return new SingleChannelSignal(freq, result);
            }
            // последовательная склейка сигналов в порядке следования в списке
            public static SingleChannelSignal Concat(IReadOnlyList<SingleChannelSignal> signals)
            {
                if (signals == null)
                    throw new ArgumentNullException(nameof(signals));
                if (signals.Count == 0)
                    throw new ArgumentException("Список сигналов пуст", nameof(signals));

                float freq = CheckSignalsAndGetFrequency(signals);

                int totalLength = 0;
                foreach (var s in signals)
                    totalLength += s.Value.Length;

                var result = new float[totalLength];
                int offset = 0;
                foreach (var s in signals)
                {
                    Array.Copy(s.Value, 0, result, offset, s.Value.Length);
                    offset += s.Value.Length;
                }

                return new SingleChannelSignal(freq, result);
            }

            private static float CheckSignalsAndGetFrequency(IReadOnlyList<SingleChannelSignal> signals)
            {
                foreach (var s in signals)
                    if (s == null)
                        throw new ArgumentNullException(nameof(signals), "Список содержит null");

                float frequency = signals[0].Freq;
                foreach (var s in signals)
                    if (Math.Abs(s.Freq - frequency) > 1e-5f)
                        throw new ArgumentException(
                        string.Format("Частоты сигналов различаются: {0} и {1} Гц", frequency, s.Freq),
                        nameof(signals));

                return frequency;
            }
        }
    }


