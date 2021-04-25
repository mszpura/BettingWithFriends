namespace Arrangers

open System
open Bwf
open A_Tournament

module An_Open_Game =
  let ``an Open Game`` () = 
    { GameId = Guid.NewGuid() |> GameId
      Tournament = ``a tournament``()
      HomeId = Guid.NewGuid() |> TeamId
      AwayId = Guid.NewGuid() |> TeamId
      StartDate = DateTime.Now.AddDays(1.0)}
    
  let ``with Home team`` homeTeam (game: OpenGame) =
    { game with HomeId = homeTeam }
    
  let ``with Away team`` awayTeam (game: OpenGame) =
    { game with AwayId = awayTeam }
    
  let ``wih gameId`` gameId (game: OpenGame) =
    { game with GameId = gameId }
    
  let ``as a game`` game =
    game |> Game.Open