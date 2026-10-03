using System; // EventHandler, EventArgs — система событий C#

namespace SignalParser
{
    // SingleChannelRangeAnalyzer = искатель «зашкалов»
    // Зашкал = кусок, где значение < min ИЛИ значение > max
    internal class SingleChannelRangeAnalyzer
    {
        private float min;       // нижняя граница нормы
        private float max;       // верхняя граница нормы
        private int minDuration; // минимальная длина зашкала в точках

        // event = «колокольчик». Кто подписался (+ =), того вызовут при находке
        // OutOfRangeEventArgs = коробочка с StartIndex и EndIndex
        public event EventHandler<OutOfRangeEventArgs> OutOfRangeDetected;

        // Конструктор: new SingleChannelRangeAnalyzer(min, max, мин_длина)
        public SingleChannelRangeAnalyzer(float min, float max, int minDurationSamples)
        {
            this.min = min;                         // сохранить нижнюю границу
            this.max = max;                         // сохранить верхнюю границу
            this.minDuration = minDurationSamples;  // сохранить мин. длину
        }

        // Analyze = пробежать по массиву data и найти все зашкалы
        public void Analyze(float[] data)
        {
            int i = 0; // текущий индекс точки

            // while = цикл «пока условие истинно»
            while (i < data.Length)
            {
                // || значит ИЛИ. Если точка ниже min ИЛИ выше max — это выход за диапазон
                if (data[i] < min || data[i] > max)
                {
                    int start = i; // запомнили начало зашкала

                    // Внутренний while: идём вперёд, пока зашкал продолжается
                    while (i < data.Length && (data[i] < min || data[i] > max))
                        i++; // шаг на следующую точку

                    // end = последняя плохая точка (i уже на хорошей или за концом массива)
                    int end = i - 1;

                    // Длина куска = end - start + 1. Если >= minDuration — сообщаем
                    if (end - start + 1 >= minDuration)
                        // ?.Invoke = вызвать событие, НО только если кто-то подписан
                        // this = мы сами (анализатор). new OutOfRangeEventArgs = данные
                        OutOfRangeDetected?.Invoke(this,
                            new OutOfRangeEventArgs(start, end));
                }
                else
                {
                    // Точка в норме — просто идём дальше
                    i++;
                }
            }
        }
    }
}
