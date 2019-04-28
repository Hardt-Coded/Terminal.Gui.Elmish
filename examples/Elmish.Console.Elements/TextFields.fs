module TextFields


open Elmish
open Terminal.Gui
open Terminal.Gui.Elmish

type Model = {
    Text:string
    SecretText:string
}

type Msg =
    | ChangeText of string
    | ChangeSecretText of string

let init () : Model * Cmd<Msg> =
    let model = {
        Text = "some Text!"
        SecretText = "Secret"
    }
    model, Cmd.none


let update (msg:Msg) (model:Model) =
    match msg with
    | ChangeText txt ->
        {model with Text = txt}, Cmd.none
    | ChangeSecretText txt ->
        {model with SecretText = txt}, Cmd.none


let view (model:Model) (dispatch:Msg -> unit) : View list=
    [
        label [
            Styles [
                Pos (CenterPos,AbsPos 1)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Terminal.Gui.Color.BrightYellow,Terminal.Gui.Color.Green)
            ]
            Text "Some Text Fields..."
        ] 

        label [
            Styles [
                Pos (AbsPos 1,AbsPos 5)
                Dim (AbsDim 14,AbsDim 1)                
            ]
            Text "Text:"
        ]

        textField [
            Styles [
                Pos (AbsPos 14,AbsPos 5)
                Dim (Fill,AbsDim 1)

            ]
            Value model.Text
            OnChanged (fun t -> dispatch (ChangeText t))
        ]

        label [
            Styles [
                Pos (AbsPos 1,AbsPos 7)
                Dim (AbsDim 14,AbsDim 1)                
            ]
            Text "Secret Text:"
        ]

        textField [
            Styles [
                Pos (AbsPos 14,AbsPos 7)
                Dim (Fill,AbsDim 1)

            ]
            Value model.SecretText
            OnChanged (fun t -> dispatch (ChangeSecretText t))
            Secret
        ]

        label [
            Styles [
                Pos (AbsPos 1,AbsPos 9)
                Dim (AbsDim 1,AbsDim 1)                
                Colors (Color.BrightYellow,Color.Red)
            ]
            Text (sprintf "The Text says: %s" model.Text)
        ]

        label [
            Styles [
                Pos (AbsPos 1,AbsPos 11)
                Dim (AbsDim 1,AbsDim 1)
                Colors (Color.BrightYellow,Color.Red)
            ]
            Text (sprintf "The Secret Text says: %s" model.SecretText)
        ]
    ]
