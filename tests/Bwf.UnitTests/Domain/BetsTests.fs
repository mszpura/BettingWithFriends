namespace Bwf.UnitTests.Domain

open System
open Arrangers
open Bwf
open Xunit
open A_Finished_Game
open FsUnit.Xunit
open An_User
open An_Open_Game

module BetsTests =
  [<Fact>]
  let ``Creates a Bet for not started Game`` () =
    // Arrange
    let typedScore = { Home = 3; Away = 2 }
    let typedScorer = Guid.NewGuid() |> PlayerId
    let game = ``an Open Game``()
    let user = ``an User``()
    // Act
    let result = user |> Bets.create typedScore typedScorer game
    // Assert
    result.Details.UserId |> should equal user.UserId
    result.BetId |> should not' (be Null)
    result.Game |> should equal game
    result.Details.Score |> should equal typedScore
    result.Details.ScorerId |> should equal typedScorer
      
  [<Theory>]
  [<InlineData(2, 1, 2, 1, 4)>]
  [<InlineData(2, 2, 2, 2, 4)>]
  [<InlineData(2, 3, 3, 2, 0)>]
  [<InlineData(0, 0, 0, 0, 4)>]
  [<InlineData(4, 2, 3, 2, 1)>]
  [<InlineData(0, 1, 12, 13, 1)>]
  let ``Calculates earned points from predicted game score``
    homeScorePredicted
    awayScorePredicted
    homeScore
    awayScore
    expectedPointsEarned
    =
    // Arrange
    let typedScorer = Guid.NewGuid() |> PlayerId
    let scorePredicted = { Home = homeScorePredicted; Away = awayScorePredicted }
    let game = ``a Finished Game`` ()
               |> ``with a Score`` { Home = homeScore; Away = awayScore }
    // Act
    let finishedBet: FinishedBet =
      { BetId = Guid.NewGuid() |> BetId
        Game = game
        Details =
          { UserId = Guid.NewGuid() |> UserId
            Score = scorePredicted
            ScorerId = typedScorer }}
    // Assert 
    finishedBet.ScoredPoints |> should equal expectedPointsEarned
    
  [<Fact>]
  let ``Calculates earned points from predicted scored player`` () =
    // Arrange
    let predictedScorer = Guid.NewGuid() |> PlayerId
    let points = [{ PlayerId = predictedScorer; TeamSide = TeamSide.Away }]
    let game = ``a Finished Game`` ()
               |> ``with some Points`` points
    let notPredictedScore = { Home = 3; Away = 0 }
    // Act
    let finishedBet: FinishedBet =
      { BetId = Guid.NewGuid() |> BetId
        Game = game
        Details =
          { UserId = Guid.NewGuid() |> UserId
            Score = notPredictedScore
            ScorerId = predictedScorer }}
    // Assert 
    finishedBet.ScoredPoints |> should equal game.Tournament.Settings.PointsForCorrectScorer
    
  [<Fact>]
  let ``Calculates 0 points when bet not predicting anything`` () =
    // Arrange
    let predictedScorer = Guid.NewGuid() |> PlayerId
    let game = ``a Finished Game`` ()
               |> ``with a Score`` { Home = 0; Away = 3 }
    let notPredictedScore = { Home = 3; Away = 0 }
    // Act
    let finishedBet: FinishedBet =
      { BetId = Guid.NewGuid() |> BetId
        Game = game
        Details =
          { UserId = Guid.NewGuid() |> UserId
            Score = notPredictedScore
            ScorerId = predictedScorer }}
    // Assert 
    finishedBet.ScoredPoints |> should equal 0