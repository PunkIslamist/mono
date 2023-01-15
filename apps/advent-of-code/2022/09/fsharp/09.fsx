let inc i = i + 1


let dec i = i - 1


let change (x, y) (``x-change``, ``y-change``) =
    (``x-change`` x, ``y-change`` y)
    
    
let positions moves =    
    ((0, 0), moves)
    ||> Seq.scan change


let diff (x, y) (x', y') =
    ((x - x'), (y - y'))


let follow tail head =
    let (diffX, diffY) = diff tail head 
    
    match (abs diffX, abs diffY) with
    | (2, _)
    | (_, 2) ->
        (diffX, diffY)
        |> fun (x, y) -> ((+) (1 * sign x), (+) (1 * sign y))
        |> fun f -> change tail f
    | _ -> tail


let posVisited headPositions =
    ((0,0), headPositions)
    ||> Seq.scan follow
    |> Seq.tail // Scan also returns the initial state which we don't want
    

let parse (motion : string) =
    match motion.Split(' ') with
    | [| "R"; n |] -> (inc, id) |> List.replicate (int n)
    | [| "L"; n |] -> (dec, id) |> List.replicate (int n)
    | [| "D"; n |] -> (id, dec) |> List.replicate (int n)
    | [| "U"; n |] -> (id, inc) |> List.replicate (int n)
    

let part1 headPositions =
    headPositions
    |> posVisited
    |> Set.ofSeq
    |> Seq.length


let part2 headPositions = 
    (headPositions, [1..9])
    ||> Seq.fold (
        fun headPositions _ ->
            posVisited headPositions)
    |> Set.ofSeq
    |> Seq.length


let example = [
    "R 4"
    "U 4"
    "L 3"
    "D 1"
    "R 4"
    "D 1"
    "L 5"
    "R 2"]
    
    
let example2 = [
    "R 5"
    "U 8"
    "L 8"
    "D 3"
    "R 17"
    "D 10"
    "L 25"
    "U 20"]
    

let rawInput =
    System.IO.File.ReadLines($"{__SOURCE_DIRECTORY__}/../input.txt")
    

rawInput
|> Seq.collect parse
|> positions
|> fun x ->
    printfn "Part 1: %A" (part1 x) // 6030
    printfn "Part 2: %A" (part2 x) // 2545
