namespace Bwf.Application.Commands

open System
open Bwf

type CreateUser =
  { Username: string
    Password: string }
  
type CreateGroup =
  { UserId: UserId
    TournamentId: TournamentId
    Name: NotEmptyString }
  
type JoinToGroup =
  { UserId: UserId
    GroupId: GroupId }
  
type AddTopScorer =
  { UserId: UserId
    GroupId: GroupId
    TypedTopScorerId: PlayerId }
  
type CreateGame =
  { TournamentId: TournamentId
    HomeId: TeamId
    AwayId: TeamId
    StartDate: DateTime }

type FinishGame =
  { GameId: GameId
    Points: Point list
    EndDate: DateTime }
  
type CreateTournament =
  { TournamentName: string
    StartDate: DateTime }