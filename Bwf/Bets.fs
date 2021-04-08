namespace Bwf

open System

type UserId = UserId of Guid
type GroupId = GroupId of Guid
type BetId = BetId of Guid

type Email = Email of string
type Password = Password of string

type User = {
  UserId: UserId
  Name: string
  Email: Email
  Password: Password
  TypedTopScorer: TeamMember option
}

type GroupSettings = {
  PointsForTypedWinner: int
  PointsForTypedExactScore: int
  PointsForTypedTopScorer: int
  PointsForTypedGoalScorer: int
}

type Group = {
  GroupId: GroupId
  Name: string
  Users: User list
  GroupSettings: GroupSettings
  Tournament: Tournament
}

type Bet = {
  BetId: BetId
  User: User
  Match: Match
  TypedHomeScore: int
  TypedAwayScore: int
  TypedScorer: TeamMember
  PointsEarned: int
}
