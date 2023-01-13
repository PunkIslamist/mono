let exampleStructure = Map [
    ([| "/" |],
        (5, [
            [| "/"; "a" |]
            [| "/"; "b" |]]))
    ([| "/"; "a" |],
        (13, []))
    ([| "/"; "b" |],
        (19, []))]


let parse terminalOutput =
    let addChild child dir =
        match dir with
        | Some (size, children) ->
            Some <| (size, child :: children)
        | None -> None
            
    let addSize size dir =
        match dir with
        | Some (current, children) ->
            Some <| (current + size, children)
        | None -> None

    let rec loop fs (terminalOutput' : string list) (cwd : string array) =
        match terminalOutput' with
        | [] -> fs
        | line :: lines ->
            match line.Split(' ') with
            | [| "$"; "ls" |] ->
                cwd |> loop fs lines

            | [| "dir"; dir |] ->
                let childPath = Array.append cwd [| dir |]
                let fs' = fs |> Map.change cwd (addChild childPath)
                cwd |> loop fs' lines

            | [| size; _ |] ->
                let fs' = fs |> Map.change cwd (addSize <| int size)
                cwd |> loop fs' lines

            | [| "$"; "cd"; ".." |] ->
                cwd[..^1] |> loop fs lines

            | [| "$"; "cd"; dir |] ->
                let childPath = Array.append cwd [| dir |]
                let fs' = fs |> Map.add childPath (0, []) 
                childPath |> loop fs' lines

    loop Map.empty terminalOutput [||]


let rec size fs dir =
    let (ownSize, children) = Map.find dir fs

    children
    |> List.sumBy (size fs)
    |> (+) ownSize


let part1 fs =
    (0, fs) ||> Map.fold (
        fun state path _ ->
            let s = size fs path

            if s > 100_000
            then state
            else state + s)
            

let part2 fs =
    let total = 70_000_000
    let unused = total - (size fs [| "/" |])
    let required = 30_000_000 - unused

    (total, fs)
    ||> Map.fold (
        fun state path _ ->
            let dirSize = size fs path

            match dirSize with
            | x when x < required -> state
            | x when x > state -> state
            | _ -> dirSize)


let example = "$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k"


let rawInput =
    System.IO.File.ReadLines($"{__SOURCE_DIRECTORY__}/../input.txt")
    |> List.ofSeq


parse rawInput |> part1 // 1642503
parse rawInput |> part2 // 6999588
