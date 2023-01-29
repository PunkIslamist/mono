#load "../../../../../libs/FSharp/Basics.fsx"

open Basics.Numbers
open Basics.Collections
open System.Text.RegularExpressions

let example = [
    "Monkey 0:"
    "  Starting items: 79, 98"
    "  Operation: new = old * 19"
    "  Test: divisible by 23"
    "    If true: throw to monkey 2"
    "    If false: throw to monkey 3"
    ""
    "Monkey 1:"
    "  Starting items: 54, 65, 75, 74"
    "  Operation: new = old + 6"
    "  Test: divisible by 19"
    "    If true: throw to monkey 2"
    "    If false: throw to monkey 0"
    ""
    "Monkey 2:"
    "  Starting items: 79, 60, 97"
    "  Operation: new = old * old"
    "  Test: divisible by 13"
    "    If true: throw to monkey 1"
    "    If false: throw to monkey 3"
    ""
    "Monkey 3:"
    "  Starting items: 74"
    "  Operation: new = old + 3"
    "  Test: divisible by 17"
    "    If true: throw to monkey 0"
    "    If false: throw to monkey 1"]


type Monkee = {
    Items : int list
    Operation : int -> int
    Test: int -> int }
    

let m0 = {
    Items = [79; 98]
    Operation = (*) 19
    Test = fun x -> if x % 23 = 0 then 2 else 3 }
let m1 = {
    Items = [54; 64; 75; 74]
    Operation = (+) 6
    Test = fun x -> if x % 19 = 0 then 2 else 0 }
let m2 = {
    Items = [79; 60; 97]
    Operation = fun x -> x * x
    Test = fun x -> if x % 13 = 0 then 1 else 3 }
let m3 = {
    Items = [74]
    Operation = (+) 3
    Test = fun x -> if x % 17 = 0 then 0 else 1 }
    

let exampleMonkee = [
    "Monkey 0:"
    "  Starting items: 79, 98"
    "  Operation: new = old * 19"
    "  Test: divisible by 23"
    "    If true: throw to monkey 2"
    "    If false: throw to monkey 3"]
    
let [id; items; op; test; ``if true``; ``if false``] = exampleMonkee


let itemsRE = @"Starting items:( (?<items>\d+),?)+"
let opRE = @"Operation: new = (?<left>old|\d+) (?<op>(\+|\*)) (?<right>old|\d+)"
let testRE = @"Test: divisible by (?<divisor>\d+)"
let targetRE = @"If \w+: throw to monkey (?<target>\d+)"


let singleValue (groupName: string) (reMatch : Match) =
    reMatch
        .Groups[groupName]
        .Value
        

let allValues (groupName: string) (reMatch : Match) =
    reMatch
        .Groups[groupName]
        .Captures
    |> Seq.map (fun x -> x.Value)


Regex.Match(items, itemsRE)
|> allValues "items"


let [left; opi; right] =
    ["left"; "op"; "right"]
    |> Seq.map (
        fun group ->
            singleValue group <| Regex.Match(op, opRE))
    |> List.ofSeq // I hate this language



Regex.Match(test, testRE)
|> singleValue "divisor"
|> int
    

[``if true``; ``if false``]
|> List.map (
    fun x ->
        Regex.Match(x, targetRE)
        |> singleValue "target"
        |> int)

let worryLevel monkee item =
    item
    |> monkee.Operation
    |> fun x -> x / 3
    

let throws monkee =
    (Map [], monkee.Items)
    ||> List.fold (
        fun targets item ->
            let worryLevel = worryLevel monkee item
            let target = monkee.Test worryLevel

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


(([| m0; m1; m2; m3 |], Map []), [1..20])
||> List.fold (
    fun (monkees, throws) _ ->
        let monkees', throws' = round monkees
        (monkees', mergeWith (+) throws throws'))
