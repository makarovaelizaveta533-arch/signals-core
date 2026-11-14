using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SignalProcessing.Core
{
    public class SingleChannelSignalReader
    {
        static public SingleChannelSignal SignalReader(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Первая строка = частота
                string freqLine = reader.ReadLine();

                // Пустой файл: строки с частотой нет
                if (freqLine == null)
                    throw new InvalidDataException("Файл пуст: отсутствует частота.");

                // TryParse не бросает исключение, а возвращает false при ошибке
                if (!float.TryParse(freqLine, NumberStyles.Float, CultureInfo.InvariantCulture, out float freq))
                    throw new InvalidDataException($"Некорректная частота: '{freqLine}'.");

                if (freq <= 0)
                    throw new InvalidDataException("Частота должна быть больше нуля.");

                // Остальные строки = отсчёты сигнала
                List<float> signal = new List<float>();
                int lineNumber = 1;

                while (true)
                {
                    string line = reader.ReadLine();
                    lineNumber++;

                    // null значит, что файл закончился
                    if (line == null)
                        break;

                    if (!float.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                        throw new InvalidDataException($"Некорректное значение в строке {lineNumber}: '{line}'.");

                    signal.Add(value);
                }

                float[] arrSignal = signal.ToArray();

                return new SingleChannelSignal(freq, arrSignal);
            }
        }
    }
}
