# WebSharper Permissions API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Permissions API](https://developer.mozilla.org/en-US/docs/Web/API/Permissions_API), enabling seamless integration of permission handling in WebSharper applications.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Permissions API.

2. **Sample Project**:
   - Demonstrates how to use the Permissions API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/Permissions/).

## Features

- WebSharper bindings for the Permissions API.
- Query and manage permission statuses for features like notifications, geolocation, and microphone access.
- Example usage for permission handling in web applications.
- Hosted demo to explore API functionality.

## Installation and Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/Permissions.git
   cd Permissions
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.Permissions/WebSharper.Permissions.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.Permissions.Sample
   dotnet build
   dotnet run
   ```

4. Open the hosted demo to see the Sample project in action:
   [https://dotnet-websharper.github.io/Permissions/](https://dotnet-websharper.github.io/Permissions/)

## Why Use the Permissions API

The Permissions API allows web applications to manage and query the status of various browser permissions. Key benefits include:

1. **Improved User Experience**: Request permissions proactively and provide better feedback to users.
2. **Enhanced Security**: Prevent unnecessary permission prompts by checking the current status.
3. **Fine-Grained Control**: Determine whether a feature (e.g., notifications, microphone) is granted, denied, or needs user approval.
4. **Seamless Integration**: Works with other Web APIs like Notifications, Geolocation, and Microphone.

**Note:** If permission prompts do not appear, please check and allow the required permissions manually in your browser settings.

Integrating the Permissions API with WebSharper allows developers to create interactive and secure web applications in F#.

## How to Use the Permissions API

### Example Usage

Below is an example of how to use the Permissions API in a WebSharper project:

```fsharp
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

        // Access the browser's Permissions API
        let permissions = As<Navigator>(JS.Window.Navigator).Permissions

        // Function to check the status of a given permission
        let checkPermission(permissionName: string) =
            let permissionQuery = permissions.Query(PermissionDescriptor(name = permissionName))

            // Handle the result of the permission query
            permissionQuery.Then(fun result ->
                permissionStatus.Value <- $"{permissionName} permission: {result.State}"
            ).Catch(fun _ ->
                permissionStatus.Value <- $"Permission {permissionName} not supported"
            )

        // Initialize the UI template and bind variables to UI elements
        IndexTemplate.Main()
            // Check geolocation permission when the corresponding button is clicked
            .checkGeolocationPermission(fun _ ->
                async {
                    do! checkPermission("geolocation").AsAsync()
                }
                |> Async.Start
            )
            // Check microphone permission when the corresponding button is clicked
            .checkMicrophonePermission(fun _ ->
                async {
                    do! checkPermission("microphone").AsAsync()
                }
                |> Async.Start
            )
            // Check notification permission when the corresponding button is clicked
            .checkNotificationPermission(fun _ ->
                async {
                    do! checkPermission("notifications").AsAsync()
                }
                |> Async.Start
            )
            // Bind the permission status variable to the UI
            .status(permissionStatus.View)
            .Doc()
        |> Doc.RunById "main"
```

This example demonstrates how to check the status of a permission before requesting access.

For a complete implementation, refer to the [Sample Project](https://dotnet-websharper.github.io/Permissions/).
