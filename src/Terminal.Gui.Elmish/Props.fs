namespace Terminal.Gui.Elmish

open Terminal.Gui
open System



type style =
    static member inline color (colorscheme:ColorScheme) = Interop.mkstyle "color" colorscheme


type prop =
    static member inline style (children:IStyle list) = Interop.mkprop "style" children
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children

    static member inline text (text:string) = Interop.mkprop "text" text

    static member inline autoSizeEnabled = Interop.mkprop "autoSize" true
    static member inline autoSizeDisabled = Interop.mkprop "autoSize" false
    static member inline tabIndex (i:int) = Interop.mkprop "tabIndex" i
    static member inline tabStop = Interop.mkprop "tabStop" true

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


type datefield =
    static member inline date (date:DateTime) = Interop.mkprop "date" date
    static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onDateChanged (f:Terminal.Gui.DateTimeEventArgs<DateTime>->unit) = Interop.mkprop "onDateChanged" f


type timefield =
    static member inline time (time:TimeSpan) = Interop.mkprop "time" time
    static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onTimeChanged (f:Terminal.Gui.DateTimeEventArgs<TimeSpan>->unit) = Interop.mkprop "onTimeChanged" f
    

