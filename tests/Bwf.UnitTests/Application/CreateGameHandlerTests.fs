namespace Bwf.UnitTests.Application

open FsUnit.Xunit
open Bwf
open Bwf.Application.CommandHandlers.CreateGameHandler
open Arrangers.A_Team
open Arrangers.A_Tournament
open Arrangers.Commands.A_CreateGameCommand
open Xunit

module CreateGameHandlerTests =
  
  [<Fact>]
  let ``Creates a Game in given Tournament``() =
    // Arrange
    let mutable expectedGame: OpenGame option = None
    let command = ``a Create Game Command``()
    let home = ``a team``()
               |> ``with TeamId`` command.HomeId
               |> ``with TournamentId`` command.TournamentId
    let away = ``a team``()
               |> ``with TeamId`` command.AwayId
               |> ``with TournamentId`` command.TournamentId
    let tournament = ``a tournament``() |> ``with a Tournament ID`` command.TournamentId
    let io =
      { GetTournament = fun _ -> async { return tournament }
        GetTeam = fun teamId ->
          async { return match teamId with
                         | teamId when teamId = home.TeamId -> home
                         | teamId when teamId = away.TeamId -> away
                         | _ -> failwith "Team not found" }
        SaveGame = fun game ->
          async { expectedGame <- match game with
                                  | Game.Open game -> Some game
                                  | Game.Finished _ -> failwith "Game is finished" } }
    // Act
    command |> handle io |> Async.RunSynchronously
    // Assert
    let game = match expectedGame with
               | Some game -> game
               | None -> failwith "Game not found"
    game.HomeId |> should equal command.HomeId
    game.AwayId |> should equal command.AwayId
    game.StartDate |> should equal command.StartDate
    game.Tournament.TournamentId |> should equal command.TournamentId