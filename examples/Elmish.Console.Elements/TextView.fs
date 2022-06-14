module TextView


open Terminal.Gui
open Terminal.Gui.Elmish

type Model = {
    Text:string 
}

type Msg =
    | ChangeText of string
    | OpenFileDialog
    | FileSelected of content:string

let init () : Model * Cmd<Msg> =
    let model = {
        Text = ""           
    }
    model, Cmd.none


let openFileDialogCmd () =
    Cmd.OfAsync.perform (
        fun () ->
            async {
                let file = Dialogs.openFileDialog "Open TextFile" "Select Textfile to Open"            
                match file with
                | None ->
                    return ""
                | Some f ->
                    try
                        let! content = System.IO.File.ReadAllTextAsync(f) |> Async.AwaitTask
                        return content;
                    with
                    | _ as e ->
                        return sprintf "Error: %s" e.Message
            }
    ) () FileSelected
    

let update (msg:Msg) (model:Model) =
    match msg with
    | ChangeText txt ->
        {model with Text = txt}, Cmd.none
    | OpenFileDialog ->
        model, openFileDialogCmd ()
    | FileSelected fileContent ->
        {model with Text = fileContent}, Cmd.none


let view (model:Model) (dispatch:Msg -> unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.at 1
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.BrightYellow, Color.Green)
            prop.text "TextView with File Dialog..."
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.at 3
            button.text "Open Textfile"
            button.onClick (fun () -> dispatch OpenFileDialog)
        ]

        View.frameView [
            prop.position.x.at 1
            prop.position.y.at 5
            prop.width.fill 1
            prop.height.fill 1
            frameView.title "TextView"
            frameView.children [
                View.textView [
                    prop.position.x.at 0
                    prop.position.y.at 0
                    prop.width.filled
                    prop.height.filled
                    prop.color (Terminal.Gui.Color.BrightMagenta,Terminal.Gui.Color.Blue)
                    textView.text model.Text
                    textView.onTextChanged (fun e -> dispatch (ChangeText e))
                ]
                
            ]
        ]
        
    ]
