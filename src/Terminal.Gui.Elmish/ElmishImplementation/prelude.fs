namespace Terminal.Gui.Elmish

(**

Note: I had to copy the elmish implementation from https://github.com/elmish/elmish because of some modification, that are needed.

Log
---------
Basic cross-platform logging API.

*)
module internal Log =

#if FABLE_COMPILER
    open Fable.Core.JS

    let onError (text: string, ex: exn) = console.error (text,ex)
    let toConsole(text: string, o: #obj) = console.log(text,o)

#else
#if NETSTANDARD2_0
    let onError (text: string, ex: exn) = System.Diagnostics.Trace.TraceError("{0}: {1}", text, ex)
    let toConsole(text: string, o: #obj) = printfn "%s: %A" text o
#else
    let onError (text: string, ex: exn) = System.Console.Error.WriteLine("{0}: {1}", text, ex)
    let toConsole(text: string, o: #obj) = printfn "%s: %A" text o
#endif
#endif
