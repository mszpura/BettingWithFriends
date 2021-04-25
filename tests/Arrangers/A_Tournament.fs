namespace Arrangers

open System
open Bwf

module A_Tournament =
  let ``a tournament`` () =
    { TournamentId = Guid.NewGuid() |> TournamentId
      Name = $"{Guid.NewGuid()}" |> NotEmptyString.create
      StartDate = DateTime.Today
      Settings =
        { PointsForCorrectWinner = 1
          PointsForCorrectScorer = 2
          PointsForCorrectGameScore = 3
          PointsForTypedTopScorer = 8 } }
    
  let ``with a Start Date`` startDate (tournament: Tournament) =
    { tournament with StartDate = startDate }
    
  let ``with a Tournament ID`` tournamentId (tournament: Tournament) =
    { tournament with TournamentId = tournamentId }