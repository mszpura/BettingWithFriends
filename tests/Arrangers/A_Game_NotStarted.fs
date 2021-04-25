namespace Arrangers

open System
open Bwf
open A_Tournament

module A_Game_NotStarted =
  let ``a not started Game`` () = 
    { GameId = Guid.NewGuid() |> GameId
      Tournament = ``a tournament``()
      HomeId = Guid.NewGuid() |> TeamId
      AwayId = Guid.NewGuid() |> TeamId
      StartDate = DateTime.Now.AddDays(1.0)}
    
  let ``with Home team`` homeTeam (game: NotStartedGame) =
    { game with HomeId = homeTeam }
    
  let ``with Away team`` awayTeam (game: NotStartedGame) =
    { game with AwayId = awayTeam }
    
  let ``wih gameId`` gameId (game: NotStartedGame) =
    { game with GameId = gameId }
    
  let ``as a game`` game =
    game |> Game.NotStarted