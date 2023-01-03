open System.IO


let [| year; day |] =
    fsi.CommandLineArgs
    |> Array.skip 1 // name of script file
    |> Array.map int



[<Literal>]
let currentFilePath = __SOURCE_DIRECTORY__


let root = $"{currentFilePath}/{year}/%02d{day}"


let files = [
    new FileInfo($"{root}/quest.md") 
    new FileInfo($"{root}/input.txt")
    new FileInfo($"{root}/fsharp/%02d{day}.fsx")]


files
|> List.iter (fun x -> x.Directory.Create(); x.Create().Close())
