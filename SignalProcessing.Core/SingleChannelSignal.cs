using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalParser // общая «папка имён» проекта
{
    // SingleChannelSignal = один сигнал целиком (частота + значения)
    internal class SingleChannelSignal
    {
        // Конструктор. Вызывается при: new SingleChannelSignal(частота, массив)
        // float freq = частота (сколько точек в секунду)
        // float[] value = массив значений сигнала
        public SingleChannelSignal(float freq, float[] value)
        {
            Freq = freq;   // сохранить частоту внутрь объекта
            Value = value; // сохранить массив внутрь объекта
        }

        // Свойство Freq. get = можно читать. Нет set = снаружи менять нельзя
        // float = число с точкой
        public float Freq { get; }

        // Свойство Value = массив амплитуд. Value[0], Value[1], Value[2]...
        public float[] Value { get; }

        // TimeTick = массив времён для графика (ось X)
        // Это не поле, а вычислялка: каждый раз когда просят — считает заново
        public float[] TimeTick
        {
            get // get = код, который выполняется при обращении signal.TimeTick
            {
                // Новый массив той же длины, что и Value
                float[] timeTick = new float[Value.Length];

                // Цикл по всем точкам
                for (int i = 0; i < Value.Length; i++)
                {
                    // Время точки = её номер / частота
                    // Пример: i=0 → 0 сек; при Freq=200, i=200 → 1 сек
                    timeTick[i] = i / Freq;
                }

                // Вернуть готовый массив времён
                return timeTick;
            }
        }
    }
}
