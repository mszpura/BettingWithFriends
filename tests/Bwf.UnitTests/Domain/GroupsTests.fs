namespace Bwf.UnitTests.Domain

open System
open Arrangers
open Bwf
open Xunit
open FsUnit.Xunit
open A_Tournament
open A_Group
open A_GroupUser
open An_User

module GroupsTests =
  [<Fact>]
  let ``Creates a group for not started Tournament`` () =
    // Arrange
    let tournament = ``a tournament``() |> ``with a Start Date`` (DateTime.Today.AddDays(1.0))
    let name = $"{Guid.NewGuid()}" |> NotEmptyString.create
    let user = ``an User``()
    // Act
    let result = Groups.create name user tournament
    // Assert
    result.TournamentId |> should equal tournament.TournamentId
    result.Name |> should equal name 
    result.Users.Length |> should equal 1
    let createdUser = result.Users.Head
    createdUser.Name |> should equal user.Username
    createdUser.IsOwner |> should equal true
    createdUser.TypedTopScorer |> should equal None
    createdUser.UserId |> should equal user.UserId
    
  [<Fact>]
  let ``Throws error when tournament already started`` () =
    // Arrange
    let tournament = ``a tournament``() |> ``with a Start Date`` DateTime.Now
    let name = $"{Guid.NewGuid()}" |> NotEmptyString.create
    let user = ``an User``()
    // Act & Assert
    (fun () -> Groups.create name user tournament |> ignore)
    |> should (throwWithMessage "Tournament already started") typeof<Exception>
    
  [<Fact>]
  let ``Append new user to group`` () =
    // Arrange
    let group = ``a Group``()
    let user = ``an User``()
    // Act
    let group = group |> Groups.appendUser user
    // Assert
    group.Users |> should haveLength 2
    let newUser = group.Users |> List.find (fun user -> user.UserId = user.UserId)
    newUser.IsOwner |> should equal false
    newUser.Name |> should equal user.Username
    newUser.TypedTopScorer |> should equal None
    
  [<Fact>]
  let ``Append throws when user already is in group`` () =
    // Arrange
    let user = ``an User``()
    let group = ``a Group``() |> ``with an User`` (``a Group User``() |> A_GroupUser.``with an UserId`` user.UserId)
    // Act & Assert
    (fun () -> group |> Groups.appendUser user |> ignore)
    |> should (throwWithMessage "User is already in the group") typeof<Exception>
    
  [<Fact>]
  let ``Choosing typed scorer by User`` () =
    // Arrange
    let userId = Guid.NewGuid() |> UserId
    let group = ``a Group``() |> ``with an User`` (``a Group User``() |> A_GroupUser.``with an UserId`` userId)
    let typedScorer = Guid.NewGuid() |> PlayerId
    // Act
    let group = group |> Groups.addTypedScorerToUser typedScorer userId
    // Assert
    let user = group.Users |> List.find (fun user -> user.UserId = userId)
    match user.TypedTopScorer with
    | Some scorer when scorer = typedScorer -> ()
    | None -> failwith "TypedScorer is not set"
    | _ -> failwith "TypedScorer is set wrong"