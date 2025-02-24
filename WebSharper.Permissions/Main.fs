namespace WebSharper.Permissions

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let PermissionState =
        Pattern.EnumStrings "PermissionState" [
            "granted"
            "denied"
            "prompt"
        ]

    let PermissionDescriptor =
        Pattern.Config "PermissionDescriptor" {
            Required = [
                "name", T<string> 
            ]
            Optional = [
                "userVisibleOnly", T<bool> 
                "sysex", T<bool> 
            ]
        }

    let PermissionStatus =
        Class "PermissionStatus"
        |=> Inherits T<Dom.EventTarget>
        |+> Instance [
            "name" =? T<string>
            "state" =? PermissionState.Type

            "onchange" =@ T<Dom.Event> ^-> T<unit>
        ]

    let Permissions =
        Class "Permissions"
        |+> Instance [
            "query" => PermissionDescriptor?descriptor ^-> T<Promise<_>>[PermissionStatus.Type]
        ]

    let Navigator = 
        Class "Navigator"
        |+> Instance [
            "permissions" =? Permissions
        ]

    let WorkerNavigator =
        Class "WorkerNavigator"
        |+> Instance [
            "permissions" =? Permissions
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.Permissions" [
                WorkerNavigator
                Navigator
                Permissions
                PermissionStatus
                PermissionState
                PermissionDescriptor
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
