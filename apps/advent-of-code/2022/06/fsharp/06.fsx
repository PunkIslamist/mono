let example = "mjqjpqmgbljsphdztnvjfqwrcgsmlb"
let example2 = "bvwbjplbgvbhsrlpgdmjqwftvncz"


[<Literal>]
let cwd = __SOURCE_DIRECTORY__


let rawInput = System.IO.File.ReadAllText($"{cwd}/../input.txt")


let solve markerLength input =
    input
    |> Seq.windowed markerLength
    |> Seq.findIndex (
        Set.ofSeq
        >> Set.count
        >> (=) markerLength)
    |> (+) markerLength


rawInput |> solve 4 // Part 1: 1850
rawInput |> solve 14 // Part 2: 2823
