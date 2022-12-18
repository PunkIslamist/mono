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
    

let points x =
    match x with
    | Shape Rock -> 1
    | Shape Paper -> 2
    | Shape Scissors -> 3
    | Result Loss -> 0
    | Result Draw -> 3
    | Result Win -> 6
    

let result (them, us) =
    match (them, us) with
    | (Rock, Paper) -> Win
    | (Rock, Scissors) -> Loss
    | (Paper, Scissors) -> Win
    | (Paper, Rock) -> Loss
    | (Scissors, Rock) -> Win
    | (Scissors, Paper) -> Loss
    | _ -> Draw
    

let score (them, us) =
    let resultScore = result (them, us) |> Result |> points
    let shapeScore = us |> Shape |> points

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
let toShape x =
    match x with
    | 'A' | 'X' -> Rock
    | 'B' | 'Y' -> Paper
    | 'C' | 'Z' -> Scissors
    

let charsToShapes (them, us) = (toShape them, toShape us)


let solution1 = // 10310
    rawInput
    |> solver charsToShapes 


// Part 2
let toResult x =
    match x with
    | 'X' -> Loss
    | 'Y' -> Draw
    | 'Z' -> Win
    

let requiredMove (them, goal) =
    match (them, goal) with
    | (Rock, Win) -> Paper
    | (Rock, Loss) -> Scissors
    | (Scissors, Win) -> Rock
    | (Scissors, Loss) -> Paper
    | (Paper, Win) -> Scissors
    | (Paper, Loss) -> Rock
    | _ -> them
    

let charsToShapes_pt2 (them, goal)=
    let theirMove = toShape them
    let ourMove = (theirMove, (toResult goal)) |> requiredMove  

    (theirMove, ourMove)


let solution2 = // 14859
    rawInput
    |> solver charsToShapes_pt2 
