// Learn more about F# at http://fsharp.org

open System

open NStack
open Terminal.Gui.Elmish
open System.IO




type RadioSelectItem =
    | RadioItem1
    | RadioItem2
    | RadioItem3

type ListSelectItem =
    | ListItem1
    | ListItem2
    | ListItem3

type Model = {
    Count:int
    Text:string
    LastSelectedMenuItem:string
    SelectedRadioItem:RadioSelectItem
    SelectedListItem:ListSelectItem
    CheckBoxChecked:bool
    ListItems: (ListSelectItem * string) list
    RadioItems: (RadioSelectItem * string) list
}

type Msg =
    | Inc 
    | Dec
    | ChangeText of string
    | MenuItemSelected of string
    | RadioChanged of RadioSelectItem
    | ListChanged of ListSelectItem
    | CheckBoxChanged of bool


let init () =
    { 
        Count = 1 
        Text = "Muh!"
        LastSelectedMenuItem = ""        
        SelectedRadioItem = RadioItem2
        SelectedListItem = ListItem3
        CheckBoxChecked=false
        RadioItems=[
            (RadioItem1,"Radio Item 1")
            (RadioItem2,"Radio Item 2")
            (RadioItem3,"Radio Item 3")
        ]
        ListItems=[
            (ListItem1,"List Item 1")
            (ListItem2,"List Item 2")
            (ListItem3,"List Item 3")
        ]
    }, Cmd.none

let update (msg:Msg) (model:Model) =
    match msg with
    | Inc ->
        {model with Count = model.Count + 1}, Cmd.none
    | Dec ->
        {model with Count = model.Count - 1}, Cmd.none
    | ChangeText s ->
        {model with Text = s}, Cmd.none
    | MenuItemSelected item ->
        {model with LastSelectedMenuItem = item}, Cmd.none
    | RadioChanged str ->
        {model with SelectedRadioItem = str}, Cmd.none
    | ListChanged str ->
        {model with SelectedListItem = str}, Cmd.none
    | CheckBoxChanged b ->
        {model with CheckBoxChecked = b}, Cmd.none




let view (state:Model) (dispatch:Msg -> unit) =
    View.page [
        View.window [
            prop.position.x.at 1
            prop.position.y.at 2
            prop.width.filled
            prop.height.filled
            window.title "Toller Titel!"
            prop.children [
                View.window [
                    prop.position.x.at 4
                    prop.position.y.at 4
                    prop.width.fill 4
                    prop.height.sized 10
                    window.title "Anderer toller Titel"
                    prop.children [
                        View.label [
                            prop.position.x.at 6
                            prop.position.y.at 4
                            prop.text $"Hello Counter: {state.Count}"
                        ]
                        View.button [
                            prop.position.x.at 4
                            prop.position.y.at 5
                            prop.text "Plus"
                            button.onclick (fun () -> dispatch Msg.Inc)
                        ]

                        View.button [
                            prop.position.x.at 14
                            prop.position.y.at 5
                            prop.text "Minus"
                            button.onclick (fun () -> dispatch Msg.Dec)
                        ]
                    ]
                ]
            ]
        ]
    ]




[<EntryPoint>]
let main argv =
    
    Program.mkProgram init update view  
    //|> Program.withSubscription (fun state ->
    //        fun dispatch ->
    //            async {
    //                while state.Count < 1_000_000 do
    //                    do! Async.Sleep 10
    //                    dispatch Inc
    //            }
    //            |> Async.StartImmediate
    //        |> Cmd.ofSub
    //)
    |> Program.run
    
    0 // return an integer exit code
