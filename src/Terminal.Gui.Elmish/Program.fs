namespace Terminal.Gui.Elmish



[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Program =
    open Elmish 
    open Terminal.Gui
    open Helpers
    
    let mkConsoleProgram 
        (init : 'arg -> 'model * Cmd<'msg>) 
        (update : 'msg -> 'model -> 'model * Cmd<'msg>)
        (view : 'model -> Dispatch<'msg> -> 'view) =
        Application.Init()
        Elmish.Program.mkProgram init update view          
        |> Program.withSetState (fun model dispatch ->
                
            let dispatch' = (fun msg ->
                    
                Application.RequestStop()
                dispatch msg
            )
    
            let focusedElements = Application.Current |> Helpers.getFocusedElements
            let state = view model dispatch' |> ignore
                
            let elementsAfterView =
                getAllElements Application.Current
    
            // set last focus
            focusedElements 
            |> Seq.iter (
                fun e -> 
                    let idFromLast = e.GetId()
                    let fittingElement =
                        elementsAfterView |> List.tryFind (fun i -> i.GetId() = idFromLast)
                    match fittingElement with
                    | None -> ()
                    | Some element ->
                        if element.SuperView <> null then
                            element.SuperView.SetFocus(element)
                        
            )
                
            Application.Run(Application.Current)
                
        )

