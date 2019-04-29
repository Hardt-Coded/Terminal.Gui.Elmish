module ListView




open Terminal.Gui
open Terminal.Gui.Elmish


type VoteResult =
    | Awesome
    | Fancy
    | Nice
    | Meh
    | IDontKnow

type Model = {
    VoteResult:VoteResult   
    VoteResultItems: (VoteResult * string) list
}

type Msg =
    | ItemSelected of VoteResult

let init () : Model * Cmd<Msg> =
    let model = {
        VoteResult = Awesome
        VoteResultItems = [
            (Awesome,"This is Awesome")
            (Fancy,"This is fancy")
            (Nice,"This is nice")
            (Meh,"Meh ... Please leave me alone ")
            (IDontKnow,"What?! I don't know")
        ]
    }
    model, Cmd.none


let update (msg:Msg) (model:Model) =
    match msg with
    | ItemSelected vr ->
        {model with VoteResult = vr}, Cmd.none


let view (model:Model) (dispatch:Msg -> unit) : View list=
    [
        label [
            Styles [
                Pos (CenterPos,AbsPos 1)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Terminal.Gui.Color.BrightYellow,Terminal.Gui.Color.Green)
            ]
            Text "List View"
        ] 

        label [
            Styles [
                Pos (CenterPos,AbsPos 5)
                Dim (Fill,AbsDim 1)  
                TextAlignment Centered
            ]
            Text "Please Vote!"
        ]


        frameView [
            Styles [
                Pos (AbsPos 1,AbsPos 4)
                Dim (FillMargin 1,FillMargin 5)                                
            ]
            Text "TextView"
        ] [
            
            listView [
                Styles [
                    Pos (AbsPos 0,AbsPos 0)                
                    Dim (Fill,Fill)

                ]
                Value model.VoteResult
                Items model.VoteResultItems
                OnChanged (fun r -> dispatch (ItemSelected r))
            ]
            
        ]
        
        label [
            Styles [
                Pos (AbsPos 1,AbsPos 18)
                Dim (AbsDim 1,AbsDim 1)                
                Colors (Color.BrightYellow,Color.Red)
            ]
            Text (sprintf "The Vote says: %s" (model.VoteResult.ToString()))
        ]

        
    ]

