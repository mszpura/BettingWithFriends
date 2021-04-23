namespace Arrangers

open System
open Bwf

module A_Tournament =
  let ``a tournament`` () =
    { TournamentId = Guid.NewGuid() |> TournamentId
      Name = $"{Guid.NewGuid()}" |> NotEmptyString.create
      StartDate = DateTime.Today }
    
  let ``with a Start Date`` startDate (tournament: Tournament) =
    { tournament with StartDate = startDate }
    
  let ``with a Tournament ID`` tournamentId (tournament: Tournament) =
    { tournament with TournamentId = tournamentId }