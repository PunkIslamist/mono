#r "nuget: FSharp.Data, 5.0.2"
open FSharp.Data

[<Literal>]
let currentFilePath = __SOURCE_DIRECTORY__


type Parser = JsonProvider<
    "../samples.json",
    SampleIsList=true,
    ResolutionFolder=currentFilePath>


let vocabulary message =
    let context = Parser.Parse(message).Context

    match context.String, context.Record, context.Array with
    | Some s, _, _ -> s
    | _, Some r, _ -> r.Vocab
    | _, _, Some arr -> arr.String
    | _ -> ""


let samples =
    [1..3]
    |> List.map (fun x -> System.IO.File.ReadAllText($"{currentFilePath}/../sample{x}.json"))


samples |> List.map vocabulary
