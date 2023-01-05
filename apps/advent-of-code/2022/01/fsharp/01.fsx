// Pretty much taken 1:1 from https://github.com/johannesegger/AdventOfCode/blob/master/2022/Day01/F#/Program.fs
let calories input =
    ([], input)
    ||> Seq.fold (fun state line ->
        if line = "" then 0 :: state
        else
            match state with
            | currentElf :: others -> (currentElf + int line) :: others
            | [] -> [int line])

let caloriesOrdered input =
    input
    |> calories
    |> Seq.sortDescending


let exampleInput = [
    "1000"
    "2000"
    "3000"
    ""
    "4000"
    ""
    "5000"
    "6000"
    ""
    "7000"
    "8000"
    "9000"
    ""
    "10000" ]


let rawInput = System.IO.File.ReadLines("./apps/advent-of-code/2022/01/input.txt")


// 66306
let solution1 = Seq.head <| caloriesOrdered rawInput


// 195292
let solution2 = caloriesOrdered rawInput |> Seq.take 3 |> Seq.sum
