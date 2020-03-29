using FSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// An in-game piece of track.
///
/// This implementation requires a Collider to detect mouse events. An
/// Animator is used to animate highlighting and rotating.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class TrackController : MonoBehaviour,
                               ITrackController,
                               ISerializationCallbackReceiver
{
    [Header("The track's initial orientation.")]

    [SerializeField]
    [Tooltip("One of the track's initial directions.")]
    private XZDirection direction1;

    [SerializeField]
    [Tooltip("One of the track's initial directions.")]
    private XZDirection direction2;

    [Header("Names of animator variables.")]

    [SerializeField]
    [Tooltip("A boolean animation property defining whether this is"
            + " highlighted.")]
    private string highlightProp = "Highlight";

    [Explanation("When a rotation animation finishes, it should invoke the" +
        " FinishRotating() method on this script through an animation event.")]
    [SerializeField]
    [Tooltip("An integer animation property defining how many times this "
            + " should be rotated from its initial position. The valid values "
            + " are 0, 1, 2, or 3 for elbow-shaped pieces and 0 or 1 for"
            + " straight pieces.")]
    private string orientationProp = "Orientation";

    /// <summary>
    /// Called by the animator to indicate that the piece finished rotating.
    /// </summary>
    public void FinishRotating()
    {
        // Set rotationTask to null before finishing it because IsReorienting
        // is expected to change before the task is finished.
        var x = rotationTask;
        rotationTask = null;
        x.SetResult(null);
    }

    #region ITrackController
    public TrackPiece TrackPiece { get; private set; }

    public bool IsReorienting => rotationTask != null;

    public bool SupportsOrientation(TrackPiece piece) =>
        supportedShapes.Contains(piece);

    public Task OrientTo(TrackPiece piece)
    {
        if (!SupportsOrientation(piece))
            return null;

        // Task may already be done or canceled, so use TrySetCanceled().
        rotationTask?.TrySetCanceled();

        TrackPiece = piece;

        rotationTask = new TaskCompletionSource<object>();
        animator.SetInteger(
            orientationProp,
            GetOrientation(initialShape, TrackPiece));

        return rotationTask.Task;
    }

    private bool highlighted;
    public bool Highlighted
    {
        get => highlighted;
        set
        {
            animator.SetBool(highlightProp, value);
            highlighted = value;
        }
    }

    public bool Hovered { get; private set; }

    public event Action OnClick;

    public event Action<bool> OnHoveredChange;
    #endregion

    #region Serialization
    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (direction1 == direction2)
        {
            Debug.LogError("The directions of a track piece cannot both be"
                          + " the same.", this);
            initialShape = TrackPiece = null;
            supportedShapes = new HashSet<TrackPiece>();
        }
        else
        {
            initialShape = TrackPiece = TrackPiece.FromDirections(
                CardinalDirection.From((CardinalDirectionE)direction1),
                CardinalDirection.From((CardinalDirectionE)direction2));
            supportedShapes = new HashSet<TrackPiece>
            {
                initialShape,
                initialShape.RotatedBy(Clock4Direction.Cw1),
                initialShape.RotatedBy(Clock4Direction.Cw2),
                initialShape.RotatedBy(Clock4Direction.Cw3),
            };
        }
    }
    #endregion

    private static int GetOrientation(TrackPiece p1, TrackPiece p2)
    {
        if (p1.Shape != p2.Shape)
            throw new ArgumentException("Pieces have different shapes.");

        return p1.RotationTo(p2).CountClockwise;
    }

    private void OnValidate()
    {
        if (direction1 == direction2)
        {
            Debug.LogWarning(
                "Track directions were adjusted because they were the same.",
                this);
            if (direction1 == XZDirection.PositiveZ)
                direction2 = XZDirection.NegativeZ;
            else
                direction2 = XZDirection.PositiveZ;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseUp()
    {
        OnClick?.Invoke();
    }

    private void OnMouseEnter()
    {
        Hovered = true;
        OnHoveredChange?.Invoke(true);
    }

    private void OnMouseExit()
    {
        Hovered = false;
        OnHoveredChange?.Invoke(false);
    }

    private TrackPiece initialShape;
    private HashSet<TrackPiece> supportedShapes;

    /// <summary>
    /// The task for the rotation animation.
    ///
    /// This is null iff no animation is going on right now.
    /// </summary>
    private TaskCompletionSource<object> rotationTask;

    private Animator animator;
}

public enum XZDirection
{
    PositiveZ = 0, PositiveX, NegativeZ, NegativeX
}