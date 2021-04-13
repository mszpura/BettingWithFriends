namespace Bwf.Tests

open System
open Arrangers
open Bwf
open Xunit
open FsUnit.Xunit
open Some_Points

module GamesTests =
  [<Fact>]
  let ``Creates a game with correct data`` () =
    // Arrange
    let gameId = Guid.NewGuid() |> GameId
    let homeTeamId = Guid.NewGuid() |> TeamId
    let awayTeamId = Guid.NewGuid() |> TeamId
    let tournamentId = Guid.NewGuid() |> TournamentId
    let startDate = DateTime.Today
    // Act
    let game = Games.create gameId homeTeamId awayTeamId tournamentId startDate
    // Assert
    game |> should not' (be Null)
    game.GameId |> should equal gameId
    game.HomeId |> should equal homeTeamId
    game.AwayId |> should equal awayTeamId
    game.TournamentId |> should equal tournamentId
    game.StartDate |> should equal startDate
    
  [<Fact>]
  let ``Finished game should have correct data`` () =
    // Arrange
    let gameId = Guid.NewGuid() |> GameId
    let homeTeamId = Guid.NewGuid() |> TeamId
    let awayTeamId = Guid.NewGuid() |> TeamId
    let tournamentId = Guid.NewGuid() |> TournamentId
    let startDate = DateTime.Today.AddDays(-5.0)
    let points = ``some Points`` TeamSide.Home 2
    let game = Games.create gameId homeTeamId awayTeamId tournamentId startDate
    let endDate = DateTime.Today
    // Act
    let finishedGame = game |> Games.finish points endDate
    // Assert
    finishedGame |> should not' (be Null)
    finishedGame.GameId |> should equal gameId
    finishedGame.TournamentId |> should equal tournamentId
    finishedGame.EndDate |> should equal endDate
    finishedGame.Points |> should equal points