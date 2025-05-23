﻿using ArchUnitNET.Loader;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

namespace ArchitectureTests;

public abstract class ArchUnitBaseTest : BaseTest
{
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
        DomainAssembly,
            ApplicationAssembly,
            InfrastructureAssembly,
            PresentationAssembly)
        .Build();

    protected static readonly IObjectProvider<IType> DomainLayer =
        ArchRuleDefinition.Types().That().ResideInAssembly(DomainAssembly).As("Domain Layer");

    protected static readonly IObjectProvider<IType> ApplicationLayer =
        ArchRuleDefinition.Types().That().ResideInAssembly(ApplicationAssembly).As("Application Layer");

    protected static readonly IObjectProvider<IType> InfrastructureLayer =
        ArchRuleDefinition.Types().That().ResideInAssembly(InfrastructureAssembly).As("Infrastructure Layer");

    protected static readonly IObjectProvider<IType> PresentationLayer =
        ArchRuleDefinition.Types().That().ResideInAssembly(PresentationAssembly).As("Presentation Layer");
}
