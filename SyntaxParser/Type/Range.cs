using System;
using System.Linq;

namespace SyntaxParser.Type
{
    public class Range : IComparable
    {
        private readonly uint _left;
        private readonly uint _right;

        public Range(string value)
        {
            var range = value.Split(SyntaxHelper.RangeSeparator);
            _left = uint.Parse(range.First());
            _right = uint.Parse(range.Last());
        }

        public Range(uint left, uint right)
        {
            _left = left;
            _right = right;
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return -1;
            if (!(obj is Range)) return -1;
            var range = (Range) obj;
            return _right.CompareTo(range._right) == 0 ? _left.CompareTo(range._left) : _right.CompareTo(range._right);
        }

        public override string ToString()
        {
            return $"{_left}{SyntaxHelper.RangeSeparator}{_right}";
        }
    }
}
