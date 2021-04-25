namespace Bwf.UnitTests.Domain

open System
open Arrangers
open Bwf
open Xunit
open FsUnit.Xunit
open Some_Points
open A_Tournament
open A_Team

module GamesTests =
  [<Fact>]
  let ``Creates a game with correct data`` () =
    // Arrange
    let homeTeam = ``a team``()
    let awayTeam = ``a team``()
    let tournament = ``a tournament``()
    let startDate = DateTime.Today
    // Act
    let game = tournament|> Games.create homeTeam awayTeam startDate  
    // Assert
    game |> should not' (be Null)
    game.HomeId |> should equal homeTeam.TeamId
    game.AwayId |> should equal awayTeam.TeamId
    game.Tournament |> should equal tournament
    game.StartDate |> should equal startDate
    
  [<Fact>]
  let ``Finished game should have correct data`` () =
    // Arrange
    let homeTeam = ``a team``()
    let awayTeam = ``a team``()
    let tournament = ``a tournament``()
    let startDate = DateTime.Today.AddDays(-5.0)
    let points = ``some Points`` TeamSide.Home 2
    let game = tournament |> Games.create homeTeam awayTeam startDate
    let endDate = DateTime.Today
    // Act
    let finishedGame = game |> Games.finish points endDate
    // Assert
    finishedGame |> should not' (be Null)
    finishedGame.Tournament |> should equal tournament
    finishedGame.EndDate |> should equal endDate
    finishedGame.Result |> should equal { Points = points }