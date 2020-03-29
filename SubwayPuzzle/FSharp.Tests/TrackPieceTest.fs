namespace FSharp.Tests

open FSharp
open NUnit.Framework
open FsCheck
open FsCheck.NUnit

[<TestFixture>]
module ``TrackPiece tests`` =

    [<Property>]
    let ``RotationTo is additive`` (p1 : TrackPiece)
                                   (p2 : TrackPiece)
                                   (p3 : TrackPiece) =
        (p1.Shape = p2.Shape && p2.Shape = p3.Shape) ==> lazy

        p1.RotatedBy(p1.RotationTo(p2))
          .RotatedBy(p2.RotationTo(p3))
            = p1.RotatedBy(p1.RotationTo(p3))