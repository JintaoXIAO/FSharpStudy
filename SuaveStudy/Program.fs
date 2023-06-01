module Main

open System
open System.Threading
open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.Utils.Collections


let sleep (milliseconds: int) message =
  fun (x: HttpContext) ->
    async {
      do! Async.Sleep milliseconds
      return! OK message x  
    }

let delay a = 
  let rand = new Random()
  async {
    do! Async.Sleep (rand.Next 2000)
    return Some a
  }

let greeting q =
  defaultArg (Option.ofChoice (q ^^ "name")) "Suave" |> sprintf "Hello World from %s"

let addResponseHeaders (additionalHeaders: (string * string) list)  = fun ctx ->
  Some { ctx with response = { ctx.response with headers = List.append ctx.response.headers additionalHeaders } } 
  |> async.Return

let addHeaders (headers: (string * string) list) = 
  fun (ctx: HttpContext) -> ()

let mapUserName (req: HttpRequest) =
  greeting req.query

let business (ctx: HttpContext) =
  let s: string = mapUserName ctx.request
  ctx |> OK s

let app = 
  choose 
    [ GET >=> choose 
        [ path "/hello" >=> 
                  delay >=> 
                  addResponseHeaders ["Admin", "jintao.xiao"; "Say", "hello world from header"] >=>
                  business
          path "/goodbye" >=> OK "Goodbye GET"
          Authentication.authenticateBasic 
            (fun (u, p) -> u = "jintao" && p = "123")
            (path "/private" >=> OK "This is a private page")
        ]  
      POST >=> choose
        [ path "/hello" >=> OK "Hello POST"
          path "/goodbye" >=> OK "Goodbye POST"
        ]
    ]


[<EntryPointAttribute>]
let main (args: string array) = 
  let cts = 
    new CancellationTokenSource()
  let conf: SuaveConfig = 
    { defaultConfig with cancellationToken = cts.Token }
  let listening, server = 
    startWebServerAsync conf app

  Async.Start(server, cts.Token)
  printfn "Make requests now"
  Console.ReadKey true |> ignore
  cts.Cancel()

  0
