namespace Arrangers

open System
open Bwf

module Test_TeamMember =
  let ``create Team Member`` = {
    TeamMemberId = Guid.NewGuid() |> TeamMemberId
    Name = $"{Guid.NewGuid()}"
    Number = 69
  }
  
  let ``with Number`` number teamMember =
    { teamMember with Number = number }
  
  let ``some Team Members`` amount =
    [1..amount]
    |> List.map (fun number -> ``create Team Member`` |> ``with Number`` number)

