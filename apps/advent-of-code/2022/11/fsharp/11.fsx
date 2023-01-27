#load "../../../../../libs/FSharp/Basics.fsx"
open Basics.Numbers

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


"
In each round
    For each monkey
        For each item
            1. Apply operation to WL of item
            2. Divide worry level (WL) by 3, rounded down
            3. Apply test to WL
            4. Note to which monkey the item would go
            5. Remove item from current monkey
        1. Group 'thrown' items by target monkey
        For each target monkey/group
            1. Append 'thrown' items to items
"

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

    (monkees, throws)
    ||> Map.fold (
        fun state idx items ->
            let monkee = state[idx]
            let monkee' = { monkee with Items = monkee.Items @ items}
            state |> Array.updateAt idx monkee')
    |> Array.updateAt idx { monkee with Items = [] }


let round monkees =
    ((0, monkees), monkees)
    ||> Array.fold (
        fun (idx, monkees) _ ->
            (inc idx, turn idx monkees))
    |> snd


[| m0; m1; m2; m3 |]
|> round
