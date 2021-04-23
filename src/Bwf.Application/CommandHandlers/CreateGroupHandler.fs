namespace Bwf.Application.CommandHandlers

open Bwf
open Bwf.Application.Commands

module CreateGroupHandler =
  type IO =
    { GetUser: UserId -> Async<User>
      GetTournament: TournamentId -> Async<Tournament>
      SaveGroup: Group -> Async<unit> }
    
  let handle io (command: CreateGroup) =
    async {
      let! user = command.UserId |> io.GetUser
      let! tournament = command.TournamentId |> io.GetTournament
      let group = Groups.create command.Name user tournament
      do! group |> io.SaveGroup
    }
    

