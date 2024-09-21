
(*
#######################################
#            View.fs                  #
#######################################
*)


namespace Terminal.Gui.Elmish

open System
open System.ComponentModel
open System.Linq.Expressions
open System.Text
open System.Linq
open Terminal.Gui
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Elmish.EventHelpers
    
type View =

    static member inline topLevel (props:IProperty list) = ToplevelElement(props) :> TerminalElement
    static member inline topLevel (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ToplevelElement(props) :> TerminalElement

    
    /// <seealso cref="Terminal.Gui.Adornment"/>
    static member inline adornment (props:IProperty list) = AdornmentElement(props) :> TerminalElement
    static member inline adornment (children:TerminalElement list) = 
        let props = [ prop.children children ]
        AdornmentElement(props) :> TerminalElement
    static member inline adornment (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        AdornmentElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Bar"/>
    static member inline bar (props:IProperty list) = BarElement(props) :> TerminalElement
    static member inline bar (children:TerminalElement list) = 
        let props = [ prop.children children ]
        BarElement(props) :> TerminalElement
    static member inline bar (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        BarElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Border"/>
    static member inline border (props:IProperty list) = BorderElement(props) :> TerminalElement
    static member inline border (children:TerminalElement list) = 
        let props = [ prop.children children ]
        BorderElement(props) :> TerminalElement
    static member inline border (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        BorderElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Button"/>
    static member inline button (props:IProperty list) = ButtonElement(props) :> TerminalElement
    static member inline button (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ButtonElement(props) :> TerminalElement
    static member inline button (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ButtonElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.CheckBox"/>
    static member inline checkBox (props:IProperty list) = CheckBoxElement(props) :> TerminalElement
    static member inline checkBox (children:TerminalElement list) = 
        let props = [ prop.children children ]
        CheckBoxElement(props) :> TerminalElement
    static member inline checkBox (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        CheckBoxElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker"/>
    static member inline colorPicker (props:IProperty list) = ColorPickerElement(props) :> TerminalElement
    static member inline colorPicker (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ColorPickerElement(props) :> TerminalElement
    static member inline colorPicker (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ColorPickerElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker16"/>
    static member inline colorPicker16 (props:IProperty list) = ColorPicker16Element(props) :> TerminalElement
    static member inline colorPicker16 (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ColorPicker16Element(props) :> TerminalElement
    static member inline colorPicker16 (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ColorPicker16Element(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ComboBox"/>
    static member inline comboBox (props:IProperty list) = ComboBoxElement(props) :> TerminalElement
    static member inline comboBox (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ComboBoxElement(props) :> TerminalElement
    static member inline comboBox (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ComboBoxElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DateField"/>
    static member inline dateField (props:IProperty list) = DateFieldElement(props) :> TerminalElement
    static member inline dateField (children:TerminalElement list) = 
        let props = [ prop.children children ]
        DateFieldElement(props) :> TerminalElement
    static member inline dateField (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        DateFieldElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DatePicker"/>
    static member inline datePicker (props:IProperty list) = DatePickerElement(props) :> TerminalElement
    static member inline datePicker (children:TerminalElement list) = 
        let props = [ prop.children children ]
        DatePickerElement(props) :> TerminalElement
    static member inline datePicker (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        DatePickerElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Dialog"/>
    static member inline dialog (props:IProperty list) = DialogElement(props) :> TerminalElement
    static member inline dialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        DialogElement(props) :> TerminalElement
    static member inline dialog (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        DialogElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FileDialog"/>
    static member inline fileDialog (props:IProperty list) = FileDialogElement(props) :> TerminalElement
    static member inline fileDialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        FileDialogElement(props) :> TerminalElement
    static member inline fileDialog (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        FileDialogElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FrameView"/>
    static member inline frameView (props:IProperty list) = FrameViewElement(props) :> TerminalElement
    static member inline frameView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        FrameViewElement(props) :> TerminalElement
    static member inline frameView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        FrameViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.GraphView"/>
    static member inline graphView (props:IProperty list) = GraphViewElement(props) :> TerminalElement
    static member inline graphView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        GraphViewElement(props) :> TerminalElement
    static member inline graphView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        GraphViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.HexView"/>
    static member inline hexView (props:IProperty list) = HexViewElement(props) :> TerminalElement
    static member inline hexView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        HexViewElement(props) :> TerminalElement
    static member inline hexView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        HexViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Label"/>
    static member inline label (props:IProperty list) = LabelElement(props) :> TerminalElement
    static member inline label (children:TerminalElement list) = 
        let props = [ prop.children children ]
        LabelElement(props) :> TerminalElement
    static member inline label (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        LabelElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LegendAnnotation"/>
    static member inline legendAnnotation (props:IProperty list) = LegendAnnotationElement(props) :> TerminalElement
    static member inline legendAnnotation (children:TerminalElement list) = 
        let props = [ prop.children children ]
        LegendAnnotationElement(props) :> TerminalElement
    static member inline legendAnnotation (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        LegendAnnotationElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Line"/>
    static member inline line (props:IProperty list) = LineElement(props) :> TerminalElement
    static member inline line (children:TerminalElement list) = 
        let props = [ prop.children children ]
        LineElement(props) :> TerminalElement
    static member inline line (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        LineElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LineView"/>
    static member inline lineView (props:IProperty list) = LineViewElement(props) :> TerminalElement
    static member inline lineView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        LineViewElement(props) :> TerminalElement
    static member inline lineView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        LineViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ListView"/>
    static member inline listView (props:IProperty list) = ListViewElement(props) :> TerminalElement
    static member inline listView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ListViewElement(props) :> TerminalElement
    static member inline listView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ListViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Margin"/>
    static member inline margin (props:IProperty list) = MarginElement(props) :> TerminalElement
    static member inline margin (children:TerminalElement list) = 
        let props = [ prop.children children ]
        MarginElement(props) :> TerminalElement
    static member inline margin (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        MarginElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.MenuBar"/>
    static member inline menuBar (props:IProperty list) = MenuBarElement(props) :> TerminalElement
    static member inline menuBar (children:TerminalElement list) = 
        let props = [ prop.children children ]
        MenuBarElement(props) :> TerminalElement
    static member inline menuBar (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        MenuBarElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.MenuBarv2"/>
    static member inline menuBarv2 (props:IProperty list) = MenuBarv2Element(props) :> TerminalElement
    static member inline menuBarv2 (children:TerminalElement list) = 
        let props = [ prop.children children ]
        MenuBarv2Element(props) :> TerminalElement
    static member inline menuBarv2 (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        MenuBarv2Element(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Menuv2"/>
    static member inline menuv2 (props:IProperty list) = Menuv2Element(props) :> TerminalElement
    static member inline menuv2 (children:TerminalElement list) = 
        let props = [ prop.children children ]
        Menuv2Element(props) :> TerminalElement
    static member inline menuv2 (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        Menuv2Element(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown (props:IProperty list) = NumericUpDownElement(props) :> TerminalElement
    static member inline numericUpDown (children:TerminalElement list) = 
        let props = [ prop.children children ]
        NumericUpDownElement(props) :> TerminalElement
    static member inline numericUpDown (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        NumericUpDownElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown<'a> (props:IProperty list) = NumericUpDownElement<'a>(props) :> TerminalElement
    static member inline numericUpDown<'a> (children:TerminalElement list) = 
        let props = [ prop.children children ]
        NumericUpDownElement<'a>(props) :> TerminalElement
    static member inline numericUpDown<'a> (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        NumericUpDownElement<'a>(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.OpenDialog"/>
    static member inline openDialog (props:IProperty list) = OpenDialogElement(props) :> TerminalElement
    static member inline openDialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        OpenDialogElement(props) :> TerminalElement
    static member inline openDialog (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        OpenDialogElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Padding"/>
    static member inline padding (props:IProperty list) = PaddingElement(props) :> TerminalElement
    static member inline padding (children:TerminalElement list) = 
        let props = [ prop.children children ]
        PaddingElement(props) :> TerminalElement
    static member inline padding (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        PaddingElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ProgressBar"/>
    static member inline progressBar (props:IProperty list) = ProgressBarElement(props) :> TerminalElement
    static member inline progressBar (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ProgressBarElement(props) :> TerminalElement
    static member inline progressBar (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ProgressBarElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.RadioGroup"/>
    static member inline radioGroup (props:IProperty list) = RadioGroupElement(props) :> TerminalElement
    static member inline radioGroup (children:TerminalElement list) = 
        let props = [ prop.children children ]
        RadioGroupElement(props) :> TerminalElement
    static member inline radioGroup (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        RadioGroupElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.SaveDialog"/>
    static member inline saveDialog (props:IProperty list) = SaveDialogElement(props) :> TerminalElement
    static member inline saveDialog (children:TerminalElement list) = 
        let props = [ prop.children children ]
        SaveDialogElement(props) :> TerminalElement
    static member inline saveDialog (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        SaveDialogElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollBarView"/>
    static member inline scrollBarView (props:IProperty list) = ScrollBarViewElement(props) :> TerminalElement
    static member inline scrollBarView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ScrollBarViewElement(props) :> TerminalElement
    static member inline scrollBarView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ScrollBarViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollView"/>
    static member inline scrollView (props:IProperty list) = ScrollViewElement(props) :> TerminalElement
    static member inline scrollView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ScrollViewElement(props) :> TerminalElement
    static member inline scrollView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ScrollViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Shortcut"/>
    static member inline shortcut (props:IProperty list) = ShortcutElement(props) :> TerminalElement
    static member inline shortcut (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ShortcutElement(props) :> TerminalElement
    static member inline shortcut (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ShortcutElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider (props:IProperty list) = SliderElement(props) :> TerminalElement
    static member inline slider (children:TerminalElement list) = 
        let props = [ prop.children children ]
        SliderElement(props) :> TerminalElement
    static member inline slider (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        SliderElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider<'a> (props:IProperty list) = SliderElement<'a>(props) :> TerminalElement
    static member inline slider<'a> (children:TerminalElement list) = 
        let props = [ prop.children children ]
        SliderElement<'a>(props) :> TerminalElement
    static member inline slider<'a> (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        SliderElement<'a>(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.SpinnerView"/>
    static member inline spinnerView (props:IProperty list) = SpinnerViewElement(props) :> TerminalElement
    static member inline spinnerView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        SpinnerViewElement(props) :> TerminalElement
    static member inline spinnerView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        SpinnerViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.StatusBar"/>
    static member inline statusBar (props:IProperty list) = StatusBarElement(props) :> TerminalElement
    static member inline statusBar (children:TerminalElement list) = 
        let props = [ prop.children children ]
        StatusBarElement(props) :> TerminalElement
    static member inline statusBar (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        StatusBarElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Tab"/>
    static member inline tab (props:IProperty list) = TabElement(props) :> TerminalElement
    static member inline tab (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TabElement(props) :> TerminalElement
    static member inline tab (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TabElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TabView"/>
    static member inline tabView (props:IProperty list) = TabViewElement(props) :> TerminalElement
    static member inline tabView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TabViewElement(props) :> TerminalElement
    static member inline tabView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TabViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TableView"/>
    static member inline tableView (props:IProperty list) = TableViewElement(props) :> TerminalElement
    static member inline tableView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TableViewElement(props) :> TerminalElement
    static member inline tableView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TableViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextField"/>
    static member inline textField (props:IProperty list) = TextFieldElement(props) :> TerminalElement
    static member inline textField (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TextFieldElement(props) :> TerminalElement
    static member inline textField (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TextFieldElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextValidateField"/>
    static member inline textValidateField (props:IProperty list) = TextValidateFieldElement(props) :> TerminalElement
    static member inline textValidateField (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TextValidateFieldElement(props) :> TerminalElement
    static member inline textValidateField (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TextValidateFieldElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextView"/>
    static member inline textView (props:IProperty list) = TextViewElement(props) :> TerminalElement
    static member inline textView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TextViewElement(props) :> TerminalElement
    static member inline textView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TextViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TileView"/>
    static member inline tileView (props:IProperty list) = TileViewElement(props) :> TerminalElement
    static member inline tileView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TileViewElement(props) :> TerminalElement
    static member inline tileView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TileViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TimeField"/>
    static member inline timeField (props:IProperty list) = TimeFieldElement(props) :> TerminalElement
    static member inline timeField (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TimeFieldElement(props) :> TerminalElement
    static member inline timeField (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TimeFieldElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Toplevel"/>
    static member inline toplevel (props:IProperty list) = ToplevelElement(props) :> TerminalElement
    static member inline toplevel (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ToplevelElement(props) :> TerminalElement
    static member inline toplevel (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        ToplevelElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView (props:IProperty list) = TreeViewElement(props) :> TerminalElement
    static member inline treeView (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TreeViewElement(props) :> TerminalElement
    static member inline treeView (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TreeViewElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView<'a when 'a : not struct> (props:IProperty list) = TreeViewElement<'a>(props) :> TerminalElement
    static member inline treeView<'a when 'a : not struct> (children:TerminalElement list) = 
        let props = [ prop.children children ]
        TreeViewElement<'a>(props) :> TerminalElement
    static member inline treeView<'a when 'a : not struct> (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        TreeViewElement<'a>(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Window"/>
    static member inline window (props:IProperty list) = WindowElement(props) :> TerminalElement
    static member inline window (children:TerminalElement list) = 
        let props = [ prop.children children ]
        WindowElement(props) :> TerminalElement
    static member inline window (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        WindowElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Wizard"/>
    static member inline wizard (props:IProperty list) = WizardElement(props) :> TerminalElement
    static member inline wizard (children:TerminalElement list) = 
        let props = [ prop.children children ]
        WizardElement(props) :> TerminalElement
    static member inline wizard (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        WizardElement(props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.WizardStep"/>
    static member inline wizardStep (props:IProperty list) = WizardStepElement(props) :> TerminalElement
    static member inline wizardStep (children:TerminalElement list) = 
        let props = [ prop.children children ]
        WizardStepElement(props) :> TerminalElement
    static member inline wizardStep (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        WizardStepElement(props) :> TerminalElement



module Dialogs =
    open System

    let showWizard (wizard:Wizard) =
        Application.Run(wizard)
        wizard.Dispose()
        ()


    let openFileDialog title =
        use dia = new OpenDialog(Title=title)
        Application.Run(dia)
        dia.Dispose()
        //Application.Top.Remove(dia) |> ignore
        if dia.Canceled then
            None
        else
            let file = 
                dia.FilePaths
                |> Seq.tryHead
                |> Option.bind (fun s ->
                    if String.IsNullOrEmpty(s) then None 
                    else Some s
                )
            file


    let messageBox (width:int) height title text (buttons:string list) =
        let result = MessageBox.Query(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ when result < 0 || result > buttons.Length - 1  -> ""
        | _ -> buttons.[result]


    let errorBox (width:int) height title text (buttons:string list) =
        let result = MessageBox.ErrorQuery(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ when result < 0 || result > buttons.Length - 1  -> ""
        | _ -> buttons.[result]


    
