namespace Bwf.Tests

open System
open Arrangers
open Bwf
open Xunit
open Test_Game
open Test_Team
open Test_User
open Test_TeamMember
open FsUnit.Xunit

module BetsTests =
  [<Fact>]
  let ``Creates a Bet for not started Game`` () =
    // Arrange
    let typedScore = { Home = 3; Away = 2 }
    let typedScorer = ``create Team Member``
    let game = ``create default not started Game``
               |> ``with Home team`` (``create default Team``
                                      |> ``with Member`` typedScorer)
    let betId = Guid.NewGuid() |> BetId
    let user = ``create default User``
    // Act
    let result = Bets.create betId game user typedScore.Home typedScore.Away typedScorer
    // Assert
    let bet = match result with
              | Bet.ClosedBet _ -> failwith "Bet is in wrong State"
              | Bet.ReadyBet readyBet -> readyBet
    bet.User |> should equal user
    bet.BetId |> should equal betId
    bet.NotStartedGame |> should equal game
    bet.TypedScore |> should equal typedScore
    bet.TypedScorer |> should equal typedScorer
    
  [<Fact>]
  let ``Creates a Bet fails when typed Scorer is not in any team played in a Game`` () =
    // Arrange
    let typedScorerNotInAnyTeam = Test_TeamMember.``create Team Member``
    let game = ``create default not started Game``
    let betId = Guid.NewGuid() |> BetId
    let typedScore = { Home = 2; Away = 2 }
    let user = ``create default User``
    // Act & Assert
    (fun () -> Bets.create betId game user typedScore.Home typedScore.Away typedScorerNotInAnyTeam |> ignore)
    |> should (throwWithMessage "Typed scorer is not in any team played.") typeof<Exception>
    
  