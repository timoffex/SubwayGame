using FSharp;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.FSharp.Collections;

/// <summary>
/// The serialized data of a <see cref="LevelController"/>.
/// </summary>
[Serializable]
public class LevelControllerConfiguration : ISerializationCallbackReceiver
{
    /// <summary>
    /// The current grid of track pieces defined by the controller grid.
    ///
    /// Controllers that are currently reorienting are not included in this
    /// grid.
    /// </summary>
    public TrackGrid TrackGrid => new TrackGrid(
            new FSharpMap<GridPosition, TrackPiece>(
                from kv in ControllerGrid
                where !kv.Value.IsReorienting
                select new Tuple<GridPosition, TrackPiece>(
                    kv.Key, kv.Value.TrackPiece)));


    /// <summary>
    /// The target grid of track pieces which will be reached after animations
    /// complete.
    /// </summary>
    public TrackGrid TargetTrackGrid => new TrackGrid(
            new FSharpMap<GridPosition, TrackPiece>(
                from kv in ControllerGrid
                select new Tuple<GridPosition, TrackPiece>(
                    kv.Key, kv.Value.TrackPiece)));

    public Dictionary<GridPosition, TrackController> ControllerGrid
    { get; private set; }

    [Tooltip("Add all track controllers here with the correct X, Z positions.")]
    [InspectorName("Track Controllers")]
    public List<TrackGridElement> serializedGrid;

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        serializedGrid = ControllerGrid
            ?.Select((kv) => new TrackGridElement
            {
                x = kv.Key.x,
                z = kv.Key.y,
                controller = kv.Value as TrackController
            })
            .ToList() ?? new List<TrackGridElement>();
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        ControllerGrid = new Dictionary<GridPosition, TrackController>();
        foreach (var element in serializedGrid)
        {
            var position = new GridPosition(element.x, element.z);
            ControllerGrid[position] = element.controller;
        }
    }
}

[Serializable]
public class TrackGridElement
{
    [Tooltip("The index of the track piece along the X axis.")]
    public int x;

    [Tooltip("The index of the track piece along the Z axis.")]
    public int z;

    [Tooltip("A reference to a track controller.")]
    public TrackController controller;
}