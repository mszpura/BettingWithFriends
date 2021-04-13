namespace Bwf.Tests

open System
open Arrangers
open Bwf
open Xunit
open A_Tournament
open An_User

module GroupsTests =

  [<Fact>]
  let ``Creates a group for not started Tournament`` () =
    // Arrange
    let tournament = ``a tournament``() |> ``with a Start Date`` (DateTime.Today.AddDays(1.0))
    let name = $"{Guid.NewGuid()}"
    let owner = ``an User``() 
    // Act
    let result = Groups.create 
    