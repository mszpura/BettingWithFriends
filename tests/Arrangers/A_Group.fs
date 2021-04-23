namespace Arrangers

open System
open Bwf
open NotEmptyString

module A_Group =
  let ``a Group`` () =
    { GroupId = Guid.NewGuid() |> GroupId
      Name = $"{Guid.NewGuid()}" |> create
      Users =
        [{ UserId = Guid.NewGuid() |> UserId
           Name = $"{Guid.NewGuid()}" |> create
           IsOwner = true
           TypedTopScorer = None }]
      TournamentId = Guid.NewGuid() |> TournamentId }
    
  let ``with an User`` user group =
    { group with Users = user :: group.Users }
    
  let ``with Group Id`` groupId group =
    { group with GroupId = groupId }