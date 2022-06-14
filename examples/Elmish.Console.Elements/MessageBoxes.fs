module MessageBoxes



open Terminal.Gui
open Terminal.Gui.Elmish

type Model = {
    Counter:int
}

type Msg =
    | Increment
    | Decrement
    | Incremented of int
    | Decremented of int
    | Reset

let init () : Model * Cmd<Msg> =
    let model = {
        Counter = 0
    }
    model, Cmd.none

let incrementCmd () =
    Cmd.OfFunc.perform (fun () ->
        let result = Dialogs.messageBox 60 10 "Increment" "How many do you want to Increment?" ["+10";"+5";"+1";"None"]
        match result with
        | "+10" -> 10
        | "+5" -> 5
        | "+1" -> 1
        | _ -> 0
    ) () Incremented

let decrementCmd () =
    Cmd.OfFunc.perform (fun () ->
        let result = Dialogs.errorBox 60 10 "Decrement" "How many do you want to Decrement?" ["-10";"-5";"-1";"None"]
        match result with
        | "-10" -> 10
        | "-5" -> 5
        | "-1" -> 1
        | _ -> 0
    ) () Decremented


let update (msg:Msg) (model:Model) =
    match msg with
    | Increment ->
        model, incrementCmd()
    | Decrement ->
        model, decrementCmd()
    | Incremented i ->
        {model with Counter = model.Counter + i}, Cmd.none
    | Decremented i ->
        {model with Counter = model.Counter - i}, Cmd.none
    | Reset ->
        {model with Counter = 0}, Cmd.none


let view (model:Model) (dispatch:Msg->unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.at 1
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.BrightYellow, Color.Green)
            label.text "Counter with Message Boxes!"
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.at 5
            button.text "Up"
            button.onClick (fun () -> dispatch Increment)
        ] 

        View.label [
            prop.position.x.center
            prop.position.y.at 6
            prop.textAlignment.centered
            prop.color (Color.Magenta, Color.BrightYellow)
            label.text $"The Count of 'Fancyness' is {model.Counter}"
        ] 


        View.button [
            prop.position.x.center
            prop.position.y.at 7
            button.text "Down"
            button.onClick (fun () -> dispatch Decrement)
        ] 
    ]

