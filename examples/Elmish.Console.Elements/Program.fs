// Learn more about F# at http://fsharp.org

open System
open Elmish
open Terminal.Gui.Elmish
open App


[<EntryPoint>]
let main argv =
    

    Program.mkConsoleProgram init update view  
    |> Program.run


    0 // return an integer exit code
