module Sample02

open Thoth.Json.Net

type User = { Name: string
              Age: int 
            }

module User = 
  let decoder: Decoder<User> = 
    Decode.object (fun get -> 
      { Name = get.Required.Field "name" Decode.string
        Age = get.Required.Field "age" Decode.int
      }
    )
  
  let encode (user: User): JsonValue = 
    Encode.object 
      [ "name", Encode.string user.Name
        "age", Encode.int user.Age ]

type Rating = 
  | One = 1
  | Two = 2
  | Three = 3

module Rating = 
  let decoder: Decoder<Rating> = 
    Decode.int 
    |> Decode.andThen (function
        | 1 -> Decode.succeed Rating.One
        | 2 -> Decode.succeed Rating.Two
        | 3 -> Decode.succeed Rating.Three
        | invalid -> Decode.fail $"%i{invalid} is not a valid rating value. expected value is 1, 2, 3"        
        )

  let encoder rating = 
    Encode.int (int rating)        


