namespace Bwf.UnitTests.Application

open System
open Bwf
open Bwf.Application.CommandHandlers.AddTopScorerHandler
open Xunit
open FsUnit.Xunit
open Arrangers.An_User
open Arrangers.A_Group
open Arrangers.Commands.An_AddTopScorerCommand

module AddTopScorerHandlerTests =
  [<Fact>]
  let ``Select Top Scorer by User in Group`` () =
    // Arrange
    let mutable expectedGroup: Group option = None
    let command = ``an Add Top Scorer command``()
    let user = ``an User``() |> ``with an UserId`` command.UserId
    let group = ``a Group``()
                |> ``with Group Id`` command.GroupId
                |>  Groups.appendUser user
    let io =
      { GetUser = fun _ -> async { return user }
        GetGroup = fun _ -> async { return group }
        SaveGroup = fun group -> async { expectedGroup <- Some group } }
    // Act
    handle io command |> Async.RunSynchronously
    // Assert
    let expectedUser = match expectedGroup with
                       | Some group -> group.Users |> List.find (fun u -> u.UserId = command.UserId)
                       | None -> failwith "Group not exist"
    expectedUser.TypedTopScorer |> should equal (Some command.TypedTopScorerId)
    
  [<Fact>]
  let``Throws when User already have selected Top Scorer`` () =
    // Arrange
    let command = ``an Add Top Scorer command``()
    let user = ``an User``() |> ``with an UserId`` command.UserId
    let group = ``a Group``()
                |> ``with Group Id`` command.GroupId
                |> Groups.appendUser user
                |> Groups.chooseTypedScorerByUser (Guid.NewGuid() |> PlayerId) user.UserId
    let io =
      { GetUser = fun _ -> async { return user }
        GetGroup = fun _ -> async { return group }
        SaveGroup = fun _ -> async { () } }
    // Act & Assert
    (fun () -> handle io command |> Async.RunSynchronously)
    |> should (throwWithMessage "User already has selected Top Scorer in this group") typeof<Exception>