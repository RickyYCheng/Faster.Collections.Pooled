// NOTE: AI-Generated Build Script for Faster.Collections.Pooled
// Usage: dotnet fsi build.fsx [command] [-c|--config <Configuration>] [-f|--framework <Framework>]

open System
open System.Diagnostics
open System.IO

let solutionRoot = __SOURCE_DIRECTORY__
let solutionFile = Path.Combine(solutionRoot, "Faster.Collections.Pooled.slnx")

let run cmd args =
    let psi = ProcessStartInfo(FileName = cmd, Arguments = args, UseShellExecute = false)
    psi.WorkingDirectory <- solutionRoot
    use p = Process.Start psi
    p.WaitForExit()
    if p.ExitCode <> 0 then failwith $"Command failed: {cmd} {args}"

let printHeader icon text =
    Console.ForegroundColor <- ConsoleColor.Cyan
    printfn ""
    printfn "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
    printf $"  {icon} "
    Console.ForegroundColor <- ConsoleColor.White
    printfn $"[{text}]"
    Console.ForegroundColor <- ConsoleColor.Cyan
    printfn "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
    Console.ResetColor()

let clean () =
    printHeader "🧹" "CLEAN"
    run "dotnet" "clean"

let restore () =
    printHeader "📦" "RESTORE"
    run "dotnet" $"restore \"{solutionFile}\""

let build config framework =
    let title = $"BUILD - {config}" + if framework <> "" then $" - {framework}" else ""
    printHeader "🔨" title
    let frameworkArg = if framework <> "" then $" -f {framework}" else ""
    run "dotnet" $"build \"{solutionFile}\" -c {config}{frameworkArg}"

let test config framework =
    let title = $"TEST - {config}" + if framework <> "" then $" - {framework}" else ""
    printHeader "🧪" title
    let frameworkArg = if framework <> "" then $" -f {framework}" else ""
    run "dotnet" $"test \"./Faster.Collections.Pooled.Tests/Faster.Collections.Pooled.Tests.csproj\" -c {config}{frameworkArg} --no-build"

let pack config framework =
    let title = $"PACK - {config}" + if framework <> "" then $" - {framework}" else ""
    printHeader "📄" title
    let frameworkArg = if framework <> "" then $" -f {framework}" else ""
    run "dotnet" $"pack \"./Faster.Collections.Pooled/Faster.Collections.Pooled.csproj\" -c {config}{frameworkArg} -o ./artifacts"

let benchmark config framework =
    let title = $"BENCHMARK - {config}" + if framework <> "" then $" - {framework}" else ""
    printHeader "⚡" title
    let frameworkArg = if framework <> "" then $" -f {framework}" else ""
    run "dotnet" $"run --project \"./Faster.Collections.Pooled.Benchmarks/Faster.Collections.Pooled.Benchmarks.csproj\" -c {config}{frameworkArg} --no-build"

let rec parseArgs (args: string[]) (config: string option) (framework: string option) =
    match args with
    | [||] -> config, framework
    | args ->
        let first = args.[0].ToLower()
        match first with
        | "-c" | "--config" ->
            if args.Length > 1 then
                parseArgs args.[2..] (Some args.[1]) framework
            else config, framework
        | "-f" | "--framework" ->
            if args.Length > 1 then
                parseArgs args.[2..] config (Some args.[1])
            else config, framework
        | _ -> parseArgs args.[1..] config framework

let args = fsi.CommandLineArgs.[1..]

let commands = ["clean"; "restore"; "build"; "test"; "pack"; "benchmark"; "ci"]
let command =
    args
    |> Array.tryFind (fun arg -> List.contains (arg.ToLower()) commands)
    |> Option.defaultValue "build"

let parsedConfig, parsedFramework = parseArgs args None None
let config = parsedConfig |> Option.defaultValue "Release"
let framework = parsedFramework |> Option.defaultValue ""

match command with
| "clean" -> clean ()
| "restore" -> restore ()
| "test" -> build config framework; test config framework
| "pack" -> build config framework; pack config framework
| "benchmark" -> build config framework; benchmark config framework
| "ci" -> clean (); restore (); build config framework; test config framework
| _ -> clean (); restore (); build config framework
