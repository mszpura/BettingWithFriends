﻿namespace Bwf.UnitTests.Application

open Bwf
open Bwf.Application.CommandHandlers.FinishGameHandler
open FsUnit.Xunit
open Xunit
open Arrangers.A_Game_NotStarted
open Arrangers.Commands.A_FinishedGameCommand

module FinishGameHandlerTests =
  
  [<Fact>]
  let ``Finishes a not finished game`` () =
    // Arrange
    let mutable expectedGame: Game option = None 
    let command = ``a Finished Game Command``()
    let game = ``a not started Game``()
               |> ``wih gameId`` command.GameId
    let io =
      { GetGame = fun _ -> async { return game |> Game.NotStarted }
        SaveGame = fun game -> async { expectedGame <- Some game } }
    // Act
    command |> handle io |> Async.RunSynchronously
    // Assert
    let game = match expectedGame with
               | None -> failwith "Game not found"
               | Some game -> match game with
                              | Game.NotStarted _ -> failwith "Game is still not finished"
                              | Game.Finished game -> game
    game.GameId |> should equal command.GameId
    game.EndDate |> should equal command.EndDate
    game.Tournament |> should equal game.Tournament
    (game.Result, { Points = command.Points }) ||> should equal