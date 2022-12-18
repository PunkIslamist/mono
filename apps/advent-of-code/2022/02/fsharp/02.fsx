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
    

let toShape x =
    match x with
    | 'A' | 'X' -> Rock
    | 'B' | 'Y' -> Paper
    | 'C' | 'Z' -> Scissors
    

let toResult (them, us) =
    match (them, us) with
    | (Rock, Paper) -> Win
    | (Rock, Scissors) -> Loss
    | (Paper, Scissors) -> Win
    | (Paper, Rock) -> Loss
    | (Scissors, Rock) -> Win
    | (Scissors, Paper) -> Loss
    | _ -> Draw
    

let score (them, us) =
    let resultScore = toResult (them, us) |> Result |> points
    let shapeScore = us |> Shape |> points

    resultScore + shapeScore
    

/// Take a string like "A Y" and convert it to a tuple of Shapes
let lineToShapes : char seq -> Shape * Shape =
    List.ofSeq >>
    (fun [them; _; us] -> (them, us)) >>
    (fun (them, us) -> (toShape them, toShape us))


let example = ["A Y"
               "B X"
               "C Z"]


let rawInput = System.IO.File.ReadLines("./apps/advent-of-code/2022/02/input.txt") |> List.ofSeq


/// 10310
let solution1 =
    rawInput
    |> List.map lineToShapes 
    |> List.map score
    |> List.sum
