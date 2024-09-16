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
            prop.position.y.absolute 1
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.BrightYellow, Color.Green)
            label.text "Some Text Fields..."
        ] 

        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 5
            prop.width.fill 14
            label.text "Text:"
        ]

        View.textField [
            prop.position.x.absolute 14
            prop.position.y.absolute 5
            prop.width.fill 0
            textField.text model.Text
            textField.textChanging (fun ev -> dispatch (ChangeText ev.NewValue))
        ]

        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 7
            prop.width.fill 14
            label.text "Secret Text:"
        ]

        View.textField [
            prop.position.x.absolute 14
            prop.position.y.absolute 7
            prop.width.fill 0
            textField.text  model.SecretText
            textField.textChanging (fun ev -> dispatch (ChangeSecretText ev.NewValue))
            textField.secret true
        ]

        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 9
            label.text $"The Text says: {model.Text}"
        ]

        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 11
            label.text $"The Secret Text says: {model.SecretText}"
        ]


        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 13
            prop.width.fill 14
            label.text "Time Field:"
        ]

        View.timeField [
            prop.position.x.absolute 16
            prop.position.y.absolute 13
            timeField.time  model.CurrentTime
            timeField.timeChanged (fun ev -> dispatch <| ChangeTime ev.NewValue)
        ]

        View.timeField [
            prop.position.x.absolute 30
            prop.position.y.absolute 13
            timeField.isShortFormat true
            timeField.time model.CurrentTime
            timeField.timeChanged (fun ev -> dispatch <| ChangeTime ev.NewValue)
        ]

        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 15
            prop.width.fill 14
            label.text "Date Field:"
        ]

        View.dateField [
            prop.position.x.absolute 16
            prop.position.y.absolute 15
            dateField.date model.CurrentDate
            dateField.dateChanged (fun ev -> dispatch <| ChangeDate ev.NewValue)
        ]

        View.dateField [
            prop.position.x.absolute 30
            prop.position.y.absolute 15
            dateField.date model.CurrentDate
            dateField.dateChanged (fun ev -> dispatch <| ChangeDate ev.NewValue)
        ]
 

        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 17
            label.text $"The DateTime (time and date Field) says: {model.CurrentDate.Add(model.CurrentTime):``ddd, yyyy-MM-dd HH:mm:ss``}"
        ]

    ]
