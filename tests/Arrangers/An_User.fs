﻿namespace Arrangers

open System
open Bwf

module An_User =
  let ``an User`` () =
    { UserId = Guid.NewGuid() |> UserId
      Name = $"{Guid.NewGuid()}"
      IsOwner = false
      TypedTopScorer = Guid.NewGuid() |> PlayerId |> Some }
