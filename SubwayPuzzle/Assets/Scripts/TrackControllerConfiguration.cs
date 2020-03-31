using System;
using FSharp;
using UnityEngine;

/// <summary>
/// The configuration for a <see cref="TrackController"/>.
///
/// This is meant to be serialized in Unity, so it is mutable and all fields
/// are public.
///
/// Call <see cref="Deserialize"/> to parse the serialized fields and set
/// the output properties. This class calls <see cref="Deserialize"/>
/// automatically in <see cref="ISerializationCallbackReceiver"/>.
/// </summary>
[Serializable]
public class TrackControllerConfiguration : ISerializationCallbackReceiver
{
    /// <summary>
    /// The configured track piece.
    ///
    /// This corresponds to <see cref="direction1"/> and
    /// <see cref="direction2"/>.
    /// </summary>
    public TrackPiece InitialTrackPiece { get; private set; }

    /// <summary>
    /// The path a train takes when moving along this 3D model.
    ///
    /// The path is oriented for a train coming from the first direction in
    /// <see cref="TrackPiece.Directions"/> and heading out through the
    /// second direction.
    /// </summary>
    public PointPath TrainPath { get; private set; }

    /// <summary>
    /// Parses the serialized fields.
    ///
    /// If there is an error, then all deserialized properties are set to null
    /// and an exception is thrown.
    ///
    /// If <paramref name="fixProperties"/> is set, this will prefer to fix
    /// properties and log a debug message when possible.
    /// </summary>
    public void Deserialize(bool fixProperties = false)
    {
        try
        {
            if (direction1 == direction2)
            {
                if (fixProperties)
                {
                    Debug.LogWarning(
                        "The directions configured on a track piece were the" +
                        " same, so they were changed automatically.");
                    if (direction1 == XZDirection.PositiveZ)
                    {
                        direction2 = XZDirection.NegativeZ;
                    }
                    else
                    {
                        direction2 = XZDirection.PositiveZ;
                    }
                }
                else
                {
                    throw new InvalidOperationException(
                        "The directions of a track piece cannot both be the" +
                        " same.");
                }
            }

            InitialTrackPiece = TrackPiece.FromDirections(
                direction1.ToCardinalDirection(),
                direction2.ToCardinalDirection());

            if (InitialTrackPiece.Directions.Item1 ==
                direction1.ToCardinalDirection())
            {
                TrainPath = serializedTrainPath.AsPointPath;
            }
            else
            {
                TrainPath = serializedTrainPath.AsPointPath.Reversed;
            }            
        } catch (Exception)
        {
            InitialTrackPiece = null;
            TrainPath = null;
            throw;
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Deserialize(fixProperties: true);
    }

    #region Serialized fields
    [Header("The track's initial orientation.")]

    [Tooltip("One of the track's initial directions.")]
    public XZDirection direction1;

    [Tooltip("One of the track's initial directions.")]
    public XZDirection direction2;

    [Header("Names of animator variables.")]

    [Tooltip("A boolean animation property defining whether this is"
            + " highlighted.")]
    public string highlightProp = "Highlight";

    [Explanation("When a rotation animation finishes, it should invoke the" +
        " FinishRotating() method on this script through an animation event.")]
    [Tooltip("An integer animation property defining how many times this "
            + " should be rotated from its initial position. The valid values "
            + " are 0, 1, 2, or 3 for elbow-shaped pieces and 0 or 1 for"
            + " straight pieces.")]
    public string orientationProp = "Orientation";

    /// <summary>
    /// The path a train takes when moving along this 3D model.
    ///
    /// This path should go from <see cref="direction1"/> to
    /// <see cref="direction2"/>.
    ///
    /// See <see cref="TrainPath"/>.
    /// </summary>
    //[HideInInspector]
    public SerializedPointPath serializedTrainPath;
    #endregion
}
