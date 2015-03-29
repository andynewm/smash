using System;
using System.Linq;
using System.Reflection;

namespace Smash
{
    public class Key
    {
        private readonly MethodInfo _method;
        private readonly object[] _arguments;

        public Key(MethodInfo method, object[] arguments)
        {
            _method = method;
            _arguments = arguments;
        }

        public Type Type
        {
            get { return _method.DeclaringType; }
        }

        public override int GetHashCode()
        {
            return _arguments.Concat(new object[] {_method})
                .Select(x => x.GetHashCode())
                .Aggregate(CombineHashCodes);
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            var otherKey = other as Key;
            if (otherKey == null) return false;

            return _method.Equals(otherKey._method)
                   && _arguments.SequenceEqual(otherKey._arguments);
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            return (((h1 << 5) + h1) ^ h2);
        }
    }
}
