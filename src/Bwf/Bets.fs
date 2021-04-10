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
  PointsForTypedScorer: int
}

type Group = {
  GroupId: GroupId
  Name: string
  Users: User list
  GroupSettings: GroupSettings
  Tournament: Tournament
}

type ReadyBet = {
  BetId: BetId
  User: User
  NotStartedGame: NotStartedGame
  TypedScore: Score
  TypedScorer: TeamMember
} 
    
type ClosedBet = {
  BetId: BetId
  User: User
  FinishedGame: FinishedGame
  TypedScore: Score
  TypedScorer: TeamMember
  PointsEarned: int
}

type Bet =
  | ReadyBet of ReadyBet
  | ClosedBet of ClosedBet

module Bets =
  let private checkIfSelectedScorerIsInPlayingTeam game typedScorer =
    let (NotStartedGame game) = game
    match game.Home.isMemberInTeam typedScorer, game.Away.isMemberInTeam typedScorer with
    | false, false -> failwith "Typed scorer is not in any team played."
    | _ -> typedScorer
    
  let create id (notStartedGame: NotStartedGame) user typedHomeScore typedAwayScore typedScorer =
    let scorer = typedScorer |> checkIfSelectedScorerIsInPlayingTeam notStartedGame
    { BetId = id
      User = user
      NotStartedGame = notStartedGame
      TypedScore = {
        Home = typedHomeScore
        Away = typedAwayScore
      }
      TypedScorer = scorer }
    |> Bet.ReadyBet
    
  let private calculatePointsForTypedScorer pointsForTypedTopScorer bet =
    let typeResult = bet.FinishedGame.Points
                     |> List.tryFind (fun point -> point.TeamMember = bet.TypedScorer)
    let pointsEarned = match typeResult with
                       | Some _ -> pointsForTypedTopScorer
                       | None -> 0
    { bet with PointsEarned = bet.PointsEarned + pointsEarned }
    
  let private calculatePointsForTypedScore pointsForTypedScore bet =
    let pointsEarned = if bet.TypedScore = bet.FinishedGame.Score then pointsForTypedScore else 0
    { bet with PointsEarned = bet.PointsEarned + pointsEarned }
    
  let private calculatePointsForTypedWinner pointsForTypedWinner bet =
    let pointsEarned = if bet.TypedScore.Winner = bet.FinishedGame.Score.Winner then pointsForTypedWinner else 0
    { bet with PointsEarned = bet.PointsEarned + pointsEarned }
    
  let calculatePointsEarned groupSettings bet =
    bet
    |> calculatePointsForTypedScorer groupSettings.PointsForTypedScorer
    |> calculatePointsForTypedScore groupSettings.PointsForTypedExactScore
    |> calculatePointsForTypedWinner groupSettings.PointsForTypedWinner
    