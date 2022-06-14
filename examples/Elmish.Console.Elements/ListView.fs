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


let view (model:Model) (dispatch:Msg -> unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.at 1
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.BrightYellow, Color.Green)
            prop.text "List View"
        ] 


        View.frameView [
            prop.position.x.at 1
            prop.position.y.at 4
            prop.width.fill 1
            prop.height.fill 5
            frameView.title "Please Vote!"
            frameView.children [
                View.listView [
                    prop.position.x.at 0
                    prop.position.y.at 0
                    prop.width.filled
                    prop.height.filled
                    listView.selectedItem (model.VoteResultItems |> List.findIndex (fun (i,_) -> i = model.VoteResult))
                    listView.source (model.VoteResultItems |> List.map snd)
                    listView.onSelectedItemChanged 
                        (fun r -> 
                            let v = fst model.VoteResultItems.[r.Item]
                            dispatch (ItemSelected v)
                        )
                ]
            ]
        ]
        
        View.label [
            prop.position.x.at 1
            prop.position.y.at 18              
            prop.color (Color.BrightYellow,Color.Red)
            label.text (sprintf "The Vote says: %s" (model.VoteResult.ToString()))
        ]

        
    ]

