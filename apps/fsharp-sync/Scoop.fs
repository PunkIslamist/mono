module Syncer.Scoop

module App =
    type T =
        { Name: string
          Version: string
          Bucket: string }

    let private parse line =
        let name = "name"
        let version = "version"
        let bucket = "bucket"
        let namePattern = $"(?<{name}>\\S+)"
        let versionPattern = $"\\(v:(?<{version}>\\S+)\\)"
        let bucketPattern = $"\\[(?<{bucket}>\\S+)\\]"

        let pattern =
            $"{namePattern} {versionPattern} {bucketPattern}"

        let matches =
            System.Text.RegularExpressions.Regex.Match(line, pattern)

        let matched (x: string) = matches.Groups.[x].Value

        { Name = matched name
          Version = matched version
          Bucket = matched bucket }

    let name (x: T) = x.Name

    let fromCli = List.map parse

    let fromJson =
        Text.Json.deserialize<T array> >> List.ofArray

    let toJson (apps: T list) = Text.Json.serialize apps

type UserOptions =
    | Keep
    | Remove

type ScoopActions =
    | Install of App.T
    | Uninstall of App.T

module UnsyncedApp =

    type T =
        | ServerOnly of App.T
        | LocalOnly of App.T

    let name x =
        match x with
        | ServerOnly x
        | LocalOnly x -> App.name x

    let fromLists local server =
        let comp x y = compare (App.name x) (App.name y)
        let diffs = Extensions.List.diff comp local server
        let local = diffs.Left |> List.map LocalOnly
        let server = diffs.Right |> List.map ServerOnly

        List.concat [ local; server ]

    let private toAction x =
        match x with
        | (Keep, LocalOnly _) -> None
        | (Remove, ServerOnly _) -> None
        | (Keep, ServerOnly x) -> Some(Install x)
        | (Remove, LocalOnly x) -> Some(Uninstall x)

    open System

    let toActions getChoice unsynced =
        let toUserOption (x: ConsoleKeyInfo) =
            match x.Key with
            | ConsoleKey.K -> Some(Keep)
            | ConsoleKey.R -> Some(Remove)
            | _ -> None

        let mark prompter app =
            let action : UserOptions = prompter app |> getChoice toUserOption
            (action, app)

        let prompt app =
            let prefix =
                match app with
                | ServerOnly _ -> ">"
                | LocalOnly _ -> "<"

            sprintf "(%s) %s?" prefix <| name app

        printfn "(K)eep or (R)emove?"

        unsynced
        |> List.map (mark prompt)
        |> List.map toAction
        |> List.choose id

let private scoop cmd = Shell.Cmd.exec "scoop" [ cmd ]

/// Gets apps installed on this machine
let installed () = scoop "export" |> App.fromCli

/// Writes apps formatted as JSON to the given path
let export path apps = App.toJson apps |> IO.File.write path

/// Deserializes the given JSON into App instances
let import jsonPath = IO.File.read jsonPath |> App.fromJson

/// Gets install/uninstall commands to execute
let commands installed synced =
    let toCmdString cmd apps =
        match apps with
        | [] -> None
        | _ ->
            apps
            |> List.map App.name
            |> List.append [ cmd ]
            |> Some

    let stringify apps =
        let rec loop apps install uninstall =
            match apps with
            | Install app :: apps' -> loop apps' (install @ [ app ]) uninstall
            | Uninstall app :: apps' -> loop apps' install (uninstall @ [ app ])
            | [] ->
                [ toCmdString "install" install
                  toCmdString "uninstall" uninstall ]
                |> List.choose id
                |> List.map (fun x -> ("scoop", x))

        loop apps [] []

    UnsyncedApp.fromLists installed synced
    |> UnsyncedApp.toActions IO.UserInteraction.userChoice
    |> stringify
