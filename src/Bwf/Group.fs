namespace Bwf

open System

type User =
  { UserId: UserId
    Name: string
    IsOwner: bool
    TypedTopScorer: PlayerId option }

type Group =
  { GroupId: GroupId
    Name: string
    Users: User list
    TournamentId: TournamentId }

type Tournament =
  { TournamentId: TournamentId
    Name: string
    StartDate: DateTime }

module Groups =
  let create name ownerId ownerName tournament =
    match tournament.StartDate with
    | startDate when startDate < DateTime.Now -> failwith "Tournament already started"
    | _ -> ()
    { GroupId = Guid.NewGuid() |> GroupId
      Name = name
      Users =
        [ { UserId = ownerId
            Name = ownerName
            IsOwner = true
            TypedTopScorer = None } ]
      TournamentId = tournament.TournamentId }
    
  let private validateUserExistInGroup userIdToAdd group =
    let userFound = group.Users |> List.tryFind (fun user -> user.UserId = userIdToAdd)
    match userFound with
    | Some _ -> failwith "User is already in the group"
    | None -> ()
    
  let appendUser userId userName group =
    group |> validateUserExistInGroup userId 
    let user =
      { UserId = userId
        Name = userName
        IsOwner = false
        TypedTopScorer = None }
    { group with Users = user :: group.Users }
    
  let private replaceUser user list =
    list |> List.map (fun oldUser -> if oldUser.UserId = user.UserId then user else oldUser)
    
  let chooseTypedScorerByUser typedScorerId userId (group: Group) =
    let userOption = group.Users |> List.tryFind (fun user -> user.UserId = userId)
    let user = match userOption with
               | Some user -> { user with TypedTopScorer = typedScorerId |> Some }
               | None -> failwith "User not found"
    { group with Users = group.Users |> replaceUser user }
    

