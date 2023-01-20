#load "../../../../../libs/FSharp/Basics.fsx"
open Basics.String
open Basics.Numbers

let registerValues start ops =
    let rec loop register ops =
        if Seq.isEmpty ops
        then
            Seq.empty
        else
            seq {
                match Seq.head ops |> split ' ' with
                | [| "addx"; count |] -> 
                    yield register
                    yield register
                    yield! loop (int count + register) (Seq.tail ops)
                | _ -> // noop
                    yield register
                    yield! loop register (Seq.tail ops) }
                    
    // Prepend start so idx = cycle
    Seq.concat [Seq.singleton start; (loop start ops)]
    

let signalStrengths idxs registerValues =
    registerValues
    |> Seq.indexed
    |> Seq.filter (fun (i, _) -> Seq.contains i idxs)
    |> Seq.map (fun (i, value) -> i * value)
    
    
let part1 input =
    input
    |> registerValues 1
    |> signalStrengths [20; 60; 100; 140; 180; 220]
    |> Seq.sum


let pixel (crtPos, spritePos) =
    match spritePos - crtPos with
    | 1 | 0 | -1 -> '#'
    | _ -> '.'


let part2 input =
    let crtWidth = 40
    let crtPositions = 
        Seq.initInfinite dec
        |> Seq.map (fun x -> x % crtWidth)

    input
    |> registerValues 1
    |> Seq.zip crtPositions
    |> Seq.skip 1
    |> Seq.map pixel
    |> Seq.chunkBySize crtWidth
    |> Seq.map System.String


let example = [
    "noop"
    "addx 3"
    "addx -5"]
    

let largeExample = [
    "addx 15"
    "addx -11"
    "addx 6"
    "addx -3"
    "addx 5"
    "addx -1"
    "addx -8"
    "addx 13"
    "addx 4"
    "noop"
    "addx -1"
    "addx 5"
    "addx -1"
    "addx 5"
    "addx -1"
    "addx 5"
    "addx -1"
    "addx 5"
    "addx -1"
    "addx -35"
    "addx 1"
    "addx 24"
    "addx -19"
    "addx 1"
    "addx 16"
    "addx -11"
    "noop"
    "noop"
    "addx 21"
    "addx -15"
    "noop"
    "noop"
    "addx -3"
    "addx 9"
    "addx 1"
    "addx -3"
    "addx 8"
    "addx 1"
    "addx 5"
    "noop"
    "noop"
    "noop"
    "noop"
    "noop"
    "addx -36"
    "noop"
    "addx 1"
    "addx 7"
    "noop"
    "noop"
    "noop"
    "addx 2"
    "addx 6"
    "noop"
    "noop"
    "noop"
    "noop"
    "noop"
    "addx 1"
    "noop"
    "noop"
    "addx 7"
    "addx 1"
    "noop"
    "addx -13"
    "addx 13"
    "addx 7"
    "noop"
    "addx 1"
    "addx -33"
    "noop"
    "noop"
    "noop"
    "addx 2"
    "noop"
    "noop"
    "noop"
    "addx 8"
    "noop"
    "addx -1"
    "addx 2"
    "addx 1"
    "noop"
    "addx 17"
    "addx -9"
    "addx 1"
    "addx 1"
    "addx -3"
    "addx 11"
    "noop"
    "noop"
    "addx 1"
    "noop"
    "addx 1"
    "noop"
    "noop"
    "addx -13"
    "addx -19"
    "addx 1"
    "addx 3"
    "addx 26"
    "addx -30"
    "addx 12"
    "addx -1"
    "addx 3"
    "addx 1"
    "noop"
    "noop"
    "noop"
    "addx -9"
    "addx 18"
    "addx 1"
    "addx 2"
    "noop"
    "noop"
    "addx 9"
    "noop"
    "noop"
    "noop"
    "addx -1"
    "addx 2"
    "addx -37"
    "addx 1"
    "addx 3"
    "noop"
    "addx 15"
    "addx -21"
    "addx 22"
    "addx -6"
    "addx 1"
    "noop"
    "addx 2"
    "addx 1"
    "noop"
    "addx -10"
    "noop"
    "noop"
    "addx 20"
    "addx 1"
    "addx 2"
    "addx 2"
    "addx -6"
    "addx -11"
    "noop"
    "noop"
    "noop"]
    
    
let rawInput =
    System.IO.File.ReadLines($"{__SOURCE_DIRECTORY__}/../input.txt")
    

rawInput
|> fun x ->
    printfn "Part 1: %d" <| part1 x // 13220
    printfn "Part 2: %A" <| part2 x // R U A K H B E K
