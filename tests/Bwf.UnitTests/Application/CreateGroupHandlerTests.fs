namespace Bwf.UnitTests.Application

open System
open Bwf
open Xunit
open FsUnit.Xunit
open Bwf.Application.Commands
open Bwf.Application.CommandHandlers.CreateGroupHandler
open Arrangers.Commands.A_CreateGroupCommand
open Arrangers.An_User
open Arrangers.A_Tournament

module CreateGroupHandlerTests =
  [<Fact>]
  let ``Creates Group by user with selected not started Tournament`` () =
    // Arrange
    let mutable expectedGroup: Group option = None
    let createGroupCommand = ``a create Group command``()
    let io =
      { GetUser = fun _ -> async { return ``an User``() |> ``with an UserId`` createGroupCommand.UserId }
        GetTournament = fun _ -> async { return ``a tournament``()
                                                |> ``with a Tournament ID`` createGroupCommand.TournamentId
                                                |> ``with a Start Date`` (DateTime.Today.AddDays(2.0)) }
        SaveGroup = fun group -> async { expectedGroup <- Some group } }
    // Act
    createGroupCommand |> handle io |> Async.RunSynchronously
    // Assert
    let group = match expectedGroup with
                | Some group -> group
                | None -> failwith "Group does not exist"
    group.Name |> should equal createGroupCommand.Name
    group.GroupId |> should not' (be Null)
    group.TournamentId |> should equal createGroupCommand.TournamentId
    group.Users |> should haveLength 1
    let user = group.Users.Head
    user.IsOwner |> should equal true
    user.UserId |> should equal createGroupCommand.UserId