open System

let priority c =
    if (Char.IsUpper c)
    then int c - 38
    else int c - 96

        
let result chunkSize elements =
    let prioPerChunk =
        Set.intersectMany
        >> Set.maxElement // there is always just one element
        >> priority

    elements
    |> Seq.map Set.ofSeq
    |> Seq.chunkBySize chunkSize
    |> Seq.map prioPerChunk
    |> Seq.sum


let example = [
    "vJrwpWtwJgWrhcsFMMfFFhFp"
    "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL"
    "PmmdzqPrVvPwwTWBwg"
    "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn"
    "ttgJtRGJQctTZtZT"
    "CrZsJsPPZsGzwwsLwLmpwMDw"
]


let rawInput = System.IO.File.ReadLines("./apps/advent-of-code/2022/03/input.txt") |> List.ofSeq


let part_1 = // 7763
    rawInput
    |> Seq.collect (Seq.splitInto 2)
    |> result 2
    

let part_2 = // 2569
    rawInput
    |> result 3
