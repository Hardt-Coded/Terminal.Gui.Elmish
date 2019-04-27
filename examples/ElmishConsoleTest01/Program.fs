// Learn more about F# at http://fsharp.org

open System

open NStack
open Elmish
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


let view (model:Model) (dispatch:Msg -> unit)=
    page [
        
        let ms = new MemoryStream([|
            10 |> byte; 20 |> byte; 30 |> byte; 40 |> byte; 50 |> byte 
            10 |> byte; 20 |> byte; 30 |> byte; 40 |> byte; 50 |> byte
            10 |> byte; 20 |> byte; 30 |> byte; 40 |> byte; 50 |> byte
        |])

        yield menuBar [
            menuBarItem "_Muh" [
                menuItem "MenuItem_1" "" (fun () -> dispatch (MenuItemSelected "MenuItem1"))
                menuItem "MenuItem_2" "" (fun () -> dispatch (MenuItemSelected "MenuItem2"))
            ]
        ]

        yield window [
            Styles [
                Pos (AbsPos 0,AbsPos 1)
                Dim (Fill,Fill)
            ]
            Title "Demo 1"
        ] [
            window [
                Styles [
                    Pos (PercentPos 20.0,PercentPos 10.0)
                    Dim (PercentDim 30.0,AbsDim 15)
                ]
            ] [
                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 1)
                    ]
                    Text "Counter Up"
                    OnClicked (fun () -> dispatch Inc)                    
                ] 

                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 2)
                    ]
                    Text "Counter Down"
                    OnClicked (fun () -> dispatch Dec)                    
                ] 

                textField [
                    Styles [
                        Pos (AbsPos 1, AbsPos 4)
                        Dim (AbsDim 20,AbsDim 1)
                    ]
                    Text model.Text
                    OnChanged (fun t -> dispatch (ChangeText t))
                ]

                frameView [
                    Styles [
                        Pos (AbsPos 1, AbsPos 5)
                        Dim (AbsDim 20,AbsDim 4)
                    ]
                    Text "FrameView w Text"
                ] [
                    textView [
                        Text "bla bla\r\nmuh!\r\nTest\r\nJuchuuu\r\nLorem ipsum\r\n Was?"
                    ]
                ]
            ]

            window [
                Styles [
                    Pos (PercentPos 55.0,PercentPos 10.0)
                    Dim (AbsDim 40,AbsDim 35)
                ]
                Title "Demo 2"
            ] [
                frameView [
                    Styles [
                        Pos (AbsPos 1,AbsPos 1)
                        Dim (AbsDim 36,AbsDim 4)
                    ]
                    Text "FrameView with HexView inside"
                ] [
                    hexView [] ms
                ]

                frameView [
                    Styles [
                        Pos (AbsPos 1,AbsPos 6)
                        Dim (AbsDim 36,AbsDim 6)
                    ]
                    Text "FrameView with List inside"                    
                ] [
                    listView [
                        Items model.ListItems
                        Value model.SelectedListItem
                        OnChanged (fun i -> dispatch (ListChanged i))
                    ]                    
                ]

                radioGroup [
                    Styles [
                        Pos (AbsPos 1,AbsPos 13)
                    ]
                    Items model.RadioItems
                    Value model.SelectedRadioItem
                    OnChanged (fun i -> dispatch (RadioChanged i))
                ] 

                checkBox [
                    Styles [
                        Pos (AbsPos 1,AbsPos 17)
                    ]
                    Text "Yes or No?"
                    Value model.CheckBoxChecked
                    OnChanged (fun v -> dispatch (CheckBoxChanged v))
                ]

                progressBar [
                    Styles [
                        Pos (AbsPos 1,AbsPos 19)
                        Dim (AbsDim 40,AbsDim 2)
                    ]
                    Value 0.8
                ]
            ]

            window [
                Styles [
                    Pos (AbsPos 1,AbsPos 17)
                    Dim (PercentDim 50.0,AbsDim 10)
                ]
                Title "Debug"
            ] [
                let countLabelText = sprintf "CurrentCount: %d" model.Count
                let tfLabelText = sprintf "Textfield shows: %s" model.Text
                let selectedMenuItem = sprintf "Last Selected MenuItem: %s" model.LastSelectedMenuItem
                let selectedRadio = sprintf "Selected RadioItem: %A" model.SelectedRadioItem
                let selectedList = sprintf "Selected ListItem: %A" model.SelectedListItem
                let isCheckBoxChecked = sprintf "Is Checked: %s" (model.CheckBoxChecked.ToString())
                yield label [
                    Styles [ Pos (AbsPos 1,AbsPos 1) ]
                    Text countLabelText
                ]
                yield label [
                    Styles [ Pos (AbsPos 1,AbsPos 2) ]
                    Text tfLabelText
                ]
                yield label [
                    Styles [ Pos (AbsPos 1,AbsPos 3) ]
                    Text selectedMenuItem
                ]
                yield label [
                    Styles [ Pos (AbsPos 1,AbsPos 4) ]
                    Text selectedRadio
                ]
                yield label [
                    Styles [ Pos (AbsPos 1,AbsPos 5) ]
                    Text isCheckBoxChecked
                ]
                yield label [
                    Styles [ Pos (AbsPos 1,AbsPos 6) ]
                    Text selectedList
                ]                
            ]
        ]       
    ]




[<EntryPoint>]
let main argv =
    
    Program.mkConsoleProgram init update view  
    |> Program.run
    
    0 // return an integer exit code
