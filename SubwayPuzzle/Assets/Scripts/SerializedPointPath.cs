using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Unity-serialized <see cref="PointPath"/>.
///
/// Since this is serializable, it can be mutated by Unity.
/// </summary>
[Serializable]
public sealed class SerializedPointPath : ISerializationCallbackReceiver
{
    public PointPath AsPointPath => _pointPath;

    /// <summary>
    /// Serializes a <see cref="PointPath"/>.
    /// </summary>
    public static SerializedPointPath From(PointPath p) =>
        new SerializedPointPath()
        {
            _points = p.ToList(),
            _pointPath = p
        };

    void ISerializationCallbackReceiver.OnBeforeSerialize() {}
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        _pointPath = new PointPath(_points);
    }

    [SerializeField]
    private List<Vector3> _points;

    /// <summary>
    /// The point path corresponding to _points.
    /// </summary>
    private PointPath _pointPath;
}
