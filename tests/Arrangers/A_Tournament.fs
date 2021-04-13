namespace Arrangers

open System
open Bwf

module A_Tournament =
  let ``a tournament`` () =
    { TournamentId = Guid.NewGuid() |> TournamentId
      Name = $"{Guid.NewGuid()}"
      StartDate = DateTime.Today }
    
  let ``with a Start Date`` startDate (tournament: Tournament) =
    { tournament with StartDate = startDate }

