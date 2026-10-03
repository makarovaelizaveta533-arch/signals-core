using System; // EventArgs = стандартный базовый класс для данных события

namespace SignalParser
{
    // OutOfRangeEventArgs = данные, которые летят вместе с событием «нашли зашкал»
    // : EventArgs = наследуемся от стандартного класса событий C#
    internal class OutOfRangeEventArgs : EventArgs
    {
        // StartIndex = номер первой плохой точки в массиве. get = только читать
        public int StartIndex { get; }

        // EndIndex = номер последней плохой точки в массиве. get = только читать
        public int EndIndex { get; }

        // Конструктор: new OutOfRangeEventArgs(начало, конец)
        public OutOfRangeEventArgs(int startIndex, int endIndex)
        {
            StartIndex = startIndex; // сохранить начало
            EndIndex = endIndex;     // сохранить конец
        }
    }
}
