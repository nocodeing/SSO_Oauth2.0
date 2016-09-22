using System;

namespace CommonTools
{
    public struct BetweenAnd<T> where T : struct
    {
        private T? _start;

        public T? Start
        {
            get { return _start; }
            set
            {
                if (value is DateTime)
                {
                    var o = (dynamic)value;
                    _start = o.Add(new TimeSpan(00, 00, 00));
                }
                else
                    _start = value;
            }
        }
        private T? _end;
        public T? End
        {
            get { return _end; }
            set
            {
                if (value is DateTime)
                {
                    var o = (dynamic)value;
                    _end = o.Add(new TimeSpan(23, 59, 59));
                }
                else
                    _end = value;
            }
        }

    }

    public struct BetweenAnd<TStart, TEnd>
    {
        private TStart _start;

        public TStart Start
        {
            get { return _start; }
            set
            {
                if (value is DateTime)
                {
                    var o = (dynamic)value;
                    _start = o.Add(new TimeSpan(00, 00, 00));
                }
                else
                    _start = value;
            }
        }
        private TEnd _end;
        public TEnd End
        {
            get { return _end; }
            set
            {
                if (value is DateTime)
                {
                    var o = (dynamic)value;
                    _end = o.Add(new TimeSpan(23, 59, 59));
                }
                else
                    _end = value;
            }
        }
    }
}
