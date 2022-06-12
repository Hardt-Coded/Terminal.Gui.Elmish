namespace Terminal.Gui.Elmish

open Terminal.Gui
open Terminal.Gui.Elmish.Elements
open System
open System.Data


type prop =
    static member inline style (children:IStyle list) = Interop.mkprop "style" children
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children

    static member inline text (text:string) = Interop.mkprop "text" text

    static member inline autoSize (value:bool) = Interop.mkprop "autoSize" value
    static member inline tabIndex (i:int) = Interop.mkprop "tabIndex" i
    static member inline tabStop (value:bool) = Interop.mkprop "tabStop" value

    static member inline enabled = Interop.mkprop "enabled" true
    static member inline disabled = Interop.mkprop "enabled" false

    static member inline color (colorscheme:ColorScheme) = Interop.mkprop "color" colorscheme

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
    
    static member inline running (value:bool)  = Interop.mkprop "running" value
    static member inline onLoaded (value:unit->unit)  = Interop.mkprop "onLoaded" value
    static member inline onReady (value:unit->unit)  = Interop.mkprop "onReady" value
    static member inline onUnloaded (value:unit->unit)  = Interop.mkprop "onUnloaded" value
    static member inline onActivate (value:unit->unit)  = Interop.mkprop "onActivate" value
    static member inline onDeactivate (value:unit->unit)  = Interop.mkprop "onDeactivate" value
    static member inline onChildClosed (value:unit->unit)  = Interop.mkprop "onChildClosed" value
    static member inline onAllChildClosed (value:unit->unit)  = Interop.mkprop "onAllChildClosed" value
    static member inline onClosing (value:unit->unit)  = Interop.mkprop "onClosing" value
    static member inline onClosed (value:unit->unit)  = Interop.mkprop "onClosed" value
    static member inline onChildLoaded (value:unit->unit)  = Interop.mkprop "onChildLoaded" value
    static member inline onChildUnloaded (value:unit->unit)  = Interop.mkprop "onChildUnloaded" value
    static member inline onResized (value:unit->unit)  = Interop.mkprop "onResized" value
    static member inline onAlternateForwardKeyChanged (value:unit->unit)  = Interop.mkprop "onAlternateForwardKeyChanged" value
    static member inline onAlternateBackwardKeyChanged (value:unit->unit)  = Interop.mkprop "onAlternateBackwardKeyChanged" value
    static member inline onQuitKeyChanged (value:unit->unit)  = Interop.mkprop "onQuitKeyChanged" value
    static member inline modal (value:bool)  = Interop.mkprop "modal" value
    static member inline isMdiContainer (value:bool)  = Interop.mkprop "isMdiContainer" value

    static member inline menuBar (props:IMenuBarProperty list) = Interop.mkprop "menuBar" props
    static member inline statusBar (value:StatusBar)  = Interop.mkprop "statusBar" value

    

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


type checkBox =
    static member inline onToggled (f:bool->unit) = Interop.mkprop "toggled" f
    static member inline isChecked (v:bool) = Interop.mkprop "checked" v


type colorPicker =
    static member inline selectedColor (color:Terminal.Gui.Color) = Interop.mkprop "selectedColor" color
    static member inline onColorChanged (f:Terminal.Gui.Color->unit) = Interop.mkprop "onColorChanged" f

type comboBox =
    static member inline selectedItem (index:int) = Interop.mkprop "selectedItem" index
    static member inline onOpenSelectedItem (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onOpenSelectedItem" f
    static member inline onSelectedItemChanged (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onSelectedItemChanged" f
    static member inline source (items:string list) = Interop.mkprop "source" items
    static member inline readonly (value:bool) = Interop.mkprop "readonly" value
    static member inline dropdownHeight (value:int) = Interop.mkprop "dropdownHeight" value


type dateField =
    static member inline date (date:DateTime) = Interop.mkprop "date" date
    static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onDateChanged (f:Terminal.Gui.DateTimeEventArgs<DateTime>->unit) = Interop.mkprop "onDateChanged" f


type timeField =
    static member inline time (time:TimeSpan) = Interop.mkprop "time" time
    static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onTimeChanged (f:Terminal.Gui.DateTimeEventArgs<TimeSpan>->unit) = Interop.mkprop "onTimeChanged" f

  
type frameView =
    static member inline effect3D = Interop.mkprop "effect3D" true

module frameView =
    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" BorderStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" BorderStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" BorderStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" BorderStyle.Single


type graphView =
    static member inline series (series: Graphs.ISeries list)           = Interop.mkprop "series" series
    static member inline scrollOffset(value:PointF)                     = Interop.mkprop "scrollOffset" value
    static member inline marginLeft  (value:uint32)                     = Interop.mkprop "marginLeft" value
    static member inline marginBottom(value:uint32)                     = Interop.mkprop "marginBottom" value
    static member inline graphColor  (value:Attribute option)           = Interop.mkprop "graphColor" value
    static member inline cellSize    (value:PointF)                     = Interop.mkprop "cellSize" value
    static member inline axisY       (value:Graphs.VerticalAxis)        = Interop.mkprop "axisY" value
    static member inline axisX       (value:Graphs.HorizontalAxis)      = Interop.mkprop "axisX" value
    static member inline annotations (value:Graphs.IAnnotation list)    = Interop.mkprop "annotations" value


type hexView =
    static member inline source (value: System.IO.Stream)               = Interop.mkprop "source" value
    static member inline displayStart (value:int64)                     = Interop.mkprop "displayStart" value
    static member inline allowEdits  (value:bool)                       = Interop.mkprop "allowEdits" value
    static member inline onEdited (value:System.Collections.Generic.KeyValuePair<int64,byte>->unit)  = Interop.mkprop "cellSize" value
    static member inline onPositionChanged (value:HexView.HexViewEventArgs->unit)        = Interop.mkprop "axisY" value


type lineView =
    static member inline startingAnchor (value:System.Rune option)  = Interop.mkprop "startingAnchor" value
    static member inline endingAnchor   (value:System.Rune option)  = Interop.mkprop "endingAnchor" value
    static member inline lineRune       (value:System.Rune)         = Interop.mkprop "lineRune" value
    static member inline orientation    (value:Graphs.Orientation)  = Interop.mkprop "orientation" value


type listView =
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

type panelView =
    static member inline child (child:TerminalElement) = Interop.mkprop "child" child
    static member inline usePanelFrame (b:bool) = Interop.mkprop "usePanelFrame" b
    static member inline effect3D = Interop.mkprop "effect3D" true

module panelView =
    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" BorderStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" BorderStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" BorderStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" BorderStyle.Single

type progressBar =
    static member inline bidirectionalMarquee (value:bool)  = Interop.mkprop "bidirectionalMarquee" value
    static member inline fraction (value:double)  = Interop.mkprop "fraction" value
    static member inline segmentCharacter (value:System.Rune)  = Interop.mkprop "segmentCharacter" value

module progressBar =
    
    type format =
        static member inline framed = Interop.mkprop "progressBarFormat" ProgressBarFormat.Framed
        static member inline framedPlusPercentage = Interop.mkprop "progressBarFormat" ProgressBarFormat.FramedPlusPercentage
        static member inline framedProgressPadded = Interop.mkprop "progressBarFormat" ProgressBarFormat.FramedProgressPadded
        static member inline simple = Interop.mkprop "progressBarFormat" ProgressBarFormat.Simple
        static member inline simplePlusPercentage = Interop.mkprop "progressBarFormat" ProgressBarFormat.SimplePlusPercentage

    type style =
        static member inline blocks = Interop.mkprop "progressBarStyle" ProgressBarStyle.Blocks
        static member inline continuous = Interop.mkprop "progressBarStyle" ProgressBarStyle.Continuous
        static member inline marqueeBlocks = Interop.mkprop "progressBarStyle" ProgressBarStyle.MarqueeBlocks
        static member inline marqueeContinuous = Interop.mkprop "progressBarStyle" ProgressBarStyle.MarqueeContinuous
            
type radioGroup =
    static member inline selectedItem (value:int)  = Interop.mkprop "selectedItem" value
    static member inline onSelectedItemChanged (f:SelectedItemChangedArgs->unit)  = Interop.mkprop "onSelectedItemChanged" f
    static member inline horizontalSpace (value:int)  = Interop.mkprop "horizontalSpace" value
    static member inline radioLabels (labels:#seq<string>)  = Interop.mkprop "radioLabels" labels

module radioGroup =
    
    type displayMode =
        static member inline horizontal = Interop.mkprop "displayMode" DisplayModeLayout.Horizontal
        static member inline vertical = Interop.mkprop "displayMode" DisplayModeLayout.Vertical

//type scrollbarview =
//    static member inline autoHideScrollBars (value:bool)  = Interop.mkprop "autoHideScrollBars" value
//    static member inline isVertical (value:bool)  = Interop.mkprop "isVertical" value
//    static member inline keepContentAlwaysInViewport (value:bool)  = Interop.mkprop "keepContentAlwaysInViewport" value
//    static member inline showScrollIndicator (value:bool)  = Interop.mkprop "showScrollIndicator" value
//    static member inline position (value:int)  = Interop.mkprop "position" value
//    static member inline size (value:int)  = Interop.mkprop "size" value
//    static member inline onChangedPosition (f:unit->unit)  = Interop.mkprop "onChangedPosition" f


type scrollView =
    static member inline autoHideScrollBars (value:bool)  = Interop.mkprop "autoHideScrollBars" value
    static member inline keepContentAlwaysInViewport (value:bool)  = Interop.mkprop "keepContentAlwaysInViewport" value
    static member inline showVerticalScrollIndicator (value:bool)  = Interop.mkprop "showVerticalScrollIndicator" value
    static member inline showHorizontalScrollIndicator (value:bool)  = Interop.mkprop "showHorizontalScrollIndicator" value
    static member inline contentSize (value:Size)  = Interop.mkprop "contentSize" value
    static member inline contentOffset (value:Point)  = Interop.mkprop "contentOffset" value


type tableView =
    static member inline table (value:DataTable)  = Interop.mkprop "table" value
    static member inline style (value:TableView.TableStyle)  = Interop.mkprop "style" value
    static member inline fullRowSelect (value:bool)  = Interop.mkprop "fullRowSelect" value
    static member inline multiSelect (value:bool)  = Interop.mkprop "multiSelect" value
    static member inline columnOffset (value:int)  = Interop.mkprop "columnOffset" value
    static member inline rowOffset (value:int)  = Interop.mkprop "rowOffset" value
    static member inline selectedColumn (value:int)  = Interop.mkprop "selectedColumn" value
    static member inline selectedRow (value:int)  = Interop.mkprop "selectedRow" value
    static member inline maxCellWidth (value:int)  = Interop.mkprop "maxCellWidth" value
    static member inline nullSymbol (value:String)  = Interop.mkprop "nullSymbol" value
    static member inline separatorSymbol (value:Char)  = Interop.mkprop "separatorSymbol" value
    static member inline onSelectedCellChanged (value:unit->unit)  = Interop.mkprop "onSelectedCellChanged" value
    static member inline onCellActivated (value:unit->unit)  = Interop.mkprop "onCellActivated" value
    static member inline cellActivationKey (value:Key)  = Interop.mkprop "cellActivationKey" value


type tabView =
    static member inline tabScrollOffset (value:int)  = Interop.mkprop "tabScrollOffset" value
    static member inline maxTabTextWidth (value:UInt32)  = Interop.mkprop "maxTabTextWidth" value
    static member inline onSelectedTabChanged (value:EventHandler<TabView.TabChangedEventArgs>)  = Interop.mkprop "onSelectedTabChanged" value
    static member inline selectedTab (value:TabView.Tab)  = Interop.mkprop "selectedTab" value
    static member inline style (value:TabView.TabStyle)  = Interop.mkprop "style" value


type textField =
    static member inline used (value:bool)  = Interop.mkprop "used" value
    static member inline readOnly (value:bool)  = Interop.mkprop "readOnly" value
    static member inline onTextChanging (value:TextChangingEventArgs->unit)  = Interop.mkprop "onTextChanging" value
    static member inline onTextChanging (value:string->unit)  = Interop.mkprop "onTextChangingString" value
    /// this event is triggert when the text is changed. In the event parameter is the old text. use textChanging instead, if you need the new text
    static member inline onTextChanged (value:string->unit)  = Interop.mkprop "onTextChanged" value
    static member inline frame (value:Rect)  = Interop.mkprop "frame" value
    static member inline text (value:string)  = Interop.mkprop "text" value
    static member inline secret = Interop.mkprop "secret" true
    static member inline cursorPosition (value:int)  = Interop.mkprop "cursorPosition" value
    static member inline canFocus (value:bool)  = Interop.mkprop "canFocus" value
    static member inline selectedStart (value:int)  = Interop.mkprop "selectedStart" value
    static member inline desiredCursorVisibility (value:CursorVisibility)  = Interop.mkprop "desiredCursorVisibility" value




        
    
    
    
