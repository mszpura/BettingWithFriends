namespace Bwf

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
    
  let private calculatePointsForTypedScorer (finishedGame: FinishedGame) bet =
    let player = finishedGame.Result.ScoredPlayers |> List.tryFind (fun playerId -> playerId = bet.TypedScorerId)
    match player with
    | Some _ ->  finishedGame.Tournament.Settings.PointsForCorrectScorer
    | None -> 0
    
  let private calculatePointsForTypedScore (finishedGame: FinishedGame) bet =
    if bet.TypedScore =
      finishedGame.Result.Score then finishedGame.Tournament.Settings.PointsForCorrectGameScore else 0
    
  let private calculatePointsForTypedWinner (finishedGame: FinishedGame) bet =
    if bet.TypedScore.Winner =
      finishedGame.Result.Score.Winner then finishedGame.Tournament.Settings.PointsForCorrectWinner else 0
    
  let finishBet finishedGame bet =
    let points = calculatePointsForTypedWinner finishedGame bet +
                 calculatePointsForTypedScore finishedGame bet +
                 calculatePointsForTypedScorer finishedGame bet
    points |> BetResult