type FsItem =
    | Dir of name : string * children : FsItem list
    | File of name : string * size : int
    

let rec size = function
    | Dir (_, items) -> Seq.sum <| Seq.map size items
    | File (_, size) -> size
    

let addChild child (Dir (name, children)) =
    Dir (name, child :: children)
    
    
let rec allItems fsItem =
    match fsItem with
    | File (_, _) -> [fsItem]
    | Dir (_, children) -> fsItem :: (List.collect allItems children)
    

let parse terminalOutput =
    let rec loop dir (terminalOutput' : string list) =
        match terminalOutput' with
        | [] -> (dir, [])
        | line :: lines ->
            match line.Split(' ') with
            | [| "$"; "cd"; ".." |] -> (dir, lines)
            | [| "$"; "ls" |]
            | [| "dir"; _ |] -> loop dir lines
            | [| size; name |] ->
                loop (addChild (File (name, int size)) dir) lines
            | [| "$"; "cd"; name' |] -> 
                let (childDir, lines'') = loop (Dir (name', List.empty)) lines
                loop (addChild childDir dir) lines''
            
    terminalOutput
    |> List.ofSeq
    |> List.skip 1
    |> loop (Dir ("/", List.empty))
    |> fst
    

let part1 =
    allItems
    >> List.choose (
        fun x ->
            match x with
            | File (_,_) -> None
            | Dir (_,_) -> 
                let s = size x
                if s > 100000
                    then None
                    else Some s)
    >> List.sum


let part2 rootDir =
    let unused = 70_000_000 - (size rootDir)
    let required = 30_000_000 - unused

    rootDir
    |> allItems
    |> List.choose (
        fun x ->
            match x with
            | File (_,_) -> None
            | Dir (_,_) -> 
                let s = size x
                if s > required
                    then Some s
                    else None)
    |> List.min


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


let rawInput = System.IO.File.ReadLines($"{__SOURCE_DIRECTORY__}/../input.txt")


parse rawInput |> part1 // 1642503
parse rawInput |> part2 // 6999588
