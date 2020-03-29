namespace FSharp

/// Enum version of CardinalDirection.
///
/// This is useful in C#. The associated values represent the number of
/// clockwise rotations from North needed to get to the value.
type CardinalDirectionE =
    | North = 0
    | East = 1
    | South = 2
    | West = 3

/// North, East, South or West
type CardinalDirection = North | East | South | West with
    override this.ToString() =
        match this with
        | North -> "North"
        | East  -> "East"
        | South -> "South"
        | West  -> "West"

    /// This value as a CardinalDirectionE.
    member this.AsEnum =
        match this with
        | North -> CardinalDirectionE.North
        | East  -> CardinalDirectionE.East
        | South -> CardinalDirectionE.South
        | West  -> CardinalDirectionE.West

    /// Parses a CardinalDirectionE.
    static member From(e) =
        match e with
        | CardinalDirectionE.North -> North
        | CardinalDirectionE.East  -> East
        | CardinalDirectionE.South -> South
        | CardinalDirectionE.West  -> West
        | _ -> invalidArg "e" (sprintf "Not a valid direction: %s"
                                        (e.ToString()))
                                        
/// Clockwise (Cw) or counterclockwise (Ccw)
type ClockDirection = Cw | Ccw

/// Enum for Clock4Direction.
///
/// This is useful in C#. The associated values correspond to the number of
/// clockwise rotations a case represents.
type Clock4DirectionE =
    | Cw0 = 0
    | Cw1 = 1
    | Cw2 = 2
    | Cw3 = 3

/// A difference between two cardinal directions.
type Clock4Direction = Cw0 | Cw1 | Cw2 | Cw3 with
    /// The number of clockwise rotations this represents.
    member this.CountClockwise = int this.AsEnum

    /// This value as a Clock4DirectionE.
    member this.AsEnum =
        match this with
        | Cw0 -> Clock4DirectionE.Cw0
        | Cw1 -> Clock4DirectionE.Cw1
        | Cw2 -> Clock4DirectionE.Cw2
        | Cw3 -> Clock4DirectionE.Cw3

    /// Parses a Clock4DirectionE.
    static member From(e) =
        match e with
        | Clock4DirectionE.Cw0 -> Cw0
        | Clock4DirectionE.Cw1 -> Cw1
        | Clock4DirectionE.Cw2 -> Cw2
        | Clock4DirectionE.Cw3 -> Cw3
        | _ -> invalidArg "e" (sprintf "Not a valid rotation: %s"
                                        (e.ToString()))

module Directions =
    /// Rotates cardinal direction by clock direction.
    let rotate clock cardinal =
        match cardinal with
        | North -> if clock = Cw then East else West
        | East  -> if clock = Cw then South else North
        | South -> if clock = Cw then West else East
        | West  -> if clock = Cw then North else South

    /// Rotates cardinal direction by clock direction.
    let rotate4 c4 cardinal =
        match c4 with
        | Cw0 -> cardinal
        | Cw1 -> rotate Cw cardinal
        | Cw2 -> rotate Cw (rotate Cw cardinal)
        | Cw3 -> rotate Ccw cardinal

type CardinalDirection with
    /// Returns this rotated by the clock direction.
    member this.RotatedBy(c) = Directions.rotate c this

    /// Returns this rotated by the clock direction.
    member this.RotatedBy(c) = Directions.rotate4 c this

    /// Returns the rotation from this direction to the argument.
    member this.RotationTo(d) =
        match this with
        | North -> match d with
                   | North -> Cw0
                   | East  -> Cw1
                   | South -> Cw2
                   | West  -> Cw3
        | East  -> match d with
                   | East  -> Cw0
                   | South -> Cw1
                   | West  -> Cw2
                   | North -> Cw3
        | South -> match d with
                   | South -> Cw0
                   | West  -> Cw1
                   | North -> Cw2
                   | East  -> Cw3
        | West  -> match d with
                   | West  -> Cw0
                   | North -> Cw1
                   | East  -> Cw2
                   | South -> Cw3