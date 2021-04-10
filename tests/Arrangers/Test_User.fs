namespace Arrangers

open System
open Bwf

module Test_User =
  let ``create default User`` = {
    UserId = Guid.NewGuid() |> UserId
    Name = $"{Guid.NewGuid()}"
    Email = $"{Guid.NewGuid()}@{Guid.NewGuid()}" |> Email
    Password = $"{Guid.NewGuid}" |> Password
    TypedTopScorer = None
  }

