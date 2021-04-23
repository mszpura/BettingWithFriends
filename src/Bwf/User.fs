namespace Bwf

open System.Text.RegularExpressions

type Password = private Password of NotEmptyString

type User =
  { UserId: UserId
    Username: NotEmptyString
    Password: Password }
  
module Password =
  let value (Password password) = password |> NotEmptyString.value
  let create (password: string) =
    let password =
      match password with
      | password when not(Regex.IsMatch(password, @".{8}")) -> failwith "Password must have at least 8 digits"
      | password when not(Regex.IsMatch(password, @"[0-9]+")) -> failwith "Password must have at least one digit"
      | password when not(Regex.IsMatch(password, @"[#?!@$%^&*_=+'/,.;:?]+")) -> failwith "Password must have at least one special character"
      | password when not(Regex.IsMatch(password, @"[a-z]+")) -> failwith "Password must have at least one lowercase character"
      | password when not(Regex.IsMatch(password, @"[A-Z]+")) -> failwith "Password must have at least one uppercase character"
      | _ -> password
    password |> NotEmptyString.create |> Password