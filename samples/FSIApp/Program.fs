open Microsoft.FSharp.Compiler.Interactive
open Microsoft.FSharp.Compiler.SourceCodeServices
open System.IO
open System

let fileName = sprintf @"%s\script.fsx" __SOURCE_DIRECTORY__
// Note: printfn gets tied to the FSI writer so using sprintf to format and write to console
let writef format   = Printf.ksprintf System.Console.Write format
let writefn format  = Printf.ksprintf System.Console.WriteLine format

//let writef format     = Printf.ksprintf Debug.Write format
// Compiling to a DLL
(*let dll = "MyTest.dll"
let standardOpts =  [| "--noframework"; "-r:mscorlib.dll"; "-r:FSharp.Core.dll"; "-r:System.dll"; "-r:System.Core.dll"; |]
let srcCodeServices = new Runner.SimpleSourceCodeServices()
let argv = [| yield "fsc.exe"; yield "-o"; yield dll; yield "-a"; yield! standardOpts; yield fileName; |]
let errors, result = srcCodeServices.Compile argv
writefn "Errors - %A; Result - %A" errors result
writefn "Generated dll" *)

/// Compiling to output stream
(*let exe = "MyTest.exe"
let standardOpts =  [| "--noframework"; "-r:mscorlib.dll"; "-r:FSharp.Core.dll"; "-r:System.dll"; "-r:System.Core.dll"; |]
let srcCodeServices = new Runner.SimpleSourceCodeServices()
let argv = [| yield "fsc.exe"; yield "-o"; yield exe; yield! standardOpts; yield fileName; |]
let exec = true
let stdin, stdout = new Samples.ConsoleApp.CompilerInputStream(), new Samples.ConsoleApp.CompilerOutputStream()
let stdins, stdouts = (new StreamReader(stdin)), (new StreamWriter(stdout))
let streams =
    if exec then
        Some ((stdins :> TextReader), (stdouts :> TextWriter), (stdouts :> TextWriter))
    else None
let errors, result, assemblyOpt = srcCodeServices.CompileToDynamicAssembly (argv, streams)
stdouts.Flush()
writefn "Errors - %A" errors
let outs = stdout.Read()
writefn "Generated output..."
writefn "%A" outs*)

(*[<EntryPoint>]
[<STAThread()>]
let MainMain _ =
    //let argv = System.Environment.GetCommandLineArgs()
    let standardOpts =  [| "--noframework"; "-r:mscorlib.dll"; "-r:FSharp.Core.dll"; "-r:System.dll"; "-r:System.Core.dll"; "-r:System.Numerics.dll" |]
    let argv = [| yield "fsi.exe"; yield! standardOpts; |]
    //let stdin, stdout = new Samples.ConsoleApp.CompilerInputStream(), new Samples.ConsoleApp.CompilerOutputStream()
    let stdins, stdouts = System.Console.In, System.Console.Out
    //let stdins, stdouts = (new StreamReader(stdin)), (new StreamWriter(stdout))
    //let streams = Some ((stdins :> TextReader), (stdouts :> TextWriter), (stdouts :> TextWriter))
    //let fsi = new Runner.InteractiveConsole(argv,(stdins :> TextReader),(stdouts :> TextWriter), (stdouts :> TextWriter))
    let fsi = new Runner.InteractiveConsole(argv,(stdins),(stdouts), (stdouts))
    fsi.Run()

    0*)

let file1 = __SOURCE_DIRECTORY__ + @"\test1.fs"
let file11 = __SOURCE_DIRECTORY__ + @"\test1_1.fs"
let file2 = __SOURCE_DIRECTORY__ + @"\test2.fs"

let getSource filename =
    System.IO.File.ReadAllLines(filename)
    |> String.concat "\n"

let checker = InteractiveChecker.Create(ignore)

let TypeCheckSource options (filename,source) version =
    checker.StartBackgroundCompile options;
    // wait for the antecedent to appear
    checker.WaitForBackgroundCompile();
    let info = checker.UntypedParse(filename, source, options)        
    // do an typecheck
    let typedInfo = checker.TypeCheckSource(info, filename, version, source, options, IsResultObsolete (fun _ -> false))
    typedInfo

///
[<EntryPoint>]
[<STAThread()>]
let MainMain _ =
    let standardOpts =  [| "--noframework"; "-r:mscorlib.dll"; "-r:FSharp.Core.dll"; "-r:System.dll"; "-r:System.Core.dll"; "-r:System.Numerics.dll" |]
    //let checker = InteractiveChecker.Create(ignore)
    let getOptions files =
        {   ProjectFileName="console.fsproj"; 
            ProjectFileNames=files; 
            ProjectOptions=standardOpts; 
            IsIncompleteTypeCheckEnvironment=false; 
            UseScriptResolutionRules=false }
    //checker.StartBackgroundCompile options;
    // wait for the antecedent to appear
    //checker.WaitForBackgroundCompile();*)

    (*let typeCheckSource filename version source =
        let info = checker.UntypedParse(filename, source, options)        
        // do an typecheck
        let typedInfo = checker.TypeCheckSource(info, filename, version, source, options, IsResultObsolete (fun _ -> false))
        typedInfo

    let typeCheck filename version = typeCheckSource filename version (getSource filename)*)

    let printCheck k =
        match k with
        | TypeCheckAnswer.TypeCheckSucceeded(r) ->
            //let t = r.TypeCheckInfo |> Option.get
            let errorText = 
                [ for x in r.Errors do  
                        yield sprintf "(%d.%d-%d.%d): %s" (x.StartLine+1) x.StartColumn (x.EndLine+1) x.EndColumn x.Message ]
                |> List.fold(fun s t -> s + "\r\n" + t) ""
            printfn "%A" errorText            
        | _ -> printfn "%A" k

    let getTuple k = k |> Array.map(fun k -> k,getSource(k))

    let opt1 = getOptions [|file1;file2|]
    TypeCheckSource opt1 (file1,getSource(file1)) 0
    |> printCheck
    TypeCheckSource opt1 (file2,getSource(file2)) 0
    |> printCheck

    let file11Src = getSource file11    
    File.WriteAllText(file1,file11Src)
    TypeCheckSource opt1 (file1,getSource(file1)) 1
    |> printCheck
    TypeCheckSource opt1 (file2,getSource(file2)) 0
    |> printCheck

    (*let doAllCheck files =
        files 
        |> Array.map(fun k -> typeCheck k 0)
        |> Array.iter(printCheck)*)

    /// loading the initial state into the checker
    //doAllCheck([| file1; file2 |])

    /// changing some values in file1 by using another file with the same contents (for testing)    
    //let puntyped,ptyped,pversion = checker.TryGetRecentTypeCheckResultsForFile(file1,options) |> Option.get

    (*let file11Src = getSource file11    
    //File.WriteAllText(file1,file11Src)
    typeCheckSource file1 1 file11Src
    |> printCheck

    /// validating if the changes in file1 is properly type checked in file2
    typeCheck file2 0
    |> printCheck*)

    Console.ReadKey(true) |> ignore
    //checker.StopBackgroundCompile()
    0

/// Intellisense
(*let standardOpts =  [| "--noframework"; "-r:FSharp.Core.dll"; "-r:System.dll"; "-r:System.Core.dll"; |]
let srcCodeServices = new Runner.SimpleSourceCodeServices()
let info = srcCodeServices.TypeCheckSource(File.ReadAllText(fileName),standardOpts)
let errorTxt =
    [ for x in info.Errors do
        yield sprintf "(%d.%d-%d.%d): %s" x.StartLine x.StartColumn x.EndLine x.EndColumn x.Message ]
writefn "%A" errorTxt

let code = File.ReadAllLines(fileName)

let tokenizeAndPrint lineNo md token =
    let col = match token with | Some(token) -> token.LeftColumn + 1 | None -> 0
    let dataTip = info.GetDataTipText(lineNo,col,[md])
    let decls   = info.GetDeclarations(lineNo,col,[md],"")
    let f1str   = info.GetF1Keyword(lineNo,col,[md])

    [   yield sprintf "-------------QUICK INFO--------------"
        yield dataTip
        yield ""
        yield sprintf "-------------DOT-COMPLETION--------------"
        yield sprintf "Decls if you pressed '.' after this token: "
        yield String.concat ","
            [ for decl in decls -> decl.Name ]
        yield ""
        match f1str with
        | Some(f1str) ->
            yield sprintf "-------------F1 HELP--------------"
            yield sprintf "The F1 Help Moniker for MSDN is '%s'" f1str
            yield ""
        | None -> yield ""
    ]
    |> List.iter (writefn "%s")

let token,_ = srcCodeServices.TokenizeLine(code.[0],0L)
let sqrToken = token.[2]
tokenizeAndPrint 0 "sqr" (Some(sqrToken))
tokenizeAndPrint 6 "Math" None*)

//System.Console.ReadKey(true) |> ignore