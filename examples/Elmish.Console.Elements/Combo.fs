module Combo


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
    IsHappy:bool
    VoteResultItems: (VoteResult * string) list
}

type Msg =
    | ChangeVoteResult of VoteResult
    | ChangeHappy of bool

let init () : Model * Cmd<Msg> =
    let model = {
        VoteResult = IDontKnow
        IsHappy = false
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
    | ChangeVoteResult vr ->
        {model with VoteResult = vr}, Cmd.none
    | ChangeHappy b ->
        {model with IsHappy = b}, Cmd.none


let view (model:Model) (dispatch:Msg -> unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.absolute 1
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.BrightYellow, Color.Green)
            label.text "Combo und Co..."
        ] 

        View.label [
            prop.position.x.center
            prop.position.y.absolute 5
            prop.width.fill 0
            prop.alignment.center
            label.text "Please Vote!"
        ]

        View.comboBox [
            prop.position.x.center
            prop.position.y.absolute 7
            prop.width.absolute 30
            prop.height.absolute 5
            comboBox.searchText (model.VoteResultItems |> List.tryFind (fun (r,_)-> r=model.VoteResult) |> Option.map (fun (_,n) -> n) |> Option.defaultValue "")
            comboBox.hideDropdownListOnClick true
            comboBox.selectedItem (model.VoteResultItems |> List.findIndex (fun (i,_) -> i = model.VoteResult))
            comboBox.source (model.VoteResultItems |> List.map snd)
            comboBox.selectedItemChanged 
                (fun r -> 
                    let v = fst model.VoteResultItems.[r.Item]
                    dispatch (ChangeVoteResult v)
                
                )
        ]

        View.label [
            prop.position.x.absolute 1
            prop.position.y.absolute 18
            //prop.autoSize true
            prop.color (Color.BrightYellow,Color.Red)
            label.text $"The Vote says: {model.VoteResult}"
        ]


    ]

