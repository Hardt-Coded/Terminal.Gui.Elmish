module RadioCheck


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
            prop.position.y.at 1
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.BrightYellow, Color.Green)
            prop.text "Radio and Check..."
        ] 

        View.label [
            prop.position.x.center
            prop.position.y.at 5
            prop.width.filled
            prop.textAlignment.centered
            prop.text "Please Vote!"
        ]

        View.radioGroup [
            prop.position.x.center
            prop.position.y.at 7
            radioGroup.selectedItem (model.VoteResultItems |> List.findIndex (fun (i,_) -> i = model.VoteResult))
            radioGroup.radioLabels (model.VoteResultItems |> List.map snd)
            radioGroup.onSelectedItemChanged 
                (fun r -> 
                    let v = fst model.VoteResultItems.[r.SelectedItem]
                    dispatch (ChangeVoteResult v)
                
                )
        ]
                

        View.checkBox [
            prop.position.x.center
            prop.position.y.at 16
            //prop.autoSize true
            checkBox.isChecked model.IsHappy
            checkBox.text "Are you happy?"
            checkBox.onToggled (fun b -> dispatch (ChangeHappy b))            
        ]

        View.label [
            prop.position.x.at 1
            prop.position.y.at 18
            //prop.autoSize true
            prop.color (Color.BrightYellow,Color.Red)
            prop.text $"The Vote says: {model.VoteResult}"
        ]

        View.label [
            prop.position.x.at 1
            prop.position.y.at 19
            prop.color (Color.BrightYellow,Color.Red)
            match model.IsHappy with
            | true ->
                prop.text "You are Happy!"
            | false ->
                prop.text "you are not Happy!"
        ]
    ]

