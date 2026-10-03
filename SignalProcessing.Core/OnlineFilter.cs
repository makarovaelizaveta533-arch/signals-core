using System.Collections.Generic; // Queue<> = очередь (первым пришёл — первым ушёл)

namespace SignalParser
{
    // OnlineFilter = ОНЛАЙН сглаживание (данные приходят кусками, как «вживую»)
    // Смотрит только в ПРОШЛОЕ. Между вызовами Filter помнит старые точки в буфере
    internal class OnlineFilter
    {
        // private = видно только внутри этого класса
        private int windowSize;      // размер окна усреднения
        private Queue<float> buffer; // буфер = очередь последних точек

        // Конструктор: new OnlineFilter(размер_окна)
        public OnlineFilter(int windowSize)
        {
            // this.windowSize = поле класса. windowSize без this = параметр конструктора
            this.windowSize = windowSize;
            // Создать пустую очередь
            buffer = new Queue<float>();
        }

        // Filter = принять кусок data, вернуть сглаженный кусок той же длины
        public float[] Filter(float[] data)
        {
            // Массив под ответ
            float[] result = new float[data.Length];

            // Цикл по каждой точке куска
            for (int i = 0; i < data.Length; i++)
            {
                // Enqueue = положить число в КОНЕЦ очереди
                buffer.Enqueue(data[i]);

                // Если в очереди стало больше, чем размер окна — выкинуть самое старое
                // Dequeue = взять и удалить число из НАЧАЛА очереди
                if (buffer.Count > windowSize)
                    buffer.Dequeue();

                // sum = сумма всего, что сейчас лежит в буфере
                float sum = 0;

                // foreach = пройтись по каждому числу v в очереди buffer
                foreach (float v in buffer)
                    sum += v; // прибавить v к сумме

                // Среднее = сумма / размер окна (делим на полный windowSize)
                result[i] = sum / windowSize;
            }

            // Вернуть сглаженный кусок
            return result;
        }
    }
}
