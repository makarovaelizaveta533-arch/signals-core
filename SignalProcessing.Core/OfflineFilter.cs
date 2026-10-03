using System; // Array.Copy — копирование массивов

namespace SignalParser
{
    // OfflineFilter = ОФЛАЙН сглаживание (весь сигнал уже есть, можно смотреть влево и вправо)
    // static class = все методы static, объект new создавать не надо: OfflineFilter.Filter(...)
    internal static class OfflineFilter
    {
        // Filter = сгладить массив. data = вход. windowSize = сколько соседних точек усреднять
        public static float[] Filter(float[] data, int windowSize)
        {
            // M = сколько точек СЛЕВА от центра (и столько же СПРАВА)
            // Пример: окно 5 → M=2 → берём [i-2, i-1, i, i+1, i+2]
            int M = (windowSize - 1) / 2;

            // extended = удлинённый массив: подушка слева + сигнал + подушка справа
            // Зачем: у первой и последней точки тоже должно быть полное окно
            float[] extended = new float[data.Length + 2 * M];

            // Левая подушка: M раз записать первое значение сигнала
            for (int i = 0; i < M; i++)
                extended[i] = data[0];

            // Середина: скопировать весь исходный сигнал в extended, начиная с позиции M
            // Array.Copy(источник, индекс_источника, приёмник, индекс_приёмника, длина)
            Array.Copy(data, 0, extended, M, data.Length);

            // Правая подушка: M раз записать последнее значение сигнала
            for (int i = 0; i < M; i++)
                extended[M + data.Length + i] = data[data.Length - 1];

            // result = ответ той же длины, что исходный data
            float[] result = new float[data.Length];

            // Внешний цикл: для каждой точки исходного сигнала
            for (int i = 0; i < data.Length; i++)
            {
                // sum = сумма чисел внутри окна
                float sum = 0;

                // Внутренний цикл: сложить windowSize подряд идущих чисел из extended
                for (int j = i; j < i + windowSize; j++)
                    sum += extended[j]; // += значит прибавить к sum

                // Среднее арифметическое = сумма / сколько чисел сложили
                result[i] = sum / windowSize;
            }

            // Вернуть сглаженный массив
            return result;
        }
    }
}
