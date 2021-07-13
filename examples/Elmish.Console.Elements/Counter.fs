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


let view (model:Model) (dispatch:Msg->unit) : ViewElement list=
    [
        label [
            Styles [
                Pos (CenterPos,AbsPos 1)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Terminal.Gui.Color.BrightYellow,Terminal.Gui.Color.Green)
            ]
            Text "'Fancy' Counter!"
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

