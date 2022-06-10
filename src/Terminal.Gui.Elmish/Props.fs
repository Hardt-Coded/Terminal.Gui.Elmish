namespace Terminal.Gui.Elmish

open Terminal.Gui
open Terminal.Gui.Elmish.Elements
open System



type style =
    static member inline color (colorscheme:ColorScheme) = Interop.mkstyle "color" colorscheme


type prop =
    static member inline style (children:IStyle list) = Interop.mkprop "style" children
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children

    static member inline text (text:string) = Interop.mkprop "text" text

    static member inline autoSize (value:bool) = Interop.mkprop "autoSize" value
    static member inline tabIndex (i:int) = Interop.mkprop "tabIndex" i
    static member inline tabStop (value:bool) = Interop.mkprop "tabStop" value

    static member inline enabled = Interop.mkprop "enabled" true
    static member inline disabled = Interop.mkprop "enabled" false

    // events
    static member inline onEnabledChanged   (f:unit->unit)                                  = Interop.mkprop "onEnabledChanged" f
    static member inline onEnter            (f:Terminal.Gui.View.FocusEventArgs->unit)      = Interop.mkprop "onEnter" f
    static member inline onKeyDown          (f:Terminal.Gui.View.KeyEventEventArgs->unit)   = Interop.mkprop "onKeyDown" f
    static member inline onKeyPress         (f:Terminal.Gui.View.KeyEventEventArgs->unit)   = Interop.mkprop "onKeyPress" f
    static member inline onKeyUp            (f:Terminal.Gui.View.KeyEventEventArgs->unit)   = Interop.mkprop "onKeyUp" f
    static member inline onLeave            (f:Terminal.Gui.View.FocusEventArgs->unit)      = Interop.mkprop "onLeave" f
    static member inline onMouseClick       (f:Terminal.Gui.View.MouseEventArgs->unit)      = Interop.mkprop "onMouseClick" f
    static member inline onMouseEnter       (f:Terminal.Gui.View.MouseEventArgs->unit)      = Interop.mkprop "onMouseEnter" f
    static member inline onMouseLeave       (f:Terminal.Gui.View.MouseEventArgs->unit)      = Interop.mkprop "onMouseLeave" f
    static member inline onVisibleChanged   (f:unit->unit)                                  = Interop.mkprop "onVisibleChanged" f

module prop =

    module position =

        type x =
            static member inline at (i:int) = Interop.mkprop "x" (Pos.At i)
            static member inline bottom = Interop.mkprop "x" Pos.Bottom
            static member inline center = Interop.mkprop "x" Pos.Center
            static member inline percent (i:double) = Interop.mkprop "x" (Pos.Percent (i |> float32))

        type y =
            static member inline at (i:int) = Interop.mkprop "y" (Pos.At i)
            static member inline bottom = Interop.mkprop "y" Pos.Bottom
            static member inline center = Interop.mkprop "y" Pos.Center
            static member inline percent (i:double) = Interop.mkprop "y" (Pos.Percent (i |> float32))

    
    type width =
        static member inline sized (i:int) = Interop.mkprop "width" (Dim.Sized i)
        static member inline filled = Interop.mkprop "width" (Dim.Fill 0)
        static member inline fill (margin:int) = Interop.mkprop "width" (Dim.Fill margin)


    type height =
        static member inline sized (i:int) = Interop.mkprop "height" (Dim.Sized i)
        static member inline filled = Interop.mkprop "height" (Dim.Fill 0)
        static member inline fill (margin:int) = Interop.mkprop "height" (Dim.Fill margin)


    type textAlignment =
        static member inline centered =     Interop.mkprop "textAlignment" TextAlignment.Centered
        static member inline left =         Interop.mkprop "textAlignment" TextAlignment.Left
        static member inline right =        Interop.mkprop "textAlignment" TextAlignment.Right
        static member inline justified =    Interop.mkprop "textAlignment" TextAlignment.Justified

    type textDirection =
        static member inline bottomTop_leftRight = Interop.mkprop "textDirection" TextDirection.BottomTop_LeftRight
        static member inline bottomTop_rightLeft = Interop.mkprop "textDirection" TextDirection.BottomTop_RightLeft
        static member inline leftRight_bottomTop = Interop.mkprop "textDirection" TextDirection.LeftRight_BottomTop
        static member inline leftRight_topBottom = Interop.mkprop "textDirection" TextDirection.LeftRight_TopBottom
        static member inline rightLeft_bottomTop = Interop.mkprop "textDirection" TextDirection.RightLeft_BottomTop
        static member inline rightLeft_topBottom = Interop.mkprop "textDirection" TextDirection.RightLeft_TopBottom
        static member inline topBottom_leftRight = Interop.mkprop "textDirection" TextDirection.TopBottom_LeftRight
        static member inline topBottom_rightLeft = Interop.mkprop "textDirection" TextDirection.TopBottom_RightLeft

    type verticalTextAlign =
        static member inline bottom =       Interop.mkprop "textAlignment" VerticalTextAlignment.Bottom
        static member inline justified =    Interop.mkprop "textAlignment" VerticalTextAlignment.Justified
        static member inline middle =       Interop.mkprop "textAlignment" VerticalTextAlignment.Middle
        static member inline top =          Interop.mkprop "textAlignment" VerticalTextAlignment.Top
        

type page =
    static member inline menubar (props:IMenuBarProperty list) = Interop.mkprop "menubar" props

type window =
    static member inline title (p:string) = Interop.mkprop "title" p
    static member inline effect3D = Interop.mkprop "effect3D" true

module window =
    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" BorderStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" BorderStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" BorderStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" BorderStyle.Single

type button =
    static member inline onClick (f:unit->unit) = Interop.mkprop "onClick" f


type checkbox =
    static member inline onToggled (f:bool->unit) = Interop.mkprop "toggled" f
    static member inline isChecked (v:bool) = Interop.mkprop "checked" v


type colorpicker =
    static member inline selectedColor (color:Terminal.Gui.Color) = Interop.mkprop "selectedColor" color
    static member inline onColorChanged (f:Terminal.Gui.Color->unit) = Interop.mkprop "onColorChanged" f

type combobox =
    static member inline selectedItem (index:int) = Interop.mkprop "selectedItem" index
    static member inline onOpenSelectedItem (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onOpenSelectedItem" f
    static member inline onSelectedItemChanged (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onSelectedItemChanged" f
    static member inline source (items:string list) = Interop.mkprop "source" items
    static member inline readonly (value:bool) = Interop.mkprop "readonly" value


type datefield =
    static member inline date (date:DateTime) = Interop.mkprop "date" date
    static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onDateChanged (f:Terminal.Gui.DateTimeEventArgs<DateTime>->unit) = Interop.mkprop "onDateChanged" f


type timefield =
    static member inline time (time:TimeSpan) = Interop.mkprop "time" time
    static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onTimeChanged (f:Terminal.Gui.DateTimeEventArgs<TimeSpan>->unit) = Interop.mkprop "onTimeChanged" f

  
type frameview =
    static member inline effect3D = Interop.mkprop "effect3D" true

module frameview =
    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" BorderStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" BorderStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" BorderStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" BorderStyle.Single


type graphview =
    static member inline series (series: Graphs.ISeries list)           = Interop.mkprop "series" series
    static member inline scrollOffset(value:PointF)                     = Interop.mkprop "scrollOffset" value
    static member inline marginLeft  (value:uint32)                     = Interop.mkprop "marginLeft" value
    static member inline marginBottom(value:uint32)                     = Interop.mkprop "marginBottom" value
    static member inline graphColor  (value:Attribute option)           = Interop.mkprop "graphColor" value
    static member inline cellSize    (value:PointF)                     = Interop.mkprop "cellSize" value
    static member inline axisY       (value:Graphs.VerticalAxis)        = Interop.mkprop "axisY" value
    static member inline axisX       (value:Graphs.HorizontalAxis)      = Interop.mkprop "axisX" value
    static member inline annotations (value:Graphs.IAnnotation list)    = Interop.mkprop "annotations" value


type hexview =
    static member inline source (value: System.IO.Stream)               = Interop.mkprop "source" value
    static member inline displayStart (value:int64)                     = Interop.mkprop "displayStart" value
    static member inline allowEdits  (value:bool)                       = Interop.mkprop "allowEdits" value
    static member inline onEdited (value:System.Collections.Generic.KeyValuePair<int64,byte>->unit)  = Interop.mkprop "cellSize" value
    static member inline onPositionChanged (value:HexView.HexViewEventArgs->unit)        = Interop.mkprop "axisY" value


type lineview =
    static member inline startingAnchor (value:System.Rune option)  = Interop.mkprop "startingAnchor" value
    static member inline endingAnchor   (value:System.Rune option)  = Interop.mkprop "endingAnchor" value
    static member inline lineRune       (value:System.Rune)         = Interop.mkprop "lineRune" value
    static member inline orientation    (value:Graphs.Orientation)  = Interop.mkprop "orientation" value


type listview =
    static member inline selectedItem (index:int) = Interop.mkprop "selectedItem" index
    static member inline onOpenSelectedItem (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onOpenSelectedItem" f
    static member inline onSelectedItemChanged (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onSelectedItemChanged" f
    static member inline onRowRender (f:Terminal.Gui.ListViewRowEventArgs->unit) = Interop.mkprop "onRowRender" f
    static member inline source (items:string list) = Interop.mkprop "source" items
    static member inline allowsMarking (value:bool) = Interop.mkprop "allowsMarking" value
    static member inline allowsMultipleSelection (value:bool) = Interop.mkprop "allowsMultipleSelection" value
    static member inline leftItem (value:int) = Interop.mkprop "leftItem" value
    static member inline topItem (value:int) = Interop.mkprop "topItem" value


type menubar =
    static member inline menus (menus:IMenuBarItem list) =                              Interop.mkMenuBarProp "menus" menus
    static member inline useKeysUpDownAsKeysLeftRight (value:bool) =                    Interop.mkMenuBarProp "useKeysUpDownAsKeysLeftRight" value
    static member inline useSubMenusSingleFrame (value:bool) =                          Interop.mkMenuBarProp "useSubMenusSingleFrame" value
    static member inline onMenuAllClosed (f:unit->unit) =                               Interop.mkMenuBarProp "onMenuAllClosed" f    
    static member inline onMenuClosing (f:Terminal.Gui.MenuClosingEventArgs->unit) =    Interop.mkMenuBarProp "onMenuClosing" f    
    static member inline onMenuOpened (f:Terminal.Gui.MenuItem->unit) =                 Interop.mkMenuBarProp "onMenuOpened" f    
    static member inline onMenuOpening (f:Terminal.Gui.MenuOpeningEventArgs->unit) =    Interop.mkMenuBarProp "onMenuOpening" f

type menu =
    static member inline menuBarItem (props:IMenuProperty list) =   (KeyValue ("menubar", props)) :> IMenuBarItem // MenuElements.MenuBarItemElement(props) :> IMenuBarItem
    static member inline submenuItem (props:IMenuProperty list) =   (KeyValue ("submenuItem", props)) :> IMenu
    static member inline menuItem (props:IMenuProperty list) =      (KeyValue ("menuItem", props)) :> IMenu
    static member inline menuItem (title:string, action:unit->unit) = 
        let props = [
            Interop.mkMenuProp "title" title
            Interop.mkMenuProp "action" action
        ]
        (KeyValue ("menuItem", props)) :> IMenu

module menu =
    type prop =
        static member inline children (children: IMenu list) = Interop.mkMenuProp "children" children
        static member inline title (title:string) = Interop.mkMenuProp "title" title
        

    type item =
        static member inline action (action:unit->unit) = Interop.mkMenuProp "action" action
        static member inline isChecked (b:bool) = Interop.mkMenuProp "checked" b
        static member inline shortcut (key:Key) = Interop.mkMenuProp "shortcut" key

    module item =
        
        type itemstyle =
            static member inline noCheck = Interop.mkMenuProp "itemstyle" MenuItemCheckStyle.NoCheck
            static member inline check = Interop.mkMenuProp "itemstyle" MenuItemCheckStyle.Checked
            static member inline radio = Interop.mkMenuProp "itemstyle" MenuItemCheckStyle.Radio
            
    

        
    
    
    
