namespace Terminal.Gui.Elmish

open Terminal.Gui
open Terminal.Gui.Elmish.Elements



type View =

    static member inline page (props:IProperty list) = PageElement(props) :> TerminalElement
    static member inline page (children:TerminalElement list) = 
        let props = [ prop.children children ]
        PageElement(props) :> TerminalElement

    static member inline window (props:IProperty list) = WindowElement(props) :> TerminalElement
    static member inline window (children:TerminalElement list) = 
        let props = [ prop.children children ]
        WindowElement(props) :> TerminalElement

    static member inline label (props:IProperty list) = LabelElement(props) :> TerminalElement
    static member inline label (children:TerminalElement list) = 
        let props = [ prop.children children ]
        LabelElement(props) :> TerminalElement

    static member inline button (props:IProperty list) = ButtonElement(props) :> TerminalElement
    static member inline button (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ButtonElement(props) :> TerminalElement

    static member inline checkbox (props:IProperty list) = CheckBoxElement(props) :> TerminalElement
    static member inline checkbox (children:TerminalElement list) = 
        let props = [ prop.children children ]
        CheckBoxElement(props) :> TerminalElement

    static member inline colorpicker (props:IProperty list) = ColorPickerElement(props) :> TerminalElement
    static member inline colorpicker (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ColorPickerElement(props) :> TerminalElement

    static member inline combobox (props:IProperty list) = ComboBoxElement(props) :> TerminalElement
    static member inline combobox (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ComboBoxElement(props) :> TerminalElement

    static member inline datefield (props:IProperty list) = DateFieldElement(props) :> TerminalElement
    static member inline datefield (children:TerminalElement list) = 
        let props = [ prop.children children ]
        DateFieldElement(props) :> TerminalElement

    static member inline frameview (props:IProperty list) = FrameViewElement(props) :> TerminalElement
    static member inline frameview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        FrameViewElement(props) :> TerminalElement

    static member inline graphview (props:IProperty list) = GraphViewElement(props) :> TerminalElement
    static member inline graphview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        GraphViewElement(props) :> TerminalElement

    static member inline hexview (props:IProperty list) = HexViewElement(props) :> TerminalElement
    static member inline hexview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        HexViewElement(props) :> TerminalElement

    static member inline lineview (props:IProperty list) = LineViewElement(props) :> TerminalElement
    

    static member inline listview (props:IProperty list) = ListViewElement(props) :> TerminalElement
    static member inline listview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ListViewElement(props) :> TerminalElement

    static member inline panelview (props:IProperty list) = 
        PanelViewElement(props) :> TerminalElement
    static member inline panelview (child:TerminalElement) = 
        let props = [ panelview.child child ]
        PanelViewElement(props) :> TerminalElement

    static member inline progressbar (props:IProperty list) = ProgressBarElement(props) :> TerminalElement
    static member inline progressbar (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ProgressBarElement(props) :> TerminalElement

    static member inline radiogroup (props:IProperty list) = RadioGroupElement(props) :> TerminalElement
    static member inline radiogroup (children:TerminalElement list) = 
        let props = [ prop.children children ]
        RadioGroupElement(props) :> TerminalElement

    static member inline scrollbarview (props:IProperty list) = ScrollBarViewElement(props) :> TerminalElement
    static member inline scrollbarview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ScrollBarViewElement(props) :> TerminalElement

    static member inline scrollview (props:IProperty list) = ScrollViewElement(props) :> TerminalElement
    static member inline scrollview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ScrollViewElement(props) :> TerminalElement

    static member inline statusbar (props:IProperty list) = StatusBarElement(props) :> TerminalElement
    static member inline statusbar (children:TerminalElement list) = 
        let props = [ prop.children children ]
        StatusBarElement(props) :> TerminalElement

    static member inline tableview (props:IProperty list) = TableViewElement(props) :> TerminalElement
    static member inline tableview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TableViewElement(props) :> TerminalElement

    static member inline tabview (props:IProperty list) = TabViewElement(props) :> TerminalElement
    static member inline tabview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TabViewElement(props) :> TerminalElement

    static member inline textfield (props:IProperty list) = TextFieldElement(props) :> TerminalElement
    static member inline textfield (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TextFieldElement(props) :> TerminalElement

    static member inline textvalidatefield (props:IProperty list) = TextValidateFieldElement(props) :> TerminalElement
    static member inline textvalidatefield (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TextValidateFieldElement(props) :> TerminalElement

    static member inline textview (props:IProperty list) = TextViewElement(props) :> TerminalElement
    static member inline textview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TextViewElement(props) :> TerminalElement

    static member inline timefield (props:IProperty list) = TimeFieldElement(props) :> TerminalElement
    static member inline timefield (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TimeFieldElement(props) :> TerminalElement

    static member inline treeview (props:IProperty list) = TreeViewElement(props) :> TerminalElement
    static member inline treeview (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TreeViewElement(props) :> TerminalElement

    static member inline dialog (props:IProperty list) = DialogElement(props) :> TerminalElement
    static member inline dialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        DialogElement(props) :> TerminalElement

    static member inline filedialog (props:IProperty list) = FileDialogElement(props) :> TerminalElement
    static member inline filedialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        FileDialogElement(props) :> TerminalElement

    static member inline savedialog (props:IProperty list) = SaveDialogElement(props) :> TerminalElement
    static member inline savedialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        SaveDialogElement(props) :> TerminalElement

    static member inline opendialog (props:IProperty list) = OpenDialogElement(props) :> TerminalElement
    static member inline opendialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        OpenDialogElement(props) :> TerminalElement




   

