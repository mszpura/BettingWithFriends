namespace Bwf

open System

type MatchId = MatchId of Guid

type Point = {
  TeamId: TeamId
  TeamMemberId: TeamMemberId
  MinuteOfMatch: int
} 

type MatchWinner =
  | Home
  | Away
  | Draw

type MatchInfo = {
  MatchId: MatchId
  TournamentId: TournamentId
  Home: Team
  Away: Team
  StartDate: DateTime
}

type InProgressMatch = {
  MatchInfo: MatchInfo
  Points: Point list
}

type MatchEndDate = MatchEndDate of DateTime

type PlayedMatch = {
  MatchInfo: MatchInfo
  Points: Point list
  EndDate: MatchEndDate
} with
  member this.Score =
    let homePoints, awayPoints =
      this.Points |> List.partition (fun point -> point.TeamId = this.MatchInfo.Home.TeamId)
    homePoints.Length, awayPoints.Length
    
  member this.Winner =
    match this.Score with
    | home, away when home > away -> MatchWinner.Home
    | home, away when home < away -> MatchWinner.Away
    | home, away when home = away -> MatchWinner.Draw
    | _ -> failwith "No match result could be found."

type Match =
  | NotStarted of MatchInfo
  | InProgress of InProgressMatch
  | Played of PlayedMatch

module Matches =
  let appendPoint (point: Point) (inProgressMatch: InProgressMatch) =
    { inProgressMatch with Points = point :: inProgressMatch.Points }
      
    