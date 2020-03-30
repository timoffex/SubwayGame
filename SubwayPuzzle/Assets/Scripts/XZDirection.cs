using System;
using FSharp;

/// <summary>
/// An X or Z axis-aligned direction.
/// </summary>
public enum XZDirection
{
    PositiveZ = 0, PositiveX, NegativeZ, NegativeX
}

public static class XZDirectionExts
{
    /// <summary>
    /// Converts the XZDirection to a <see cref="CardinalDirectionE"/>.
    ///
    /// North is mapped to the positive Z axis, and East is the positive X axis.
    /// </summary>
    public static CardinalDirectionE ToCardinalDirectionE(this XZDirection d) =>
        (CardinalDirectionE)d;

    /// <summary>
    /// Converts the XZDirection to a <see cref="CardinalDirection"/>.
    ///
    /// North is mapped to the positive Z axis, and East is the positive X axis.
    /// </summary>
    public static CardinalDirection ToCardinalDirection(this XZDirection d) =>
        CardinalDirection.From(d.ToCardinalDirectionE());
}