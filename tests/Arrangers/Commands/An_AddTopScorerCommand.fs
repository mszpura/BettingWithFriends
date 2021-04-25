namespace Arrangers.Commands

open System
open Bwf
open Bwf.Application.Commands

module An_AddTopScorerCommand =
  let ``an Add Top Scorer command`` (): AddTopScorer =
    { UserId = Guid.NewGuid() |> UserId
      GroupId = Guid.NewGuid() |> GroupId
      TypedTopScorerId = Guid.NewGuid() |> PlayerId }

