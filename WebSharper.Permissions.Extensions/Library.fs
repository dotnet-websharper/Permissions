namespace WebSharper.Permissions

open WebSharper
open WebSharper.JavaScript

[<JavaScript; AutoOpen>]
module Extensions =

    type Navigator with
        [<Inline "$this.permissions">]
        member this.Permissions with get(): Permissions = X<Permissions>

    type WorkerNavigator with
        [<Inline "$this.permissions">]
        member this.Permissions with get(): Permissions = X<Permissions>
