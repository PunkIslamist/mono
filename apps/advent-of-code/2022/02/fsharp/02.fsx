#load "../../../../../libs/FSharp/Ring.fsx"
open Ring

type Shape =
    | Rock
    | Paper
    | Scissors
    

type Result =
    | Win
    | Loss
    | Draw
    

type Scorable =
    | Shape of Shape
    | Result of Result
    

let shapes = Ring [Rock; Paper; Scissors]
    

let points = function
    | Shape Rock -> 1
    | Shape Paper -> 2
    | Shape Scissors -> 3
    | Result Loss -> 0
    | Result Draw -> 3
    | Result Win -> 6
    

let result (theirs, ours) =
    match ours with
    | _ when ours = theirs -> Draw
    | _ when ours = Ring.next shapes theirs -> Win
    | _ -> Loss
    

let score (theirs, ours) =
    let resultScore = result (theirs, ours) |> Result |> points
    let shapeScore = ours |> Shape |> points

    resultScore + shapeScore
    

// charsToShapes must map char tuple like ("A", "X") to Shapes tuple
// (it's more generic but that is how it's used)
let solver charsToShapes =
    let lineToShapes =
        List.ofSeq
        >> (fun [x; _; y] -> (x, y))
        >> charsToShapes

    List.map lineToShapes >> List.map score >> List.sum


let example = ["A Y"
               "B X"
               "C Z"]


let rawInput = System.IO.File.ReadLines("./apps/advent-of-code/2022/02/input.txt") |> List.ofSeq


// Part 1
let toShape = function
    | 'A' | 'X' -> Rock
    | 'B' | 'Y' -> Paper
    | 'C' | 'Z' -> Scissors
    

let charsToShapes (theirs, ours) = (toShape theirs, toShape ours)


let solution1 = // 10310
    rawInput
    |> solver charsToShapes 


// Part 2
let toResult = function
    | 'X' -> Loss
    | 'Y' -> Draw
    | 'Z' -> Win
    

let requiredMove (theirs, goal) =
    match goal with
    | Win -> Ring.next shapes theirs
    | Loss -> Ring.previous shapes theirs
    | Draw -> theirs
    

let charsToShapes_pt2 (theirs, goal) =
    let theirMove = toShape theirs
    let ourMove = requiredMove (theirMove, (toResult goal))

    (theirMove, ourMove)


let solution2 = // 14859
    rawInput
    |> solver charsToShapes_pt2 
