#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Xamarin
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open System
open System.Text
open System.Text.RegularExpressions
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


Target.create "Android-Package" (fun _ ->
    AndroidPackage (fun defaults ->
        { defaults with
            ProjectPath = "./PetArmy.Android"
            Configuration = "Debug"
            OutputPath = androidBuildDir })
    |> Some  
    |> Option.map (fun file -> 
        let apk_unsigned:string = file.FullName
        let apk_signed = Path.combine androidProdDir (Regex.Replace(file.FullName, ".apk$", "-Signed.apk") )
        executer "zipalign" ["-f";"-v";"4";apk_unsigned;apk_signed]
        |> fun output -> 
            match output.ExitCode with
            | 0 -> Trace.logVerbosefn "Output:", output.ToString
            | _ -> Trace.logVerbosefn "Error:", output.Result.ToString
            |> ignore
        FileInfo.ofPath(apk_signed)
        )
    |> Option.map (fun file ->
        let keystore = @"/home/freexploit/.local/share/Xamarin/Mono for Android/debug.keystore" |> Path.getFullName

        executer "apksigner" [ "sign";"--ks"; keystore; "--ks-key-alias"; "androiddebugkey"; "--ks-pass";"pass:android"; file.FullName ]

        |> fun output -> 
            match output.ExitCode with
            | 0 -> Trace.logVerbosefn "Output:", output.ToString
            | _ -> Trace.logVerbosefn "Error:", output.Result.ToString
            |> ignore
        file

        ) 
    |> Option.map (fun file -> file.CopyTo(Path.combine androidProdDir file.Name) ) 
    |>ignore
    )


Target.create "Android" ignore

"Clean"
==> "BuildAndroid"
==> "Android-Package"
==> "Android"


Target.create "All" ignore

"Clean" ==> "Build" ==> "All"

Target.runOrDefault "All"
