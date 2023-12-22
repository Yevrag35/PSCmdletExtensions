using MG.Posh.PSObjects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;
using UnitTesting.Extensions;

namespace UnitTesting
{
    public interface ITestObject
    {
        string Name { get; }
        string? Description { get; }
        int Id { get; }
        Guid UniqueId { get; }
        string UnboundData { get; }
    }

    public class ProjectionTests
    {
        private static ITestObject GetMockObject(string name, string? description, int id, Guid uniqueId, string unboundData)
        {
            ITestObject mock = Substitute.For<ITestObject>();
            mock.Name.Returns(name);
            mock.Description.Returns(description);
            mock.Id.Returns(id);
            mock.UniqueId.Returns(uniqueId);
            mock.UnboundData.Returns(unboundData);

            return mock;
        }

        public static IEnumerable<object?[]> MockData()
        {
            yield return new object?[] { " Mr. Guy ", "Just a description", 123, Guid.NewGuid(), "Unbound data" };
            yield return new object?[] { " Dr. Nobody", null, 123, Guid.NewGuid(), "Unbound data2" };
        }

        [Theory]
        [MemberData(nameof(MockData))]
        public void TestProjectTo(string name, string? description, int id, Guid uniqueId, string unboundData)
        {
            ITestObject mockObj = GetMockObject(name, description, id, uniqueId, unboundData);
            PSObject pso = mockObj.ProjectTo(x => new
            {
                Name = x.Name.Trim(),
                x.Id,
                GlobalId = x.UniqueId,
                Desc = x.Description,
            });

            AssertMemberEqual(pso, mockObj, x => x.Name, (x, y) =>
            {
                Assert.NotEqual(x, y);
                Assert.Equal(x?.Trim(), y);
            });
            AssertMemberEqual(pso, "Desc", description, Assert.Equal);
            AssertMemberEqual(pso, "GlobalId", uniqueId, Assert.Equal);
            AssertMemberEqual(pso, mockObj, x => x.Id, Assert.Equal);

            PSPropertyInfo? notExists = pso.Properties[nameof(ITestObject.UnboundData)];
            Assert.Null(notExists);
        }

        [Fact]
        public void TestBadProjectTo()
        {
            ITestObject mockObj = GetMockObject(string.Empty, null, default, default, string.Empty);

            Assert.Throws<ArgumentException>(() =>
            {
                _ = mockObj.ProjectTo(x => string.Format("{0}", "whatev"));
            });
        }

        private static void AssertMemberEqual<T, TMem>(PSObject pso, T obj, Expression<Func<T, TMem?>> memberExpression, Action<TMem?, TMem?> equality)
        {
            if (!memberExpression.TryGetAsProperty(out PropertyInfo? pi))
            {
                throw new ArgumentException("Expression is not a valid property member.");
            }

            TMem? memObj = (TMem?)pi.GetValue(obj);

            PSPropertyInfo? prop = pso.Properties[pi.Name];
            Assert.NotNull(prop);

            if (prop.Value is not null)
            {
                TMem psoObj = Assert.IsAssignableFrom<TMem>(prop.Value);
                equality(memObj, psoObj);
            }
            else
            {
                equality(memObj, default);
            }
        }
        private static void AssertMemberEqual<TMem>(PSObject pso, string memberName, TMem? valueToCompare, Action<TMem?, TMem?> equality)
        {
            PSPropertyInfo? prop = pso.Properties[memberName];
            Assert.NotNull(prop);

            if (prop.Value is not null)
            {
                TMem memObj = Assert.IsAssignableFrom<TMem>(prop.Value);
                equality(valueToCompare, memObj);
            }
            else
            {
                equality(valueToCompare, default);
            } 
        }
    }
}
