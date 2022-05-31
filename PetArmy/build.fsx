#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Xamarin
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.DotNet.Xamarin
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

let androidBuildDir = "./build-android/"
let androidProdDir = "./pack/"

androidProdDir |> Directory.ensure

Target.create "Clean" (fun _ ->
    !! "**/bin"
    ++ "**/obj"
    ++ "./build-android"
    ++ "./pack"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "BuildAndroid" (fun _ -> 
    let setParams (defaults:MSBuildParams) =
        { defaults with
            Verbosity = Some(Quiet)
            Targets = ["Build"]
            DoRestore = true
            WorkingDirectory = "./PetArmy.Android"
            Properties =
                [
                    "Optimize", "True"
                    "DebugSymbols", "True"
                    "Configuration", "Debug"
                ]
         }
    
    !! "PetArmy.Android/*.csproj"
    |> MSBuild.runDebug setParams "./build-android/" "Build"
    |> Trace.logItems "Appbuild-output:"
)


Target.create "Android-Package" (fun _ ->
    AndroidPackage(fun defaults ->
                    { defaults with 
                        ProjectPath = "./PetArmy.Android"
                        Configuration = "Debug"
                        OutputPath = androidBuildDir
                    }
                )
    |> fun file -> file.CopyTo(Path.combine androidProdDir file.Name) |> ignore

)

Target.create "Android" ignore

"Clean"
   ==> "BuildAndroid"
   ==> "Android-Package"
   ==> "Android"


Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "All"

Target.runOrDefault "All"
