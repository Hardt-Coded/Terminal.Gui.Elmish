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
                let file = openDialog "Open TextFile" "Select Textfile to Open"            
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


let view (model:Model) (dispatch:Msg -> unit) : View list=
    [
        yield label [
            Styles [
                Pos (CenterPos,AbsPos 1)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Terminal.Gui.Color.BrightYellow,Terminal.Gui.Color.Green)
            ]
            Text "TextView with File Dialog..."
        ] 

        yield button [
            Styles [
                Pos (CenterPos, AbsPos 3)
            ]
            Text "Open Textfile"
            OnClicked (fun () -> dispatch OpenFileDialog)
        ]

        yield frameView [
            Styles [
                Pos (AbsPos 1,AbsPos 4)
                Dim (FillMargin 1,FillMargin 1)                                
            ]
            Text "TextView"
        ] [
            
            textView [
                Styles [
                    Pos (AbsPos 0,AbsPos 0)
                    Dim (Fill,Fill)                    
                    Colors (Terminal.Gui.Color.BrightMagenta,Terminal.Gui.Color.Blue)
                ]
                Text model.Text
            ]
            
        ]
        

        
    ]
