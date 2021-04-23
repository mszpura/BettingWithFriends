namespace Arrangers

open System
open Bwf

module An_User =
  let ``an User`` () =
    { UserId = Guid.NewGuid() |> UserId
      Username = $"{Guid.NewGuid()}" |> NotEmptyString.create
      Password = $"{Guid.NewGuid()}M1#" |> Password.create }
    
  let ``with an UserId`` userId (user: User) =
    { user with UserId = userId }