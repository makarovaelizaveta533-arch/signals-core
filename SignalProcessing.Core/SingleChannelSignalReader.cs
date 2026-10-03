using System.Collections.Generic; // List<> = список, который растёт сам
using System.IO; // StreamReader = читать текстовый файл построчно

namespace SignalParser
{
    // SingleChannelSignalReader = читалка сигнала из csv-файла
    internal class SingleChannelSignalReader
    {
        // static = можно вызвать без new: SingleChannelSignalReader.SignalReader(путь)
        // string filePath = путь к файлу на диске
        static public SingleChannelSignal SignalReader(string filePath)
        {
            // using = открыли файл; когда блок закончится — файл закроется сам
            // StreamReader = читатель текста
            using (StreamReader reader = new StreamReader(filePath))
            {
                // ReadLine = прочитать ОДНУ строку. float.Parse = текст → число
                // Первая строка файла у нас = частота
                float freq = float.Parse(reader.ReadLine());

                // List = список чисел сигнала (длина заранее неизвестна)
                List<float> Signal = new List<float>();

                // while(true) = крутиться вечно, пока не сделаем break
                while (true)
                {
                    // Следующая строка файла
                    string line = reader.ReadLine();

                    // null значит «файл закончился» → выходим из цикла
                    if (line == null)
                        break;

                    // Текст строки → число float, добавить в список
                    Signal.Add(float.Parse(line));
                }

                // ToArray = превратить List в обычный массив float[]
                float[] arrSignal = Signal.ToArray();

                // Упаковать частоту + массив в объект сигнала
                SingleChannelSignal data = new SingleChannelSignal(freq, arrSignal);

                // Вернуть сигнал наружу
                return data;
            }
        }
    }
}
