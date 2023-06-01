module PrismsDemo
open Aether
open Aether.Operators


type RecordA = 
  { B: RecordB option }

  static member B_ = 
    (fun a -> a.B), (fun b a -> { a with B = Some b })

and RecordB = 
  { Value: string }  

  static member Value_ = 
    (fun b -> b.Value), (fun value b -> { b with Value = value })  


let avalue_ = 
  RecordA.B_ >?> RecordB.Value_

let a1 =
  { B = Some { Value = "Hello World!" } }

let a2 = 
  { B = None }

let a1value = 
  Optic.get avalue_ a1  

let a2value = 
  Optic.get avalue_ a2

let a1' = 
  Optic.set avalue_ "Goodbye World!" a1

let a2' = 
  Optic.set avalue_ "Goodbye World!" a2

type MyUnion = 
  | First of int
  | Second of string 

  static member First_ = 
    (fun m -> 
      match m with
      | First i -> Some i
      | _ -> None ),
    (fun i m -> 
      match m with
      | First _ -> First i
      | m -> m )

      