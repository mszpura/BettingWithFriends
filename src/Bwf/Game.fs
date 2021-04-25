namespace Bwf

open System

type TeamSide =
  | Home
  | Away

type Point =
  { PlayerId: PlayerId
    TeamSide: TeamSide }
  
type Result =
  { Points: Point list }
  member this.Score =
    let home, away = this.Points |> List.partition (fun point -> point.TeamSide = TeamSide.Home)
    { Home = home.Length; Away = away.Length}
  member this.ScoredPlayers =
    this.Points |> List.map (fun point -> point.PlayerId) |> List.distinct

type OpenGame = {
  GameId: GameId
  Tournament: Tournament
  HomeId: TeamId
  AwayId: TeamId
  StartDate: DateTime
}

type FinishedGame = {
  GameId: GameId
  Tournament: Tournament
  EndDate: DateTime
  Result: Result
} 

type Game =
  | Open of OpenGame
  | Finished of FinishedGame

module Games =
  let create (home: Team) (away: Team) startDate (tournament: Tournament) =
    { GameId = Guid.NewGuid() |> GameId
      Tournament = tournament
      HomeId = home.TeamId
      AwayId = away.TeamId
      StartDate = startDate }
  
  let finish points endDate (game: OpenGame) =
    { GameId = game.GameId
      Tournament = game.Tournament
      EndDate = endDate
      Result = { Points = points } }