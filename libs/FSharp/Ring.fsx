namespace Ring

/// <summary>
/// A ring is like a list where the ends are connected.
/// This means the "last" element is followed by the the "first" element,
/// and the "first" element is preceded by the "last" element.
/// This also means that it is an infinite collection with a finite
/// number of values because they just keep repeating.
/// </summary>
/// <param name="items">
/// The values to construct the ring from.
/// The resulting ring will be the input list with "both ends connected".
/// </param>
/// <typeparam name="'a">The type of values contained in this ring.</typeparam>
type Ring<'a>(items : 'a list) =
    let infinite x = Seq.initInfinite (fun _ -> x) |> Seq.concat
    
    member internal this.items = items

    interface System.Collections.Generic.IEnumerable<'a> with
        member this.GetEnumerator() = (infinite items).GetEnumerator()

    interface System.Collections.IEnumerable with
        member this.GetEnumerator() =
            (infinite items).GetEnumerator() :> System.Collections.IEnumerator


module Ring =
    /// <summary>
    /// Returns the given ring in reverse order.
    /// <code>
    /// Ring.next (Ring.reverse x) = Ring.previous x
    /// Ring.previous (Ring.reverse x) = Ring.next x
    /// </code>
    /// </summary>
    let reverse (ring : Ring<'a>) = List.rev ring.items |> Ring


    /// <summary>
    /// Returns the item after <paramref name="item"/>.
    /// </summary>
    /// <exception cref="System.ArgumentException">
    /// Thrown if <paramref name="item"/> is not in <paramref name="ring"/>.
    /// </exception>
    let next (ring : Ring<'a>) item =
        let rec findIn items =
            match items with
            | [] -> raise (System.ArgumentException $"Ring does not contain {item}")
            | [x] when x = item -> List.head ring.items
            | [_] -> raise (System.ArgumentException $"Ring does not contain {item}")
            | first :: tail ->
                if first = item
                then List.head tail
                else findIn tail
            
        findIn ring.items
        

    /// <summary>
    /// Returns the item before <paramref name="item"/>.
    /// </summary>
    /// <exception cref="System.ArgumentException">
    /// Thrown if <paramref name="item"/> is not in <paramref name="ring"/>.
    /// </exception>
    let previous (ring : Ring<'a>) x = next (reverse ring) x


    /// <summary>
    /// Safe version of <see cref="Ring.next"/>.
    /// </summary>
    let tryNext (ring : Ring<'a>) item =
        try next ring item |> Some
        with _ -> None


    /// <summary>
    /// Safe version of <see cref="Ring.previous"/>.
    /// </summary>
    let tryPrevious (ring : Ring<'a>) x = tryNext (reverse ring) x
