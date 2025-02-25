namespace WebSharper.Permissions.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.Permissions

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    [<SPAEntryPoint>]
    let Main () =
        let permissionStatus = Var.Create ""

        let permissions = As<Navigator>(JS.Window.Navigator).Permissions        

        let checkPermission(permissionName: string) = 
            let permissionQuery = permissions.Query(PermissionDescriptor(name = permissionName))

            permissionQuery.Then(fun result -> 
                permissionStatus := $"{permissionName} permission: {result.State}"
            ).Catch(fun _ -> 
                permissionStatus := $"Permission {permissionName} not supported"
            )

        let requestGeolocation() = 
            JS.Window.Navigator.Geolocation.GetCurrentPosition((fun position ->
                permissionStatus := $"Latitude: {position.Coords.Latitude}, Longitude: {position.Coords.Longitude}"
            ), (fun error ->
                permissionStatus := $"Geolocation error: {error.Message}"
            ))


        IndexTemplate.Main()
            .checkGeolocationPermission(fun _ -> 
                async { 
                    do! checkPermission("geolocation").AsAsync()
                } 
                |> Async.Start
            )
            .requestGeolocation(fun _ -> requestGeolocation())
            .status(permissionStatus.View)
            .Doc()
        |> Doc.RunById "main"
