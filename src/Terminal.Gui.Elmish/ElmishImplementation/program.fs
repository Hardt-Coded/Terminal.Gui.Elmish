(**

Note: I had to copy the elmish implementation from https://github.com/elmish/elmish because of some modification, that are needed.

Program
---------
Core abstractions for creating and running the dispatch loop.

*)

namespace Terminal.Gui.Elmish

open Terminal.Gui
open Terminal.Gui.Elmish.Elements


open Terminal.Gui
open System
open System.Reflection
open System.Diagnostics



/// Program type captures various aspects of program behavior
type Program<'arg, 'model, 'msg, 'view> = private {
    init : 'arg -> 'model * Cmd<'msg>
    update : 'msg -> 'model -> 'model * Cmd<'msg>
    subscribe : 'model -> Cmd<'msg>
    view : 'model -> Dispatch<'msg> -> TerminalElement
    setState : 'model -> Dispatch<'msg> -> unit
    onError : (string*exn) -> unit
    syncDispatch: Dispatch<'msg> -> Dispatch<'msg>
}

/// Program module - functions to manipulate program instances
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Program =
    /// Typical program, new commands are produced by `init` and `update` along with the new state.
    let mkProgram 
        (init : 'arg -> 'model * Cmd<'msg>) 
        (update : 'msg -> 'model -> 'model * Cmd<'msg>)
        (view : 'model -> Dispatch<'msg> -> TerminalElement) =
        { init = init
          update = update
          view = view
          setState = fun model -> view model >> ignore
          subscribe = fun _ -> Cmd.none
          onError = Log.onError
          syncDispatch = id }

    /// Simple program that produces only new state with `init` and `update`.
    let mkSimple 
        (init : 'arg -> 'model) 
        (update : 'msg -> 'model -> 'model)
        (view : 'model -> Dispatch<'msg> -> TerminalElement) =
        { init = init >> fun state -> state,Cmd.none
          update = fun msg -> update msg >> fun state -> state,Cmd.none
          view = view
          setState = fun model -> view model >> ignore
          subscribe = fun _ -> Cmd.none
          onError = Log.onError
          syncDispatch = id }

    /// Subscribe to external source of events.
    /// The subscription is called once - with the initial model, but can dispatch new messages at any time.
    let withSubscription (subscribe : 'model -> Cmd<'msg>) (program: Program<'arg, 'model, 'msg, 'view>) =
        let sub model =
            Cmd.batch [ program.subscribe model
                        subscribe model ]
        { program with subscribe = sub }

    /// Trace all the updates to the console
    let withConsoleTrace (program: Program<'arg, 'model, 'msg, 'view>) =
        let traceInit (arg:'arg) =
            let initModel,cmd = program.init arg
            Log.toConsole ("Initial state:", initModel)
            initModel,cmd

        let traceUpdate msg model =
            Log.toConsole ("New message:", msg)
            let newModel,cmd = program.update msg model
            Log.toConsole ("Updated state:", newModel)
            newModel,cmd

        { program with
            init = traceInit 
            update = traceUpdate }

    /// Trace all the messages as they update the model
    let withTrace trace (program: Program<'arg, 'model, 'msg, 'view>) =
        { program
            with update = fun msg model -> trace msg model; program.update msg model}

    /// Handle dispatch loop exceptions
    let withErrorHandler onError (program: Program<'arg, 'model, 'msg, 'view>) =
        { program
            with onError = onError }

    /// For library authors only: map existing error handler and return new `Program` 
    let mapErrorHandler map (program: Program<'arg, 'model, 'msg, 'view>) =
        { program
            with onError = map program.onError }

    /// For library authors only: function to render the view with the latest state 
    let withSetState (setState:'model -> Dispatch<'msg> -> unit)
                     (program: Program<'arg, 'model, 'msg, 'view>) =        
        { program
            with setState = setState }

    /// For library authors only: return the function to render the state 
    let setState (program: Program<'arg, 'model, 'msg, 'view>) =        
        program.setState

    /// For library authors only: return the view function 
    let view (program: Program<'arg, 'model, 'msg, 'view>) =        
        program.view

    /// For library authors only: function to synchronize the dispatch function
    let withSyncDispatch (syncDispatch:Dispatch<'msg> -> Dispatch<'msg>)
                         (program: Program<'arg, 'model, 'msg, 'view>) =        
        { program
            with syncDispatch = syncDispatch }

    /// For library authors only: map the program type
    let map mapInit mapUpdate mapView mapSetState mapSubscribe
            (program: Program<'arg, 'model, 'msg, 'view>) =
        { init = mapInit program.init
          update = mapUpdate program.update
          view = mapView program.view
          setState = mapSetState program.setState
          subscribe = mapSubscribe program.subscribe
          onError = program.onError
          syncDispatch = id }

    /// Start the program loop.
    /// arg: argument to pass to the init() function.
    /// program: program created with 'mkSimple' or 'mkProgram'.
    let runWith (arg: 'arg) (program: Program<'arg, 'model, 'msg, 'view>) =
        Application.Init()
        let (model,cmd) = program.init arg
        let rb = RingBuffer 10
        let mutable reentered = false
        let mutable state = model     
        let mutable currentTreeState:TerminalElement option = None
        let rec dispatch msg = 
            if reentered then
                rb.Push msg
            else
                
                // stop Console Application Event Loop
                //Application.RequestStop()

                reentered <- true
                let mutable nextMsg = Some msg
                while Option.isSome nextMsg do
                    let msg = nextMsg.Value
                    try
                        let (model',cmd') = program.update msg state

                        match currentTreeState with
                        | None ->
                            ()
                        | Some currentState ->
                            let nextTreeState = program.view model' syncDispatch
                            Differ.update currentState nextTreeState
                            currentTreeState <- Some nextTreeState

                        Application.Invoke(fun () -> ())
                        
                        //Application.MainLoop.Invoke(fun () ->
                        //    match currentTreeState with
                        //    | None ->
                        //        ()
                        //    | Some currentState ->
                        //        //let nextTreeState = program.view model' syncDispatch
                        //        //Differ.update currentState nextTreeState
                        //        //currentTreeState <- Some nextTreeState
                        //        ()
                        //        //Application.Top.RemoveAll()
                        //        //Application.Top.Add(newState.Subviews |> Seq.toArray)
                        //        //Application.Top.LayoutSubviews()
                        //        //Application.Top |> setViewElementState toSynchViewStates
                        //        //Application.Driver.Refresh()
                        //)
                                                
                        cmd' |> Cmd.exec syncDispatch
                        state <- model'
                        
                    with ex ->
                        program.onError (sprintf "Unable to process the message: %A" msg, ex)
                        reraise() 
                    nextMsg <- rb.Pop()
                reentered <- false


        and syncDispatch = program.syncDispatch dispatch            

        program.setState model syncDispatch

        let startState = program.view model syncDispatch   
        
        Differ.initializeTree None startState
        currentTreeState <- Some startState

        match startState.element with
        | null ->
            failwith ("error state not initialized")
        | topElement ->
            match topElement with
            | :? Toplevel as te ->
                let sub = 
                    try 
                        program.subscribe model 
                    with ex ->
                        program.onError ("Unable to subscribe:", ex)
                        Cmd.none
                sub @ cmd |> Cmd.exec syncDispatch
        
                // some reflection to set the actual top
                // let topProp = typeof<Application>.GetProperty("Top")
                //let currentProp = typeof<Application>.GetProperty("Current")
                //currentProp.SetValue(null,te)
                // topProp.SetValue(null,te)
                //Application.Begin(te) |> ignore
                
                Application.Run(te) |> ignore
                te.Dispose()
                Application.Shutdown()
            | _ ->
                failwith("first element must be a toplevel!")

    /// Start the dispatch loop with `unit` for the init() function.
    let run (program: Program<unit, 'model, 'msg, 'view>) = runWith () program


    let quitWithErrorCode errorcode =        
        System.Console.Clear()
        System.Environment.Exit(errorcode)

    let quit() =
        Application.Shutdown()
        quitWithErrorCode 0
        

    
