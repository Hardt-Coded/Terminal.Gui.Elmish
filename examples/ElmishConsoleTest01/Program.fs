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
    IsVisible: bool
}

type Msg =
    | Inc 
    | Dec
    | ChangeText of string
    | MenuItemSelected of string
    | RadioChanged of RadioSelectItem
    | ListChanged of ListSelectItem
    | CheckBoxChanged of bool
    | ChangeVisibility of bool


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
        IsVisible = false
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
    | ChangeVisibility b ->
        { model with IsVisible = b }, Cmd.none




let view (state:Model) (dispatch:Msg -> unit) =
    View.page [
        page.menubar [
            menubar.menus [
                menu.menuBarItem [
                    menu.prop.title "Menu 1"
                    menu.prop.children [
                        menu.submenuItem [
                            menu.prop.title "Sub Menu 1"
                            menu.prop.children [
                                menu.menuItem ("Sub Item 1", (fun () -> System.Diagnostics.Debug.WriteLine($"Sub menu 1 triggered")))
                                menu.menuItem [
                                    menu.prop.title "Sub Item 2"
                                    menu.item.action (fun () -> System.Diagnostics.Debug.WriteLine($"Sub menu 2 triggered"))
                                    menu.item.itemstyle.check
                                    menu.item.isChecked true
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
        prop.children [
            View.window [
                prop.position.x.at 0
                prop.position.y.at 1
                prop.width.filled
                prop.height.filled
                window.title "Toller Titel!"
                window.borderStyle.rounded
                prop.children [
                    View.window [
                        prop.position.x.at 1
                        prop.position.y.at 1
                        prop.width.fill 4
                        prop.height.sized 21
                        window.title "Anderer toller Titel"  
                        window.borderStyle.double
                        window.effect3D
                        prop.children [
                            if state.IsVisible then
                                View.label [
                                    prop.text $"Hello Counter: {state.Count}"

                                    let c = (state.Count |> float) / 100.0
                                    let x = (16.0 * Math.Cos(c)) |> int 
                                    let y = (8.0 * Math.Sin(c)) |> int

                                    prop.position.x.at (x + 20)
                                    prop.position.y.at (y + 10)
                                    prop.textAlignment.centered
                                
                                ]
                            View.button [
                                prop.position.x.at 4
                                prop.position.y.at 5
                                prop.text "Plus"
                                button.onClick (fun () -> dispatch Msg.Inc)
                                if state.IsVisible then prop.enabled else prop.disabled
                            ]

                            View.button [
                                prop.position.x.at 14
                                prop.position.y.at 5
                                prop.text "Minus"
                                button.onClick (fun () -> dispatch Msg.Dec)
                            ]

                            View.checkbox [
                                prop.position.x.at 14
                                prop.position.y.at 11
                                prop.text "Minus"
                                if state.IsVisible then
                                    prop.onMouseEnter (fun e -> System.Diagnostics.Debug.WriteLine($"mouse enter event"))
                                checkbox.onToggled (fun t -> System.Diagnostics.Debug.WriteLine($"check toggeld {t}"))
                                checkbox.isChecked true
                            ]

                            View.colorpicker [
                                prop.position.x.at 54
                                prop.position.y.at 2
                                prop.text "Minus"
                                colorpicker.selectedColor Terminal.Gui.Color.BrightCyan
                                colorpicker.onColorChanged (fun color -> System.Diagnostics.Debug.WriteLine($"color changed {color}"))
                            ]

                            View.button [
                                prop.position.x.at 34
                                prop.position.y.at 5
                                prop.text "Visible"
                                button.onClick (fun () -> dispatch <| Msg.ChangeVisibility (state.IsVisible |> not))
                            ]


                            View.combobox [
                                prop.position.x.at 34
                                prop.position.y.at 8
                                prop.width.sized 10
                                prop.text "Combobox"
                                combobox.source [ "Hallo"; "Dies"; "Ist"; "Eine"; "ComboBox" ]
                                combobox.onOpenSelectedItem (fun t ->  System.Diagnostics.Debug.WriteLine($"open selected item {t.Value}"))
                                combobox.onSelectedItemChanged (fun t ->  System.Diagnostics.Debug.WriteLine($"selected item changed {t.Value}"))
                                
                                combobox.readonly state.IsVisible
                            ]

                            View.datefield [
                                prop.position.x.at 49
                                prop.position.y.at 8
                                prop.width.sized 10
                                prop.text "DateField"
                                datefield.date DateTime.Now

                            ]

                            View.timefield [
                                prop.position.x.at 65
                                prop.position.y.at 8
                                prop.width.sized 10
                                prop.text "Timefield"
                                timefield.time DateTime.Now.TimeOfDay
                            ]

                            View.frameview [
                                prop.position.x.at 5
                                prop.position.y.at 12
                                prop.width.sized 20
                                prop.height.sized 7
                                prop.text "FrameView"
                                frameview.borderStyle.rounded
                                frameview.effect3D
                            ]

                            View.hexview [
                                prop.position.x.at 30
                                prop.position.y.at 12
                                prop.width.sized 20
                                prop.height.sized 7
                                prop.text "Hex"
                                hexview.source (new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes("Hello World")))
                            ]

                            View.lineview [
                                prop.position.y.at 1
                                lineview.lineRune (Rune('~'))
                                lineview.startingAnchor (Some <| Rune('>'))
                                lineview.endingAnchor (Some <| Rune('<'))
                            ]


                            View.listview [
                                prop.position.x.at 45
                                prop.position.y.at 12
                                prop.width.sized 30
                                prop.height.sized 5
                                prop.text "List"
                                listview.topItem 3
                                listview.leftItem 2
                                listview.source [ "Hallo"; "Dies"; "Ist"; "Eine"; "ListBox" ]
                                listview.onOpenSelectedItem (fun t ->  System.Diagnostics.Debug.WriteLine($"LB: open selected item {t.Value}"))
                                listview.onSelectedItemChanged (fun t ->  System.Diagnostics.Debug.WriteLine($"LB: selected item changed {t.Value}"))
                            ]
                        ]
                    ]
                ]
            ]
        ]
        
    ]




[<EntryPoint>]
let main argv =
    
    Program.mkProgram init update view  
    |> Program.withSubscription (fun state ->
            fun dispatch ->
                async {
                    while state.Count < 1_000_000 do
                        do! Async.Sleep 10
                        dispatch Inc
                }
                |> Async.StartImmediate
            |> Cmd.ofSub
    )
    |> Program.run
    
    0 // return an integer exit code
