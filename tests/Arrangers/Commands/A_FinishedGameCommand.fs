namespace Arrangers.Commands

open System
open Arrangers
open Bwf
open Bwf.Application.Commands
open Some_Points

module A_FinishedGameCommand = 
  let ``a Finished Game Command`` () =
    { GameId = Guid.NewGuid() |> GameId
      Points = (``some Points`` TeamSide.Home 3) @ (``some Points`` TeamSide.Away 2)
      EndDate = DateTime.Now }