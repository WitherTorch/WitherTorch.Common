using System;

namespace RiceTea.Core.Exceptions;

public sealed class DebugException : Exception
{
    public DebugException(string? message) : base(message) { }
}
