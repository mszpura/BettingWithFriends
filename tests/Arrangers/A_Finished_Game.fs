namespace Arrangers

open System
open Bwf
open Some_Points
open A_Tournament

module A_Finished_Game =
  let ``a Finished Game`` () =
    { GameId = Guid.NewGuid() |> GameId
      Tournament = ``a tournament``()
      EndDate = DateTime.Now
      Result = { Points = [] } }
    
  let ``with some Points`` points (game: FinishedGame) =
    let result = { Points = points }
    { game with Result = result }  
    
  let ``with a Score`` (score: Score) (game: FinishedGame) =
    let points = score.Home
                 |> ``some Points`` TeamSide.Home
                 |> List.append (``some Points`` TeamSide.Away score.Away)
    let result = { Points = points }
    { game with Result = result } 
    
  let ``as a Game`` game =
    game |> Game.Finished