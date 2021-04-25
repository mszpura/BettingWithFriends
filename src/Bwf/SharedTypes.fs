namespace Bwf

open System

type GameId = GameId of Guid
type TournamentId = TournamentId of Guid
type TeamId = TeamId of Guid
type PlayerId = PlayerId of Guid
type UserId = UserId of Guid
type GroupId = GroupId of Guid
type BetId = BetId of Guid
type NotEmptyString = private NotEmptyString of string

type GameWinner =
  | Home
  | Away
  | Draw
  
type Score =
  { Home: int
    Away: int }
  with
  member this.Winner =
    match this.Home, this.Away with
    | home, away when home > away -> GameWinner.Home
    | home, away when home < away -> GameWinner.Away
    | home, away when home = away -> GameWinner.Draw
    | _ -> failwith "No game result could be found."
    
type Player =
  { PlayerId: PlayerId
    TeamId: TeamId
    Name: NotEmptyString
    Number: int }
    
type Team =
  { TeamId: TeamId
    TournamentId: TournamentId
    Name: NotEmptyString }
        
type TournamentSettings =
  { PointsForCorrectWinner: int
    PointsForCorrectScorer: int
    PointsForCorrectGameScore: int
    PointsForTypedTopScorer: int }
        
type Tournament =
  { TournamentId: TournamentId
    Name: NotEmptyString
    StartDate: DateTime
    Settings: TournamentSettings }
    
module NotEmptyString =
  let create string =
    match String.IsNullOrWhiteSpace(string) with
    | true -> failwith "Non-empty string is required."
    | _ -> NotEmptyString(string)
    
  let value (NotEmptyString notEmptyString) = notEmptyString