namespace Basics

module Numbers =
    let inc i = i + 1


    let dec i = i - 1
    

module Functions =
    let always x = fun _ -> x
    

    let tupleMap f (x, y) = (f x, f y)
    

    let tap f x =
        f x
        x


module Collections =
    let rec takeUntil pred source =
        if Seq.isEmpty source
        then Seq.empty
        else
            seq {
                let head = Seq.head source

                if pred head
                then yield head
                else
                    yield head
                    yield! takeUntil pred (Seq.tail source) }
    

    // let map1 = Map [(0, 2); (1, 4); (2, 3)]
    // let map2 = Map [(1, 6); (2, 1); (3, 5)]
    // let resultMap = Map [(0, 2); (1, 10); (2, 4); (3, 5)]
    // mergeWith (+) map1 map2 = resultMap
    // Thanks https://stackoverflow.com/a/3974842
    let mergeWith f map1 map2 =
        (map1, map2)
        ||> Map.fold (
            fun state key value ->
                state |> Map.change key (
                    function
                    | Some x -> Some <| f value x
                    | None -> Some value))



module String =
    let contains (item : string) (s : string) = s.Contains(item)


    let split (around : char) (s : string) = s.Split(around)
