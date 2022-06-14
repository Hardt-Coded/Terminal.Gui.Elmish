module Counter

open Terminal.Gui
open Terminal.Gui.Elmish
open System

type Model = {
    Counter:int
    IsSpinning: bool
}

type Msg =
    | Increment
    | Decrement
    | Reset
    | StartSpin
    | StopSpin
    | Spinned

let init () : Model * Cmd<Msg> =
    let model = {
        Counter = 0
        IsSpinning = false
    }
    model, Cmd.none


module Commands =
    let startSpinning isSpinning =
        fun dispatch ->
            async {
                do! Async.Sleep 20
                if isSpinning then
                    dispatch Increment
                    dispatch Spinned
            }
            |> Async.StartImmediate
        |> Cmd.ofSub

let update (msg:Msg) (model:Model) =
    match msg with
    | Increment ->
        {model with Counter = model.Counter + 1}, Cmd.none
    | Decrement ->
        {model with Counter = model.Counter - 1}, Cmd.none
    | Reset ->
        {model with Counter = 0}, Cmd.none
    | StartSpin ->
        {model with IsSpinning = true}, Commands.startSpinning true
    | StopSpin ->
        {model with IsSpinning = false}, Cmd.none
    | Spinned ->
        model, Commands.startSpinning model.IsSpinning
        


let view (model:Model) (dispatch:Msg->unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.at 1
            prop.textAlignment.centered
            prop.color (Color.BrightYellow, Color.Green)
            prop.text "'F#ncy' Counter!"
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.at 5
            prop.text "Up"
            button.onClick (fun () -> dispatch Increment)
        ] 

        View.label [
            let c = (model.Counter |> float) / 100.0
            let x = (16.0 * Math.Cos(c)) |> int 
            let y = (8.0 * Math.Sin(c)) |> int

            prop.position.x.at (x + 20)
            prop.position.y.at (y + 10)
            prop.textAlignment.centered
            prop.color (Color.Magenta, Color.BrightYellow)
            prop.text $"The Count of 'Fancyness' is {model.Counter}"
        ] 


        View.button [
            prop.position.x.center
            prop.position.y.at 7
            prop.text "Down"
            button.onClick (fun () -> dispatch Decrement)
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.at 9
            prop.text "Start Spinning"
            button.onClick (fun () -> dispatch StartSpin)
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.at 11
            prop.text "Stop Spinning"
            button.onClick (fun () -> dispatch StopSpin)
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.at 13
            prop.text "Reset"
            button.onClick (fun () -> dispatch Reset)
        ] 
    ]

