namespace Bwf.UnitTests.Application

open System
open Bwf
open Xunit
open FsUnit.Xunit
open Bwf.Application.CommandHandlers.CreateUserHandler
open Arrangers.Commands.A_CreateUserCommand

module CreateUserHandlerTests =
  
  [<Fact>]
  let ``Creates user with unique name`` () =
    // Arrange
    let mutable expectedUser: User option = None
    let userCommand = ``a Create User Command`` ()
    let io = { SaveUser = fun user -> async { expectedUser <- Some user } }
    // Act
    handle io userCommand |> Async.RunSynchronously
    // Assert
    let user = match expectedUser with
               | Some user -> user
               | None -> failwith "User don't exist"
    user.UserId |> should not' (be Null)
    user.Username |> NotEmptyString.value |> should equal userCommand.Username
    user.Password |> Password.value |> should equal userCommand.Password
    
  [<Theory>]
  [<InlineData("TestPassword##", "Password must have at least one digit")>]
  [<InlineData("TestPassword123", "Password must have at least one special character")>]
  [<InlineData("TESTPASSWORD123!@#", "Password must have at least one lowercase character")>]
  [<InlineData("testpassword123!@", "Password must have at least one uppercase character")>]
  [<InlineData("Passw", "Password must have at least 8 digits")>]
  [<InlineData("", "Password must have at least 8 digits")>]
  let ``Password of User to create is not valid`` invalidPassword expectedError =
    // Arrange
    let mutable expectedUser: User option = None
    let userCommand = ``a Create User Command``() |> ``with a Password`` invalidPassword
    let io = { SaveUser = fun user -> async { expectedUser <- Some user } }
    // Act & Assert
    (fun () -> handle io userCommand |> Async.RunSynchronously)
    |> should (throwWithMessage expectedError) typeof<Exception>