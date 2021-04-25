namespace Bwf.Application.CommandHandlers

open Bwf
open Bwf.Application.Commands

module JoinToGroupHandler =
  type IO =
    { GetGroup: GroupId -> Async<Group>
      GetUser: UserId -> Async<User>
      SaveGroup: Group -> Async<unit> }
  
  let handle io (command: JoinToGroup) =
    async {
      let! group = command.GroupId |> io.GetGroup
      let! user = command.UserId |> io.GetUser
      
      do! group
          |> Groups.appendUser user
          |> io.SaveGroup
    }