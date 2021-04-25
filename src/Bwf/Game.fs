namespace Bwf

open System

type TeamSide =
  | Home
  | Away

type Point =
  { PlayerId: PlayerId
    TeamSide: TeamSide }

type NotStartedGame = {
  GameId: GameId
  TournamentId: TournamentId
  HomeId: TeamId
  AwayId: TeamId
  StartDate: DateTime
}

type FinishedGame = {
  GameId: GameId
  TournamentId: TournamentId
  EndDate: DateTime
  Points: Point list
} with
  member this.Score =
    let home, away = this.Points |> List.partition (fun point -> point.TeamSide = TeamSide.Home)
    { Home = home.Length; Away = away.Length}
  member this.ScoredPlayers =
    this.Points |> List.map (fun point -> point.PlayerId) |> List.distinct

type Game =
  | NotStarted of NotStartedGame
  | Finished of FinishedGame

module Games =
  let create (home: Team) (away: Team) startDate (tournament: Tournament) =
    { GameId = Guid.NewGuid() |> GameId
      TournamentId = tournament.TournamentId
      HomeId = home.TeamId
      AwayId = away.TeamId
      StartDate = startDate }
  
  let finish points endDate (game: NotStartedGame) =
    { GameId = game.GameId
      TournamentId = game.TournamentId
      EndDate = endDate
      Points = points }