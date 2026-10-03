using System.IO; // StreamWriter = писать текстовый файл построчно

namespace SignalParser
{
    // SingleChannelSignalWriter = записывалка сигнала в csv-файл
    internal class SingleChannelSignalWriter
    {
        // static = вызов без new. obj = сигнал. filepath = куда сохранить
        static public void SignalWriter(SingleChannelSignal obj, string filepath)
        {
            // using = открыли файл на запись, в конце блока закроется сам
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                // WriteLine = записать строку и перейти на новую. Сначала частота
                writer.WriteLine(obj.Freq);

                // foreach = для каждого числа el в массиве Value
                foreach (float el in obj.Value)
                {
                    // Записать одно значение сигнала на отдельную строку
                    writer.WriteLine(el);
                }
            }
            // Тут файл уже закрыт
        }
    }
}
