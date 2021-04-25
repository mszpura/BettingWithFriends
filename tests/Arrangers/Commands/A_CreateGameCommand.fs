namespace Arrangers.Commands

open System
open Bwf
open Bwf.Application.Commands

module A_CreateGameCommand =
  let ``a Create Game Command`` () =
    { TournamentId = Guid.NewGuid() |> TournamentId
      HomeId = Guid.NewGuid() |> TeamId
      AwayId = Guid.NewGuid() |> TeamId
      StartDate = DateTime.Today }

