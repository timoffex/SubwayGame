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
    public TrackControllerConfiguration configuration;

    /// <summary>
    /// Called by the animator to indicate that the piece finished rotating.
    /// </summary>
    public void FinishRotating()
    {
        // Set rotationTask to null before finishing it because IsReorienting
        // is expected to change before the task is finished.
        var x = _rotationTask;
        _rotationTask = null;
        x.SetResult(null);
    }

    /// <summary>
    /// The path a train takes when moving along this 3D model.
    ///
    /// The path is oriented for a train coming from the first direction in
    /// <see cref="TrackPiece.Directions"/> and heading out through the
    /// second direction.
    /// </summary>
    public PointPath TrainPath => configuration.TrainPath;

    #region ITrackController
    public TrackPiece TrackPiece { get; private set; }

    public bool IsReorienting => _rotationTask != null;

    public bool SupportsOrientation(TrackPiece piece) =>
        _supportedShapes.Contains(piece);

    public Task OrientTo(TrackPiece piece)
    {
        if (!SupportsOrientation(piece))
            return null;

        // Task may already be done or canceled, so use TrySetCanceled().
        _rotationTask?.TrySetCanceled();

        TrackPiece = piece;

        _rotationTask = new TaskCompletionSource<object>();
        _animator.SetInteger(
            configuration.orientationProp,
            GetOrientation(configuration.InitialTrackPiece, TrackPiece));

        return _rotationTask.Task;
    }

    private bool highlighted;
    public bool Highlighted
    {
        get => highlighted;
        set
        {
            _animator.SetBool(configuration.highlightProp, value);
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
        var initialShape = configuration.InitialTrackPiece;

        TrackPiece = initialShape;
        _supportedShapes = new HashSet<TrackPiece>
        {
            initialShape,
            initialShape.RotatedBy(Clock4Direction.Cw1),
            initialShape.RotatedBy(Clock4Direction.Cw2),
            initialShape.RotatedBy(Clock4Direction.Cw3),
        };
    }
    #endregion

    private static int GetOrientation(TrackPiece p1, TrackPiece p2)
    {
        if (p1.Shape != p2.Shape)
            throw new ArgumentException("Pieces have different shapes.");

        return p1.RotationTo(p2).CountClockwise;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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

    private HashSet<TrackPiece> _supportedShapes;

    /// <summary>
    /// The task for the rotation animation.
    ///
    /// This is null iff no animation is going on right now.
    /// </summary>
    private TaskCompletionSource<object> _rotationTask;

    private Animator _animator;
}