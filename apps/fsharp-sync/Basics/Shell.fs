namespace Shell

module Cmd =
    let private split (mark: string) (s: string) =
        let options =
            System.StringSplitOptions.RemoveEmptyEntries
            &&& System.StringSplitOptions.TrimEntries

        s.Split(mark, options) |> List.ofArray

    let exec cmd args =
        let arguments = String.concat " " args

        SimpleExec.Command.Read("cmd.exe", args = $"/c %s{cmd} %s{arguments}", noEcho = true)
        |> split "\n"
        |> List.filter (fun s -> s.Length > 0)
