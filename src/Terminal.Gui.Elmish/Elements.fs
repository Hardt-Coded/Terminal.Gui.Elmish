namespace Terminal.Gui.Elmish

open System.Reflection
open System.Collections
open System
open Terminal.Gui
open Terminal.Gui
open Terminal.Gui





[<AbstractClass>]
type TerminalElement (props:IProperty list) =
    let mutable view: View = null
    let c = props |> Interop.getValueDefault<TerminalElement list> "children" []

    member this.element with get() = view and set v = view <- v
    member _.properties = props
    member _.children   = c

    abstract create: unit -> unit
    abstract update: prevElement:View -> oldProps:IProperty list -> unit
    abstract canUpdate: prevElement:View -> oldProps:IProperty list -> bool
    abstract name: string


[<RequireQualifiedAccess>]
module ViewElement =

    let setEvents (view:View) props =
        let onEnabledChanged = props |> Interop.getValue<unit->unit> "onEnabledChanged"
        let onEnter          = props |> Interop.getValue<Terminal.Gui.View.FocusEventArgs->unit> "onEnter"
        let onKeyDown        = props |> Interop.getValue<Terminal.Gui.View.KeyEventEventArgs->unit> "onKeyDown"
        let onKeyPress       = props |> Interop.getValue<Terminal.Gui.View.KeyEventEventArgs->unit> "onKeyPress"
        let onKeyUp          = props |> Interop.getValue<Terminal.Gui.View.KeyEventEventArgs->unit> "onKeyUp"
        let onLeave          = props |> Interop.getValue<Terminal.Gui.View.FocusEventArgs->unit> "onLeave"
        let onMouseClick     = props |> Interop.getValue<Terminal.Gui.View.MouseEventArgs->unit> "onMouseClick"
        let onMouseEnter     = props |> Interop.getValue<Terminal.Gui.View.MouseEventArgs->unit> "onMouseEnter"
        let onMouseLeave     = props |> Interop.getValue<Terminal.Gui.View.MouseEventArgs->unit> "onMouseLeave"
        let onVisibleChanged = props |> Interop.getValue<unit->unit> "onVisibleChanged"

        
        onEnabledChanged 
        |> Option.iter (fun onEnabledChanged -> 
            Interop.removeEventHandlerIfNecessary "EnabledChanged" view
            view.add_EnabledChanged onEnabledChanged
        )
        
        onEnter 
        |> Option.iter (fun onEnter -> 
            Interop.removeEventHandlerIfNecessary "Enter" view
            view.add_Enter onEnter
        )
        
        onKeyDown 
        |> Option.iter (fun onKeyDown -> 
            Interop.removeEventHandlerIfNecessary "KeyDown" view
            view.add_KeyDown onKeyDown
        )
        
        onKeyPress 
        |> Option.iter (fun onKeyPress -> 
            Interop.removeEventHandlerIfNecessary "KeyPress" view
            view.add_KeyPress onKeyPress
        )
        
        onKeyUp 
        |> Option.iter (fun onKeyUp -> 
            Interop.removeEventHandlerIfNecessary "KeyUp" view
            view.add_KeyUp onKeyUp
        )
        
        onLeave 
        |> Option.iter (fun onLeave -> 
            Interop.removeEventHandlerIfNecessary "Leave" view
            view.add_Leave onLeave
        )
        
        onMouseClick 
        |> Option.iter (fun onMouseClick -> 
            Interop.removeEventHandlerIfNecessary "MouseClick" view
            view.add_MouseClick onMouseClick
        )
        
        onMouseEnter 
        |> Option.iter (fun onMouseEnter -> 
            Interop.removeEventHandlerIfNecessary "MouseEnter" view
            view.add_MouseEnter onMouseEnter
        )
        
        onMouseLeave 
        |> Option.iter (fun onMouseLeave ->
            Interop.removeEventHandlerIfNecessary "MouseLeave" view
            view.add_MouseLeave onMouseLeave
        )
        
        onVisibleChanged 
        |> Option.iter (fun onVisibleChanged -> 
            Interop.removeEventHandlerIfNecessary "VisibleChanged" view
            view.add_VisibleChanged onVisibleChanged
        )
        

    let setProps (view:View) props =
        setEvents view props
        props |> Interop.getValue<Pos> "x"      |> Option.iter (fun v -> view.X <- v)
        props |> Interop.getValue<Pos> "y"      |> Option.iter (fun v -> view.Y <- v)
        props |> Interop.getValue<Dim> "width"  |> Option.iter (fun v -> view.Width <- v)
        props |> Interop.getValue<Dim> "height" |> Option.iter (fun v -> view.Height <- v)

        props |> Interop.getValue<string> "text"                    |> Option.iter (fun v -> view.Text <- v)
        props |> Interop.getValue<TextAlignment> "textAlignment"    |> Option.iter (fun v -> view.TextAlignment <- v)
        props |> Interop.getValue<TextDirection> "textDirection"    |> Option.iter (fun v -> view.TextDirection <- v)
            
        props |> Interop.getValue<int> "tabIndex" |> Option.iter (fun v -> view.TabIndex <- v)
        props |> Interop.getValue<bool> "autoSize" |> Option.iter (fun v -> view.AutoSize <- v)
        
        let tabStop = props |> Interop.valueExists "tabStop"
        if view.TabStop <> tabStop then
            view.TabStop <- tabStop

        props |> Interop.getValue<bool> "enabled" |> Option.iter (fun v -> view.Enabled <- v)
        


    let removeEvents (view:View) props =
        let onEnabledChanged = props |> Interop.getValue<unit->unit> "onEnabledChanged"
        let onEnter          = props |> Interop.getValue<Terminal.Gui.View.FocusEventArgs->unit> "onEnter"
        let onKeyDown        = props |> Interop.getValue<Terminal.Gui.View.KeyEventEventArgs->unit> "onKeyDown"
        let onKeyPress       = props |> Interop.getValue<Terminal.Gui.View.KeyEventEventArgs->unit> "onKeyPress"
        let onKeyUp          = props |> Interop.getValue<Terminal.Gui.View.KeyEventEventArgs->unit> "onKeyUp"
        let onLeave          = props |> Interop.getValue<Terminal.Gui.View.FocusEventArgs->unit> "onLeave"
        let onMouseClick     = props |> Interop.getValue<Terminal.Gui.View.MouseEventArgs->unit> "onMouseClick"
        let onMouseEnter     = props |> Interop.getValue<Terminal.Gui.View.MouseEventArgs->unit> "onMouseEnter"
        let onMouseLeave     = props |> Interop.getValue<Terminal.Gui.View.MouseEventArgs->unit> "onMouseLeave"
        let onVisibleChanged = props |> Interop.getValue<unit->unit> "onVisibleChanged"


        onEnabledChanged 
        |> Option.iter (fun onEnabledChanged -> 
            Interop.removeEventHandlerIfNecessary "EnabledChanged" view
        )

        onEnter 
        |> Option.iter (fun onEnter -> 
            Interop.removeEventHandlerIfNecessary "Enter" view
        )

        onKeyDown 
        |> Option.iter (fun onKeyDown -> 
            Interop.removeEventHandlerIfNecessary "KeyDown" view
        )

        onKeyPress 
        |> Option.iter (fun onKeyPress -> 
            Interop.removeEventHandlerIfNecessary "KeyPress" view
        )

        onKeyUp 
        |> Option.iter (fun onKeyUp -> 
            Interop.removeEventHandlerIfNecessary "KeyUp" view
        )

        onLeave 
        |> Option.iter (fun onLeave -> 
            Interop.removeEventHandlerIfNecessary "Leave" view
        )

        onMouseClick 
        |> Option.iter (fun onMouseClick -> 
            Interop.removeEventHandlerIfNecessary "MouseClick" view
        )

        onMouseEnter 
        |> Option.iter (fun onMouseEnter -> 
            Interop.removeEventHandlerIfNecessary "MouseEnter" view
        )

        onMouseLeave 
        |> Option.iter (fun onMouseLeave ->
            Interop.removeEventHandlerIfNecessary "MouseLeave" view
        )

        onVisibleChanged 
        |> Option.iter (fun onVisibleChanged -> 
            Interop.removeEventHandlerIfNecessary "VisibleChanged" view
        )        


    let removeProps (view:View) props =
        removeEvents view props


    let canUpdate (view:View) props =
        true


type PageElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (toplevel:Toplevel) props =
        // todo: StatusBar
        // toplevel.StatusBar
        ()

    let removeProps (element:Toplevel) props =
        ()

    override _.name = "Page"


    override this.create () =
        let el = Toplevel.Create()
        ViewElement.setProps el props
        setProps el props
        this.element <- el

    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> Toplevel
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type WindowElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let title = props |> Interop.getValueDefault "title" ""
    

    let setProps (window:Window) props =
        props |> Interop.getValue<string> "title" |> Option.map Interop.ustr |> Option.iter (fun v -> window.Title <- v)
        props |> Interop.getValue<BorderStyle> "borderStyle" |> Option.iter (fun v -> window.Border.BorderStyle <- v)
        props |> Interop.getValue<bool> "effect3D" |> Option.iter (fun v -> window.Border.Effect3D <- v)

        
    let removeProps (element:Window) props =
        ()

    override _.name = "Window"


    override this.create () =
        let el = new Window(title |> Interop.ustr)
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> Window
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type LabelElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let text = props |> Interop.getValueDefault "text" ""
    

    let setProps (element:Label) props =
        props |> Interop.getValue<string> "text" |> Option.iter (fun v -> element.Text <- v)
        
        // onCLick
        props 
        |> Interop.getValue<unit->unit> "onClick" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Clicked" element
            element.add_Clicked v
        )
        

    let removeProps (element:Label) props =
        // onCLick
        props 
        |> Interop.getValue<unit->unit> "onClick" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Clicked" element
        )

    override _.name = $"Label"


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.create () =
        let el = new Label(text |> Interop.ustr)
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.update prevElement oldProps = 
        let element = prevElement :?> Label
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type ButtonElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let text = props |> Interop.getValueDefault "text" ""

    let setProps (element:Button) props =
        props |> Interop.getValue<string> "text" |> Option.iter (fun v -> element.Text <- v)
        // onCLick
        props 
        |> Interop.getValue<unit->unit> "onClick" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Clicked" element
            element.add_Clicked v
        )
        
            

    let removeProps (element:Button) props =
        // onCLick
        props 
        |> Interop.getValue<unit->unit> "onClick" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Clicked" element
        )

    override _.name = $"Button"


    override this.create () =
        System.Diagnostics.Debug.WriteLine($"button created!")
        let el = new Button(text |> Interop.ustr)
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> Button
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type CheckBoxElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:CheckBox) props =
        props |> Interop.getValue<string> "text" |> Option.iter (fun v -> element.Text <- v)
        props |> Interop.getValue<bool> "checked" |> Option.iter (fun v -> element.Checked <- v)
        // onToggled
        props 
        |> Interop.getValue<bool->unit> "toggled" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Toggled" element
            element.add_Toggled v
        )

    let removeProps (element:CheckBox) props =
        // onToggled
        props 
        |> Interop.getValue<bool->unit> "toggled" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Toggled" element
            element.add_Toggled v
        )

    override _.name = $"CheckBox"


    override this.create () =
        let el = new CheckBox()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> CheckBox
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type ColorPickerElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ColorPicker) props =
        props |> Interop.getValue<Terminal.Gui.Color> "selectedColor" |> Option.iter (fun v -> element.SelectedColor <- v)
        //onColorChanged
        props 
        |> Interop.getValue<Terminal.Gui.Color->unit> "onColorChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ColorChanged" element
            let action = fun () -> element.SelectedColor |> v
            element.add_ColorChanged action
        )

    let removeProps (element:ColorPicker) props =
        //onColorChanged
        props 
        |> Interop.getValue<unit->unit> "onColorChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ColorChanged" element
        )

    override _.name = $"ColorPicker"


    override this.create () =
        let el = new ColorPicker()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> ColorPicker
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type ComboBoxElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ComboBox) props =
        props |> Interop.getValue<int> "selectedItem" |> Option.iter (fun v -> element.SelectedItem <- v)
        props 
        |> Interop.getValue<string list> "source" 
        |> Option.iter (fun v -> 
            element.SetSource(v |> System.Linq.Enumerable.ToList)
            //let (_,height) = element.GetCurrentHeight()
            //if height < 3 then 
            //    let height = Math.Min(7,v.Length + 1)
            //    element.Height <- height
            
        )
        //
        props 
        |> Interop.getValue<Terminal.Gui.ListViewItemEventArgs->unit> "onOpenSelectedItem" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "OpenSelectedItem" element
            element.add_OpenSelectedItem v
        )

        props 
        |> Interop.getValue<Terminal.Gui.ListViewItemEventArgs->unit> "onSelectedItemChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedItemChanged" element
            element.add_SelectedItemChanged v
        )

    let removeProps (element:ComboBox) props =
        props 
        |> Interop.getValue<Terminal.Gui.ListViewItemEventArgs->unit> "onOpenSelectedItem" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "OpenSelectedItem" element
        )

        props 
        |> Interop.getValue<Terminal.Gui.ListViewItemEventArgs->unit> "onSelectedItemChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedItemChanged" element
        )

    override _.name = $"ComboBox"


    override this.create () =
        let el = new ComboBox()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> ComboBox
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type DateFieldElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:DateField) props =
        props |> Interop.getValue<DateTime> "date" |> Option.iter (fun v -> element.Date <- v)
        props |> Interop.getValue<bool> "isShortFormat" |> Option.iter (fun v -> element.IsShortFormat <- v)
        // onDateChanged
        props 
        |> Interop.getValue<Terminal.Gui.DateTimeEventArgs<DateTime>->unit> "onDateChanged" 
        |> Option.iter (fun v ->
            Interop.removeEventHandlerIfNecessary "DateChanged" element
            element.add_DateChanged v
        )

    let removeProps (element:DateField) props =
        // onDateChanged
        props 
        |> Interop.getValue<Terminal.Gui.DateTimeEventArgs<DateTime>->unit> "onDateChanged" 
        |> Option.iter (fun v ->
            Interop.removeEventHandlerIfNecessary "DateChanged" element
        )

    override _.name = $"DateField"


    override this.create () =
        let el = new DateField()
        ViewElement.setProps el props
        setProps el props
        // cursor to the end
        el.CursorPosition <- 10
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> DateField
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type FrameViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:FrameView) props =
        props |> Interop.getValue<BorderStyle> "borderStyle" |> Option.iter (fun v -> element.Border.BorderStyle <- v)
        props |> Interop.getValue<bool> "effect3D" |> Option.iter (fun v -> element.Border.Effect3D <- v)

    let removeProps (element:FrameView) props =
        ()

    override _.name = $"FrameView"


    override this.create () =
        let el = new FrameView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> FrameView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


// Seems incomplete
type GraphViewElement(props:IProperty list) =
    inherit TerminalElement(props) 


    (*
    series (series: Graphs.ISeries list)       
    scrollOffset(value:PointF)                 
    marginLeft  (value:uint32)                 
    marginBottom(value:uint32)                 
    graphColor  (value:Attribute option)       
    cellSize    (value:PointF)                 
    axisY       (value:Graphs.VerticalAxis)    
    axisX       (value:Graphs.HorizontalAxis)  
    annotations (value:Graphs.IAnnotation list)
    *)
    let setProps (element:GraphView) props =
        props |> Interop.getValue<PointF> "scrollOffset"                    |> Option.iter (fun v -> element.ScrollOffset <- v)
        props |> Interop.getValue<uint32> "marginLeft"                      |> Option.iter (fun v -> element.MarginLeft <- v)
        props |> Interop.getValue<uint32> "marginBottom"                    |> Option.iter (fun v -> element.MarginBottom <- v)
        props |> Interop.getValue<Attribute option> "graphColor"            |> Option.iter (fun v -> element.GraphColor <- v |> Option.toNullable)
        props |> Interop.getValue<PointF> "cellSize"                        |> Option.iter (fun v -> element.CellSize <- v)
        props |> Interop.getValue<Graphs.VerticalAxis> "axisY"              |> Option.iter (fun v -> element.AxisY <- v)
        props |> Interop.getValue<Graphs.HorizontalAxis> "axisX"            |> Option.iter (fun v -> element.AxisX <- v)

    let removeProps (element:GraphView) props =
        ()

    override _.name = $"GraphView"


    override this.create () =
        let el = new GraphView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> GraphView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type HexViewElement(props:IProperty list) =
    inherit TerminalElement(props) 


    let setProps (element:HexView) props =
        props |> Interop.getValue<System.IO.Stream> "source"    |> Option.iter (fun v -> element.Source <- v)
        props |> Interop.getValue<int64> "displayStart"         |> Option.iter (fun v -> element.DisplayStart <- v)
        props |> Interop.getValue<bool> "allowEdits"            |> Option.iter (fun v -> element.AllowEdits <- v)
        // onEdited
        props 
        |> Interop.getValue<System.Collections.Generic.KeyValuePair<int64,byte>->unit> "onEdited" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Edited" element
            element.add_Edited v
        )
        // onPositionChanged
        props 
        |> Interop.getValue<HexView.HexViewEventArgs->unit> "onPositionChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "PositionChanged" element
            element.add_PositionChanged v
        )

    let removeProps (element:HexView) props =
        // onEdited
        props 
        |> Interop.getValue<System.Collections.Generic.KeyValuePair<int64,byte>->unit> "onEdited" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Edited" element
        )
        // onPositionChanged
        props 
        |> Interop.getValue<HexView.HexViewEventArgs->unit> "onPositionChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "PositionChanged" element
        )

    override _.name = $"HexView"


    override this.create () =
        let el = new HexView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> HexView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type LineViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:LineView) props =
        props |> Interop.getValue<System.Rune option> "startingAnchor"  |> Option.iter (fun v -> element.StartingAnchor <- v |> Option.toNullable)
        props |> Interop.getValue<System.Rune option> "endingAnchor"    |> Option.iter (fun v -> element.EndingAnchor <- v |> Option.toNullable)
        props |> Interop.getValue<System.Rune> "lineRune"               |> Option.iter (fun v -> element.LineRune <- v)
        props |> Interop.getValue<Graphs.Orientation> "orientation"     |> Option.iter (fun v -> element.Orientation <- v)

    let removeProps (element:LineView) props =
        ()

    override _.name = $"LineView"


    override this.create () =
        let el = new LineView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> LineView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type ListViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ListView) props =
        ()

    let removeProps (element:ListView) props =
        ()

    override _.name = $"ListView"


    override this.create () =
        let el = new ListView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> ListView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type MenuBarElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:MenuBar) props =
        ()

    let removeProps (element:MenuBar) props =
        ()

    override _.name = $"MenuBar"


    override this.create () =
        let el = new MenuBar()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> MenuBar
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type PanelViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:PanelView) props =
        ()

    let removeProps (element:PanelView) props =
        ()

    override _.name = $"PanelView"


    override this.create () =
        let el = new PanelView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> PanelView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type ProgressBarElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ProgressBar) props =
        ()

    let removeProps (element:ProgressBar) props =
        ()

    override _.name = $"ProgressBar"


    override this.create () =
        let el = new ProgressBar()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> ProgressBar
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type RadioGroupElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:RadioGroup) props =
        ()

    let removeProps (element:RadioGroup) props =
        ()

    override _.name = $"RadioGroup"


    override this.create () =
        let el = new RadioGroup()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> RadioGroup
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type ScrollBarViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ScrollBarView) props =
        ()

    let removeProps (element:ScrollBarView) props =
        ()

    override _.name = $"ScrollBarView"


    override this.create () =
        let el = new ScrollBarView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> ScrollBarView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type ScrollViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ScrollView) props =
        ()

    let removeProps (element:ScrollView) props =
        ()

    override _.name = $"ScrollView"


    override this.create () =
        let el = new ScrollView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> ScrollView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type StatusBarElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:StatusBar) props =
        ()

    let removeProps (element:StatusBar) props =
        ()

    override _.name = $"StatusBar"


    override this.create () =
        let el = new StatusBar()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> StatusBar
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type TableViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:TableView) props =
        ()

    let removeProps (element:TableView) props =
        ()

    override _.name = $"TableView"


    override this.create () =
        let el = new TableView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> TableView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type TabViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:TabView) props =
        ()

    let removeProps (element:TabView) props =
        ()

    override _.name = $"TabView"


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.create () =
        let el = new TabView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.update prevElement oldProps = 
        let element = prevElement :?> TabView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type TextFieldElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:TextField) props =
        ()

    let removeProps (element:TextField) props =
        ()

    override _.name = $"TextField"


    override this.create () =
        let el = new TextField()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> TextField
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type TextValidateFieldElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:TextValidateField) props =
        ()

    let removeProps (element:TextValidateField) props =
        ()

    override _.name = $"TextValidateField"


    override this.create () =
        let el = new TextValidateField()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> TextValidateField
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type TextViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:TextView) props =
        ()

    let removeProps (element:TextView) props =
        ()

    override _.name = $"TextView"


    override this.create () =
        let el = new TextView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> TextView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type TimeFieldElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:TimeField) props =
        props |> Interop.getValue<TimeSpan> "time" |> Option.iter (fun v -> element.Time <- v)
        props |> Interop.getValue<bool> "isShortFormat" |> Option.iter (fun v -> element.IsShortFormat <- v)
        // onDateChanged
        props 
        |> Interop.getValue<Terminal.Gui.DateTimeEventArgs<TimeSpan>->unit> "onTimeChanged" 
        |> Option.iter (fun v ->
            Interop.removeEventHandlerIfNecessary "TimeChanged" element
            element.add_TimeChanged v
        )

    let removeProps (element:TimeField) props =
        // onDateChanged
        props 
        |> Interop.getValue<Terminal.Gui.DateTimeEventArgs<TimeSpan>->unit> "onTimeChanged" 
        |> Option.iter (fun v ->
            Interop.removeEventHandlerIfNecessary "TimeChanged" element
        )

    override _.name = $"TimeField"


    override this.create () =
        let el = new TimeField()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> TimeField
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type TreeViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:TreeView) props =
        ()

    let removeProps (element:TreeView) props =
        ()

    override _.name = $"TreeView"


    override this.create () =
        let el = new TreeView()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> TreeView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type DialogElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:Dialog) props =
        ()

    let removeProps (element:Dialog) props =
        ()

    override _.name = $"Dialog"


    override this.create () =
        let el = new Dialog()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> Dialog
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type FileDialogElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:FileDialog) props =
        ()

    let removeProps (element:FileDialog) props =
        ()

    override _.name = $"FileDialog"


    override this.create () =
        let el = new FileDialog()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> FileDialog
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type SaveDialogElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:SaveDialog) props =
        ()

    let removeProps (element:SaveDialog) props =
        ()

    override _.name = $"SaveDialog"


    override this.create () =
        let el = new SaveDialog()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps = 
        let element = prevElement :?> SaveDialog
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



type OpenDialogElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:OpenDialog) props =
        ()

    let removeProps (element:OpenDialog) props =
        ()

    override _.name = $"OpenDialog"


    override this.create () =
        let el = new OpenDialog()
        ViewElement.setProps el props
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let canUpdateView = ViewElement.canUpdate prevElement oldProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps =
        let element = prevElement :?> OpenDialog
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement






(*

Window

Button

CheckBox

ColorPicker

ComboBox

DateField

FrameView

GraphView

HexView

Label

LineView

ListView

Menu

MenuBar

PanelView

ProgressBar

RadioGroup

ScrollBarView

ScrollView

StatusBar

TableView

TabView

TextField

TextValidateField

TextView

TimeField

TreeView

TreeView`1

Dialog

DirListView

FileDialog

SaveDialog

OpenDialog

Popup

ToplevelContainer

ChildContentView

ContentView

ContentView

ChildContentView

TabRowView


*)








//[<AutoOpen>]
//module StyleHelpers =
    
//    open Terminal.Gui

//    type Position =
//        | AbsPos of int
//        | PercentPos of float
//        | CenterPos

//    type Dimension =
//        | Fill
//        | FillMargin of int
//        | AbsDim of int
//        | PercentDim of float

//    type TextAlignment =
//        | Left 
//        | Right
//        | Centered
//        | Justified

//    type ColorScheme =
//        | Normal
//        | Focus
//        | HotNormal
//        | HotFocus

//    type ScrollBar =
//        | NoBars
//        | HorizontalBar
//        | VerticalBar
//        | BothBars

//    type Style =
//        | Pos of x:Position * y:Position
//        | Dim of width:Dimension * height:Dimension
//        | TextAlignment of TextAlignment
//        | TextColorScheme of ColorScheme
//        | Colors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
//        // Additional Colors
//        | FocusColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
//        | HotNormalColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
//        | HotFocusedColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
//        | DisabledColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
        
        

//    type Prop<'TValue> =
//        | Styles of Style list
//        | Value of 'TValue
//        | Text of string
//        | Title of string
//        | OnChanged of ('TValue -> unit)
//        | OnClicked of (unit -> unit)
//        | Items of ('TValue * string) list
//        | Secret
//        // Scrollbar Stuff
//        | ScrollContentSize of int * int
//        | ScrollOffset of int * int
//        | ScrollBar of ScrollBar
//        | Frame of (int * int * int * int)
//        // Date And Time Field Stuff
//        | IsShort

        

        

    
//    let private convDim (dim:Dimension) =
//        match dim with
//        | Fill -> Dim.Fill()
//        | FillMargin m -> Dim.Fill(m)
//        | AbsDim i -> Dim.Sized(i)
//        | PercentDim p -> Dim.Percent(p |> float32)

//    let private convPos (dim:Position) =
//        match dim with
//        | Position.AbsPos i -> Pos.At(i)
//        | Position.PercentPos p -> Pos.Percent(p |> float32)
//        | Position.CenterPos -> Pos.Center()



    
//    let addTextAlignmentToLabel (label:Label) (alignment:TextAlignment) =
//        match alignment with
//        | Left -> label.TextAlignment <- Terminal.Gui.TextAlignment.Left
//        | Right -> label.TextAlignment <- Terminal.Gui.TextAlignment.Right
//        | Centered -> label.TextAlignment <- Terminal.Gui.TextAlignment.Centered
//        | Justified -> label.TextAlignment <- Terminal.Gui.TextAlignment.Justified

    

//    let addStyleToView (view:View) (style:Style) =
//        match style with
//        | Pos (x,y) ->
//            view.X <- x |> convPos
//            view.Y <- y |> convPos

//        | Dim (width,height) ->
//            view.Width <- width |> convDim
//            view.Height <- height |> convDim

//        | TextAlignment alignment ->
//            match view with
//            | :? Label as label ->                
//                alignment |> addTextAlignmentToLabel label
//            | _ -> 
//                ()

//        | TextColorScheme color ->
//            let colorScheme =
//                let s = 
//                    if Application.Current<> null then
//                        Application.Current.ColorScheme
//                    else
//                        Terminal.Gui.ColorScheme()
//                match color with
//                | Normal -> s.Normal
//                | Focus -> s.Focus
//                | HotNormal -> s.HotNormal
//                | HotFocus -> s.HotFocus
//            match view with
//            | :? Label as label ->                
//                label.TextColor <- colorScheme
//            | _ -> 
//                ()

//        | Colors (fg,bg) ->
//            let color = Terminal.Gui.Attribute.Make(fg,bg)
//            match view with
//            | :? Label as label ->                
//                label.TextColor <- color
//            | _ as view ->
//                if view.ColorScheme = null then
//                    view.ColorScheme <- Terminal.Gui.ColorScheme()
//                else
//                    view.ColorScheme.Normal <- color

//        | FocusColors (fg,bg) ->
//            let color = Terminal.Gui.Attribute.Make(fg,bg)
//            if view.ColorScheme = null then
//                view.ColorScheme <- Terminal.Gui.ColorScheme()
//            else
//                view.ColorScheme.Focus <- color

//        | HotNormalColors(fg,bg) ->
//            let color = Terminal.Gui.Attribute.Make(fg,bg)
//            if view.ColorScheme = null then
//                view.ColorScheme <- Terminal.Gui.ColorScheme()
//            else
//                view.ColorScheme.HotNormal <- color

//        | HotFocusedColors(fg,bg) ->
//            let color = Terminal.Gui.Attribute.Make(fg,bg)
//            if view.ColorScheme = null then
//                view.ColorScheme <- Terminal.Gui.ColorScheme()
//            else
//                view.ColorScheme.HotFocus <- color

//        | DisabledColors(fg,bg) ->
//            let color = Terminal.Gui.Attribute.Make(fg,bg)
//            if view.ColorScheme = null then
//                view.ColorScheme <- Terminal.Gui.ColorScheme()
//            else
//                view.ColorScheme.Disabled <- color
            
            
            
    
//    let addStyles (styles:Style list) (view:View)=
//        styles
//        |> List.iter (fun si ->
//            si |> addStyleToView view                    
//        )

//    let tryGetStylesFromProps (props:Prop<'TValue> list) =
//        props
//        |> List.tryFind (fun i -> match i with | Styles _ -> true | _ -> false)

//    let inline addPossibleStylesFromProps (props:Prop<'TValue> list) (view:'T when 'T :> View) =
//        let styles = tryGetStylesFromProps props
//        match styles with
//        | None ->
//            view
//        | Some (Styles styles) ->
//            view |> addStyles styles
//            view
//        | Some _ ->
//            view

    
//    let getTitleFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | Title _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | Title t -> t | _ -> "")
//        |> Option.defaultValue ""

//    let getTextFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | Text _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | Text t -> t | _ -> "")
//        |> Option.defaultValue ""

//    let tryGetValueFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | Value _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | Value t -> t | _ -> failwith "What?No!Never should this happen!")

//    let tryGetFrameFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | Frame _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | Frame t -> t | _ -> failwith "What?No!Never should this happen!")

//    let tryGetOnChangedFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | OnChanged _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | OnChanged t -> t | _ -> failwith "What?No!Never should this happen!")

//    let tryGetOnClickedFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | OnClicked _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | OnClicked t -> t | _ -> failwith "What?No!Never should this happen!")

//    let getItemsFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | Items _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | Items t -> t | _ -> failwith "What?No!Never should this happen!")
//        |> Option.defaultValue []

//    let getScrollBarFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | ScrollBar _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | ScrollBar t -> t | _ -> failwith "What?No!Never should this happen!")
//        |> Option.defaultValue NoBars

//    let getScrollContentSizeFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | ScrollContentSize _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | ScrollContentSize (w,h) -> (w,h) | _ -> failwith "What?No!Never should this happen!")

//    let getScrollOffsetFromProps (props:Prop<'TValue> list) = 
//        props
//        |> List.tryFind (fun i -> match i with | ScrollOffset _ -> true | _ -> false)
//        |> Option.map (fun i -> match i with | ScrollOffset (w,h) -> (w,h) | _ -> failwith "What?No!Never should this happen!")

//    let getAbsPosFromProps (props:Prop<'TValue> list) =
//        tryGetStylesFromProps props
//        |> Option.bind (
//            function
//            | Styles styles ->
//                styles
//                |> List.choose (fun i -> 
//                    match i with 
//                    | Pos (AbsPos x, AbsPos y) -> Some (x,y) 
//                    | Pos (_, AbsPos y) -> Some (0,y) 
//                    | Pos (AbsPos x, _) -> Some (x,0) 
//                    | _ -> None)
//                |> List.tryHead
//            | _ ->
//                Some (0,0)
//        )
//        |> Option.defaultValue (0,0)
        
//    let hasShortProp (props:Prop<'TValue> list) =
//        props
//        |> List.exists (fun i -> match i with | IsShort _ -> true | _ -> false)
        

//    let hasSecretInProps (props:Prop<'TValue> list) = 
//        props
//        |> List.exists (fun i -> match i with | Secret _ -> true | _ -> false)
        
        
       

    



//[<AutoOpen>]
//module Elements =

//    open Terminal.Gui
//    open NStack

//    let ustr (x:string) = ustring.Make(x)


//    let page (subViews:View list) =
//        let top = Toplevel.Create()        
//        subViews |> List.iter (fun v -> top.Add(v))
//        top
       

//    /// able to change the color scheme of the toplevel page
//    let styledPage (props:Prop<'TValue> list) (subViews:View list) =
//        let top = Toplevel.Create()        
//        subViews |> List.iter (fun v -> top.Add(v))
//        top
//        |> addPossibleStylesFromProps props


//    let window (props:Prop<'TValue> list) (subViews:View list) =        
//        let title = getTitleFromProps props
//        let window = Window(title |> ustr)
//        subViews |> List.iter (fun v -> window.Add(v))        
//        window
//        |> addPossibleStylesFromProps props


//    let button (props:Prop<'TValue> list) = 
//        let text = getTextFromProps props
//        let b = Button(text |> ustr)
//        let clicked = tryGetOnClickedFromProps props
//        match clicked with
//        | Some clicked ->
//            b.Clicked <- Action((fun () -> clicked() ))
//        | None ->
//            ()
//        b
//        |> addPossibleStylesFromProps props


//    let label (props:Prop<'TValue> list) =   
//        let text = getTextFromProps props
//        let l = Label(text |> ustr)
//        l
//        |> addPossibleStylesFromProps props


//    let textField (props:Prop<string> list) =        
//        let value = 
//            tryGetValueFromProps props
//            |> Option.defaultValue ""
        
//        let t = TextField(value |> ustr,Used = true)

//        let changed = tryGetOnChangedFromProps props
//        match changed with
//        | Some changed ->
//            t.Changed.AddHandler(fun o _ -> changed (((o:?>TextField).Text).ToString()))        
//        | None -> ()

//        let secret = hasSecretInProps props
//        t.Secret <- secret

//        t
//        |> addPossibleStylesFromProps props


//    /// DateField:
//    /// Only AbsPos for Positioning 
//    /// Exclusive 'isShort' Prop
//    /// Currently in Version 0.81 of the Terminal.Gui this use a DateTime
//    /// From the time beeing, the master branch uses TimeSpan
//    let timeField (props:Prop<TimeSpan> list) =
//        let value = 
//            tryGetValueFromProps props
//            |> Option.defaultValue TimeSpan.Zero

//        let (x,y) =
//            getAbsPosFromProps props

//        let isShort =
//            hasShortProp props
        

//        let t = TimeField(x,y,System.DateTime.MinValue.Add(value),isShort)

//        let changed = tryGetOnChangedFromProps props
//        match changed with
//        | Some changed ->
//            let dtToTs (dt:DateTime) =
//                TimeSpan(dt.Hour,dt.Minute,dt.Second)
//            t.Changed.AddHandler(fun o _ -> changed ((o:?>TimeField).Time |> dtToTs))        
//        | None -> ()
        
//        t
//        |> addPossibleStylesFromProps props


//    /// DateField:
//    /// Only AbsPos for Positioning 
//    /// Exclusive 'isShort' Prop
//    let dateField (props:Prop<DateTime> list) =
//        let value = 
//            tryGetValueFromProps props
//            |> Option.defaultValue DateTime.MinValue

//        let (x,y) =
//            getAbsPosFromProps props

//        let isShort =
//            hasShortProp props
        
//        let t = DateField(x,y,value,isShort)

//        let changed = tryGetOnChangedFromProps props
//        match changed with
//        | Some changed ->
//            t.Changed.AddHandler(fun o _ -> changed ((o:?>DateField).Date))        
//        | None -> ()
        
//        t
//        |> addPossibleStylesFromProps props


//    let textView (props:Prop<'TValue> list) =
//        let text = getTextFromProps props
//        let t = TextView()
//        t.Text <- (text|> ustr)
//        t
//        |> addPossibleStylesFromProps props
   

//    let frameView (props:Prop<'TValue> list) (subViews:View list) =
//        let text = getTextFromProps props
//        let f = FrameView(text |> ustr)
//        subViews |> List.iter (fun v -> f.Add(v))
//        f
//        |> addPossibleStylesFromProps props


//    let hexView (props:Prop<'TValue> list) stream =
//        HexView(stream)
//        |> addPossibleStylesFromProps props


//    let inline listView (props:Prop<'TValue> list) = 
//        let items = getItemsFromProps props
//        let displayValues = items |> List.map (snd) |> List.toArray :> IList
//        let value = tryGetValueFromProps props
//        let selectedIdx = 
//            value
//            |> Option.bind (fun value ->
//                items |> List.tryFindIndex (fun (v,_) -> v = value) 
//            )
            
//        let lv = 
//            ListView(displayValues)
//            |> addPossibleStylesFromProps props
//        let addSelectedChanged (lv:ListView) =
//            let onChange =
//                tryGetOnChangedFromProps props
//            match onChange with
//            | Some onChange ->
//                let action = Action((fun () -> 
//                    let (value,disp) = items.[lv.SelectedItem]
//                    onChange (value)
//                ))
//                lv.add_SelectedChanged(action)
//                lv
//            | None ->
//                lv
        
//        match selectedIdx with
//        | None ->
//            lv
//            |> addSelectedChanged
//            |> addPossibleStylesFromProps props
//        | Some idx ->
//            lv.SelectedItem <- idx
//            lv
//            |> addSelectedChanged
//            |> addPossibleStylesFromProps props
            

//    let menuItem title help action = 
//        MenuItem(title |> ustr,help ,(fun () -> action () ))


//    let menuBarItem text (items:MenuItem list) = 
//        MenuBarItem(text |> ustr,items |> List.toArray)


//    let menuBar (items:MenuBarItem list) = 
//        MenuBar (items |> List.toArray)

//    /// able to change the color scheme of the status bar
//    let styledMenuBar (props:Prop<'TValue> list) (items:MenuBarItem list) = 
//        MenuBar (items |> List.toArray)
//        |> addPossibleStylesFromProps props 


//    let progressBar (props:Prop<float> list) = 
//        let value = 
//            tryGetValueFromProps props
//            |> Option.defaultValue 0.0

//        let pb = ProgressBar(Fraction = (value |> float32))        
//        pb
//        |> addPossibleStylesFromProps props


//    let checkBox (props:Prop<bool> list) = 
//        let isChecked = 
//            tryGetValueFromProps props
//            |> Option.defaultValue false

//        let text = getTextFromProps props
//        let cb = CheckBox(text |> ustr,isChecked)
//        let onChanged = tryGetOnChangedFromProps props
//        match onChanged with
//        | Some onChanged ->
//            cb.Toggled.AddHandler((fun o e -> (o:?>CheckBox).Checked |> onChanged))
//        | None ->
//            ()
//        cb
//        |> addPossibleStylesFromProps props
    

//    let setCursorRadioGroup (x:int) (rg:RadioGroup) =
//        rg.Cursor <- x
//        rg


//    let inline radioGroup (props:Prop<'TValue> list) =
//        let items = getItemsFromProps props        
//        let value = tryGetValueFromProps props
//        let displayValues = items |> List.map (snd) |> List.toArray
//        let idxItem = 
//            value
//            |> Option.bind (fun value ->
//                items |> List.tryFindIndex (fun (v,_) -> v = value)
//            )

//        let addSelectedChanged (rg:RadioGroup) =
//            let onChange =
//                tryGetOnChangedFromProps props
//            match onChange with
//            | Some onChange ->
//                let action = Action<int>((fun idx -> 
//                    let (value,disp) = items.[idx]
//                    onChange (value)
//                ))
//                rg.SelectionChanged <- action
//                rg
//            | None ->
//                rg

//        match idxItem with
//        | None ->
//            RadioGroup(displayValues)
//            |> addSelectedChanged
//            |> addPossibleStylesFromProps props
//        | Some idx ->
//            RadioGroup(displayValues,idx)
//            |> setCursorRadioGroup idx
//            |> addSelectedChanged
//            |> addPossibleStylesFromProps props
            
    
//    let scrollView (props:Prop<'TValue> list) (subViews:View list) =
//        let frame = tryGetFrameFromProps props
//        match frame with
//        | None ->
//            failwith "Scrollview need a Frame Prop"
//        | Some (x,y,w,h) ->
//            let sv = ScrollView(Rect(x,y,w,h))
//            // Scroll bars
//            let bars = getScrollBarFromProps props
//            match bars with
//            | NoBars ->
//                ()
//            | HorizontalBar ->
//                sv.ShowHorizontalScrollIndicator <- true
//            | VerticalBar ->
//                sv.ShowVerticalScrollIndicator <- true
//            | BothBars ->
//                sv.ShowHorizontalScrollIndicator <- true
//                sv.ShowVerticalScrollIndicator <- true

//            // ContentSize
//            let contentSize = getScrollContentSizeFromProps props
//            match contentSize with
//            | None -> ()
//            | Some (w,h) ->
//                sv.ContentSize <- Size(w,h)

//            // Offset
//            let offset = getScrollOffsetFromProps props
//            match offset with
//            | None -> ()
//            | Some (x,y) ->
//                sv.ContentOffset <- Point(x,y)

            

//            subViews |> List.iter (fun i -> sv.Add(i))
//            sv
//            |> addPossibleStylesFromProps props


//    let fileDialog title prompt nameFieldLabel message =
//        let dia = FileDialog(title |> ustr,prompt |> ustr,nameFieldLabel |> ustr,message |> ustr)
//        Application.Run(dia)
//        let file = 
//            dia.FilePath
//            |> Option.ofObj 
//            |> Option.map string
//            |> Option.bind (fun s ->
//                if String.IsNullOrEmpty(s) then None 
//                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
//            )
//        file


//    let openDialog title message =
//        let dia = OpenDialog(title |> ustr,message |> ustr)                
//        Application.Run(dia)
//        let file = 
//            dia.FilePath
//            |> Option.ofObj 
//            |> Option.map string
//            |> Option.bind (fun s ->
//                if String.IsNullOrEmpty(s) then None 
//                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
//            )
//        file
        

//    let saveDialog title message =
//        let dia = SaveDialog(title |> ustr,message |> ustr)        
//        Application.Run(dia)
//        let file = 
//            dia.FileName
//            |> Option.ofObj 
//            |> Option.map string
//            |> Option.bind (fun s ->
//                if String.IsNullOrEmpty(s) then None 
//                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
//            )
//        file


//    let messageBox width height title text (buttons:string list) =
//        let result = MessageBox.Query(width,height,title,text,buttons |> List.toArray)
//        match buttons with
//        | [] -> ""
//        | _ -> buttons.[result]


//    let errorBox width height title text (buttons:string list) =
//        let result = MessageBox.ErrorQuery(width,height,title,text,buttons |> List.toArray)
//        match buttons with
//        | [] -> ""
//        | _ -> buttons.[result]


//    let statusBar (items:StatusItem list) =
//        StatusBar(items |> List.toArray)

//    /// able to change the color scheme of the status bar
//    let styledStatusBar (props:Prop<'TValue> list) (items:StatusItem list) =
//        StatusBar(items |> List.toArray)
//        |> addPossibleStylesFromProps props 


//    let statusItem text (key:Terminal.Gui.Key) action =
//        if key = Key.F9 then
//            invalidArg "key" ("F9 is reserved to open a menu, sorry.")
//        else
//            StatusItem(key,text |> ustr, Action(action))




//[<AutoOpen>]
//module Elements =

//    open Terminal.Gui
//    open NStack

//    let ustr (x:string) = ustring.Make(x)


//    let page (subViews:ViewElement list) =
//        {
//            Type = PageElement
//            Element = None
//            Props = []
//            Children = subViews
//        }
       

//    ///// able to change the color scheme of the toplevel page
//    let styledPage (props:IProp list) (subViews:ViewElement list) =
//        {
//            Type = PageElement
//            Element = None
//            Props = props
//            Children = subViews
//        }


//    let window (props:IProp list) (subViews:ViewElement list) =        
//        {
//            Type = WindowElement
//            Element = None
//            Props = props
//            Children = subViews
//        }


//    let button (props:IProp list) = 
//        {
//            Type = ButtonElement
//            Element = None
//            Props = props
//            Children = []
//        }


//    let label (props:IProp list) =   
//        {
//            Type = LabelElement
//            Element = None
//            Props = props
//            Children = []
//        }


//    let textField (props:IProp<string> list) =        
//        {
//            Type = TextFieldElement
//            Element = None
//            Props = props  |> List.map (fun i -> i :> IProp)
//            Children = []
//        }


//    ///// DateField:
//    ///// Only AbsPos for Positioning 
//    ///// Exclusive 'isShort' Prop
//    let timeField (props:IProp<TimeSpan> list) =
//        {
//            Type = TimeFieldElement
//            Element = None
//            Props = props |> List.map (fun i -> i :> IProp)
//            Children = []
//        }


//    ///// DateField:
//    ///// Only AbsPos for Positioning 
//    ///// Exclusive 'isShort' Prop
//    let dateField (props:IProp<DateTime> list) =
//        {
//            Type = DateFieldElement
//            Element = None
//            Props = props  |> List.map (fun i -> i :> IProp)
//            Children = []
//        }


//    let textView (props:IProp list) =
//        {
//            Type = TextViewElement
//            Element = None
//            Props = props
//            Children = []
//        }
   

//    let frameView (props:IProp list) (subViews:ViewElement list) =        
//        {
//            Type = FrameViewElement
//            Element = None
//            Props = props
//            Children = subViews
//        }


//    let hexView (props:IProp list) =
//        {
//            Type = HexViewElement
//            Element = None
//            Props = props
//            Children = []
//        }


//    let inline listView (props:IProp list) =
//        {
//            Type = ListViewElement
//            Element = None
//            Props = props
//            Children = []
//        }
            

//    let menuItem title help action = 
//        MenuItem(title |> ustr,help ,(fun () -> action () ))


//    let menuBarItem text (items:MenuItem list) = 
//        MenuBarItem(text |> ustr,items |> List.toArray)


//    let menuBar (items:MenuBarItem list) = 
//        MenuBar (items |> List.toArray)

//    ///// able to change the color scheme of the status bar
//    //let styledMenuBar (props:Prop<'TValue> list) (items:MenuBarItem list) = 
//    //    MenuBar (items |> List.toArray)
//    //    |> addPossibleStylesFromProps props 


//    let progressBar (props:IProp list) =
//        {
//            Type = ProgressBarElement
//            Element = None
//            Props = props
//            Children = []
//        }


//    let checkBox (props:IProp list) =
//        {
//            Type = CheckBoxElement
//            Element = None
//            Props = props
//            Children = []
//        }
    

//    //let setCursorRadioGroup (x:int) (rg:RadioGroup) =
//    //    rg.Cursor <- x
//    //    rg


//    let inline radioGroup (props:IProp list) =
//        {
//            Type = RadioGroupElement
//            Element = None
//            Props = props
//            Children = []
//        }
            
    
//    let scrollView (props:IProp list) (subViews:ViewElement list) =        
//        {
//            Type = ScrollViewElement
//            Element = None
//            Props = props
//            Children = subViews
//        }


//    let fileDialog title prompt nameFieldLabel message =
//        let dia = FileDialog(title |> ustr,prompt |> ustr,nameFieldLabel |> ustr,message |> ustr)
//        Application.Run(dia)
//        let file = 
//            dia.FilePath
//            |> Option.ofObj 
//            |> Option.map string
//            |> Option.bind (fun s ->
//                if String.IsNullOrEmpty(s) then None 
//                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
//            )
//        file


//    let openDialog title message =
//        let dia = OpenDialog(title |> ustr,message |> ustr)                
//        Application.Run(dia)
//        let file = 
//            dia.FilePath
//            |> Option.ofObj 
//            |> Option.map string
//            |> Option.bind (fun s ->
//                if String.IsNullOrEmpty(s) then None 
//                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
//            )
//        file
        

//    let saveDialog title message =
//        let dia = SaveDialog(title |> ustr,message |> ustr)        
//        Application.Run(dia)
//        let file = 
//            dia.FileName
//            |> Option.ofObj 
//            |> Option.map string
//            |> Option.bind (fun s ->
//                if String.IsNullOrEmpty(s) then None 
//                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
//            )
//        file


//    let messageBox width height (title:string) (text:string) (buttons:string list) =
//        let result = MessageBox.Query(width, height, title |> ustring.Make, text |> ustring.Make, buttons |> List.map ustring.Make |> List.toArray)
//        match buttons with
//        | [] -> ""
//        | _ -> buttons.[result]


//    let errorBox width height (title:string) (text:string) (buttons:string list) =
//        let result = MessageBox.ErrorQuery(width, height, title |> ustring.Make, text |> ustring.Make, buttons |> List.map ustring.Make |> List.toArray)
//        match buttons with
//        | [] -> ""
//        | _ -> buttons.[result]


//    let statusBar (items:StatusItem list) =
//        StatusBar(items |> List.toArray)

//    ///// able to change the color scheme of the status bar
//    //let styledStatusBar (props:Prop<'TValue> list) (items:StatusItem list) =
//    //    StatusBar(items |> List.toArray)
//    //    |> addPossibleStylesFromProps props 


//    let statusItem text (key:Terminal.Gui.Key) action =
//        if key = Key.F9 then
//            invalidArg "key" ("F9 is reserved to open a menu, sorry.")
//        else
//            StatusItem(key,text |> ustr, Action(action))

    


    

    