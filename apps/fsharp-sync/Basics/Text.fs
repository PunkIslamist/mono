namespace Text

module Json =
    open System.Text.Json

    let serialize x =
        let options =
            JsonSerializerOptions(WriteIndented = true)

        JsonSerializer.Serialize(x, options)

    let deserialize<'T> (x: string) = JsonSerializer.Deserialize<'T>(x)
