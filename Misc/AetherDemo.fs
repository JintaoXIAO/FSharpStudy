module AetherDemo

type RecordA = 
  { B: RecordB }

  static member B_ =
    (fun a -> a.B), (fun b a -> { a with B = b })

and RecordB =
  { Value: string }

  static member Value_ = 
    (fun (b: RecordB) -> b.Value), (fun (value: string) (b: RecordB) -> { b with Value = value })  

module Demo01 = 
  open Aether

  let a = 
    { B = { Value = "Hello World" } }  
  
  let avalue_ = 
    Compose.lens RecordA.B_ RecordB.Value_  

  let value = 
    Optic.get avalue_ a

  let a' = 
    Optic.set avalue_ "GoodbyeWorld!" a    

module Demo02 = 
  open Aether
  open Aether.Operators

  let a = 
    { B = { Value = "Hello World" } }  

  let avalue_ = 
    RecordA.B_ >-> RecordB.Value_

  let value = 
    Optic.get avalue_ a

  let a' = 
    Optic.set (RecordA.B_ >-> RecordB.Value_) "Goodbye World!" a            