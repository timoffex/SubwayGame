namespace FSharp.Tests

open FSharp
open NUnit.Framework
open FsCheck.NUnit

[<TestFixture>]
module ``CardinalDirection tests`` =

    [<Property>]
    let ``AsEnum inverse of From`` e =
        CardinalDirection.From(e).AsEnum() = e

    [<Property>]
    let ``RotatedBy RotationTo correct`` (d1 : CardinalDirection) d2 =
        d1.RotatedBy(d1.RotationTo(d2)) = d2

    [<Property>]
    let ``CardinalDirectionE value is number of CW rotations from North`` e =
        let rec iter n f x =
            match n with
            | 0 -> x
            | _ -> f (iter (n - 1) f x)

        iter    (int e)
                (fun (d : CardinalDirection) -> d.RotatedBy(Cw))
                North
            = CardinalDirection.From(e)

[<TestFixture>]
module ``Clock4Direction tests`` =
    
    [<Property>]
    let ``AsEnum inverse of From`` e =
        Clock4Direction.From(e).AsEnum() = e