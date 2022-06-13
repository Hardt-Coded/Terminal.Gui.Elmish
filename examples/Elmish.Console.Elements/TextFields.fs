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
        
   


let view (model:Model) (dispatch:Msg -> unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.at 1
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.BrightYellow, Color.Green)
            prop.text "Some Text Fields..."
        ] 

        View.label [
            prop.position.x.at 1
            prop.position.y.at 5
            prop.width.sized 14
            prop.text "Text:"
        ]

        View.textField [
            prop.position.x.at 14
            prop.position.y.at 5
            prop.width.filled
            textField.text model.Text
            textField.onTextChanging (fun t -> dispatch (ChangeText t))
        ]

        View.label [
            prop.position.x.at 1
            prop.position.y.at 7
            prop.width.sized 14
            prop.text "Secret Text:"
        ]

        View.textField [
            prop.position.x.at 14
            prop.position.y.at 7
            prop.width.filled
            textField.text  model.SecretText
            textField.onTextChanging (fun text -> dispatch (ChangeSecretText text))
            textField.secret
        ]

        View.label [
            prop.position.x.at 1
            prop.position.y.at 9
            prop.text $"The Text says: {model.Text}"
        ]

        View.label [
            prop.position.x.at 1
            prop.position.y.at 11
            prop.text $"The Secret Text says: {model.SecretText}"
        ]


        View.label [
            prop.position.x.at 1
            prop.position.y.at 13
            prop.width.sized 14
            prop.text "Time Field:"
        ]

        View.timeField [
            prop.position.x.at 16
            prop.position.y.at 13
            timeField.time  model.CurrentTime
            timeField.onTimeChanged (fun ev -> dispatch <| ChangeTime ev.NewValue)
        ]

        View.timeField [
            prop.position.x.at 30
            prop.position.y.at 13
            timeField.isShortFormat true
            timeField.time model.CurrentTime
            timeField.onTimeChanged (fun ev -> dispatch <| ChangeTime ev.NewValue)
        ]

        View.label [
            prop.position.x.at 1
            prop.position.y.at 15
            prop.width.sized 14
            prop.text "Date Field:"
        ]

        View.dateField [
            prop.position.x.at 16
            prop.position.y.at 15
            dateField.date model.CurrentDate
            dateField.onDateChanged (fun ev -> dispatch <| ChangeDate ev.NewValue)
        ]

        View.dateField [
            prop.position.x.at 30
            prop.position.y.at 15
            dateField.isShortFormat true
            dateField.date model.CurrentDate
            dateField.onDateChanged (fun ev -> dispatch <| ChangeDate ev.NewValue)
        ]
 

        View.label [
            prop.position.x.at 1
            prop.position.y.at 17
            prop.text $"The DateTime (time and date Field) says: {model.CurrentDate.Add(model.CurrentTime):``ddd, yyyy-MM-dd HH:mm:ss``}"
        ]

    ]
