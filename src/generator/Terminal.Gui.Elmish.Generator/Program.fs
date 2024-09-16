// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

open System
open System.Diagnostics;

//Trace.Listeners.Add(new ConsoleTraceListener());
#if DEBUG
    Trace.WriteLine("write line");
#endif

Console.WriteLine("console");

Console.ReadLine();