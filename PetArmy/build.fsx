#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Xamarin
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open System
open System.Text
open Fake.Core
open Fake.DotNet
open Fake.DotNet.Xamarin
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

let executer (command:string) args =
    Command.RawCommand(command, Arguments.OfArgs args)
        |> CreateProcess.fromCommand
        |> Proc.run
        |> fun result -> 
            let output = String.Join (Environment.NewLine, result.ToString)
            Trace.logVerbosefn "Process: \r\n%A", output
        |> ignore


type ApkSignerParams = 
    { KeystorePath : string
      KeystorePassword: string;
      KeyStoreAlias: string }


let androidBuildDir = "./build-android/"
let androidProdDir = "./pack/"

androidProdDir |> Directory.ensure

Target.create "Clean" (fun _ ->
    !! "**/bin"
    ++ "**/obj"
    ++ "./build-android"
    ++ "./pack"
    |> Shell.cleanDirs)

Target.create "Build" (fun _ -> !! "**/*.*proj" |> Seq.iter (DotNet.build id))

Target.create "BuildAndroid" (fun _ ->
    let setParams (defaults: MSBuildParams) =
        { defaults with
            Verbosity = Some(Quiet)
            Targets = [ "Build" ]
            DoRestore = true
            WorkingDirectory = "./PetArmy.Android"
            Properties =
                [ "Optimize", "True"
                  "DebugSymbols", "True"
                  "Configuration", "Debug" ] }

    !! "PetArmy.Android/*.csproj"
    |> MSBuild.runDebug setParams "./build-android/" "Build"
    |> Trace.logItems "Appbuild-output:")

let exceptions fileInfo =
  try
    AndroidSignAndAlign (fun defaults ->
            { defaults with
                KeystorePath = "/home/freexploit/.local/share/Xamarin/Mono\ for\ Android/debug.keystore"
                KeystorePassword = "android" // TODO: don't store this in the build script for a real app!
                KeystoreAlias = "androiddebugkey"
                SignatureAlgorithm = "SHA256withRSA"
            }
        ) fileInfo |> Some
  with
    | :? System.Exception as ex -> printfn "Exception! %s " (ex.Message); None

Target.create "Android-Package" (fun _ ->
    AndroidPackage (fun defaults ->
        { defaults with
            ProjectPath = "./PetArmy.Android"
            Configuration = "Debug"
            OutputPath = androidBuildDir })
    |> exceptions  
    |> Option.map (fun file -> file.CopyTo(Path.combine androidProdDir file.Name) ) |> ignore 
    )

Target.create "debugging" (fun _ -> 
    executer "/bin/ls" ["-a";"-l"]
)


Target.create "Android" ignore

"Clean"
==> "BuildAndroid"
==> "Android-Package"
==> "Android"


Target.create "All" ignore

"Clean" ==> "Build" ==> "All"

Target.runOrDefault "All"
