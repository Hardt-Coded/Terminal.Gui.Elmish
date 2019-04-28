namespace Terminal.Gui.Elmish

module meh =
    let a = 1


//// I had it copied from https://github.com/elmish/elmish/
//// because I need to modify the runWith function
//module RingBuffer =

//    open System
    
//    [<Struct>]
//    type internal RingState<'item> =
//        | Writable of wx:'item array * ix:int
//        | ReadWritable of rw:'item array * wix:int * rix:int
    
//    type internal RingBuffer<'item>(size) =
//        let doubleSize ix (items: 'item array) =
//            seq { yield! items |> Seq.skip ix
//                  yield! items |> Seq.take ix
//                  for _ in 0..items.Length do
//                    yield Unchecked.defaultof<'item> }
//            |> Array.ofSeq
    
//        let mutable state : 'item RingState =
//            Writable (Array.zeroCreate (max size 10),0)
    
//        member __.Pop() =
//            match state with
//            | ReadWritable (items,wix,rix) ->
//                let rix' = (rix + 1) % items.Length
//                match rix' = wix with
//                | true -> 
//                    state <- Writable(items,wix)
//                | _ ->
//                    state <- ReadWritable(items,wix,rix')
//                Some items.[rix]
//            | _ ->
//                None
    
//        member __.Push (item:'item) =
//            match state with
//            | Writable (items,ix) ->
//                items.[ix] <- item
//                let wix = (ix + 1) % items.Length
//                state <- ReadWritable(items,wix,ix)
//            | ReadWritable (items,wix,rix) ->
//                items.[wix] <- item
//                let wix' = (wix + 1) % items.Length
//                match wix' = rix with
//                | true -> 
//                    let items = items |> doubleSize rix                                
//                    state <- ReadWritable(items,wix',0)
//                | _ -> 
//                    state <- ReadWritable(items,wix',rix)


//[<RequireQualifiedAccess>]
//[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
//module Program =
//    open Elmish 
//    open Terminal.Gui
//    open Helpers
//    open RingBuffer

//    type Program<'arg, 'model, 'msg, 'view> = private {
//        init : 'arg -> 'model * Cmd<'msg>
//        update : 'msg -> 'model -> 'model * Cmd<'msg>
//        subscribe : 'model -> Cmd<'msg>
//        view : 'model -> Dispatch<'msg> -> 'view
//        setState : 'model -> Dispatch<'msg> -> unit
//        onError : (string*exn) -> unit
//        syncDispatch: Dispatch<'msg> -> Dispatch<'msg>
//    }
    
//    let mkConsoleProgram 
//        (init : 'arg -> 'model * Cmd<'msg>) 
//        (update : 'msg -> 'model -> 'model * Cmd<'msg>)
//        (view : 'model -> Dispatch<'msg> -> 'view) =
//        Application.Init()
//        Elmish.Program.mkProgram init update view          
//        |> Program.withSetState (fun model dispatch ->
                
//            let dispatch' = (fun msg ->
                    
//                Application.RequestStop()
//                dispatch msg
//            )
    
//            let focusedElements = Application.Current |> Helpers.getFocusedElements
//            let state = view model dispatch' |> ignore
                
//            let elementsAfterView =
//                getAllElements Application.Current
    
//            // set last focus
//            focusedElements 
//            |> Seq.iter (
//                fun e -> 
//                    let idFromLast = e.GetId()
//                    let fittingElement =
//                        elementsAfterView |> List.tryFind (fun i -> i.GetId() = idFromLast)
//                    match fittingElement with
//                    | None -> ()
//                    | Some element ->
//                        if element.SuperView <> null then
//                            element.SuperView.SetFocus(element)
                        
//            )
                
//            Application.Run(Application.Current)
//        )


//    /// Start the program loop.
//    /// arg: argument to pass to the init() function.
//    /// program: program created with 'mkSimple' or 'mkProgram'.
//    let runWith (arg: 'arg) (program: Program<'arg, 'model, 'msg, 'view>) =
//        let (model,cmd) = program.init arg
//        let rb = RingBuffer 10
//        let mutable reentered = false
//        let mutable state = model        
//        let rec dispatch msg = 
//            if reentered then
//                rb.Push msg
//            else
//                reentered <- true
//                let mutable nextMsg = Some msg
//                while Option.isSome nextMsg do
//                    let msg = nextMsg.Value
//                    try
//                        let (model',cmd') = program.update msg state
//                        program.setState model' syncDispatch
//                        cmd' |> Cmd.exec syncDispatch
//                        state <- model'
//                    with ex ->
//                        program.onError (sprintf "Unable to process the message: %A" msg, ex)
//                    nextMsg <- rb.Pop()
//                reentered <- false
//        and syncDispatch = program.syncDispatch dispatch            

//        program.setState model syncDispatch
//        let sub = 
//            try 
//                program.subscribe model 
//            with ex ->
//                program.onError ("Unable to subscribe:", ex)
//                Cmd.none
//        sub @ cmd |> Cmd.exec syncDispatch

