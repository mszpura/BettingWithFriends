namespace Arrangers

open System
open Bwf

module A_GroupUser =
  let ``a Group User`` () =
    { UserId = Guid.NewGuid() |> UserId
      Name = $"{Guid.NewGuid()}" |> NotEmptyString.create
      IsOwner = false
      TypedTopScorer = None }
    
  let ``with an UserId`` userId (user: GroupUser) =
    { user with UserId = userId }