// Learn more about F# at http://fsharp.org

open System
open Terminal.Gui.Elmish
open App


[<EntryPoint>]
let main argv =
    

    Program.mkProgram init update view 
    //|> Program.withSubscription (fun _ -> Cmd.ofSub App.timerSubscription)
    |> Program.run


    0 // return an integer exit code
