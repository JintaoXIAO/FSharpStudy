module Sample01

open Thoth.Json.Net


type User = { Name: string
              Age: int
              KnownLangs: string list }

let encoder  (user: User) = 
  Encode.object [ 
    "name", Encode.string user.Name
    "age", Encode.int user.Age
    "known-langs", user.KnownLangs 
                    |> List.map Encode.string
                    |> Encode.list  
  ]

let decoder: Decoder<User> = 
  Decode.object (fun get -> 
    {
      Name = get.Required.Field "name" Decode.string
      Age = get.Required.Field "age" Decode.int
      KnownLangs = get.Required.Field "known-langs" (Decode.list Decode.string)  
    }
  )

let user = { Name = "jt"
                    Age = 123
                    KnownLangs = ["Scala"; "FSharp"] }  

let toStr = user
                  |> encoder
                  |> Encode.toString 2

let fromStr = toStr
                      |> Decode.fromString decoder 
                      |> function
                          | Ok a -> printfn "yay!"; a
                          | Error e -> printfn "error:\n%A" e; { Name = "Fake User"; Age = 123; KnownLangs = [] }

let run = 
  printfn "%A" toStr
  printfn "%A" fromStr
