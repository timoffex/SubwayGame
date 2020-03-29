using System;
using System.Threading.Tasks;
using FSharp;

/// <summary>
/// An in-game object representing a track piece.
/// </summary>
public interface ITrackController
{
    /// <summary>
    /// The target orientation of this track.
    ///
    /// See also <see cref="IsReorienting"/>.
    /// </summary>
    TrackPiece TrackPiece { get; }

    /// <summary>
    /// Whether this is currently animating to match <see cref="TrackPiece"/>.
    /// </summary>
    bool IsReorienting { get; }

    /// <summary>
    /// Whether this supports orienting to <paramref name="piece"/>.
    ///
    /// This may be different on different invocations.
    /// </summary>
    /// <param name="piece">The desired orientation.</param>
    bool SupportsOrientation(TrackPiece piece);

    /// <summary>
    /// Sets <see cref="TrackPiece"/> to <paramref name="piece"/> and animates
    /// toward it.
    ///
    /// If the given orientation is not supported, this returns null.
    /// </summary>
    /// <param name="piece">The desired orientation.</param>
    /// <returns>
    /// If the orientation is not supported, this returns null.
    ///
    /// Otherwise, this returns a Task that completes when the animation
    /// finishes, immediately after <see cref="IsReorienting"/> is set to false.
    /// The returned Task becomes canceled if <see cref="OrientTo(TrackPiece)"/>
    /// is invoked again.
    /// </returns>
    Task OrientTo(TrackPiece piece);

    /// <summary>
    /// Whether this track piece is highlighted.
    /// </summary>
    bool Highlighted { get; set; }

    /// <summary>
    /// Whether this track piece is hovered over.
    /// </summary>
    bool Hovered { get; }

    /// <summary>
    /// Raised when this is clicked.
    /// </summary>
    event Action OnClick;

    /// <summary>
    /// Raised when the hovered state of this changes.
    /// </summary>
    event Action<bool> OnHoveredChange;
}
