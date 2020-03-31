using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A path in 3D space.
/// </summary>
public sealed class PointPath : IEnumerable<Vector3>
{
    /// <summary>
    /// The number of points in the path.
    /// </summary>
    public int Length => _points.Length;

    /// <summary>
    /// Gets a point on the path.
    /// </summary>
    /// <param name="idx">The point's index, which must be between 0 (inclusive)
    /// and <see cref="Length"/> (exclusive).</param>
    /// <returns>The point corresponding to <paramref name="idx"/>.</returns>
    public Vector3 this[int idx] => _points[idx];

    /// <summary>
    /// This path, but reversed.
    /// </summary>
    public PointPath Reversed => new PointPath(_points.Reverse());

    public PointPath(IEnumerable<Vector3> points)
    {
        _points = points.ToArray();
    }

    /// <summary>
    /// Applies the transformation to the points in the path.
    /// </summary>
    public PointPath Map(Func<Vector3, Vector3> transformation) =>
        new PointPath(this.Select(transformation));

    #region IEnumerable
    public IEnumerator<Vector3> GetEnumerator()
    {
        return ((IEnumerable<Vector3>)_points).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<Vector3>)_points).GetEnumerator();
    }
    #endregion

    public override bool Equals(object obj) =>
        obj is PointPath &&
        ((PointPath)obj)._points == _points;

    public override int GetHashCode() => _points.GetHashCode();

    private readonly Vector3[] _points;
}
