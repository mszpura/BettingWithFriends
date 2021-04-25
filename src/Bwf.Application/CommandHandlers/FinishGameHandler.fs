namespace Bwf.Application.CommandHandlers

open Bwf
open Bwf.Application.Commands

module FinishGameHandler =
  type IO =
    { GetGame: GameId -> Async<Game>
      SaveGame: Game -> Async<unit> }
  
  let handle io (command: FinishGame) =
    async {
      let! game = command.GameId |> io.GetGame
      let game = match game with
                 | Game.Open game -> game
                 | Game.Finished _ -> failwith "Game is already finished."
                 
      do! game
          |> Games.finish command.Points command.EndDate
          |> Game.Finished
          |> io.SaveGame
    }