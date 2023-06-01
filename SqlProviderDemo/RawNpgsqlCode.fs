module Raw

open Npgsql


let run = 
  let connStr =
      "Host=localhost;Username=postgres;Password=123;Database=dvdrental"
      
  let datasource = 
      NpgsqlDataSource.Create connStr

  let command = 
      datasource.CreateCommand "select * from actor_info limit 10"


  async {
      let! reader = command.ExecuteReaderAsync() |> Async.AwaitTask
      let! hasNext = reader.ReadAsync()|> Async.AwaitTask
      if (hasNext)
      then
          let fn = reader.GetString(1) 
          let ln = reader.GetString(2)
          return $"firstname: {fn}, lastname: {ln}" 
      else 
          return "no value"
  }
  |> Async.RunSynchronously 
  |> printfn "%A"