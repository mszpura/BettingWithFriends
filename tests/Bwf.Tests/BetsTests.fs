namespace Bwf.Tests

open System
open Arrangers
open Bwf
open Xunit
open A_Game_Finished
open FsUnit.Xunit

module BetsTests =
  let private PointsToWin =
    { PointsForCorrectWinner = 1
      PointsForCorrectScorer = 2
      PointsForCorrectGameScore = 3 }
  
  [<Fact>]
  let ``Creates a Bet for not started Game`` () =
    // Arrange
    let typedScore = { Home = 3; Away = 2 }
    let typedScorer = Guid.NewGuid() |> PlayerId
    let gameId = Guid.NewGuid() |> GameId
    let betId = Guid.NewGuid() |> BetId
    let userId = Guid.NewGuid() |> UserId
    // Act
    let result = Bets.create betId gameId userId typedScore typedScorer
    // Assert
    result.UserId |> should equal userId
    result.BetId |> should equal betId
    result.GameId |> should equal gameId
    result.TypedScore |> should equal typedScore
    result.TypedScorerId |> should equal typedScorer
      
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
    let betId = Guid.NewGuid() |> BetId
    let userId = Guid.NewGuid() |> UserId
    let typedScorer = Guid.NewGuid() |> PlayerId
    let scorePredicted = { Home = homeScorePredicted; Away = awayScorePredicted }
    let game = ``a Finished Game`` ()
               |> ``with a Score`` { Home = homeScore; Away = awayScore }
    let bet = Bets.create betId game.GameId userId scorePredicted typedScorer
    // Act
    let (BetResult points) = bet |> Bets.finishBet PointsToWin game
    // Assert 
    points |> should equal expectedPointsEarned
    
  [<Fact>]
  let ``Calculates earned points from predicted scored player`` () =
    // Arrange
    let betId = Guid.NewGuid() |> BetId
    let userId = Guid.NewGuid() |> UserId
    let predictedScorer = Guid.NewGuid() |> PlayerId
    let points = [{ PlayerId = predictedScorer; TeamSide = TeamSide.Away }]
    let game = ``a Finished Game`` ()
               |> ``with some Points`` points
    let notPredictedScore = { Home = 3; Away = 0 }
    let bet = Bets.create betId game.GameId userId notPredictedScore predictedScorer
    // Act
    let (BetResult points) = bet |> Bets.finishBet PointsToWin game
    // Assert 
    points |> should equal PointsToWin.PointsForCorrectScorer
    
  [<Fact>]
  let ``Calculates 0 points when bet not predicting anything`` () =
    // Arrange
    let betId = Guid.NewGuid() |> BetId
    let userId = Guid.NewGuid() |> UserId
    let predictedScorer = Guid.NewGuid() |> PlayerId
    let game = ``a Finished Game`` ()
               |> ``with a Score`` { Home = 0; Away = 3 }
    let notPredictedScore = { Home = 3; Away = 0 }
    let bet = Bets.create betId game.GameId userId notPredictedScore predictedScorer
    // Act
    let (BetResult points) = bet |> Bets.finishBet PointsToWin game
    // Assert 
    points |> should equal 0