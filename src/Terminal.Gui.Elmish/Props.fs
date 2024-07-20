namespace Terminal.Gui.Elmish

open System.Drawing
open Terminal.Gui
open Terminal.Gui.Elmish.Elements
open System
open System.Data



type prop =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline ref (reference:View->unit) = Interop.mkprop "ref" reference

    static member inline title (title:string) = Interop.mkprop "title" title

    static member inline tabIndex (i:int) = Interop.mkprop "tabIndex" i
    static member inline tabStop (value:bool) = Interop.mkprop "tabStop" value

    static member inline enabled = Interop.mkprop "enabled" true
    static member inline disabled = Interop.mkprop "enabled" false

    static member inline colorScheme    (colorscheme:ColorScheme)           = Interop.mkprop "colorScheme" colorscheme
    static member inline colorDisabled  (forground:Color,background:Color)  = Interop.mkprop "colorDisabled"    <| Terminal.Gui.Attribute(&forground,&background)
    static member inline colorFocus     (forground:Color,background:Color)  = Interop.mkprop "colorFocus"       <| Terminal.Gui.Attribute(&forground,&background)
    static member inline colorHotFocus  (forground:Color,background:Color)  = Interop.mkprop "colorHotFocus"    <| Terminal.Gui.Attribute(&forground,&background)
    static member inline colorHotNormal (forground:Color,background:Color)  = Interop.mkprop "colorHotNormal"   <| Terminal.Gui.Attribute(&forground,&background)
    static member inline colorNormal    (forground:Color,background:Color)  = Interop.mkprop "colorNormal"      <| Terminal.Gui.Attribute(&forground,&background)
    static member inline color          (forground:Color,background:Color)  = Interop.mkprop "color"            <| Terminal.Gui.Attribute(&forground,&background)

    // events
    static member inline onEnabledChanged   (f:unit->unit)                      = Interop.mkprop "onEnabledChanged" f
    static member inline onEnter            (f:FocusEventArgs->unit)            = Interop.mkprop "onEnter" f
    static member inline onKeyDown          (f:Key->unit)                       = Interop.mkprop "onKeyDown" f
    static member inline onKeyPress         (f:Key->unit)                       = Interop.mkprop "onKeyPress" f
    static member inline onKeyUp            (f:Key->unit)                       = Interop.mkprop "onKeyUp" f
    static member inline onLeave            (f:FocusEventArgs->unit)            = Interop.mkprop "onLeave" f
    static member inline onMouseClick       (f:MouseEventEventArgs->unit)       = Interop.mkprop "onMouseClick" f
    static member inline onMouseEnter       (f:MouseEventEventArgs->unit)       = Interop.mkprop "onMouseEnter" f
    static member inline onMouseLeave       (f:MouseEventEventArgs->unit)       = Interop.mkprop "onMouseLeave" f
    static member inline onVisibleChanged   (f:unit->unit)                      = Interop.mkprop "onVisibleChanged" f

module prop =

    module position =

        type x =
            static member inline absolute (i:int) = Interop.mkprop "x" (Pos.Absolute i)
            //static member inline bottom = Interop.mkprop "x" (Pos.Bottom())
            static member inline center = Interop.mkprop "x" (Pos.Center())
            static member inline percent (i:int) = Interop.mkprop "x" (Pos.Percent i)

        type y =
            static member inline absolute (i:int) = Interop.mkprop "y" (Pos.Absolute i)
            //static member inline bottom = Interop.mkprop "y" (Pos.Bottom())
            static member inline center = Interop.mkprop "y" (Pos.Center())
            static member inline percent (i:int) = Interop.mkprop "y" (Pos.Percent i)

    
    type width =
        static member inline absolute (i:int) = Interop.mkprop "width" (Dim.Absolute i)
        static member inline filled = Interop.mkprop "width" (Dim.Fill 0)
        static member inline fill (margin:int) = Interop.mkprop "width" (Dim.Fill margin)
        static member inline percent (percent:int) = Interop.mkprop "width" (Dim.Percent percent)
        static member inline auto = Interop.mkprop "auto" (Dim.Auto())


    type height =
        static member inline absolute (i:int) = Interop.mkprop "height" (Dim.Absolute i)
        static member inline filled = Interop.mkprop "height" (Dim.Fill 0)
        static member inline fill (margin:int) = Interop.mkprop "height" (Dim.Fill margin)
        static member inline percent (percent:int) = Interop.mkprop "height" (Dim.Percent percent)
        static member inline auto = Interop.mkprop "auto" (Dim.Auto()) // Todo: more options

    
    
    type alignment =
        static member inline center     =   Interop.mkprop "alignment" Alignment.Center
        static member inline ``end``    =   Interop.mkprop "alignment" Alignment.End
        static member inline start      =   Interop.mkprop "alignment" Alignment.Start
        static member inline fill       =   Interop.mkprop "alignment" Alignment.Fill

    type textDirection =
        static member inline bottomTop_leftRight = Interop.mkprop "textDirection" TextDirection.BottomTop_LeftRight
        static member inline bottomTop_rightLeft = Interop.mkprop "textDirection" TextDirection.BottomTop_RightLeft
        static member inline leftRight_bottomTop = Interop.mkprop "textDirection" TextDirection.LeftRight_BottomTop
        static member inline leftRight_topBottom = Interop.mkprop "textDirection" TextDirection.LeftRight_TopBottom
        static member inline rightLeft_bottomTop = Interop.mkprop "textDirection" TextDirection.RightLeft_BottomTop
        static member inline rightLeft_topBottom = Interop.mkprop "textDirection" TextDirection.RightLeft_TopBottom
        static member inline topBottom_leftRight = Interop.mkprop "textDirection" TextDirection.TopBottom_LeftRight

    
        

type page =
    
    static member inline running                        (value:bool)  = Interop.mkprop "running" value
    static member inline onLoaded                       (value:unit->unit)  = Interop.mkprop "onLoaded" value
    static member inline onReady                        (value:unit->unit)  = Interop.mkprop "onReady" value
    static member inline onUnloaded                     (value:unit->unit)  = Interop.mkprop "onUnloaded" value
    static member inline onActivate                     (value:ToplevelEventArgs->unit)  = Interop.mkprop "onActivate" value
    static member inline onDeactivate                   (value:ToplevelEventArgs->unit)  = Interop.mkprop "onDeactivate" value
    static member inline onChildClosed                  (value:ToplevelEventArgs->unit)  = Interop.mkprop "onChildClosed" value
    static member inline onAllChildClosed               (value:unit->unit)  = Interop.mkprop "onAllChildClosed" value
    static member inline onClosing                      (value:ToplevelClosingEventArgs->unit)  = Interop.mkprop "onClosing" value
    static member inline onClosed                       (value:ToplevelEventArgs->unit)  = Interop.mkprop "onClosed" value
    static member inline onChildLoaded                  (value:ToplevelEventArgs->unit)  = Interop.mkprop "onChildLoaded" value
    static member inline onChildUnloaded                (value:ToplevelEventArgs->unit)  = Interop.mkprop "onChildUnloaded" value
    static member inline onAlternateForwardKeyChanged   (value:KeyChangedEventArgs->unit)  = Interop.mkprop "onAlternateForwardKeyChanged" value
    static member inline onAlternateBackwardKeyChanged  (value:KeyChangedEventArgs->unit)  = Interop.mkprop "onAlternateBackwardKeyChanged" value
    static member inline onQuitKeyChanged               (value:KeyChangedEventArgs->unit)  = Interop.mkprop "onQuitKeyChanged" value
    static member inline modal                          (value:bool)  = Interop.mkprop "modal" value

    static member inline menuBar (props:IMenuBarProperty list) = Interop.mkprop "menuBar" props
    static member inline statusBar (value:StatusBar)  = Interop.mkprop "statusBar" value

    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children

    

type window =
    static member inline title (p:string) = Interop.mkprop "title" p
    static member inline text (p:string) = Interop.mkprop "text" p
    static member inline effect3D = Interop.mkprop "effect3D" true
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children

module window =
    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" LineStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" LineStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" LineStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" LineStyle.Single

    type shadowStyle =
        static member inline none = Interop.mkprop          "shadowStyle" ShadowStyle.None
        static member inline opaque = Interop.mkprop        "shadowStyle" ShadowStyle.Opaque
        static member inline transparent = Interop.mkprop   "shadowStyle" ShadowStyle.Transparent

type button =
    /// raises when button is clicked or space, enter, hotkey is used
    static member inline onAccept (f:unit->unit) = Interop.mkprop "onAccept" f
    static member inline text (value:string)  = Interop.mkprop "text" value
    static member inline isDefault (value:bool)  = Interop.mkprop "isDefault" value
    static member inline hotKey (value:Key)  = Interop.mkprop "hotKey" value
    static member inline hotKeySpecifier (value:Text.Rune)  = Interop.mkprop "hotKeySpecifier" value
    static member inline autoSize (value:bool)  = Interop.mkprop "autoSize" value


type checkBox =
    static member inline onToggle (f:CancelEventArgs<CheckState>->unit) = Interop.mkprop "onToggle" f
    static member inline onToggled (f:bool->unit) = Interop.mkprop "onToggledBool" f
    static member inline state (v:CheckState) = Interop.mkprop "checkState" v
    static member inline isChecked (v:bool) = Interop.mkprop "isChecked" v
    static member inline text (text:string) = Interop.mkprop "text" text


type colorPicker =
    static member inline title (title:string) = Interop.mkprop "title" title
    static member inline selectedColor (color:ColorName) = Interop.mkprop "selectedColor" color
    static member inline onColorChanged (f:ColorEventArgs->unit) = Interop.mkprop "onColorChanged" f

type comboBox =
    static member inline selectedItem (index:int) = Interop.mkprop "selectedItem" index
    static member inline onOpenSelectedItem (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onOpenSelectedItem" f
    static member inline onSelectedItemChanged (f:Terminal.Gui.ListViewItemEventArgs->unit) = Interop.mkprop "onSelectedItemChanged" f
    static member inline source (items:string list) = Interop.mkprop "source" items
    static member inline readonly (value:bool) = Interop.mkprop "readonly" value
    static member inline dropdownHeight (value:int) = Interop.mkprop "dropdownHeight" value
    static member inline text (text:string) = Interop.mkprop "text" text


type dateField =
    static member inline date (date:DateTime) = Interop.mkprop "date" date
    // Todo: removed?
    //static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onDateChanged (f:Terminal.Gui.DateTimeEventArgs<DateTime>->unit) = Interop.mkprop "onDateChanged" f


type timeField =
    static member inline time (time:TimeSpan) = Interop.mkprop "time" time
    static member inline isShortFormat (b:bool) = Interop.mkprop "isShortFormat" b
    static member inline onTimeChanged (f:Terminal.Gui.DateTimeEventArgs<TimeSpan>->unit) = Interop.mkprop "onTimeChanged" f

  
type frameView =
    static member inline title (value:string)  = Interop.mkprop "title" value
    static member inline text (value:string)  = Interop.mkprop "text" value
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children

module frameView =
    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" LineStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" LineStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" LineStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" LineStyle.Single
        
   type shadowStyle =
        static member inline none = Interop.mkprop          "shadowStyle" ShadowStyle.None
        static member inline opaque = Interop.mkprop        "shadowStyle" ShadowStyle.Opaque
        static member inline transparent = Interop.mkprop   "shadowStyle" ShadowStyle.Transparent

    

type graphView =
    static member inline series (series: ISeries list)           = Interop.mkprop "series" series
    static member inline scrollOffset(value:PointF)                     = Interop.mkprop "scrollOffset" value
    static member inline marginLeft  (value:uint32)                     = Interop.mkprop "marginLeft" value
    static member inline marginBottom(value:uint32)                     = Interop.mkprop "marginBottom" value
    static member inline graphColor  (value:Attribute option)           = Interop.mkprop "graphColor" value
    static member inline cellSize    (value:PointF)                     = Interop.mkprop "cellSize" value
    static member inline axisY       (value:VerticalAxis)        = Interop.mkprop "axisY" value
    static member inline axisX       (value:HorizontalAxis)      = Interop.mkprop "axisX" value


type hexView =
    static member inline source (value: System.IO.Stream)               = Interop.mkprop "source" value
    static member inline displayStart (value:int64)                     = Interop.mkprop "displayStart" value
    static member inline allowEdits  (value:bool)                       = Interop.mkprop "allowEdits" value
    static member inline onEdited (value:HexViewEditEventArgs->unit)  = Interop.mkprop "cellSize" value
    static member inline onPositionChanged (value:HexViewEventArgs->unit)        = Interop.mkprop "axisY" value


type label =
    static member inline text (value:string)  = Interop.mkprop "text" value

type lineView =
    static member inline startingAnchor (value:System.Text.Rune option) = Interop.mkprop "startingAnchor" value
    static member inline endingAnchor   (value:System.Text.Rune option) = Interop.mkprop "endingAnchor" value
    static member inline lineRune       (value:System.Text.Rune)        = Interop.mkprop "lineRune" value
    static member inline orientation    (value:Orientation)             = Interop.mkprop "orientation" value


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
    static member inline onMenuOpened (f:Terminal.Gui.MenuOpenedEventArgs->unit)   =    Interop.mkMenuBarProp "onMenuOpened" f    
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
        static member inline shortcut (key:KeyCode) = Interop.mkMenuProp "shortcut" key

    module item =
        
        type itemstyle =
            static member inline noCheck = Interop.mkMenuProp "itemstyle" MenuItemCheckStyle.NoCheck
            static member inline check = Interop.mkMenuProp "itemstyle" MenuItemCheckStyle.Checked
            static member inline radio = Interop.mkMenuProp "itemstyle" MenuItemCheckStyle.Radio

(*
type panelView =
    static member inline child (child:TerminalElement) = Interop.mkprop "child" child
    static member inline usePanelFrame (b:bool) = Interop.mkprop "usePanelFrame" b
    static member inline effect3D = Interop.mkprop "effect3D" true

module panelView =
    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" LineStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" LineStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" LineStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" LineStyle.Single
        *)

type progressBar =
    static member inline text (value:string)  = Interop.mkprop "text" value
    static member inline bidirectionalMarquee (value:bool)  = Interop.mkprop "bidirectionalMarquee" value
    static member inline fraction (value:double)  = Interop.mkprop "fraction" value
    static member inline segmentCharacter (value:System.Text.Rune)  = Interop.mkprop "segmentCharacter" value

module progressBar =
    
    type format =
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
        static member inline horizontal = Interop.mkprop "orientation" Orientation.Horizontal
        static member inline vertical = Interop.mkprop "orientation" Orientation.Vertical

//type scrollbarview =
//    static member inline autoHideScrollBars (value:bool)  = Interop.mkprop "autoHideScrollBars" value
//    static member inline isVertical (value:bool)  = Interop.mkprop "isVertical" value
//    static member inline keepContentAlwaysInViewport (value:bool)  = Interop.mkprop "keepContentAlwaysInViewport" value
//    static member inline showScrollIndicator (value:bool)  = Interop.mkprop "showScrollIndicator" value
//    static member inline position (value:int)  = Interop.mkprop "position" value
//    static member inline size (value:int)  = Interop.mkprop "size" value
//    static member inline onChangedPosition (f:unit->unit)  = Interop.mkprop "onChangedPosition" f


type scrollView =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline autoHideScrollBars (value:bool)  = Interop.mkprop "autoHideScrollBars" value
    static member inline keepContentAlwaysInViewport (value:bool)  = Interop.mkprop "keepContentAlwaysInViewport" value
    static member inline showVerticalScrollIndicator (value:bool)  = Interop.mkprop "showVerticalScrollIndicator" value
    static member inline showHorizontalScrollIndicator (value:bool)  = Interop.mkprop "showHorizontalScrollIndicator" value
    static member inline contentSize (value:Size)  = Interop.mkprop "contentSize" value
    static member inline contentOffset (value:Point)  = Interop.mkprop "contentOffset" value


type tableView =
    static member inline table (value:DataTable)  = Interop.mkprop "table" value
    static member inline listTableSource (value:ListTableSource)  = Interop.mkprop "listTableSource" value
    static member inline enumTableSource (value:EnumerableTableSource<_>)  = Interop.mkprop "enumTableSource" value
    static member inline style (value:TableStyle)  = Interop.mkprop "style" value
    static member inline fullRowSelect (value:bool)  = Interop.mkprop "fullRowSelect" value
    static member inline multiSelect (value:bool)  = Interop.mkprop "multiSelect" value
    static member inline columnOffset (value:int)  = Interop.mkprop "columnOffset" value
    static member inline rowOffset (value:int)  = Interop.mkprop "rowOffset" value
    static member inline selectedColumn (value:int)  = Interop.mkprop "selectedColumn" value
    static member inline selectedRow (value:int)  = Interop.mkprop "selectedRow" value
    static member inline maxCellWidth (value:int)  = Interop.mkprop "maxCellWidth" value
    static member inline nullSymbol (value:String)  = Interop.mkprop "nullSymbol" value
    static member inline separatorSymbol (value:Char)  = Interop.mkprop "separatorSymbol" value
    static member inline onSelectedCellChanged (value:CellActivatedEventArgs->unit)  = Interop.mkprop "onSelectedCellChanged" value
    static member inline onCellActivated (value:CellActivatedEventArgs->unit)  = Interop.mkprop "onCellActivated" value
    static member inline cellActivationKey (value:KeyCode)  = Interop.mkprop "cellActivationKey" value


type tabView =
    static member inline tabScrollOffset (value:int)  = Interop.mkprop "tabScrollOffset" value
    static member inline maxTabTextWidth (value:UInt32)  = Interop.mkprop "maxTabTextWidth" value
    static member inline onSelectedTabChanged (value:TabChangedEventArgs->unit)  = Interop.mkprop "onSelectedTabChanged" value
    static member inline selectedTab (value:Tab)  = Interop.mkprop "selectedTab" value
    static member inline style (value:TabStyle)  = Interop.mkprop "style" value
    static member inline tabs (value: ITabProperty list) = Interop.mkprop "tabs" value

type tab =
    static member create (value: ITabItemProperty list) = Interop.mkTabProp "tabItems" value

type tabItem =
    static member inline title (value: string) = Interop.mkTabItemProp "title" value
    static member inline view (value: TerminalElement) = Interop.mkTabItemProp "view" value

type textField =
    static member inline used (value:bool)  = Interop.mkprop "used" value
    static member inline readOnly (value:bool)  = Interop.mkprop "readOnly" value
    static member inline onTextChanging (value:CancelEventArgs<string>->unit)  = Interop.mkprop "onTextChanging" value
    static member inline onTextChanging (value:string->unit)  = Interop.mkprop "onTextChangingString" value
    /// this event is triggert when the text is changed. In the event parameter is the old text. use textChanging instead, if you need the new text
    static member inline onTextChanged (value:string->unit)  = Interop.mkprop "onTextChanged" value
    static member inline frame (value:Rectangle)  = Interop.mkprop "frame" value
    static member inline text (value:string)  = Interop.mkprop "text" value
    static member inline secret = Interop.mkprop "secret" true
    static member inline cursorPosition (value:int)  = Interop.mkprop "cursorPosition" value
    static member inline canFocus (value:bool)  = Interop.mkprop "canFocus" value
    static member inline selectedStart (value:int)  = Interop.mkprop "selectedStart" value
    static member inline cursorVisibility (value:CursorVisibility)  = Interop.mkprop "cursorVisibility" value


type textValidateField =
    static member inline provider (value:TextValidateProviders.ITextValidateProvider)  = Interop.mkprop "provider" value
    static member inline text (value:string)  = Interop.mkprop "text" value

type textView =
    static member inline canFocus (value:bool)  = Interop.mkprop "canFocus" value
    static member inline onTextChanged (value:string->unit)  = Interop.mkprop "onTextChanged" value
    static member inline used (value:bool)  = Interop.mkprop "used" value
    static member inline text (value:string)  = Interop.mkprop "text" value
    static member inline frame (value:Rectangle)  = Interop.mkprop "frame" value
    static member inline topRow (value:int)  = Interop.mkprop "topRow" value
    static member inline leftColumn (value:int)  = Interop.mkprop "leftColumn" value
    static member inline cursorPosition (value:Point)  = Interop.mkprop "cursorPosition" value
    static member inline selectionStartColumn (value:int)  = Interop.mkprop "selectionStartColumn" value
    static member inline selectionStartRow (value:int)  = Interop.mkprop "selectionStartRow" value
    static member inline selecting (value:bool)  = Interop.mkprop "selecting" value
    static member inline wordWrap (value:bool)  = Interop.mkprop "wordWrap" value
    static member inline allowsReturn (value:bool)  = Interop.mkprop "allowsReturn" value
    static member inline allowsTab (value:bool)  = Interop.mkprop "allowsTab" value
    static member inline tabWidth (value:int)  = Interop.mkprop "tabWidth" value
    static member inline multiline (value:bool)  = Interop.mkprop "multiline" value
    static member inline readOnly (value:bool)  = Interop.mkprop "readOnly" value
    static member inline cursorVisibility (value:CursorVisibility)  = Interop.mkprop "cursorVisibility" value


type treeView =
    static member inline treeBuilder (value:ITreeBuilder<ITreeNode>)                = Interop.mkprop "treeBuilder" value
    static member inline style (value:TreeStyle)                                    = Interop.mkprop "style" value
    static member inline multiSelect (value:bool)                                   = Interop.mkprop "multiSelect" value
    static member inline allowLetterBasedNavigation (value:bool)                    = Interop.mkprop "allowLetterBasedNavigation" value
    static member inline selectedObject (value:ITreeNode)                           = Interop.mkprop "selectedObject" value
    static member inline onObjectActivated (value:ObjectActivatedEventArgs<ITreeNode>->unit)  = Interop.mkprop "onObjectActivated" value
    static member inline objectActivationKey (value:KeyCode)                        = Interop.mkprop "objectActivationKey" value
    static member inline objectActivationButton (value:Nullable<MouseFlags>)        = Interop.mkprop "objectActivationButton" value
    static member inline colorGetter (value:ITreeNode->ColorScheme)                 = Interop.mkprop "colorGetter" value
    static member inline onSelectionChanged (value:SelectionChangedEventArgs<ITreeNode>->unit)  = Interop.mkprop "onSelectionChanged" value
    static member inline scrollOffsetVertical (value:int)                           = Interop.mkprop "scrollOffsetVertical" value
    static member inline scrollOffsetHorizontal (value:int)                         = Interop.mkprop "scrollOffsetHorizontal" value
    static member inline aspectGetter (value:AspectGetterDelegate<ITreeNode>)       = Interop.mkprop "aspectGetter" value
    static member inline cursorVisibility (value:CursorVisibility)                  = Interop.mkprop "cursorVisibility" value
    static member inline items (value: string list)                                 = Interop.mkprop "items" value
    static member inline nodes (value: ITreeNode list)                              = Interop.mkprop "nodes" value


