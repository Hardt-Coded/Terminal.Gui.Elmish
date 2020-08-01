module TextFields

open System
open Terminal.Gui
open Terminal.Gui.Elmish

type Model = {
    Text:string
    SecretText:string
    CurrentTime:TimeSpan
    CurrentDate:DateTime
}

type Msg =
    | ChangeText of string
    | ChangeSecretText of string
    | ChangeTime of TimeSpan
    | ChangeDate of DateTime
   

let init () : Model * Cmd<Msg> =
    let model = {
        Text = "some Text!"
        SecretText = "Secret"
        CurrentTime = TimeSpan(9,1,35)
        CurrentDate = DateTime.Today
    }
    model, Cmd.none


let update (msg:Msg) (model:Model) =
    match msg with
    | ChangeText txt ->
        {model with Text = txt}, Cmd.none
    | ChangeSecretText txt ->
        {model with SecretText = txt}, Cmd.none
    | ChangeTime time ->
        {model with CurrentTime = time}, Cmd.none
    | ChangeDate date ->
        {model with CurrentDate = date}, Cmd.none
        
   


let view (model:Model) (dispatch:Msg -> unit) : View list=
    [
        label[]
        

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


        label [
            Styles [
                Pos (AbsPos 1,AbsPos 13)
                Dim (AbsDim 14,AbsDim 1)                
            ]
            Text "Time Field:"
        ]

        timeField [
            Styles [
                Pos (AbsPos 16,AbsPos 13)
            ]
            Value model.CurrentTime
            OnChanged (fun time -> dispatch <| ChangeTime time)
        ]

        timeField [
            Styles [
                Pos (AbsPos 30,AbsPos 13)
            ]
            IsShort
            Value model.CurrentTime
            OnChanged (fun time -> dispatch <| ChangeTime time)
        ]

        label [
            Styles [
                Pos (AbsPos 1,AbsPos 15)
                Dim (AbsDim 14,AbsDim 1)                
            ]
            Text "Date Field:"
        ]

        dateField [
            Styles [
                Pos (AbsPos 16,AbsPos 15)
            ]
            Value model.CurrentDate
            OnChanged (fun time -> dispatch <| ChangeDate time)
        ]

        dateField [
            Styles [
                Pos (AbsPos 30,AbsPos 15)
            ]
            IsShort
            Value model.CurrentDate
            OnChanged (fun time -> dispatch <| ChangeDate time)
        ]
        


        

        


        label [
            Styles [
                Pos (AbsPos 1,AbsPos 17)
                Dim (AbsDim 1,AbsDim 1)
                Colors (Color.BrightYellow,Color.Red)
            ]
            Text (sprintf "The DateTime (time and date Field) says: %s" <| model.CurrentDate.Add(model.CurrentTime).ToString("ddd, yyyy-MM-dd HH:mm:ss"))
        ]

    ]
