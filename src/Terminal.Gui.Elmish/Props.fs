namespace Terminal.Gui.Elmish

open System.ComponentModel
open System.Drawing
open System.Text
open Terminal.Gui
open Terminal.Gui.Elmish.Elements
open System
open System.Data



type prop =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline ref (reference:View->unit) = Interop.mkprop "ref" reference

    
    // View
    static member inline accept (handler: EventHandler<HandledEventArgs>) = Interop.mkprop "accept" handler
    static member inline data (value: obj) = Interop.mkprop "data" value
    static member inline id (value: string) = Interop.mkprop "id" value
    static member inline initialized (handler: EventHandler) = Interop.mkprop "initialized" handler
    static member inline enabled (value: bool) = Interop.mkprop "enabled" value
    static member inline enabledChanged (handler: EventHandler) = Interop.mkprop "enabledChanged" handler
    static member inline visible (value: bool) = Interop.mkprop "visible" value
    static member inline visibleChanged (handler: EventHandler) = Interop.mkprop "visibleChanged" handler
    static member inline clearOnVisibleFalse (value: bool) = Interop.mkprop "clearOnVisibleFalse" value
    static member inline title (value: string) = Interop.mkprop "title" value
    static member inline titleChanged (handler: EventHandler<EventArgs<string>>) = Interop.mkprop "titleChanged" handler
    static member inline titleChanging (handler: EventHandler<CancelEventArgs<string>>) = Interop.mkprop "titleChanging" handler
    // View Keyboard
    static member inline hotKeyChanged (handler: EventHandler<KeyChangedEventArgs>) = Interop.mkprop "hotKeyChanged" handler
    static member inline hotKey (hotKey: Key) = Interop.mkprop "hotKey" hotKey
    static member inline hotKeySpecifier (hotKeySpecifier: Rune) = Interop.mkprop "hotKeySpecifier" hotKeySpecifier
    //static member inline tabIndexes (tabIndexes: IList<View>) = Interop.mkprop "tabIndexes" tabIndexes
    static member inline tabIndex (tabIndex: int) = Interop.mkprop "tabIndex" tabIndex
    static member inline tabStop (tabStop: bool) = Interop.mkprop "tabStop" tabStop
    static member inline keyDown (handler: EventHandler<Key>) = Interop.mkprop "keyDown" handler
    static member inline processKeyDown (handler: EventHandler<Key>) = Interop.mkprop "processKeyDown" handler
    static member inline keyUp (handler: EventHandler<Key>) = Interop.mkprop "keyUp" handler
    static member inline invokingKeyBindings (handler: EventHandler<Key>) = Interop.mkprop "invokingKeyBindings" handler
    static member inline keyBindings (keyBindings: KeyBindings) = Interop.mkprop "keyBindings" keyBindings
    // View Adornments
    static member inline margin (value: Margin) = Interop.mkprop "margin" value
    static member inline shadowStyle (value: ShadowStyle) = Interop.mkprop "shadowStyle" value
    static member inline border (value: Border) = Interop.mkprop "border" value
    static member inline borderStyle (value: LineStyle) = Interop.mkprop "borderStyle" value
    static member inline padding (value: Padding) = Interop.mkprop "padding" value
    static member inline borderStyleChanging (handler: EventHandler<CancelEventArgs<LineStyle>>) = Interop.mkprop "borderStyleChanging" handler
    // View Arrangement
    static member inline arrangement (value: ViewArrangement) = Interop.mkprop "arrangement" value
    // View Content
    static member inline contentSizeTracksViewport (value: bool) = Interop.mkprop "contentSizeTracksViewport" value
    static member inline viewportSettings (value: ViewportSettings) = Interop.mkprop "viewportSettings" value
    static member inline viewport (value: Rectangle) = Interop.mkprop "viewport" value
    static member inline contentSizeChanged (handler: EventHandler<SizeChangedEventArgs>) = Interop.mkprop "contentSizeChanged" handler
    static member inline viewportChanged (handler: EventHandler<DrawEventArgs>) = Interop.mkprop "viewportChanged" handler
    // View Drawing
    static member inline colorScheme (value: ColorScheme) = Interop.mkprop "colorScheme" value
    static member inline lineCanvas (value: LineCanvas) = Interop.mkprop "lineCanvas" value
    static member inline needsDisplay (value: bool) = Interop.mkprop "needsDisplay" value
    static member inline subViewNeedsDisplay (value: bool) = Interop.mkprop "subViewNeedsDisplay" value
    static member inline superViewRendersLineCanvas (value: bool) = Interop.mkprop "superViewRendersLineCanvas" value
    static member inline drawContent (handler: EventHandler<DrawEventArgs>) = Interop.mkprop "drawContent" handler
    static member inline drawContentComplete (handler: EventHandler<DrawEventArgs>) = Interop.mkprop "drawContentComplete" handler
    // View Mouse
    static member inline mouseClick (handler: EventHandler<MouseEventEventArgs>) = Interop.mkprop "mouseClick" handler
    static member inline mouseEnter (handler: EventHandler<MouseEventEventArgs>) = Interop.mkprop "mouseEnter" handler
    static member inline mouseEvent (handler: EventHandler<MouseEventEventArgs>) = Interop.mkprop "mouseEvent" handler
    static member inline mouseLeave (handler: EventHandler<MouseEventEventArgs>) = Interop.mkprop "mouseLeave" handler
    static member inline highlightStyle (style: HighlightStyle) = Interop.mkprop "highlightStyle" style
    static member inline wantContinuousButtonPressed (value: bool) = Interop.mkprop "wantContinuousButtonPressed" value
    static member inline wantMousePositionReports (value: bool) = Interop.mkprop "wantMousePositionReports" value
    // View Layouts
    static member inline frame (frame: Rectangle) = Interop.mkprop "frame" frame
    //static member inline x (x: Pos) = Interop.mkprop "x" x
    //static member inline y (y: Pos) = Interop.mkprop "y" y
    //static member inline height (height: Dim) = Interop.mkprop "height" height
    //static member inline width (width: Dim) = Interop.mkprop "width" width
    static member inline validatePosDim (validatePosDim: bool) = Interop.mkprop "validatePosDim" validatePosDim

    static member inline layoutComplete (handler: EventHandler<LayoutEventArgs>) = Interop.mkprop "layoutComplete" handler
    static member inline layoutStarted (handler: EventHandler<LayoutEventArgs>) = Interop.mkprop "layoutStarted" handler
    // View Text
    static member inline preserveTrailingSpaces (value: bool) = Interop.mkprop "preserveTrailingSpaces" value
    static member inline text (value: string) = Interop.mkprop "text" value
    static member inline textAlignment (value: Alignment) = Interop.mkprop "textAlignment" value
    static member inline textDirection (value: TextDirection) = Interop.mkprop "textDirection" value
    static member inline textFormatter (value: TextFormatter) = Interop.mkprop "textFormatter" value
    static member inline verticalTextAlignment (value: Alignment) = Interop.mkprop "verticalTextAlignment" value

    static member inline textChanged (handler: EventHandler) = Interop.mkprop "textChanged" handler
    
    
module prop =

    module position =

        type x =
            static member inline absolute (position:int)                                        = Interop.mkprop "x" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "x" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd ()                                                   = Interop.mkprop "x" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "x" (Pos.AnchorEnd(offset))
            static member inline center ()                                                      = Interop.mkprop "x" (Pos.Center())
            static member inline func (f:unit -> int)                                           = Interop.mkprop "x" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "x" (Pos.Percent(percent))

        type y =
            static member inline absolute (position:int)                                        = Interop.mkprop "y" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "y" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd ()                                                   = Interop.mkprop "y" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "y" (Pos.AnchorEnd(offset))
            static member inline center ()                                                      = Interop.mkprop "y" (Pos.Center())
            static member inline func (f:unit -> int)                                           = Interop.mkprop "y" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "y" (Pos.Percent(percent))

    type width =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "width" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "width" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "width" (Dim.Fill(margin))
        static member inline func (f:unit->int)                                                                     = Interop.mkprop "width" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "width" (Dim.Percent(percent, mode))


    type height =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "height" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "height" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "height" (Dim.Fill(margin))
        static member inline func (f:unit->int)                                                                     = Interop.mkprop "height" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "height" (Dim.Percent(percent, mode))

    
    
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

    type borderStyle =
        static member inline double = Interop.mkprop    "borderStyle" LineStyle.Double
        static member inline none = Interop.mkprop      "borderStyle" LineStyle.None
        static member inline rounded = Interop.mkprop   "borderStyle" LineStyle.Rounded
        static member inline single = Interop.mkprop    "borderStyle" LineStyle.Single

    type shadowStyle =
        static member inline none = Interop.mkprop          "shadowStyle" ShadowStyle.None
        static member inline opaque = Interop.mkprop        "shadowStyle" ShadowStyle.Opaque
        static member inline transparent = Interop.mkprop   "shadowStyle" ShadowStyle.Transparent

    
        

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
    static member inline defaultShadow (value: ShadowStyle)         = Interop.mkprop "defaultShadow" value
    static member inline defaultBorderStyle (value: LineStyle)      = Interop.mkprop "defaultBorderStyle" value


type bar =
    static member inline orientation (value: Orientation) = Interop.mkprop "orientation" value
    static member inline alignmentModes (value: AlignmentModes) = Interop.mkprop "alignmentModes" value
    static member inline initialized (handler: EventHandler) = Interop.mkprop "initialized" handler

    
type button =
    /// raises when button is clicked or space, enter, hotkey is used
    static member inline onAccept (f:unit->unit) = Interop.mkprop "onAccept" f
    // direct
    static member inline wantContinuousButtonPressed (value: bool) = Interop.mkprop "wantContinuousButtonPressed" value
    static member inline text (value: string) = Interop.mkprop "text" value
    static member inline hotKeySpecifier (value: Rune) = Interop.mkprop "hotKeySpecifier" value
    static member inline isDefault (value: bool) = Interop.mkprop "isDefault" value
    static member inline noDecorations (value: bool) = Interop.mkprop "noDecorations" value
    static member inline noPadding (value: bool) = Interop.mkprop "noPadding" value
    static member inline titleChanged (handler: EventArgs<string>->unit) = Interop.mkprop "titleChanged" handler
    static member inline mouseClick (handler: MouseEventEventArgs->unit) = Interop.mkprop "mouseClick" handler


type checkBox =
    static member inline onToggle (f:CancelEventArgs<CheckState>->unit) = Interop.mkprop "onToggle" f
    static member inline onToggled (f:bool->unit) = Interop.mkprop "onToggledBool" f
    static member inline allowCheckStateNone (value: bool) = Interop.mkprop "allowCheckStateNone" value
    static member inline state (value: CheckState) = Interop.mkprop "state" value
    static member inline toggle (handler: EventHandler<CancelEventArgs<CheckState>>) = Interop.mkprop "toggle" handler
    static member inline text (value: string) = Interop.mkprop "text" value
    static member inline hotKeySpecifier (value: Rune) = Interop.mkprop "hotKeySpecifier" value


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


