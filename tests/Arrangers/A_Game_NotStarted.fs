namespace Arrangers

open System
open Bwf

module A_Game_NotStarted =
  let ``a not started Game`` () = 
    { GameId = Guid.NewGuid() |> GameId
      TournamentId = Guid.NewGuid() |> TournamentId
      HomeId = Guid.NewGuid() |> TeamId
      AwayId = Guid.NewGuid() |> TeamId
      StartDate = DateTime.Now.AddDays(1.0)}
    
  let ``with Home team`` homeTeam (game: NotStartedGame) =
    { game with HomeId = homeTeam }
    
  let ``with Away team`` awayTeam (game: NotStartedGame) =
    { game with AwayId = awayTeam }
    
  let ``as a game`` game =
    game |> Game.NotStarted
  