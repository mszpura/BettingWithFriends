namespace Bwf

open System

type GameId = GameId of Guid

type Point = {
  TeamId: TeamId
  TeamMember: TeamMember
  ScoredMinute: int
}

type GameWinner =
  | Home
  | Away
  | Draw
  
type Score = {
  Home: int
  Away: int
} with
  member this.Winner =
    match this.Home, this.Away with
    | home, away when home > away -> GameWinner.Home
    | home, away when home < away -> GameWinner.Away
    | home, away when home = away -> GameWinner.Draw
    | _ -> failwith "No game result could be found."

type GameDetails = {
  GameId: GameId
  TournamentId: TournamentId
  Home: Team
  Away: Team
  StartDate: DateTime
}

type NotStartedGame = NotStartedGame of GameDetails

type InProgressGame = {
  Details: GameDetails
  Points: Point list
  Score: Score
}

type FinishedGame = {
  Details: GameDetails
  Points: Point list
  Score: Score
  EndDate: DateTime
}

type Game =
  | NotStarted of NotStartedGame
  | InProgress of InProgressGame
  | Finished of FinishedGame

module Games =
  let create id home away tournamentId startDate =
    { GameId = id
      TournamentId = tournamentId
      Home = home
      Away = away
      StartDate = startDate }
    |> NotStartedGame
    |> Game.NotStarted
    
  let start (notStartedGame: NotStartedGame) =
    let (NotStartedGame gameDetails) = notStartedGame
    { Details = gameDetails; Points = []; Score = { Home = 0; Away = 0 } }
    |> Game.InProgress
  
  let appendPoint point (inProgressGame: InProgressGame) =
    let score =
      match point.TeamId with
      | point when point = inProgressGame.Details.Home.TeamId -> { inProgressGame.Score with Home = inProgressGame.Score.Home + 1 }
      | point when point = inProgressGame.Details.Away.TeamId -> { inProgressGame.Score with Away = inProgressGame.Score.Away + 1 }
      | _ -> failwith "Team scored given point is not playing this Game."
    { inProgressGame with
        Points = point :: inProgressGame.Points
        Score = score }
    |> Game.InProgress
    
  let finish (inProgressGame: InProgressGame) =
    { Details = inProgressGame.Details
      Points = inProgressGame.Points
      Score = inProgressGame.Score
      EndDate = DateTime.Now }
    |> Game.Finished