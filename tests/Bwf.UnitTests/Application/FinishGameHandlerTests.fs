namespace Bwf.UnitTests.Application

open System
open Bwf
open Bwf.Application.CommandHandlers.FinishGameHandler
open FsUnit.Xunit
open Xunit
open Arrangers.An_Open_Game
open Arrangers.A_Finished_Game
open Arrangers.Commands.A_FinishGameCommand

module FinishGameHandlerTests =
  
  [<Fact>]
  let ``Finishes a not finished game`` () =
    // Arrange
    let mutable expectedGame: Game option = None 
    let command = ``a Finish Game Command``()
    let game = ``an Open Game``()
               |> ``wih gameId`` command.GameId
    let io =
      { GetGame = fun _ -> async { return game |> Game.Open }
        SaveGame = fun game -> async { expectedGame <- Some game } }
    // Act
    command |> handle io |> Async.RunSynchronously
    // Assert
    let game = match expectedGame with
               | None -> failwith "Game not found"
               | Some game -> match game with
                              | Game.Open _ -> failwith "Game is still not finished"
                              | Game.Finished game -> game
    game.GameId |> should equal command.GameId
    game.EndDate |> should equal command.EndDate
    game.Tournament |> should equal game.Tournament
    (game.Result, { Points = command.Points }) ||> should equal
  
  [<Fact>]
  let ``Throw when game is already finished`` () =
    // Arrange
    let command = ``a Finish Game Command``()
    let game = ``a Finished Game``()
               |> ``with Game Id`` command.GameId
               |> ``as a Game``
    let io =
      { GetGame = fun _ -> async { return game }
        SaveGame = fun _ -> async { () } }
    // Act & Assert
    (fun () -> command |> handle io |> Async.RunSynchronously)
    |> should (throwWithMessage "Game is already finished.") typeof<Exception>
              