module Main

open FSharp.Data.Sql
open FSharp.Data.Sql.Common

[<Literal>]
let connStr = "Host=localhost;Username=postgres;Password=123;Database=dvdrental"

[<Literal>]
let schema = "public"

type sql = SqlDataProvider<
    DatabaseProviderTypes.POSTGRESQL,
    connStr,
    "",
    "C:\\Users\\xiaoj\\Dev\\FSharpStudy\\SqlProviderDemo\\compilelibs",
    1000,
    NullableColumnType.OPTION,
    schema>

[<EntryPoint>]
let main argv = 
    let ctx = sql.GetDataContext()
    let actors = ctx.``Public``.Actor |> Seq.toArray
    let actor = actors[0]
    printfn "%A" actor.FirstName
    0