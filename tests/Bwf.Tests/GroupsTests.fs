namespace Bwf.Tests

open System
open Arrangers
open Bwf
open Xunit
open FsUnit.Xunit
open A_Tournament
open A_Group
open An_User

module GroupsTests =
  [<Fact>]
  let ``Creates a group for not started Tournament`` () =
    // Arrange
    let tournament = ``a tournament``() |> ``with a Start Date`` (DateTime.Today.AddDays(1.0))
    let name = $"{Guid.NewGuid()}"
    let username = $"{Guid.NewGuid()}"
    let userId = Guid.NewGuid() |> UserId
    // Act
    let result = Groups.create name userId username tournament
    // Assert
    result.TournamentId |> should equal tournament.TournamentId
    result.Name |> should equal name 
    result.Users.Length |> should equal 1
    let user = result.Users.Head
    user.Name |> should equal username
    user.IsOwner |> should equal true
    user.TypedTopScorer |> should equal None
    user.UserId |> should equal userId
    
  [<Fact>]
  let ``Throws error when tournament already started`` () =
    // Arrange
    let tournament = ``a tournament``() |> ``with a Start Date`` DateTime.Now
    let name = $"{Guid.NewGuid()}"
    let username = $"{Guid.NewGuid()}"
    let userId = Guid.NewGuid() |> UserId
    // Act & Assert
    (fun () -> Groups.create name userId username tournament |> ignore)
    |> should (throwWithMessage "Tournament already started") typeof<Exception>
    
  [<Fact>]
  let ``Append new user to group`` () =
    // Arrange
    let group = ``a Group``()
    let userId = Guid.NewGuid() |> UserId
    let name = $"{Guid.NewGuid()}"
    // Act
    let group = group |> Groups.appendUser userId name
    // Assert
    group.Users |> should haveLength 2
    let newUser = group.Users |> List.find (fun user -> user.UserId = userId)
    newUser.IsOwner |> should equal false
    newUser.Name |> should equal name
    newUser.TypedTopScorer |> should equal None
    
  [<Fact>]
  let ``Append throws when user already is in group`` () =
    // Arrange
    let userId = Guid.NewGuid() |> UserId
    let name = $"{Guid.NewGuid()}"
    let group = ``a Group``() |> ``with an User`` (``an User``() |> ``with an UserId`` userId)
    // Act & Assert
    (fun () -> group |> Groups.appendUser userId name |> ignore)
    |> should (throwWithMessage "User is already in the group") typeof<Exception>
    
  [<Fact>]
  let ``Choosing typed scorer by User`` () =
    // Arrange
    let userId = Guid.NewGuid() |> UserId
    let group = ``a Group``() |> ``with an User`` (``an User``() |> ``with an UserId`` userId)
    let typedScorer = Guid.NewGuid() |> PlayerId
    // Act
    let group = group |> Groups.chooseTypedScorerByUser typedScorer userId
    // Assert
    let user = group.Users |> List.find (fun user -> user.UserId = userId)
    match user.TypedTopScorer with
    | Some scorer when scorer = typedScorer -> ()
    | None -> failwith "TypedScorer is not set"
    | _ -> failwith "TypedScorer is set wrong"