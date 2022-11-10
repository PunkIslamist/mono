let rotate xs = xs |> List.transpose |> List.rev

let snailSort =
    let rec loop snail matrix =
        match matrix with
        | [] -> snail
        | (x :: xs) -> loop (snail @ x) (rotate xs)

    function
    | [] -> []
    | [ x ] -> x
    | xs -> loop [] xs

let input = [ [ 1; 2; 3 ]; [ 4; 5; 6 ]; [ 7; 8; 9 ]; [ 10; 11; 12 ] ]

printfn "Input was: %A" input
printfn "Snail is: %A" (snailSort input)
