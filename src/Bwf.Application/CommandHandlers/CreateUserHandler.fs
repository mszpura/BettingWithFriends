namespace Bwf.Application.CommandHandlers

open System
open Bwf
open Bwf.Application.Commands

module CreateUserHandler =
  type IO = { SaveUser: User -> Async<unit> }
    
  let handle io command =
    async {
      let user =
        { UserId = Guid.NewGuid() |> UserId
          Username = command.Username |> NotEmptyString.create
          Password = command.Password |> Password.create }
        
      do! io.SaveUser user
    }