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


let view (model:Model) (dispatch:Msg -> unit) : ViewElement list=
    [
        label [
            Styles [
                Pos (CenterPos,AbsPos 1)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Terminal.Gui.Color.BrightYellow,Terminal.Gui.Color.Green)
            ]
            Text "Radio and Check..."
        ] 

        label [
            Styles [
                Pos (CenterPos,AbsPos 5)
                Dim (Fill,AbsDim 1)  
                TextAlignment Centered
            ]
            Text "Please Vote!"
        ]

        radioGroup [
            Styles [
                Pos (CenterPos,AbsPos 7)                

            ]
            Value model.VoteResult
            Items model.VoteResultItems
            OnChanged (fun r -> dispatch (ChangeVoteResult r))
        ]
                

        checkBox [
            Styles [
                Pos (CenterPos,AbsPos 16)               

            ]
            Value model.IsHappy
            Text "Are you happy?"
            OnChanged (fun b -> dispatch (ChangeHappy b))            
        ]

        label [
            Styles [
                Pos (AbsPos 1,AbsPos 18)
                Dim (AbsDim 1,AbsDim 1)                
                Colors (Color.BrightYellow,Color.Red)
            ]
            Text (sprintf "The Vote says: %s" (model.VoteResult.ToString()))
        ]

        label [
            yield Styles [
                Pos (AbsPos 1,AbsPos 19)
                Dim (AbsDim 1,AbsDim 1)
                Colors (Color.BrightYellow,Color.Red)
            ]
            match model.IsHappy with
            | true ->
                yield Text "You are Happy!"
            | false ->
                yield Text "you are not Happy!"
        ]
    ]

