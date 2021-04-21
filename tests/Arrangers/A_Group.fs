namespace Arrangers

open System
open Bwf

module A_Group =
  let ``a Group`` () =
    { GroupId = Guid.NewGuid() |> GroupId
      Name = $"{Guid.NewGuid()}"
      Users =
        [{ UserId = Guid.NewGuid() |> UserId
           Name = $"{Guid.NewGuid()}"
           IsOwner = true
           TypedTopScorer = None }]
      TournamentId = Guid.NewGuid() |> TournamentId }
    
  let ``with an User`` user group =
    { group with Users = user :: group.Users }