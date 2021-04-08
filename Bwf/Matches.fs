namespace Bwf

open System

type MatchId = MatchId of Guid

type PointScoredBy =
  | HomePlayer of TeamMemberId
  | AwayPlayer of TeamMemberId

type Point = {
  ScoredBy: PointScoredBy
  MinuteOfMatch: int
}

type MatchWinner =
  | Home
  | Away
  | Draw

type PlayedMatch = {
  HomePoints: Point list
  AwayPoints: Point list
  EndDate: DateTime option
} with
  member this.Winner =
    match (this.HomePoints.Length, this.AwayPoints.Length) with
    | home, away when home > away -> MatchWinner.Home
    | home, away when home < away -> MatchWinner.Away
    | home, away when home = away -> MatchWinner.Draw
    | _ -> failwith "No match result could be found."
    
type MatchStatus =
  | NotPlayed
  | Played of PlayedMatch
  | Canceled
  
type Match = {
  MatchId: MatchId
  TournamentId: TournamentId
  Home: Team
  Away: Team
  StartDate: DateTime
  MatchStatus: MatchStatus
}

module Matches =
  let appendPoint (point: Point) (playingMatch: Match) =
    match point.ScoredBy with
    | PointScoredBy.HomePlayer homePlayer ->
      
    