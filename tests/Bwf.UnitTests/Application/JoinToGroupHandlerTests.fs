namespace Bwf.UnitTests.Application

open Bwf
open Bwf.Application.CommandHandlers.JoinToGroupHandler
open Xunit
open FsUnit.Xunit
open Arrangers.A_Group
open Arrangers.An_User
open Arrangers.Commands.A_JoinToGroupCommand

module JoinToGroupHandlerTests =
  [<Fact>]
  let ``User joins to group`` () =
    // Arrange
    let mutable expectedGroup: Group option = None
    let command = ``a Join to Group Command`` ()
    let user = ``an User``()
    let io =
      { GetGroup = fun _ -> async { return ``a Group``() |> ``with Group Id`` command.GroupId }
        GetUser = fun _ -> async { return user |> ``with an UserId`` command.UserId}
        SaveGroup = fun group -> async { expectedGroup <- Some group } }
    // Act
    handle io command |> Async.RunSynchronously
    // Assert
    let group = match expectedGroup with
                       | Some group -> group
                       | None -> failwith "Group not exist"
    group.Users |> should haveLength 2
    let groupUser = group.Users |> List.find (fun user -> user.UserId = command.UserId)
    groupUser.IsOwner |> should equal false
    groupUser.Name |> should equal user.Username