module RawLens

type RecordA = 
  { B: RecordB }
and RecordB =
  { Value: string }

let getB = 
  fun a -> a.B

let getValue = 
  fun b -> b.Value    

(*
let get = 
  getB >> getValue
*)
let setB =
  fun b a -> { a with B = b }

let setValue = 
  fun value b -> { b with Value = value }  

(*
let set = 
  fun value a -> setB (setValue value (getB a)) a  
*)

type Lens<'a, 'b> = 
  ('a -> 'b) * ('b -> 'a -> 'a)

//  Lens<'a, 'b> -> Lens<'b, 'c> -> Lens<'a, 'c> 
let compose = 
  fun (g1, s1) (g2, s2) -> 
    (fun a -> g2 (g1 a)), (fun c a -> s1 (s2 c (g1 a)) a)

let get (g, _) = 
  fun a -> g a 

let set (_, s) = 
  fun b a -> s b a   

