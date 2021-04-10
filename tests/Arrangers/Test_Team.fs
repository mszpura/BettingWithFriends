namespace Arrangers

open System
open Bwf

module Test_Team =
  let ``create default Team`` = {
      TeamId = Guid.NewGuid() |> TeamId
      Name = $"{Guid.NewGuid()}"
      Members = Test_TeamMember.``some Team Members`` 11
  }
  
  let ``with Member`` teamMember team =
    { team with Members = teamMember :: team.Members }

