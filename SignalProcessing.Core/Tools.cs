using System; // Math.Sin, Math.PI, Array.Copy, Random — математика и массивы
using System.Collections.Generic; // List<> — список

namespace SignalProcessing.Core // та же «папка имён», что и у остальных файлов проекта
{
    // Tools = набор функций-генераторов. static методы = вызываем без new: Tools.Sin(...)
    public class Tools
    {
        // Line = сделать прямую y = k * t + b
        // len = длина в секундах, freq = точек в секунду, k = наклон, b = сдвиг
        public static SingleChannelSignal Line(float len, float freq, float k, float b)
        {
            // Сколько точек нужно: длина * частота. (int) отбрасывает дробь. +0.5 = округление
            int count = (int)(len * freq + 0.5);

            // new float[count] = создать массив из count чисел (пока нули)
            float[] y = new float[count];

            // for = цикл по всем точкам. i = 0,1,2,... пока i < count
            for (int i = 0; i < count; i++)
            {
                // t = время этой точки в секундах = номер / частота
                float t = i / freq;
                // формула прямой: значение = наклон * время + сдвиг
                y[i] = k * t + b;
            }

            // Упаковать частоту и массив в объект сигнала и вернуть его наружу
            return new SingleChannelSignal(freq, y);
        }

        // Sin = сделать синусоиду
        // period = длительность одного цикла, phase = сдвиг, scale = высота
        public static SingleChannelSignal Sin(float len, float freq, float period, float phase, float scale)
        {
            // Количество точек = длина * частота (+0.5 для округления)
            int count = (int)(len * freq + 0.5);

            // Массив под значения синуса
            float[] y = new float[count];

            // diff = шаг времени между соседними точками (1/частота)
            float diff = 1 / freq;

            // Цикл по всем точкам
            for (int i = 0; i < count; i++)
            {
                // t = текущее время
                float t = i * diff;
                // Math.Sin = синус. 2*Math.PI = полный круг.
                // (t-phase)/period = сколько циклов прошло.
                // (float) = привести double от Math.Sin к типу float
                // scale * ... = растянуть по высоте
                y[i] = scale * (float)Math.Sin((t - phase) * 2 * Math.PI / period);
            }

            // Вернуть готовый сигнал
            return new SingleChannelSignal(freq, y);
        }

        // Noise = шум. R = максимальная амплитуда (от -R до +R)
        public static SingleChannelSignal Noise(float len, float freq, float R)
        {
            // Random = генератор случайных чисел
            Random random = new Random();

            // Сколько точек
            int count = (int)(len * freq + 0.5);

            // Массив под шум
            float[] y = new float[count];

            // Цикл по всем точкам
            for (int i = 0; i < count; i++)
            {
                // NextDouble() = случайное число от 0.0 до 1.0
                // * 2 * R = растянуть до диапазона 0..2R
                // - R = сдвинуть в -R..+R
                // (float) = привести к float
                y[i] = (float)(random.NextDouble() * 2 * R - R);
            }

            // Вернуть сигнал-шум
            return new SingleChannelSignal(freq, y);
        }

        // Summ = сложить все сигналы ПОТОЧЕЧНО (в одной точке времени)
        public static SingleChannelSignal Summ(List<SingleChannelSignal> signals)
        {
            // Частоту берём у первого сигнала в списке (signals[0])
            float freq = signals[0].Freq;

            // minLen = длина самого короткого сигнала (чтобы не вылезти за край массива)
            int minLen = signals[0].Value.Length;

            // Цикл по остальным сигналам: если короче — запоминаем его длину
            for (int i = 1; i < signals.Count; i++)
                if (signals[i].Value.Length < minLen)
                    minLen = signals[i].Value.Length;

            // Массив результата
            float[] result = new float[minLen];

            // Внешний цикл: по каждой точке времени
            for (int i = 0; i < minLen; i++)
            {
                // sum = накопитель суммы в этой точке
                float sum = 0;

                // Внутренний цикл: по каждому сигналу
                for (int j = 0; j < signals.Count; j++)
                    // Прибавить значение j-го сигнала в точке i
                    sum += signals[j].Value[i];

                // Записать сумму в результат
                result[i] = sum;
            }

            // Вернуть суммарный сигнал
            return new SingleChannelSignal(freq, result);
        }

        // Skley = склеить сигналы подряд (не складывать значения, а пристыковать массивы)
        public static SingleChannelSignal Skley(List<SingleChannelSignal> signals)
        {
            // Частота от первого
            float freq = signals[0].Freq;

            // Считаем общую длину всех кусков
            int totalLen = 0;
            for (int i = 0; i < signals.Count; i++)
                totalLen += signals[i].Value.Length; // += значит прибавить к totalLen

            // Большой массив на всех
            float[] result = new float[totalLen];

            // pos = куда сейчас пишем в result
            int pos = 0;

            // Для каждого сигнала копируем его кусок в result
            for (int i = 0; i < signals.Count; i++)
            {
                // Array.Copy(откуда, с_какого_индекса, куда, с_какой_позиции, сколько)
                Array.Copy(signals[i].Value, 0, result, pos, signals[i].Value.Length);
                // Сдвинуть позицию на длину только что скопированного куска
                pos += signals[i].Value.Length;
            }

            // Вернуть склеенный сигнал
            return new SingleChannelSignal(freq, result);
        }
    }
}
