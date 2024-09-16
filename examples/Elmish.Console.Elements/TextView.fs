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
                let file = Dialogs.openFileDialog "Open TextFile"             
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
            prop.position.y.absolute 1
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.BrightYellow, Color.Green)
            label.text "TextView with File Dialog..."
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.absolute 3
            button.text "Open Textfile"
            prop.accept (fun ev -> dispatch OpenFileDialog)
        ]

        View.frameView [
            prop.position.x.absolute 1
            prop.position.y.absolute 5
            prop.width.fill 1
            prop.height.fill 1
            prop.title "TextView"
            prop.children [
                View.textView [
                    prop.position.x.absolute 0
                    prop.position.y.absolute 0
                    prop.width.fill 0
                    prop.height.fill 0
                    prop.color (Terminal.Gui.Color.BrightMagenta,Terminal.Gui.Color.Blue)
                    textView.text model.Text
                    textView.textChanged (fun e -> dispatch (ChangeText e))
                ]
                
            ]
        ]
        
    ]
