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
        let result = messageBox 60 10 "Increment" "How many do you want to Increment?" ["+10";"+5";"+1";"None"]
        match result with
        | "+10" -> 10
        | "+5" -> 5
        | "+1" -> 1
        | _ -> 0
    ) () Incremented

let decrementCmd () =
    Cmd.OfFunc.perform (fun () ->
        let result = errorBox 60 10 "Decrement" "How many do you want to Decrement?" ["-10";"-5";"-1";"None"]
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


let view (model:Model) (dispatch:Msg->unit) : ViewElement list=
    [
        label [
            Styles [
                Pos (CenterPos,AbsPos 1)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Terminal.Gui.Color.BrightYellow,Terminal.Gui.Color.Green)
            ]
            Text "Counter with Message Boxes!"
        ] 

        button [
            Styles [
                Pos (CenterPos,AbsPos 5)                
            ]
            Text "Up"
            OnClicked (fun () -> dispatch Increment)
        ] 

        label [
            Styles [
                Pos (CenterPos,AbsPos 6)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Terminal.Gui.Color.Magenta,Terminal.Gui.Color.BrightYellow)
            ]
            Text (sprintf "The Count of 'Fancyness' is %i" model.Counter)
        ] 


        button [
            Styles [
                Pos (CenterPos,AbsPos 7)                
            ]
            Text "Down"
            OnClicked (fun () -> dispatch Decrement)
        ] 
    ]

