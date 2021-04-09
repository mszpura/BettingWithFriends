namespace Bwf

open System

type TournamentId = TournamentId of Guid
type TeamId = TeamId of Guid
type TeamMemberId = TeamMemberId of Guid

type TeamMember = {
  TeamMemberId: TeamMemberId
  Name: string
  Number: int
}

type Team = {
  TeamId: TeamId
  Name: string
  Members: TeamMember list
}

type Tournament = {
  TournamentId: TournamentId
  Name: string
  Teams: Team list
  StartDate: DateTime
  EndDate: DateTime
  EndOfBettingTopScorerDate: DateTime
}

