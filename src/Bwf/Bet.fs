namespace Bwf

open System

type UserBetDetails =
  { UserId: UserId
    Score: Score
    ScorerId: PlayerId }

type OpenBet =
  { BetId: BetId
    Game: OpenGame
    Details: UserBetDetails }
  
type FinishedBet =
  { BetId: BetId
    Game: FinishedGame
    Details: UserBetDetails }
  with
  member this.GotPointsForWinner =
    this.Details.Score.Winner = this.Game.Result.Score.Winner
  member this.GotPointsForScore =
    this.Details.Score = this.Game.Result.Score
  member this.GotPointsForScorer =
    this.Game.Result.Points |> List.exists (fun point -> point.PlayerId = this.Details.ScorerId)
  member this.ScoredPoints =
    let pointsForWinner = if this.GotPointsForWinner then this.Game.Tournament.Settings.PointsForCorrectWinner else 0
    let pointsForScore = if this.GotPointsForScore then this.Game.Tournament.Settings.PointsForCorrectGameScore else 0
    let pointsForScorer = if this.GotPointsForScorer then this.Game.Tournament.Settings.PointsForCorrectScorer else 0
    pointsForWinner + pointsForScore + pointsForScorer

type Bet =
  | Open of OpenBet
  | Finished of FinishedBet

module Bets =
  let create typedScore typedScorerId game (user: User): OpenBet =
    { BetId = Guid.NewGuid() |> BetId
      Game = game
      Details =
        { UserId = user.UserId
          Score = typedScore
          ScorerId = typedScorerId } }