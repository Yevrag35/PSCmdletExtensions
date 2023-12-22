using MG.Posh.Extensions.Spans;
using System.ComponentModel;

namespace UnitTesting
{
    public class WildcardTests
    {
        const string _instTestWith = "This is a string * a known length";
        const string _instTestWithout = "This is a string that happens to have a known length";

        public static IEnumerable<object?[]> GetIndexingData()
        {
            yield return new object?[] { _instTestWith };
            yield return new object?[] { WildcardString.Empty };
            yield return new object?[] { (WildcardString)default };
        }

        [Fact]
        public void TestInstance()
        {
            WildcardString wc = new WildcardString(_instTestWith);

            Assert.Equal(_instTestWith.Length, wc.Length);
            Assert.False(wc.IsEmpty);
            Assert.True(wc.ContainsWildcards);

            WildcardString nc = _instTestWithout;
            Assert.Equal(_instTestWithout.Length, nc.Length);
            Assert.False(nc.IsEmpty);
            Assert.False(nc.ContainsWildcards);
        }

        [Theory]
        [MemberData(nameof(GetIndexingData))]
        public void TestIndexing(WildcardString wcString)
        {
            int length = wcString.Length;
            Assert.Throws<IndexOutOfRangeException>(() => wcString[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => wcString[wcString.Length]);

            if (wcString.IsEmpty)
            {
                Assert.Throws<IndexOutOfRangeException>(() => wcString[0]);
            }
        }

        [Fact]
        public void TestWildcardIsMatch()
        {
            WildcardString wc = _instTestWith;
            bool isMatch = wc.IsMatch(_instTestWithout, StringComparison.InvariantCultureIgnoreCase);

            Assert.True(isMatch);
        }

        [Fact]
        public void TestNonWildcardIsMatch()
        {
            WildcardString nc = _instTestWithout;
            bool isMatch = nc.IsMatch(_instTestWithout, StringComparison.InvariantCulture);
            bool nonMatch = nc.IsMatch(_instTestWith, StringComparison.InvariantCultureIgnoreCase);

            Assert.True(isMatch);
            Assert.False(nonMatch);
        }

        [Fact]
        public void WildcardEquality()
        {
            WildcardString wc = _instTestWith;
            WildcardString nc = _instTestWithout;

            Assert.True(wc.Equals(wc));
            Assert.False(wc.Equals(nc));
            Assert.True(wc.Equals(_instTestWith));
            Assert.False(wc.Equals(_instTestWithout));

            Assert.Equal(wc, wc);
            Assert.Equal(nc, nc);

            Assert.NotEqual(wc, nc);

            WildcardString wc2 = _instTestWith;
            Assert.True(wc == wc2);
            Assert.False(nc == wc2);
        }
    }
}