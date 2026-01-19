#r "nuget: ScottPlot, *-*"
#r "nuget: FSharp.Data"

open System
open System.IO
open FSharp.Data
open ScottPlot

let parseTime (timeStr: string) =
    let cleaned = timeStr.Replace(" ns", "").Replace(",", "").Replace("\"", "").Trim()
    float cleaned

let parseMemory (memStr: string) =
    let cleaned = memStr.Replace(" B", "").Replace(",", "").Replace("\"", "").Trim()
    float cleaned

let loadData (csvPath: string) =
    let csv = CsvFile.Load(csvPath)
    let headers = csv.Headers |> Option.defaultValue [||]
    let methodColIdx = Array.findIndex (fun (s: string) -> s.Contains("Method")) headers
    let meanColIdx = Array.findIndex (fun (s: string) -> s.Contains("Mean")) headers
    let allocColIdx = Array.findIndex (fun (s: string) -> s.Contains("Allocated")) headers
    let nColIdx =
        match Array.tryFindIndex (fun (s: string) -> s = "N") headers with
        | Some idx -> idx
        | None -> -1

    [
        for row in csv.Rows do
            let columns = row.Columns |> Seq.toArray
            let methodName = columns.[methodColIdx].Replace("\"", "").Trim()
            let mean = parseTime columns.[meanColIdx]
            let allocated = parseMemory columns.[allocColIdx]
            let nValue =
                if nColIdx >= 0 && nColIdx < columns.Length then
                    int columns.[nColIdx]
                else 0
            (methodName, nValue, mean, allocated)
    ]

let createClassChart (className: string) (data: (string * int * float * float) list) =
    let plt = new Plot()
    let maxN = data |> List.map (fun (_, n, _, _) -> n) |> List.max

    let filteredData =
        data
        |> List.filter (fun (_, n, _, _) -> n = maxN)
        |> List.sortBy (fun (method, _, _, _) -> method)

    let labels = filteredData |> List.map (fun (method, n, _, _) -> method) |> List.toArray
    let timeValues = filteredData |> List.map (fun (_, _, time, _) -> time) |> List.toArray
    let memoryValues = filteredData |> List.map (fun (_, _, _, memory) -> memory) |> List.toArray

    let timeColor = Color.FromHex "#4286F5"
    let memoryColor = Color.FromHex "#DC4437"
    let fontColor = Color.FromHex "#777777"

    let groupSpacing =
        match labels.Length with
        | n when n <= 2 -> 2.0
        | n when n <= 3 -> 2.8
        | n when n <= 4 -> 3.5
        | _ -> 4.2
    let positions = [|0.0 .. float (labels.Length - 1)|] |> Array.map (fun i -> i * groupSpacing)

    let maxTime = Array.max timeValues
    let maxMemory = Array.max memoryValues
    let maxTimeMs = maxTime / 1000000.0
    let maxMemoryMB = maxMemory / (1024.0 * 1024.0)
    let rawMaxAxisValue = max maxTimeMs maxMemoryMB

    let getTickInterval (value: float) =
        match value with
        | v when v <= 1.0 -> 0.25
        | v when v <= 5.0 -> 1.0
        | v when v <= 20.0 -> 5.0
        | v when v <= 100.0 -> 20.0
        | v when v <= 500.0 -> 100.0
        | v when v <= 1000.0 -> 250.0
        | v when v <= 5000.0 -> 1000.0
        | v when v <= 20000.0 -> 5000.0
        | _ -> 10000.0

    let tickInterval = getTickInterval rawMaxAxisValue
    let maxAxisValue = ceil(rawMaxAxisValue / tickInterval) * tickInterval
    let yAxisMaxNs = maxAxisValue * 1000000.0
    let yAxisMaxBytes = maxAxisValue * (1024.0 * 1024.0)
    let timeYOffset = (maxAxisValue * 1.4 * 1000000.0) * 0.02
    let memoryYOffset = (maxAxisValue * 1.4 * (1024.0 * 1024.0)) * 0.02

    plt.Axes.Left.IsVisible <- false
    plt.Axes.Right.IsVisible <- false
    plt.Axes.Bottom.IsVisible <- true
    plt.Axes.Top.IsVisible <- false
    plt.Grid.IsVisible <- false

    let lineStartX = -1.0
    let lineEndX = positions.[positions.Length - 1] + 1.0

    let tickPositions = 
        if maxMemoryMB >= maxTimeMs then
            [|0.25; 0.5; 0.75; 1.0|] |> Array.map (fun ratio -> maxAxisValue * ratio * (1024.0 * 1024.0))
        else
            [|0.25; 0.5; 0.75; 1.0|] |> Array.map (fun ratio -> maxAxisValue * ratio * 1000000.0)

    let formatTickLabel (value: float) =
        if value = 0.0 then "0"
        elif value < 0.1 then sprintf "%.3f" value
        elif value < 1.0 then sprintf "%.2f" value
        elif value < 10.0 then sprintf "%.1f" value
        else sprintf "%.0f" value

    let tickLabels = [|0.25; 0.5; 0.75; 1.0|] |> Array.map (fun ratio -> 
        let value = maxAxisValue * ratio
        formatTickLabel value)

    for i in 0 .. tickPositions.Length - 1 do
        let line = plt.Add.Line(lineStartX, tickPositions.[i], lineEndX, tickPositions.[i])
        line.Color <- fontColor
        line.LineWidth <- 1f

    for i in 0 .. tickLabels.Length - 1 do
        let label = plt.Add.Text(tickLabels.[i], lineStartX - 0.25, tickPositions.[i])
        label.LabelFontSize <- 24f
        label.LabelFontColor <- fontColor
        label.Alignment <- Alignment.MiddleRight

    let timeBarsPositions = Array.map (fun pos -> pos - 0.4) positions
    let timeBars = plt.Add.Bars(timeBarsPositions, timeValues)
    for bar in timeBars.Bars do
        bar.FillColor <- timeColor
        bar.LineColor <- timeColor
        bar.LineWidth <- 0f

    let memoryBarsPositions = Array.map (fun pos -> pos + 0.4) positions
    let memoryBars = plt.Add.Bars(memoryBarsPositions, memoryValues)
    for bar in memoryBars.Bars do
        bar.FillColor <- memoryColor
        bar.LineColor <- memoryColor
        bar.LineWidth <- 0f

    for i, pos in Array.indexed positions do
        let timeMs = timeValues.[i] / 1000000.0
        let txt = plt.Add.Text($"{timeMs:F2}ms", pos - 0.4, timeValues.[i] + timeYOffset)
        txt.LabelFontSize <- 32f
        txt.LabelBold <- true
        txt.LabelFontColor <- timeColor
        txt.Alignment <- Alignment.MiddleCenter

    for i, pos in Array.indexed positions do
        let memMB = memoryValues.[i] / (1024.0 * 1024.0)
        let txt = plt.Add.Text($"{memMB:F2}MB", pos + 0.4, memoryValues.[i] + memoryYOffset)
        txt.LabelFontSize <- 32f
        txt.LabelBold <- true
        txt.LabelFontColor <- memoryColor
        txt.Alignment <- Alignment.MiddleCenter

    plt.Axes.SetLimitsX(lineStartX - 0.5, lineEndX)
    plt.Axes.Left.Min <- 0.0
    plt.Axes.Left.Max <- yAxisMaxNs * 1.4
    plt.Axes.Right.Min <- 0.0
    plt.Axes.Right.Max <- yAxisMaxBytes * 1.4

    plt.Layout.Fixed(new PixelPadding(200f, 200f, 50f, 120f))

    let legendTimeBar = plt.Add.Bar(0, 0)
    legendTimeBar.Color <- timeColor
    legendTimeBar.LegendText <- "Mean Time (ms)"

    let legendMemoryBar = plt.Add.Bar(0, 0)
    legendMemoryBar.Color <- memoryColor
    legendMemoryBar.LegendText <- "Allocated (MB)"

    plt.Legend.IsVisible <- true
    plt.Legend.Alignment <- Alignment.UpperRight
    plt.Legend.FontSize <- 32f
    plt.Legend.FontColor <- fontColor

    plt.Axes.Bottom.TickLabelStyle.FontSize <- 36f
    plt.Axes.Bottom.TickLabelStyle.ForeColor <- fontColor
    plt.Axes.Left.TickLabelStyle.FontSize <- 36f
    plt.Axes.Left.TickLabelStyle.ForeColor <- fontColor
    plt.Axes.Right.TickLabelStyle.FontSize <- 36f
    plt.Axes.Right.TickLabelStyle.ForeColor <- fontColor

    let titleName = sprintf "%s \nN=%i" className maxN
    let titlePosX = lineStartX - 0.4
    let titleAnnotation = plt.Add.Text(titleName, titlePosX, yAxisMaxNs * 1.25)
    titleAnnotation.LabelFontSize <- 48f
    titleAnnotation.LabelBold <- true
    titleAnnotation.LabelFontColor <- fontColor
    titleAnnotation.Alignment <- Alignment.MiddleLeft

    plt.Title ""
    plt.YLabel ""
    plt.XLabel ""

    plt.Axes.Bottom.TickGenerator <- TickGenerators.NumericManual(positions, labels)
    plt.Axes.Bottom.TickLabelStyle.Rotation <- 0f
    plt.Axes.Bottom.TickLabelStyle.Alignment <- Alignment.UpperCenter

    plt.Axes.Bottom.FrameLineStyle.Width <- 0f
    plt.Axes.Bottom.MajorTickStyle.Length <- 0f
    plt.Axes.Bottom.MinorTickStyle.Length <- 0f

    let y0Line = plt.Add.Line(lineStartX, 0.0, lineEndX, 0.0)
    y0Line.Color <- Colors.Black
    y0Line.LineWidth <- 3f

    plt

let main (relativeOutputDir: string) (pattern: string option) =
    let regexPattern = defaultArg pattern ".*"
    let regex = System.Text.RegularExpressions.Regex(regexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    
    let resultsDir = Path.Combine(__SOURCE_DIRECTORY__, "BenchmarkDotNet.Artifacts", "results")
    let outputDir = Path.Combine(__SOURCE_DIRECTORY__, relativeOutputDir)
    
    Directory.CreateDirectory(outputDir) |> ignore

    if not (Directory.Exists(resultsDir)) then
        printfn "Error: Benchmark results directory not found: %s" resultsDir
        exit 1

    printfn "Loading benchmark data..."
    printfn "Filter pattern: %s" regexPattern

    let csvFiles = Directory.GetFiles(resultsDir, "*-report.csv")
    if csvFiles.Length = 0 then
        printfn "Error: No CSV files found in %s" resultsDir
        exit 1

    let mutable processedCount = 0
    
    for csvFile in csvFiles do
        try
            let fileName = Path.GetFileNameWithoutExtension(csvFile).Replace("-report", "")
            if regex.IsMatch(fileName) then
                printfn "\nProcessing: %s" fileName
                let data = loadData csvFile
                let chart = createClassChart fileName data
                let chartPath = Path.Combine(outputDir, $"{fileName}.png")
                chart.SavePng(chartPath, 1600, 1000) |> ignore
                printfn "  Saved chart: %s" chartPath
                processedCount <- processedCount + 1
            else
                printfn "\nSkipping (doesn't match pattern): %s" fileName
        with ex ->
            printfn "  Error: %s" ex.Message

    printfn "\nProcessed %d of %d files" processedCount csvFiles.Length
    printfn "All charts saved to: %s" outputDir

main "docs/charts" None
