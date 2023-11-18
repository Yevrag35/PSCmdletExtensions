using System.Diagnostics.CodeAnalysis;

namespace MG.Posh.Internal
{
    internal readonly ref struct OneOf<T0, T1> 
        where T0 : class
        where T1 : class
    {
        readonly bool _isT0;
        readonly bool _isT1;
        readonly bool _isNotNull;
        readonly T0? _t0;
        readonly T1? _t1;

        //[MemberNotNullWhen(true, nameof(_t0))]
        public readonly bool IsT0 => _isT0;
        //[MemberNotNullWhen(true, nameof(_t1))]
        public readonly bool IsT1 => _isT1;
        public readonly bool IsNull => !_isNotNull;

        private OneOf(T0? item0, T1? item1, bool item0NotNull, bool item1NotNull)
        {
            _isT0 = item0NotNull;
            _isT1 = item1NotNull;
            _isNotNull = item0NotNull || item1NotNull;

            _t0 = item0;
            _t1 = item1;
        }

        internal bool TryGetT0([NotNullWhen(true)] out T0? t0, [MaybeNullWhen(false)] out T1? t1)
        {
            t0 = default;
            t1 = default;
            if (this.IsT0)
            {
                t0 = _t0!;
                return true;
            }
            else if (this.IsT1)
            {
                t1 = _t1!;
            }

            return false;
        }
        internal bool TryGetT1([NotNullWhen(true)] out T1? t1, [MaybeNullWhen(false)] out T0? t0)
        {
            t0 = default;
            t1 = default;
            if (this.IsT1)
            {
                t1 = _t1!;
                return true;
            }
            else if (this.IsT0)
            {
                t0 = _t0!;
            }

            return false;
        }

        internal static OneOf<T0, T1> FromT0(T0 item0)
        {
            Guard.NotNull(item0, nameof(item0));
            return new OneOf<T0, T1>(item0, null, true, false);
        }
        internal static OneOf<T0, T1> FromT1(T1 item1)
        {
            Guard.NotNull(item1, nameof(item1));
            return new OneOf<T0, T1>(null, item1, false, true);
        }

        internal static OneOf<T0, T1> Null => new OneOf<T0, T1>(null, null, false, false);

        public static implicit operator OneOf<T0, T1>(T0 item0)
        {
            return FromT0(item0);
        }
        public static implicit operator OneOf<T0, T1>(T1 item1)
        {
            return FromT1(item1);
        }
    }
}

