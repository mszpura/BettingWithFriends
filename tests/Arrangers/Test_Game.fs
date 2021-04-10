namespace Arrangers

open System
open Bwf

module Test_Game =
  let ``create default not started Game`` = 
    { GameId = Guid.NewGuid() |> GameId
      TournamentId = Guid.NewGuid() |> TournamentId
      Home = Test_Team.``create default Team``
      Away = Test_Team.``create default Team``
      StartDate = DateTime.Now.AddDays(1.0)}
    |> NotStartedGame
    
  let ``with Home team`` homeTeam game =
    let (NotStartedGame notStartedGame) = game
    { notStartedGame with Home = homeTeam }
    |> NotStartedGame
    
  let ``with Away team`` awayTeam game =
    let (NotStartedGame notStartedGame) = game
    { notStartedGame with Away = awayTeam }
    |> NotStartedGame
  