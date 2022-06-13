module Counter



open Terminal.Gui
open Terminal.Gui.Elmish

type Model = {
    Counter:int
}

type Msg =
    | Increment
    | Decrement
    | Reset

let init () : Model * Cmd<Msg> =
    let model = {
        Counter = 0
    }
    model, Cmd.none


let update (msg:Msg) (model:Model) =
    match msg with
    | Increment ->
        {model with Counter = model.Counter + 1}, Cmd.none
    | Decrement ->
        {model with Counter = model.Counter - 1}, Cmd.none
    | Reset ->
        {model with Counter = 0}, Cmd.none


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
            prop.position.x.center
            prop.position.y.at 6
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
    ]

