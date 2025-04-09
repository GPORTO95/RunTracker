using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using FluentAssertions;
using SharedKernel;

namespace ArchitectureTests.Domain;

public class ArchUnitDomainTests : ArchUnitBaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .Check(Architecture);
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostFix()
    {
        ArchRuleDefinition
            .Classes()
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .Check(Architecture);
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        IEnumerable<Class> entityTypes = ArchRuleDefinition
            .Classes()
            .That()
            .AreAssignableTo(typeof(Entity))
            .GetObjects(Architecture);

        var failingTypes = new List<Class>();
        foreach (Class entityType in entityTypes)
        {
            IEnumerable<MethodMember> constructors = entityType.GetConstructors();

            if (!constructors.Any(c => c.Visibility == Visibility.Private &&
                                       !c.Parameters.Any()))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
}
