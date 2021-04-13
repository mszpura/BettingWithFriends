namespace Bwf

type Email = Email of string
type Password = Password of string

type PointsToWin =
  { PointsForCorrectWinner: int
    PointsForCorrectScorer: int
    PointsForCorrectGameScore: int }

type Bet =
  { BetId: BetId
    GameId: GameId
    UserId: UserId
    TypedScore: Score
    TypedScorerId: PlayerId } 

type BetResult = BetResult of int

module Bets =
  let create id gameId userId typedScore typedScorerId =
    { BetId = id
      UserId = userId
      GameId = gameId
      TypedScore = typedScore
      TypedScorerId = typedScorerId }  
    
  let private calculatePointsForTypedScorer pointsForTypedTopScorer (finishedGame: FinishedGame) bet =
    let player = finishedGame.ScoredPlayers |> List.tryFind (fun playerId -> playerId = bet.TypedScorerId)
    match player with
    | Some _ ->  pointsForTypedTopScorer
    | None -> 0
    
  let private calculatePointsForTypedScore pointsForTypedScore (finishedGame: FinishedGame) bet =
    if bet.TypedScore = finishedGame.Score then pointsForTypedScore else 0
    
  let private calculatePointsForTypedWinner pointsForTypedWinner (finishedGame: FinishedGame) bet =
    if bet.TypedScore.Winner = finishedGame.Score.Winner then pointsForTypedWinner else 0
    
  let finishBet pointsToEarn finishedGame bet =
    let points = calculatePointsForTypedWinner pointsToEarn.PointsForCorrectWinner finishedGame bet +
                 calculatePointsForTypedScore pointsToEarn.PointsForCorrectGameScore finishedGame bet +
                 calculatePointsForTypedScorer pointsToEarn.PointsForCorrectScorer finishedGame bet
    points |> BetResult
    