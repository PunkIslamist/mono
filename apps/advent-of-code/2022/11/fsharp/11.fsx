#load "../../../../../libs/FSharp/Basics.fsx"

open Basics.Numbers
open Basics.Collections
open System.Text.RegularExpressions

type Test = {
    Divisor: int64
    ``If divisable``: int
    ``Not divisable``: int }


type Monkee = {
    Items : int64 list
    Operation : string * string
    Test: Test }
    

let singleValue (groupName: string) (reMatch : Match) =
    reMatch
        .Groups[groupName]
        .Value
        

let allValues (groupName: string) (reMatch : Match) =
    reMatch
        .Groups[groupName]
        .Captures
    |> Seq.map (fun x -> x.Value)


let parseMonkee lines =
    let itemsPattern = @"Starting items:( (?<items>\d+),?)+"
    let operationPattern = @"Operation: new = (?<left>old|\d+) (?<op>(\+|\*)) (?<right>old|\d+)"
    let testPattern = @"Test: divisible by (?<divisor>\d+)"
    let targetPattern = @"If \w+: throw to monkey (?<target>\d+)"

    let [|_; items; operation; test; ``if true``; ``if false``|] = lines

    let items =
        Regex.Match(items, itemsPattern)
        |> allValues "items"
        |> Seq.map int64

    let divisor =
        Regex.Match(test, testPattern)
        |> singleValue "divisor"
        |> int
        
    let [``If divisable``; ``Not divisable``] =
        [``if true``; ``if false``]
        |> List.map (
            fun x ->
                Regex.Match(x, targetPattern)
                |> singleValue "target"
                |> int)
    
    let [op; right] =
        ["op"; "right"]
        |> List.map (
            fun group ->
                singleValue group <| Regex.Match(operation, operationPattern))

    {
        Items = items |> Seq.toList
        Operation = (op, right)
        Test = {
            Divisor = divisor
            ``If divisable`` = ``If divisable``
            ``Not divisable`` = ``Not divisable`` }}
    

let worryLevel monkee item =
    match monkee.Operation with
    | ("*", "old") -> item * item
    | ("+", "old") -> item + item
    | ("*", n) -> item * (int64 n)
    | ("+", n) -> item + (int64 n)
    |> fun x -> x / 3L
    
let target monkee worryLevel =
    let test = monkee.Test

    match worryLevel % test.Divisor with
    | 0L -> test.``If divisable``
    | _ -> test.``Not divisable``


let throws monkee =
    printfn "Monkey: %A" monkee

    (Map [], monkee.Items)
    ||> List.fold (
        fun targets item ->
            let worryLevel = worryLevel monkee item
            let target = target monkee worryLevel

            printfn "Item: %d -> WL: %d -> Target: %d" item worryLevel target

            targets |> Map.change target (
                function
                | Some x -> Some <| worryLevel :: x
                | None -> Some [worryLevel]))
    |> Map.map (fun _ item -> List.rev item)

            
let turn idx monkees =
    let monkee = monkees |> Array.item idx
    let throws = throws monkee
    let monkees' =
        (monkees, throws)
        ||> Map.fold (
            fun state idx items ->
                let monkee = state[idx]
                let monkee' = { monkee with Items = monkee.Items @ items}
                state |> Array.updateAt idx monkee')
        |> Array.updateAt idx { monkee with Items = [] }

    (List.length monkee.Items, monkees')


let round monkees =
    ((0, monkees, Map []), monkees)
    ||> Array.fold (
        fun (idx, monkees, throws) _ ->
            let count, monkees' = turn idx monkees
            let throws' =
                throws |> Map.change idx (
                    function
                    | Some x -> Some <| x + count
                    | None -> Some count)
            (inc idx, monkees', throws'))
    |> fun (_, monkees, throws) -> monkees, throws


let example = [|
    [|
    "Monkey 0:"
    "  Starting items: 79, 98"
    "  Operation: new = old * 19"
    "  Test: divisible by 23"
    "    If true: throw to monkey 2"
    "    If false: throw to monkey 3"|]
    [|
    "Monkey 1:"
    "  Starting items: 54, 65, 75, 74"
    "  Operation: new = old + 6"
    "  Test: divisible by 19"
    "    If true: throw to monkey 2"
    "    If false: throw to monkey 0"|]
    [|
    "Monkey 2:"
    "  Starting items: 79, 60, 97"
    "  Operation: new = old * old"
    "  Test: divisible by 13"
    "    If true: throw to monkey 1"
    "    If false: throw to monkey 3"|]
    [|
    "Monkey 3:"
    "  Starting items: 74"
    "  Operation: new = old + 3"
    "  Test: divisible by 17"
    "    If true: throw to monkey 0"
    "    If false: throw to monkey 1"|]|]


let rawInput =
    System.IO.File.ReadAllLines($"{__SOURCE_DIRECTORY__}/../input.txt")
    

let part1 monkees =
    let (_, throws) =
        ((monkees, Map []), [1..20])
        ||> List.fold (
            fun (monkees, throws) _ ->
                printfn "####### Round start #######"
                let monkees', throws' = round monkees
                (monkees', mergeWith (+) throws throws'))

    throws
    |> Map.toSeq
    |> Seq.sortByDescending snd
    |> Seq.take 2
    |> Seq.map snd
    |> Seq.reduce (*)
            
example
|> Array.map parseMonkee
|> part1
            
rawInput
|> Array.chunkBySize 7
|> Array.map (Array.take 6)
|> Array.map parseMonkee
|> fun x ->
    printfn "Part 1: %d" <| part1 x
