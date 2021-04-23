namespace Arrangers.Commands

open System
open Bwf
open Bwf.Application.Commands

module A_CreateGroupCommand =
  let ``a create Group command``() =
    { UserId = Guid.NewGuid() |> UserId
      TournamentId = Guid.NewGuid() |> TournamentId
      Name = $"{Guid.NewGuid()}" |> NotEmptyString.create }

