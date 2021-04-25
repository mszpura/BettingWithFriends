namespace Arrangers

open System
open Bwf

module A_Team =
  let ``a team`` () =
    { TeamId = Guid.NewGuid() |> TeamId
      TournamentId = Guid.NewGuid() |> TournamentId
      Name = $"{Guid.NewGuid()}" |> NotEmptyString.create }
    
  let ``with TournamentId`` tournamentId (team: Team) =
    { team with TournamentId = tournamentId }
    
  let ``with TeamId`` teamId (team: Team) =
    { team with TeamId = teamId}