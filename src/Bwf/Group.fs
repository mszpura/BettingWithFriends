namespace Bwf

open System

type GroupUser =
  { UserId: UserId
    Name: NotEmptyString
    IsOwner: bool
    TypedTopScorer: PlayerId option }

type Group =
  { GroupId: GroupId
    Name: NotEmptyString
    Users: GroupUser list
    TournamentId: TournamentId }

type Tournament =
  { TournamentId: TournamentId
    Name: NotEmptyString
    StartDate: DateTime }

module Groups =
  let create name (owner: User) tournament =
    match tournament.StartDate with
    | startDate when startDate < DateTime.Now -> failwith "Tournament already started"
    | _ -> ()
    { GroupId = Guid.NewGuid() |> GroupId
      Name = name
      Users =
        [ { UserId = owner.UserId
            Name = owner.Username
            IsOwner = true
            TypedTopScorer = None } ]
      TournamentId = tournament.TournamentId }
    
  let private validateUserExistInGroup userIdToAdd group =
    let userFound = group.Users |> List.tryFind (fun user -> user.UserId = userIdToAdd)
    match userFound with
    | Some _ -> failwith "User is already in the group"
    | None -> ()
    
  let appendUser (user: User) group =
    group |> validateUserExistInGroup user.UserId 
    let user =
      { UserId = user.UserId
        Name = user.Username
        IsOwner = false
        TypedTopScorer = None }
    { group with Users = user :: group.Users }
    
  let private replaceUser user list =
    list |> List.map (fun oldUser -> if oldUser.UserId = user.UserId then user else oldUser)
    
  let addTypedScorerToUser typedScorerId userId (group: Group) =
    let userOption = group.Users |> List.tryFind (fun user -> user.UserId = userId)
    let user = match userOption with
               | Some user -> { user with TypedTopScorer = typedScorerId |> Some }
               | None -> failwith "User not found"
    { group with Users = group.Users |> replaceUser user }