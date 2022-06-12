// Learn more about F# at http://fsharp.org

open System

open NStack
open Terminal.Gui.Elmish
open System.IO
open Terminal.Gui
open Terminal.Gui
open Terminal.Gui








type Model = {
    Count:int
    Text:string
    LastSelectedMenuItem:string
    SelectedRadioItem:int
    SelectedListItem:int
    CheckBoxChecked:bool
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


module Data =

    open System.Data

    let xml = """

<NewDataSet>
<xs:schema xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" id="NewDataSet">
<xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:MainDataTable="RecentMatter" msdata:UseCurrentLocale="true">
<xs:complexType>
<xs:choice minOccurs="0" maxOccurs="unbounded">
<xs:element name="RecentMatter">
<xs:complexType>
<xs:sequence>
<xs:element name="UserLogin">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="MatterNumber">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="ClientName">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="MatterName">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="ClientCode" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="OfficeCode" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="OfficeName" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="Billable" type="xs:boolean"/>
<xs:element name="ReferenceId" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="LastUsed" type="xs:dateTime"/>
</xs:sequence>
</xs:complexType>
</xs:element>
</xs:choice>
</xs:complexType>
<xs:unique name="Constraint1" msdata:PrimaryKey="true">
<xs:selector xpath=".//RecentMatter"/>
<xs:field xpath="UserLogin"/>
<xs:field xpath="MatterNumber"/>
</xs:unique>
</xs:element>
</xs:schema>

<RecentMatter>
  <UserLogin>PSLTP6\RJK</UserLogin>
  <MatterNumber>99999-2302</MatterNumber>
  <ClientName>Test Matters</ClientName>
  <MatterName>DP Test Matter</MatterName>
  <ClientCode>99999</ClientCode>
  <OfficeCode/>
  <OfficeName/>
  <Billable>true</Billable>
  <ReferenceId/>
  <LastUsed>2011-08-23T23:40:24.13+01:00</LastUsed>
</RecentMatter>
<RecentMatter>
  <UserLogin>PSLTP6\RJK</UserLogin>
  <MatterNumber>999991.0002</MatterNumber>
  <ClientName>Lathe 1</ClientName>
  <MatterName>LW Test 2</MatterName>
  <ClientCode/>
  <OfficeCode/>
  <OfficeName/>
  <Billable>true</Billable>
  <ReferenceId/>
  <LastUsed>2011-07-12T16:57:27.173+01:00</LastUsed>
</RecentMatter>
<RecentMatter>
  <UserLogin>PSLTP6\RJK</UserLogin>
  <MatterNumber>999991-0001</MatterNumber>
  <ClientName>Lathe 1</ClientName>
  <MatterName>LW Test 1</MatterName>
  <ClientCode/>
  <OfficeCode/>
  <OfficeName/>
  <Billable>false</Billable>
  <ReferenceId/>
  <LastUsed>2011-07-12T01:59:06.887+01:00</LastUsed>
</RecentMatter>
</NewDataSet>

    """

    let table = 
        use sr = new StringReader(xml)
        let dt = new DataTable()
        dt.ReadXml(sr) |> ignore
        dt

let init () =
    { 
        Count = 1 
        Text = "Muh!"
        LastSelectedMenuItem = ""        
        SelectedRadioItem = 1
        SelectedListItem = 1
        CheckBoxChecked=false
        RadioItems=[
            "Hello"
            "this"
            "is"
            "a"
            "RadioGroup"
        ]
        ListItems=[
            "Hello"
            "this"
            "is"
            "a"
            "List"
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


let myColorScheme () =
    let color = Attribute.Make(Color.BrightYellow,Color.Green)
    ColorScheme(Focus=color,Normal=color)

let view (state:Model) (dispatch:Msg -> unit) =
    View.page [
        page.menuBar [
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
                        prop.height.sized 23
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

                            View.checkBox [
                                prop.position.x.at 14
                                prop.position.y.at 11
                                prop.text "Checkbox"
                                if state.IsVisible then
                                    prop.onMouseEnter (fun e -> System.Diagnostics.Debug.WriteLine($"mouse enter event"))
                                checkBox.onToggled (fun t -> System.Diagnostics.Debug.WriteLine($"check toggeld {t}"))
                                checkBox.isChecked true
                            ]

                            View.colorPicker [
                                prop.position.x.at 34
                                prop.position.y.at 2
                                prop.text "Color"
                                colorPicker.selectedColor Terminal.Gui.Color.BrightCyan
                                colorPicker.onColorChanged (fun color -> System.Diagnostics.Debug.WriteLine($"color changed {color}"))
                            ]

                            View.button [
                                prop.position.x.at 24
                                prop.position.y.at 5
                                prop.text "Visible"
                                button.onClick (fun () -> dispatch <| Msg.ChangeVisibility (state.IsVisible |> not))
                            ]

                            
                            View.comboBox [
                                prop.position.x.at 34
                                prop.position.y.at 8
                                prop.width.sized 10
                                prop.text "Combobox"
                                prop.color Colors.Base
                                comboBox.source state.ListItems
                                comboBox.selectedItem state.SelectedListItem
                                comboBox.onOpenSelectedItem (fun t ->  System.Diagnostics.Debug.WriteLine($"open selected item {t.Value}"))
                                comboBox.onSelectedItemChanged (fun e -> dispatch <| ListChanged e.Item)
                                comboBox.readonly state.IsVisible
                            ]

                            View.dateField [
                                prop.position.x.at 49
                                prop.position.y.at 8
                                prop.width.sized 10
                                prop.text "DateField"
                                dateField.date DateTime.Now

                            ]

                            View.timeField [
                                prop.position.x.at 65
                                prop.position.y.at 8
                                prop.width.sized 10
                                prop.text "Timefield"
                                timeField.time DateTime.Now.TimeOfDay
                            ]

                            View.frameView [
                                prop.position.x.at 5
                                prop.position.y.at 12
                                prop.width.sized 20
                                prop.height.sized 7
                                prop.text "FrameView"
                                frameView.borderStyle.rounded
                                frameView.effect3D
                            ]

                            View.hexView [
                                prop.position.x.at 30
                                prop.position.y.at 12
                                prop.width.sized 20
                                prop.height.sized 7
                                prop.text "Hex"
                                hexView.source (new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes("Hello World")))
                            ]

                            View.lineView [
                                prop.position.y.at 1
                                lineView.lineRune (Rune('~'))
                                lineView.startingAnchor (Some <| Rune('>'))
                                lineView.endingAnchor (Some <| Rune('<'))
                            ]


                            View.panelView [
                                prop.position.x.at 45
                                prop.position.y.at 12
                                panelView.borderStyle.rounded
                                panelView.effect3D
                                panelView.child <|
                                    View.listView [
                                    prop.width.sized 15
                                    prop.height.sized 5
                                    prop.text "List"
                                    listView.topItem 3
                                    listView.leftItem 2
                                    listView.source state.ListItems
                                    listView.selectedItem state.SelectedListItem
                                    listView.onOpenSelectedItem (fun t ->  System.Diagnostics.Debug.WriteLine($"LB: open selected item {t.Value}"))
                                    listView.onSelectedItemChanged (fun e -> dispatch <| ListChanged e.Item)
                                ]
                            ]

                            View.progressBar [
                                prop.position.x.at 65
                                prop.position.y.at 9
                                prop.width.sized 15
                                prop.text "Progress"
                                progressBar.format.simplePlusPercentage
                                progressBar.style.blocks
                                progressBar.bidirectionalMarquee true
                                progressBar.fraction ((DateTime.Now.Second |> float) / 60.0)
                            ]

                            View.progressBar [
                                prop.position.x.at 65
                                prop.position.y.at 11
                                prop.width.sized 15
                                prop.text "Progress"
                                progressBar.format.framedProgressPadded
                                progressBar.style.marqueeBlocks
                                progressBar.fraction ((DateTime.Now.Second |> float) / 60.0)
                            ]

                            View.progressBar [
                                prop.position.x.at 65
                                prop.position.y.at 17
                                prop.width.sized 15
                                prop.text "Progress"
                                progressBar.format.framed
                                progressBar.style.marqueeContinuous
                                progressBar.fraction ((DateTime.Now.Second |> float) / 60.0)
                            ]


                            View.radioGroup [
                                prop.position.x.at 82
                                prop.position.y.at 2
                                prop.text "Progress"
                                radioGroup.displayMode.vertical
                                radioGroup.radioLabels state.RadioItems
                                radioGroup.onSelectedItemChanged (fun e -> dispatch <| RadioChanged e.SelectedItem)
                                radioGroup.selectedItem state.SelectedRadioItem
                            ]


                            View.scrollView [
                                prop.position.x.at 68
                                prop.position.y.at 2
                                prop.width.sized 12
                                prop.height.sized 5
                                scrollView.showHorizontalScrollIndicator true
                                scrollView.showVerticalScrollIndicator true
                                scrollView.contentSize (Size(10,10))
                                prop.children [
                                    View.radioGroup [
                                        prop.text "Scroll"
                                        radioGroup.displayMode.vertical
                                        radioGroup.radioLabels (state.RadioItems @ state.ListItems)
                                        radioGroup.onSelectedItemChanged (fun e -> dispatch <| RadioChanged e.SelectedItem)
                                        radioGroup.selectedItem state.SelectedRadioItem
                                    ]
                                
                                ]
                            ]

                            View.tableView [
                                prop.position.x.at 82
                                prop.position.y.at 8
                                prop.width.sized 30
                                prop.height.sized 8
                                tableView.table Data.table
                            ]

                            View.textField [
                                prop.position.x.at 82
                                prop.position.y.at 18
                                prop.width.sized 20
                                prop.text state.Text
                                textField.onTextChanging (fun (newText:string) -> dispatch <| Msg.ChangeText newText)
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
