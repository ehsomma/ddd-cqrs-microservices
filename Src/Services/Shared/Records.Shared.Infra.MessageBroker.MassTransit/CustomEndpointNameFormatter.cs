#region Usings

using MassTransit;
using System;

#endregion

namespace Records.Shared.Infra.MessageBroker.MassTransit;

#pragma warning disable  // Disable all warnings
/// <inheritdoc />
public class CustomEndpointNameFormatter : IEndpointNameFormatter
{
    private readonly string _prefix;

    public CustomEndpointNameFormatter(string prefix)
    {
        _prefix = prefix;
    }

    public string Separator => throw new NotImplementedException();

    public string Format(Type type)
    {
        return $"{_prefix}-{type.Name.ToLower()}";
    }

    public string Format<T>()
    {
        return $"{_prefix}-{typeof(T).Name.ToLower()}";
    }

    public string Message<T>()
        where T : class
    {
        throw new NotImplementedException();
    }

    public string SanitizeName(string name)
    {
        throw new NotImplementedException();
    }

    public string TemporaryEndpoint(string tag)
    {
        throw new NotImplementedException();
    }

    string IEndpointNameFormatter.CompensateActivity<T, TLog>()
    {
        throw new NotImplementedException();
    }

    string IEndpointNameFormatter.Consumer<T>()
    {
        throw new NotImplementedException();
    }

    string IEndpointNameFormatter.ExecuteActivity<T, TArguments>()
    {
        throw new NotImplementedException();
    }

    string IEndpointNameFormatter.Saga<T>()
    {
        throw new NotImplementedException();
    }
}
#pragma warning restore  // Restore all warnings
