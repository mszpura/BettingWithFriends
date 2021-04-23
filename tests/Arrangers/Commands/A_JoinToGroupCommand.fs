namespace Arrangers.Commands

open System
open Bwf
open Bwf.Application.Commands

module A_JoinToGroupCommand =
  let ``a Join to Group Command`` (): JoinToGroup =
    { UserId = Guid.NewGuid() |> UserId
      GroupId = Guid.NewGuid() |> GroupId }