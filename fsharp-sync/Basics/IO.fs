namespace IO

open System

module File =
    let read path = IO.File.ReadAllText(path)

    let write path x = IO.File.WriteAllText(path, x)

module UserInteraction =
    let userChoice map prompt =
        let read () = Console.ReadKey true

        do printfn "%s" prompt

        let rec userChoice () =
            match read () |> map with
            | Some x -> x
            | None -> userChoice ()

        userChoice ()
