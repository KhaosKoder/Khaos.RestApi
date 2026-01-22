using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Common.Analyzers.Infrastructure;

internal enum GatewayLayer
{
    External,
    Domain,
    Internal,
    Client,
    Other
}

internal static class LayerMetadata
{
    public static GatewayLayer FromAssemblyName(string? assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName))
        {
            return GatewayLayer.Other;
        }

        var name = assemblyName!;

        if (name.StartsWith("External.", StringComparison.Ordinal))
        {
            return GatewayLayer.External;
        }

        if (name.StartsWith("Domain.", StringComparison.Ordinal))
        {
            return GatewayLayer.Domain;
        }

        if (name.StartsWith("Internal.", StringComparison.Ordinal))
        {
            return GatewayLayer.Internal;
        }

        if (name.StartsWith("Client.", StringComparison.Ordinal))
        {
            return GatewayLayer.Client;
        }

        return GatewayLayer.Other;
    }

    public static Location GetProjectLocation(Compilation compilation)
    {
        var tree = compilation.SyntaxTrees.FirstOrDefault();
        if (tree is null)
        {
            return Location.None;
        }

        return Location.Create(tree, new Microsoft.CodeAnalysis.Text.TextSpan(0, 0));
    }
}
