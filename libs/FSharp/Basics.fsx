namespace Basics

module Numbers =
    let inc i = i + 1


    let dec i = i - 1
    

module Functions =
    let always x = fun _ -> x
    

    let tupleMap f (x, y) = (f x, f y)


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


module String =
    let contains (item : string) (s : string) = s.Contains(item)


    let split (around : char) (s : string) = s.Split(around)
