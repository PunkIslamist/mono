let toSet (start, finish) =
    Set.ofList [start..finish]
    

let ``isOneSubset?`` set1 set2 =
    Set.isSuperset set1 set2 ||
    Set.isSubset set1 set2
    

let lineToRangePair (line : string) =
    let elementToRange (element : string) =
        let [| start; finish |] = element.Split('-')
        (int start, int finish)
        
    let [| first; second |] = line.Split(',')
    (elementToRange first, elementToRange second)


let example = [
    ((2,4), (6,8))
    ((2,3), (4,5))
    ((5,7), (7,9))
    ((2,8), (3,7))
    ((6,6), (4,6))
    ((2,6), (4,8))
]


let rawInput = System.IO.File.ReadLines("./apps/advent-of-code/2022/04/input.txt") |> List.ofSeq
    
    
let result1 = // 526
    rawInput
    |> List.map (lineToRangePair
                    >> fun (a, b) -> (toSet a, toSet b)
                    >> fun (a, b) -> ``isOneSubset?`` a b)
    |> List.sumBy (fun oneIsSubset -> if oneIsSubset then 1 else 0)


let result2 = // 886
    rawInput
    |> List.map (lineToRangePair
                    >> fun (a, b) -> (toSet a, toSet b)
                    >> fun (a, b) -> Set.intersect a b
                    >> Set.isEmpty)
    |> List.sumBy (fun noIntersection -> if noIntersection then 0 else 1)
