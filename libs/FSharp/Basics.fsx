namespace Basics

module Numbers =
    let inc i = i + 1


    let dec i = i - 1
    

module Functions =
    let always x = fun _ -> x
    

    let tupleMap f (x, y) = (f x, f y)


module Collections =
    let takeUntil pred source =
        let rec loop state matches =
            match Seq.tryHead matches with
            | None -> state
            | Some i ->
                if pred i
                then (i :: state)
                else loop (i :: state) (Seq.tail matches)
            
        loop [] source |> Seq.rev


module String =
    let contains (item : string) (s : string) = s.Contains(item)


    let split (around : char) (s : string) = s.Split(around)
