#load "../../../../../libs/FSharp/Basics.fsx"
open Basics.Collections
open Basics.Functions
open Basics.Numbers

let countVisible startIdx idxChange trees =
    ((-1, startIdx trees, []), trees)
    ||> Seq.fold (
        fun (highest, index, visibleTrees) hight ->
            if hight > highest
            then (hight, idxChange index, index :: visibleTrees)
            else (highest, idxChange index, visibleTrees))
    |> fun (_, _, visibleTrees) -> visibleTrees


let forwards (treeLine : int seq) =
    treeLine |> countVisible (always 0) inc


let ``in-reverse`` (treeLine : int seq) =
    treeLine |> countVisible (dec << Seq.length) dec
    
    
let mapRows visibleF =
    Seq.mapi(
        fun rowIdx row ->
            row
            |> visibleF
            |> Seq.map (fun columnIdx -> (rowIdx, columnIdx)))
    >> Seq.collect id


let mapColumns visibleF =
    Seq.mapi(
        fun columnIdx column ->
            column
            |> visibleF
            |> Seq.map (fun rowIdx -> (rowIdx, columnIdx)))
    >> Seq.collect id


let leftToRight = mapRows forwards
let rightToLeft = Seq.map Seq.rev >> mapRows ``in-reverse``
let topToBot = Seq.transpose >> mapColumns forwards
let botToTop = Seq.transpose >> Seq.map Seq.rev >> mapColumns ``in-reverse``


let grid = Seq.map (Seq.map (string >> int))


let part1 (trees : int seq seq) =
    [
        leftToRight
        rightToLeft
        topToBot
        botToTop
    ]
    |> Seq.collect (fun f -> f trees)
    |> Set.ofSeq
    |> Set.count


let example = [
    "30373"
    "25512"
    "65332"
    "33549"
    "35390"]
    

let rawInput =
    System.IO.File.ReadLines($"{__SOURCE_DIRECTORY__}/../input.txt")
    

rawInput |> grid |> part1 // 1779


// Part 2

let takeSurroundingUntil cond source =
    let rec loop state previous rest =
        match Seq.tryHead rest with
        | None -> state
        | Some x ->
            let following = Seq.tail rest
            let matching = (
                previous |> takeUntil (cond x),
                following |> takeUntil (cond x))

            following |> loop (matching :: state) (x :: previous)
    
    loop [] [] source |> Seq.rev
    

let tupleMap f (x, y) = (f x, f y)


let perCol input =
    input
    |> Seq.transpose
    |> Seq.map (takeSurroundingUntil (<=))
    |> Seq.transpose
    |> Seq.collect (Seq.map (tupleMap Seq.length))
    

let perRow input =
    input
    |> Seq.map (takeSurroundingUntil (<=))
    |> Seq.collect (Seq.map (tupleMap Seq.length))
    

let part2 input =
    let product ((a, b), (x, y)) = a * b * x * y

    Seq.zip (perRow input) (perCol input)
    |> Seq.map product
    |> Seq.max
    

rawInput |> grid |> part2 // 172224
