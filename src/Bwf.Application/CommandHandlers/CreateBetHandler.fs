namespace Bwf.Application.CommandHandlers

open Bwf
open Bwf.Application.Commands

module CreateBetHandler =
  type IO =
    { GetGame: GameId -> Async<Game>
      GetUser: UserId -> Async<User>
      SaveBet: Bet -> Async<Unit> }
    
  let handle io (command: CreateBet) =
    let validateGameIsOpen game =
      match game with
      | Game.Open game -> game
      | Game.Finished _ -> failwith "Game is already finished"
    
    async {
      let game = command.GameId
                 |> io.GetGame
                 |> Async.RunSynchronously
                 |> validateGameIsOpen
      let! user = command.UserId |> io.GetUser
      do! (game, user)
          ||> Bets.create command.Score command.ScorerId
          |> Bet.Open
          |> io.SaveBet
    }