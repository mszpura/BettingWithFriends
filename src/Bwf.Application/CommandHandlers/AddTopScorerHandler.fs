namespace Bwf.Application.CommandHandlers

open Bwf
open Bwf.Application.Commands

module AddTopScorerHandler =
  type IO =
    { GetUser: UserId -> Async<User>
      GetGroup: GroupId -> Async<Group>
      SaveGroup: Group -> Async<unit> }
    
  let handle io (command: AddTopScorer) =
    async {
      let! user = command.UserId |> io.GetUser
      let! group = command.GroupId |> io.GetGroup
      
      do! group
          |> Groups.chooseTypedScorerByUser command.TypedTopScorerId user.UserId
          |> io.SaveGroup
    }