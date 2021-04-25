namespace Bwf.Application.CommandHandlers

open Bwf
open Bwf.Application.Commands

module CreateGameHandler =
  type IO =
    { GetTournament: TournamentId -> Async<Tournament>
      GetTeam: TeamId -> Async<Team>
      SaveGame: Game -> Async<unit> }

  let handle io (command: CreateGame) =
    async {
      let! homeTeam = command.HomeId |> io.GetTeam
      let! awayTeam = command.AwayId |> io.GetTeam
      
      do! command.TournamentId
          |> io.GetTournament
          |> Async.RunSynchronously
          |> Games.create homeTeam awayTeam command.StartDate
          |> Game.NotStarted
          |> io.SaveGame
    }
