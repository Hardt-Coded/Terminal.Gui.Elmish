// Learn more about F# at http://fsharp.org

open System

open System.Diagnostics
open System.Drawing
open System.Text
open Terminal.Gui.Elmish
open System.IO
open Terminal.Gui


type Model =
    {
        Count: int
        Text: string
        LastSelectedMenuItem: string
        SelectedRadioItem: int
        SelectedListItem: int
        CheckBoxChecked: bool
        ListItems: string list
        RadioItems: string list
        IsVisible: bool
    }

type Msg =
    | Inc
    | Dec
    | ChangeText of string
    | MenuItemSelected of string
    | RadioChanged of int
    | ListChanged of int
    | CheckBoxChanged of bool
    | ChangeVisibility of bool



let init () =
    {
        Count = 1
        Text = "Muh!"
        LastSelectedMenuItem = ""
        SelectedRadioItem = 1
        SelectedListItem = 1
        CheckBoxChecked = false
        RadioItems =
            [
                "Hello"
                "this"
                "is"
                "a"
                "RadioGroup"
            ]
        ListItems = [ "Hello"; "this"; "is"; "a"; "List" ]
        IsVisible = false
    },
    Cmd.none

let update (msg: Msg) (model: Model) =
    match msg with
    | Inc -> { model with Count = model.Count + 1 }, Cmd.none
    | Dec -> { model with Count = model.Count - 1 }, Cmd.none
    | ChangeText s -> { model with Text = s }, Cmd.none
    | MenuItemSelected item ->
        { model with
            LastSelectedMenuItem = item
        },
        Cmd.none
    | RadioChanged str -> { model with SelectedRadioItem = str }, Cmd.none
    | ListChanged str -> { model with SelectedListItem = str }, Cmd.none
    | CheckBoxChanged b -> { model with CheckBoxChecked = b }, Cmd.none
    | ChangeVisibility b -> { model with IsVisible = b }, Cmd.none


let myColorScheme () =
    let yellow = Color.BrightYellow
    let green = Color.Green
    let color = new Attribute(&yellow, &green)
    ColorScheme(Focus = color, Normal = color)

let view (state: Model) (dispatch: Msg -> unit) =
    View.page [
        page.menuBar [
            menubar.menus [
                menu.menuBarItem [
                    menu.prop.title "Menu 1"
                    menu.prop.children [
                        menu.submenuItem [
                            menu.prop.title "Sub Menu 1"
                            menu.prop.children [
                                menu.menuItem (
                                    "Sub Item 1",
                                    (fun () -> System.Diagnostics.Debug.WriteLine($"Sub menu 1 triggered"))
                                )
                                menu.menuItem [
                                    menu.prop.title "Sub Item 2"
                                    menu.item.action (fun () ->
                                        System.Diagnostics.Debug.WriteLine($"Sub menu 2 triggered")
                                    )
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
                prop.position.x.absolute 0
                prop.position.y.absolute 1
                prop.width.fill 0
                prop.height.fill 0
                prop.title "Toller Titel!"
                prop.children [
                    View.window [
                        prop.position.x.absolute 1
                        prop.position.y.absolute 1
                        prop.width.fill 4
                        prop.height.fill 5
                        prop.title "Anderer toller Titel"
                        prop.children [
                            if state.IsVisible then
                                View.label [
                                    label.text $"Hello Counter: {state.Count}"

                                    let c = (state.Count |> float) / 100.0
                                    let x = (16.0 * Math.Cos(c)) |> int
                                    let y = (8.0 * Math.Sin(c)) |> int

                                    prop.position.x.absolute (x + 20)
                                    prop.position.y.absolute (y + 10)
                                    prop.alignment.center

                                    ]
                            View.button [
                                prop.position.x.absolute 2
                                prop.position.y.absolute 2
                                button.text "Plus"
                                button.onAccept (fun () -> dispatch Msg.Inc)
                                button.titleChanged (fun e -> dispatch <| ChangeText $"title changed {e}")
                                //button.mouseClick (fun e -> dispatch <| ChangeText $"mouse click {e}";dispatch Msg.Inc)
                            ]
                            
                            View.label [
                                prop.position.x.absolute 2
                                prop.position.y.absolute 3
                                label.text $"{state.Text} - {state.Count}"
                            ]

                            (*
                            View.button [
                                prop.position.x.absolute 2
                                prop.position.y.absolute 4
                                button.text "Minus"
                                button.onAccept (fun () -> dispatch Msg.Dec)
                            ]

                            View.checkBox [
                                prop.position.x.absolute 2
                                prop.position.y.absolute 6
                                checkBox.text "Checkbox"
                                if state.IsVisible then
                                    prop.onMouseEnter (fun e -> System.Diagnostics.Debug.WriteLine($"mouse enter event")
                                    )
                                checkBox.onToggled (fun t -> System.Diagnostics.Debug.WriteLine($"check toggeld {t}"))
                                checkBox.isChecked true
                            ]

                            View.colorPicker [
                                prop.position.x.absolute 15
                                prop.position.y.absolute 2
                                colorPicker.title "Color"
                                colorPicker.selectedColor Terminal.Gui.Color.BrightCyan
                                colorPicker.onColorChanged (fun color ->
                                    System.Diagnostics.Debug.WriteLine($"color changed {color}")
                                )
                            ]

                            View.button [
                                prop.position.x.absolute 2
                                prop.position.y.absolute 8
                                button.text "Visible"
                                button.onAccept (fun () ->
                                    dispatch
                                    <| Msg.ChangeVisibility(state.IsVisible |> not)
                                )
                            ]


                            View.comboBox [
                                prop.position.x.absolute 15
                                prop.position.y.absolute 8
                                prop.width.fill 10
                                comboBox.text "Combobox"
                                prop.color (Color.BrightYellow, Color.Green)
                                comboBox.source state.ListItems
                                comboBox.selectedItem state.SelectedListItem
                                comboBox.onOpenSelectedItem (fun t ->
                                    System.Diagnostics.Debug.WriteLine($"open selected item {t.Value}")
                                )
                                comboBox.onSelectedItemChanged (fun e -> dispatch <| ListChanged e.Item)
                            ]

                            View.dateField [
                                prop.position.x.absolute 26
                                prop.position.y.absolute 8
                                prop.width.fill 10
                                dateField.date DateTime.Now

                                ]

                            View.timeField [
                                prop.position.x.absolute 38
                                prop.position.y.absolute 8
                                prop.width.fill 10
                                prop.color (Color.BrightYellow, Color.Green)
                                timeField.time DateTime.Now.TimeOfDay
                            ]

                            View.frameView [
                                prop.position.x.absolute 2
                                prop.position.y.absolute 10
                                prop.width.fill 15
                                prop.height.fill 7
                                frameView.text "FrameView"
                                frameView.borderStyle.rounded
                            ]

                            View.scrollView [
                                prop.position.x.absolute 19
                                prop.position.y.absolute 10
                                prop.width.fill 15
                                prop.height.fill 7
                                scrollView.showHorizontalScrollIndicator true
                                scrollView.showVerticalScrollIndicator true
                                scrollView.contentSize (Size(25, 20))
                                prop.children [
                                    View.hexView [
                                        prop.width.fill 25
                                        prop.height.fill 20
                                        hexView.source (
                                            new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes("Hello World"))
                                        )
                                    ]
                                ]
                            ]

                            View.lineView [
                                prop.position.y.absolute 1
                                lineView.lineRune (Rune('-'))
                                lineView.startingAnchor (Some <| Rune('>'))
                                lineView.endingAnchor (Some <| Rune('<'))
                            ]


                            

                            View.progressBar [
                                prop.position.x.absolute 57
                                prop.position.y.absolute 9
                                prop.width.fill 15
                                progressBar.text "Progress"
                                progressBar.format.simplePlusPercentage
                                progressBar.style.blocks
                                progressBar.bidirectionalMarquee true
                                progressBar.fraction ((DateTime.Now.Second |> float) / 60.0)
                            ]

                            View.progressBar [
                                prop.position.x.absolute 57
                                prop.position.y.absolute 11
                                prop.width.fill 15
                                progressBar.text "Progress"
                                progressBar.format.simple
                                progressBar.style.marqueeBlocks
                                progressBar.fraction ((DateTime.Now.Second |> float) / 60.0)
                            ]

                            View.progressBar [
                                prop.position.x.absolute 57
                                prop.position.y.absolute 17
                                prop.width.fill 15
                                progressBar.text "Progress"
                                progressBar.format.simple
                                progressBar.style.marqueeContinuous
                                progressBar.fraction ((DateTime.Now.Second |> float) / 60.0)
                            ]


                            View.radioGroup [
                                prop.position.x.absolute 57
                                prop.position.y.absolute 2
                                radioGroup.displayMode.vertical
                                radioGroup.radioLabels state.RadioItems
                                radioGroup.onSelectedItemChanged (fun e -> dispatch <| RadioChanged e.SelectedItem)
                                radioGroup.selectedItem state.SelectedRadioItem
                            ]


                            View.tableView [
                                prop.position.x.absolute 72
                                prop.position.y.absolute 2
                                prop.width.fill 50
                                prop.height.fill 8
                                tableView.table Data.table
                            ]

                            View.textField [
                                prop.position.x.absolute 2
                                prop.position.y.absolute 20
                                prop.width.fill 20
                                textField.text state.Text
                                textField.onTextChanging (fun (newText: string) -> dispatch <| Msg.ChangeText newText)
                            ]


                            View.textField [
                                prop.position.x.absolute 20
                                prop.position.y.absolute 20
                                prop.width.fill 20
                                textField.text state.Text
                                textField.onTextChanging (fun (newText: string) -> dispatch <| Msg.ChangeText newText)
                            ]

                            View.textView [
                                prop.position.x.absolute 80
                                prop.position.y.absolute 9
                                prop.width.fill 20
                                prop.height.fill 10
                                textView.text "This is Text!"
                            ]*)
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
                ()
                (*while state.Count < 1_000_000 do
                    do! Async.Sleep 10
                    dispatch Inc*)
            }
            |> Async.StartImmediate
        |> Cmd.ofSub
    )
    |> Program.run

    0 // return an integer exit code
