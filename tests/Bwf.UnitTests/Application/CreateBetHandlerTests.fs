namespace Bwf.UnitTests.Application

open System
open Bwf
open Xunit
open FsUnit.Xunit
open Arrangers.An_Open_Game
open Arrangers.An_User
open Arrangers.Commands.A_CreateBetCommand
open Arrangers.A_Finished_Game
open Bwf.Application.CommandHandlers.CreateBetHandler

module CreateBetHandlerTests =
  [<Fact>]
  let ``Creates a bet for opened game`` () =
    // Arrange
    let mutable expectedBet: Bet option = None
    let command = ``a create Bet command`` ()
    let game = ``an Open Game`` ()
               |> ``wih gameId`` command.GameId
               |> ``as a game``
    let user = ``an User`` () |> ``with an UserId`` command.UserId
    let io =
      { GetGame = fun _ -> async { return game }
        GetUser = fun _ -> async { return user }
        SaveBet = fun bet -> async { expectedBet <- Some bet } }
    // Act
    command |> handle io |> Async.RunSynchronously
    // Assert
    let bet = match expectedBet with
              | None -> failwith "Bet not found"
              | Some bet -> match bet with
                            | Bet.Open bet -> bet
                            | Bet.Finished _ -> failwith "Wrong status of bet"
    bet.Game |> Game.Open |> should equal game
    bet.Details.Score |> should equal command.Score
    bet.Details.ScorerId |> should equal command.ScorerId
    bet.Details.UserId |> should equal command.UserId
    
  [<Fact>]
  let ``Fails when game is already finished`` () =
    // Arrange
    let command = ``a create Bet command`` ()
    let game = ``a Finished Game`` ()
               |> ``with Game Id`` command.GameId
               |> ``as a Game``
    let user = ``an User`` () |> ``with an UserId`` command.UserId
    let io =
      { GetGame = fun _ -> async { return game }
        GetUser = fun _ -> async { return user }
        SaveBet = fun _ -> async { () } }
    // Act & Assert
    (fun () -> command |> handle io |> Async.RunSynchronously)
    |> should (throwWithMessage "Game is already finished") typeof<Exception>