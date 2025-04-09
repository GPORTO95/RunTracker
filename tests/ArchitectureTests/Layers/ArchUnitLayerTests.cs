using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;

namespace ArchitectureTests.Layers;

public class ArchUnitLayerTests : ArchUnitBaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnAppplication()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
            .Should()
            .NotDependOnAny(ApplicationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(DomainLayer)
            .Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(ApplicationLayer)
            .Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentatioLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(ApplicationLayer)
            .Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotHaveDependencyOn_PresentatioLayer()
    {
        ArchRuleDefinition
            .Types()
            .That()
            .Are(InfrastructureLayer)
            .Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }
}
