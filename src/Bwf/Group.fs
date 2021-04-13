namespace Bwf

open System

type User =
  { UserId: UserId
    Name: string
    IsOwner: bool
    TypedTopScorer: PlayerId }

type Group =
  { GroupId: GroupId
    Name: string
    Users: User list
    TournamentId: TournamentId }

type Tournament =
  { TournamentId: TournamentId
    Name: string
    StartDate: DateTime }

module Groups =
  let create groupId name owner tournament =
    { GroupId = groupId
      Name = name
      Users = [owner]
      TournamentId = tournament.TournamentId }

