﻿namespace FSharp

/// Either straight or elbow.
type TrackShape =
    | Straight = 1
    | Elbow    = 2

/// Two cardinal directions representing the shape and orientation of a track.
type TrackPiece =
    /// Straight piece running North to South.
    | NorthSouth
    /// Straight piece running East to West.
    | EastWest
    /// Elbow piece connecting North to East.
    | NorthEast
    /// Elbow piece connecting North to West.
    | NorthWest
    /// Elbow piece connecting South to East.
    | SouthEast
    /// Elbow piece connecting South to West.
    | SouthWest
    with

    /// The shape of this track piece.
    member this.Shape =
        match this with
        | NorthSouth -> TrackShape.Straight
        | EastWest   -> TrackShape.Straight
        | _          -> TrackShape.Elbow

    /// The directions defined by the track.
    ///
    /// The directions are distinct.
    member this.Directions =
        match this with
        | NorthSouth -> (North, South)
        | EastWest   -> (East, West)
        | NorthEast  -> (North, East)
        | NorthWest  -> (North, West)
        | SouthEast  -> (South, East)
        | SouthWest  -> (South, West)

    /// This track piece rotated in the given clock direction.
    member this.RotatedBy(c : ClockDirection) =
        let (d1, d2) = this.Directions
        TrackPiece.FromDirections(d1.RotatedBy(c), d2.RotatedBy(c))

    /// This track piece rotated in the given clock direction.
    member this.RotatedBy(c : Clock4Direction) =
        let (d1, d2) = this.Directions
        TrackPiece.FromDirections(d1.RotatedBy(c), d2.RotatedBy(c))

    /// Computes the rotation to the given track piece.
    ///
    /// The track piece must have the same shape as this, or else an
    /// ArgumentError is thrown.
    member this.RotationTo(p : TrackPiece) =
        do
            if p.Shape <> this.Shape then
                invalidArg "p" "Cannot compute rotation between tracks of \
                                different shapes."
        let (d1, d2) = this.Directions
        let (p1, p2) = p.Directions

        match this.Shape with
        | TrackShape.Elbow ->
            let r1 = d1.RotationTo(p1)
            let r2 = d2.RotationTo(p2)

            if r1 = r2
            // Matching d1 and p1 matches d2 and p2.
            then r1
            // d1 should be matched to p2.
            else d1.RotationTo(p2)
        | TrackShape.Straight ->
            match d1.RotationTo(p1) with
            | Cw0 | Cw2 -> Cw0
            | Cw1 | Cw3 -> Cw1
        | _ -> invalidOp "Invalid shape (programming error)"

    /// Creates a TrackPiece from the given directions.
    ///
    /// The arguments must be distinct directions, or an error is thrown.
    static member FromDirections(d1, d2) =
        if d1 = d2 then
            invalidOp (sprintf "Tried to create a track piece with only one direction: %s" (d1.ToString()))
        else
            match d1 with
            | North -> if d2 = East then NorthEast
                       elif d2 = West then NorthWest
                       else NorthSouth
            | East  -> if d2 = North then NorthEast
                       elif d2 = West then EastWest
                       else SouthEast
            | South -> if d2 = North then NorthSouth
                       elif d2 = East then SouthEast
                       else SouthWest
            | West  -> if d2 = North then NorthWest
                       elif d2 = East then EastWest
                       else SouthWest