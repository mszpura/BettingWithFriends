namespace Arrangers.Commands

open System
open Bwf
open Bwf.Application.Commands

module A_CreateBetCommand =
  let ``a create Bet command`` () =
    { GameId = Guid.NewGuid() |> GameId
      UserId = Guid.NewGuid() |> UserId
      Score =
        { Home = 2
          Away = 0 }
      ScorerId = Guid.NewGuid() |> PlayerId }

