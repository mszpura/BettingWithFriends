namespace Arrangers

open System
open Bwf

module Some_Points =
  let ``some Points`` teamSide amount =
    [1..amount]
    |> List.map (fun point -> { PlayerId = Guid.NewGuid() |> PlayerId; TeamSide = teamSide })

