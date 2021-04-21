namespace Arrangers

open System
open Bwf
open Some_Points

module A_Game_Finished =
  let ``a Finished Game`` () =
    { GameId = Guid.NewGuid() |> GameId
      TournamentId = Guid.NewGuid() |> TournamentId
      EndDate = DateTime.Now
      Points = [] }
    
  let ``with some Points`` points game =
    { game with Points = points }  
    
  let ``with a Score`` (score: Score) game =
    let points = score.Home
                 |> ``some Points`` TeamSide.Home
                 |> List.append (``some Points`` TeamSide.Away score.Away)
    { game with Points = points }
    
  let ``as a Game`` game =
    game |> Game.Finished