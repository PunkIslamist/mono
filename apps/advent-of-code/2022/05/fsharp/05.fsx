let stacks rows =
    let elementWidth = 4  // "[A] ".Length = 4

    let stackCount =
        rows
        |> Array.last
        |> Seq.length
        |> (+) 1 // Last element not followed by a space
        |> (/) <| elementWidth
        
    [| for i in 0..(stackCount - 1) ->
        let stackIndex = 1 + i * elementWidth

        rows
        |> Array.map (Seq.item stackIndex)
        |> List.ofArray
        |> List.filter ((<>) ' ') |]


let moves segments =
    // "move 1 from 2 to 3" -> (1, 2, 3)
    let extract (x : string) =
        x.Split(' ')
        |> fun [| "move"; count; "from"; from; "to"; target |] -> (int count, int from - 1, int target - 1)

    segments |> Array.map extract


let rearrange order (count, start, target) (stacks : char list array) =
    // Based on https://github.com/lboshuizen/aoc22/blob/main/advent2022/Day5.fs
    let (movingCrates, remainingCrates) = stacks[start] |> List.splitAt count

    stacks
    |> Array.updateAt target (order movingCrates @ stacks[target])
    |> Array.updateAt start remainingCrates


let contains (item : string) (s : string) = s.Contains(item)


let stackSegment lines = lines |> Array.takeWhile (contains "[")


let moveSegment lines = lines |> Array.skipWhile (not << contains "move")


let split (around : char) (s : string) = s.Split(around)


let example =
    split '\n' <|
    "    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2"
    

[<Literal>]
let cwd = __SOURCE_DIRECTORY__
let rawInput = System.IO.File.ReadLines($"{cwd}/../input.txt") |> Array.ofSeq


let result order input =
    (stacks <| stackSegment input, moves <| moveSegment input)
    ||> Seq.fold (fun stacks move -> rearrange order move stacks)
    |> Seq.map Seq.tryHead
    |> Seq.choose id


rawInput
|> result List.rev
|> System.String.Concat
|> printfn "Part 1: %s" // "VRWBSFZWM"


rawInput
|> result id
|> System.String.Concat
|> printfn "Part 2: %s" // "RBTWJWMCF"
