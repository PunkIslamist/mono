namespace Syncer

module Syncer =
    [<EntryPoint>]
    let main args =

        (Scoop.installed (), Scoop.import "out/test.json")
        ||> Scoop.commands
        |> List.map (fun (cmd, args) -> Shell.Cmd.exec cmd args)
        |> List.collect id
        |> List.iter (printfn "%A")

        0
