namespace Arrangers.Commands

open System
open Bwf.Application.Commands

module A_CreateUserCommand =
  let ``a Create User Command`` () =
    { Username = $"{Guid.NewGuid()}"
      Password = $"{Guid.NewGuid()}#A" }
    
  let ``with a Password`` password command =
    { command with Password = password }