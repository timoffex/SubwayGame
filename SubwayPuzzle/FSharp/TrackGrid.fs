namespace FSharp

/// A position on a grid.
type GridPosition =
    { x : int
      y : int }

/// A grid of track pieces.
type TrackGrid =
    { map : Map<GridPosition, TrackPiece> }
