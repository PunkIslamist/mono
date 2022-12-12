let exampleInput =
    [[1000; 2000; 3000];
     [4000]; [5000; 6000];
     [7000; 8000; 9000];
     [10000;]]

let prepend x xs = Seq.append (Seq.singleton x) xs


let updateWith index updater source =
    let updated = updater <| Seq.item index source
    Seq.updateAt index updated source


let splitAround splitter = 
    let folder state item =
        if item = splitter then prepend Seq.empty state
        else state |> updateWith 0 (prepend item)

    Seq.fold folder (Seq.singleton Seq.empty)
                           

let stringToInt (x : string) = int x


let rawInput = System.IO.File.ReadLines("./apps/advent-of-code/2022/01/input.txt")


let inventories = rawInput |> splitAround "" |> Seq.map (Seq.map stringToInt)


let elfsOrderedByCalories = inventories |> Seq.map Seq.sum |> Seq.sortDescending


// 66306
let solution1 = Seq.head elfsOrderedByCalories


// 195292
let solution2 = Seq.take 3 elfsOrderedByCalories |> Seq.sum
