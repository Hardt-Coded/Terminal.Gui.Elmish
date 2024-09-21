
(*
#######################################
#            Props.fs              #
#######################################
*)

namespace Terminal.Gui.Elmish

open System
open System.Text
open System.Drawing
open System.ComponentModel
open System.Collections.ObjectModel
open System.IO
open System.Collections.Generic
open System.Collections.Specialized
open System.Globalization
open Terminal.Gui.Elmish
open Terminal.Gui.TextValidateProviders
open Terminal.Gui
open Terminal.Gui.Elmish.Elements
// View

type prop =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline ref (reference:View->unit) = Interop.mkprop "ref" reference
            
    // Properties
    static member inline arrangement (value:ViewArrangement) = Interop.mkprop "view.arrangement" value
    static member inline borderStyle (value:LineStyle) = Interop.mkprop "view.borderStyle" value
    static member inline canFocus (value:bool) = Interop.mkprop "view.canFocus" value
    static member inline colorScheme (value:ColorScheme) = Interop.mkprop "view.colorScheme" value
    static member inline contentSizeTracksViewport (value:bool) = Interop.mkprop "view.contentSizeTracksViewport" value
    static member inline cursorVisibility (value:CursorVisibility) = Interop.mkprop "view.cursorVisibility" value
    static member inline data (value:Object) = Interop.mkprop "view.data" value
    static member inline enabled (value:bool) = Interop.mkprop "view.enabled" value
    static member inline frame (value:Rectangle) = Interop.mkprop "view.frame" value
    static member inline hasFocus (value:bool) = Interop.mkprop "view.hasFocus" value
    static member inline height (value:Dim) = Interop.mkprop "view.height" value
    static member inline highlightStyle (value:HighlightStyle) = Interop.mkprop "view.highlightStyle" value
    static member inline hotKey (value:Key) = Interop.mkprop "view.hotKey" value
    static member inline hotKeySpecifier (value:Rune) = Interop.mkprop "view.hotKeySpecifier" value
    static member inline id (value:string) = Interop.mkprop "view.id" value
    static member inline isInitialized (value:bool) = Interop.mkprop "view.isInitialized" value
    static member inline needsDisplay (value:bool) = Interop.mkprop "view.needsDisplay" value
    static member inline preserveTrailingSpaces (value:bool) = Interop.mkprop "view.preserveTrailingSpaces" value
    static member inline shadowStyle (value:ShadowStyle) = Interop.mkprop "view.shadowStyle" value
    static member inline superViewRendersLineCanvas (value:bool) = Interop.mkprop "view.superViewRendersLineCanvas" value
    static member inline tabStop (value:TabBehavior option) = Interop.mkprop "view.tabStop" value
    static member inline text (value:string) = Interop.mkprop "view.text" value
    static member inline textAlignment (value:Alignment) = Interop.mkprop "view.textAlignment" value
    static member inline textDirection (value:TextDirection) = Interop.mkprop "view.textDirection" value
    static member inline title (value:string) = Interop.mkprop "view.title" value
    static member inline validatePosDim (value:bool) = Interop.mkprop "view.validatePosDim" value
    static member inline verticalTextAlignment (value:Alignment) = Interop.mkprop "view.verticalTextAlignment" value
    static member inline viewport (value:Rectangle) = Interop.mkprop "view.viewport" value
    static member inline viewportSettings (value:ViewportSettings) = Interop.mkprop "view.viewportSettings" value
    static member inline visible (value:bool) = Interop.mkprop "view.visible" value
    static member inline wantContinuousButtonPressed (value:bool) = Interop.mkprop "view.wantContinuousButtonPressed" value
    static member inline wantMousePositionReports (value:bool) = Interop.mkprop "view.wantMousePositionReports" value
    static member inline width (value:Dim) = Interop.mkprop "view.width" value
    static member inline x (value:Pos) = Interop.mkprop "view.x" value
    static member inline y (value:Pos) = Interop.mkprop "view.y" value
    // Events
    static member inline accept (handler:HandledEventArgs->unit) = Interop.mkprop "view.accept" handler
    static member inline added (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.added" handler
    static member inline borderStyleChanging (handler:CancelEventArgs<LineStyle>->unit) = Interop.mkprop "view.borderStyleChanging" handler
    static member inline canFocusChanged (handler:unit->unit) = Interop.mkprop "view.canFocusChanged" handler
    static member inline contentSizeChanged (handler:SizeChangedEventArgs->unit) = Interop.mkprop "view.contentSizeChanged" handler
    static member inline disposing (handler:unit->unit) = Interop.mkprop "view.disposing" handler
    static member inline drawContent (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawContent" handler
    static member inline drawContentComplete (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawContentComplete" handler
    static member inline enabledChanged (handler:unit->unit) = Interop.mkprop "view.enabledChanged" handler
    static member inline hasFocusChanged (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.hasFocusChanged" handler
    static member inline hasFocusChanging (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.hasFocusChanging" handler
    static member inline highlight (handler:CancelEventArgs<HighlightStyle>->unit) = Interop.mkprop "view.highlight" handler
    static member inline hotKeyChanged (handler:KeyChangedEventArgs->unit) = Interop.mkprop "view.hotKeyChanged" handler
    static member inline initialized (handler:unit->unit) = Interop.mkprop "view.initialized" handler
    static member inline invokingKeyBindings (handler:Key->unit) = Interop.mkprop "view.invokingKeyBindings" handler
    static member inline keyDown (handler:Key->unit) = Interop.mkprop "view.keyDown" handler
    static member inline keyUp (handler:Key->unit) = Interop.mkprop "view.keyUp" handler
    static member inline layoutComplete (handler:LayoutEventArgs->unit) = Interop.mkprop "view.layoutComplete" handler
    static member inline layoutStarted (handler:LayoutEventArgs->unit) = Interop.mkprop "view.layoutStarted" handler
    static member inline mouseClick (handler:MouseEventEventArgs->unit) = Interop.mkprop "view.mouseClick" handler
    static member inline mouseEnter (handler:MouseEventEventArgs->unit) = Interop.mkprop "view.mouseEnter" handler
    static member inline mouseEvent (handler:MouseEventEventArgs->unit) = Interop.mkprop "view.mouseEvent" handler
    static member inline mouseLeave (handler:MouseEventEventArgs->unit) = Interop.mkprop "view.mouseLeave" handler
    static member inline processKeyDown (handler:Key->unit) = Interop.mkprop "view.processKeyDown" handler
    static member inline removed (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.removed" handler
    static member inline textChanged (handler:unit->unit) = Interop.mkprop "view.textChanged" handler
    static member inline titleChanged (handler:string->unit) = Interop.mkprop "view.titleChanged" handler
    static member inline titleChanging (handler:CancelEventArgs<string>->unit) = Interop.mkprop "view.titleChanging" handler
    static member inline viewportChanged (handler:DrawEventArgs->unit) = Interop.mkprop "view.viewportChanged" handler
    static member inline visibleChanged (handler:unit->unit) = Interop.mkprop "view.visibleChanged" handler

    static member inline color (fg:Color, bg:Color) = Interop.mkprop "view.colorScheme" (
            let attrib = Attribute(&fg, &bg)
            ColorScheme(attrib)
        )            
            


module prop =    
    module position =
        type x =
            static member inline absolute (position:int)                                        = Interop.mkprop "view.x" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "view.x" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd                                                      = Interop.mkprop "view.x" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.x" (Pos.AnchorEnd(offset))
            static member inline center                                                         = Interop.mkprop "view.x" (Pos.Center())
            static member inline func (f:unit -> int)                                           = Interop.mkprop "view.x" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "view.x" (Pos.Percent(percent))

        type y =
            static member inline absolute (position:int)                                        = Interop.mkprop "view.y" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "view.y" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd                                                      = Interop.mkprop "view.y" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.y" (Pos.AnchorEnd(offset))
            static member inline center                                                         = Interop.mkprop "view.y" (Pos.Center())
            static member inline func (f:unit -> int)                                           = Interop.mkprop "view.y" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "view.y" (Pos.Percent(percent))
        
    type width =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "view.width" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "view.width" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "view.width" (Dim.Fill(margin))
        static member inline func (f:unit->int)                                                                     = Interop.mkprop "view.width" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.width" (Dim.Percent(percent, mode))
        static member inline percent (percent:int)                                                                  = Interop.mkprop "view.width" (Dim.Percent(percent, DimPercentMode.ContentSize))

    type height =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "view.height" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "view.height" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "view.height" (Dim.Fill(margin))
        static member inline func (f:unit->int)                                                                     = Interop.mkprop "view.height" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.height" (Dim.Percent(percent, mode))
        static member inline percent (percent:int)                                                                  = Interop.mkprop "view.height" (Dim.Percent(percent, DimPercentMode.ContentSize))
    
    
    type alignment =
        static member inline center     =   Interop.mkprop "view.alignment" Alignment.Center
        static member inline ``end``    =   Interop.mkprop "view.alignment" Alignment.End
        static member inline start      =   Interop.mkprop "view.alignment" Alignment.Start
        static member inline fill       =   Interop.mkprop "view.alignment" Alignment.Fill

    type textDirection =
        static member inline bottomTop_leftRight = Interop.mkprop "view.textDirection" TextDirection.BottomTop_LeftRight
        static member inline bottomTop_rightLeft = Interop.mkprop "view.textDirection" TextDirection.BottomTop_RightLeft
        static member inline leftRight_bottomTop = Interop.mkprop "view.textDirection" TextDirection.LeftRight_BottomTop
        static member inline leftRight_topBottom = Interop.mkprop "view.textDirection" TextDirection.LeftRight_TopBottom
        static member inline rightLeft_bottomTop = Interop.mkprop "view.textDirection" TextDirection.RightLeft_BottomTop
        static member inline rightLeft_topBottom = Interop.mkprop "view.textDirection" TextDirection.RightLeft_TopBottom
        static member inline topBottom_leftRight = Interop.mkprop "view.textDirection" TextDirection.TopBottom_LeftRight

    type borderStyle =
        static member inline double = Interop.mkprop    "view.borderStyle" LineStyle.Double
        static member inline none = Interop.mkprop      "view.borderStyle" LineStyle.None
        static member inline rounded = Interop.mkprop   "view.borderStyle" LineStyle.Rounded
        static member inline single = Interop.mkprop    "view.borderStyle" LineStyle.Single

    type shadowStyle =
        static member inline none = Interop.mkprop          "view.shadowStyle" ShadowStyle.None
        static member inline opaque = Interop.mkprop        "view.shadowStyle" ShadowStyle.Opaque
        static member inline transparent = Interop.mkprop   "view.shadowStyle" ShadowStyle.Transparent
    
    

// Adornment
type adornment =
    // Properties
    static member inline superViewRendersLineCanvas (value:bool) = Interop.mkprop "adornment.superViewRendersLineCanvas" value
    static member inline thickness (value:Thickness) = Interop.mkprop "adornment.thickness" value
    static member inline viewport (value:Rectangle) = Interop.mkprop "adornment.viewport" value
    // Events
    static member inline thicknessChanged (handler:unit->unit) = Interop.mkprop "adornment.thicknessChanged" handler

// Bar
type bar =
    // Properties
    static member inline alignmentModes (value:AlignmentModes) = Interop.mkprop "bar.alignmentModes" value
    static member inline orientation (value:Orientation) = Interop.mkprop "bar.orientation" value
    // Events
    static member inline orientationChanged (handler:Orientation->unit) = Interop.mkprop "bar.orientationChanged" handler
    static member inline orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "bar.orientationChanging" handler

// Border
type border =
    // Properties
    static member inline colorScheme (value:ColorScheme) = Interop.mkprop "border.colorScheme" value
    static member inline lineStyle (value:LineStyle) = Interop.mkprop "border.lineStyle" value
    static member inline settings (value:BorderSettings) = Interop.mkprop "border.settings" value

// Button
type button =
    // Properties
    static member inline hotKeySpecifier (value:Rune) = Interop.mkprop "button.hotKeySpecifier" value
    static member inline isDefault (value:bool) = Interop.mkprop "button.isDefault" value
    static member inline noDecorations (value:bool) = Interop.mkprop "button.noDecorations" value
    static member inline noPadding (value:bool) = Interop.mkprop "button.noPadding" value
    static member inline text (value:string) = Interop.mkprop "button.text" value
    static member inline wantContinuousButtonPressed (value:bool) = Interop.mkprop "button.wantContinuousButtonPressed" value

// CheckBox
type checkBox =
    // Properties
    static member inline allowCheckStateNone (value:bool) = Interop.mkprop "checkBox.allowCheckStateNone" value
    static member inline checkedState (value:CheckState) = Interop.mkprop "checkBox.checkedState" value
    static member inline hotKeySpecifier (value:Rune) = Interop.mkprop "checkBox.hotKeySpecifier" value
    static member inline text (value:string) = Interop.mkprop "checkBox.text" value
    // Events
    static member inline checkedStateChanging (handler:CancelEventArgs<CheckState>->unit) = Interop.mkprop "checkBox.checkedStateChanging" handler

    static member inline ischecked (value:bool) = Interop.mkprop "checkBox.checkedState" (if value then CheckState.Checked else CheckState.UnChecked)
            

// ColorPicker
type colorPicker =
    // Properties
    static member inline selectedColor (value:Color) = Interop.mkprop "colorPicker.selectedColor" value
    static member inline style (value:ColorPickerStyle) = Interop.mkprop "colorPicker.style" value
    // Events
    static member inline colorChanged (handler:ColorEventArgs->unit) = Interop.mkprop "colorPicker.colorChanged" handler

// ColorPicker16
type colorPicker16 =
    // Properties
    static member inline boxHeight (value:Int32) = Interop.mkprop "colorPicker16.boxHeight" value
    static member inline boxWidth (value:Int32) = Interop.mkprop "colorPicker16.boxWidth" value
    static member inline cursor (value:Point) = Interop.mkprop "colorPicker16.cursor" value
    static member inline selectedColor (value:ColorName) = Interop.mkprop "colorPicker16.selectedColor" value
    // Events
    static member inline colorChanged (handler:ColorEventArgs->unit) = Interop.mkprop "colorPicker16.colorChanged" handler

// ComboBox
type comboBox =
    // Properties
    static member inline colorScheme (value:ColorScheme) = Interop.mkprop "comboBox.colorScheme" value
    static member inline hideDropdownListOnClick (value:bool) = Interop.mkprop "comboBox.hideDropdownListOnClick" value
    static member inline readOnly (value:bool) = Interop.mkprop "comboBox.readOnly" value
    static member inline searchText (value:string) = Interop.mkprop "comboBox.searchText" value
    static member inline selectedItem (value:Int32) = Interop.mkprop "comboBox.selectedItem" value
    static member inline source (value:string list) = Interop.mkprop "comboBox.source" value
    static member inline text (value:string) = Interop.mkprop "comboBox.text" value
    // Events
    static member inline collapsed (handler:unit->unit) = Interop.mkprop "comboBox.collapsed" handler
    static member inline expanded (handler:unit->unit) = Interop.mkprop "comboBox.expanded" handler
    static member inline openSelectedItem (handler:ListViewItemEventArgs->unit) = Interop.mkprop "comboBox.openSelectedItem" handler
    static member inline selectedItemChanged (handler:ListViewItemEventArgs->unit) = Interop.mkprop "comboBox.selectedItemChanged" handler

// DateField
type dateField =
    // Properties
    static member inline culture (value:CultureInfo) = Interop.mkprop "dateField.culture" value
    static member inline cursorPosition (value:Int32) = Interop.mkprop "dateField.cursorPosition" value
    static member inline date (value:DateTime) = Interop.mkprop "dateField.date" value
    // Events
    static member inline dateChanged (handler:DateTimeEventArgs<DateTime>->unit) = Interop.mkprop "dateField.dateChanged" handler

// DatePicker
type datePicker =
    // Properties
    static member inline culture (value:CultureInfo) = Interop.mkprop "datePicker.culture" value
    static member inline date (value:DateTime) = Interop.mkprop "datePicker.date" value

// Dialog
type dialog =
    // Properties
    static member inline buttonAlignment (value:Alignment) = Interop.mkprop "dialog.buttonAlignment" value
    static member inline buttonAlignmentModes (value:AlignmentModes) = Interop.mkprop "dialog.buttonAlignmentModes" value
    static member inline canceled (value:bool) = Interop.mkprop "dialog.canceled" value

// FileDialog
type fileDialog =
    // Properties
    static member inline allowedTypes (value:IAllowedType list) = Interop.mkprop "fileDialog.allowedTypes" value
    static member inline allowsMultipleSelection (value:bool) = Interop.mkprop "fileDialog.allowsMultipleSelection" value
    static member inline fileOperationsHandler (value:IFileOperations) = Interop.mkprop "fileDialog.fileOperationsHandler" value
    static member inline mustExist (value:bool) = Interop.mkprop "fileDialog.mustExist" value
    static member inline openMode (value:OpenMode) = Interop.mkprop "fileDialog.openMode" value
    static member inline path (value:string) = Interop.mkprop "fileDialog.path" value
    static member inline searchMatcher (value:ISearchMatcher) = Interop.mkprop "fileDialog.searchMatcher" value
    // Events
    static member inline filesSelected (handler:FilesSelectedEventArgs->unit) = Interop.mkprop "fileDialog.filesSelected" handler

// FrameView
// No properties or events FrameView

// GraphView
type graphView =
    // Properties
    static member inline axisX (value:HorizontalAxis) = Interop.mkprop "graphView.axisX" value
    static member inline axisY (value:VerticalAxis) = Interop.mkprop "graphView.axisY" value
    static member inline cellSize (value:PointF) = Interop.mkprop "graphView.cellSize" value
    static member inline graphColor (value:Attribute option) = Interop.mkprop "graphView.graphColor" value
    static member inline marginBottom (value:int) = Interop.mkprop "graphView.marginBottom" value
    static member inline marginLeft (value:int) = Interop.mkprop "graphView.marginLeft" value
    static member inline scrollOffset (value:PointF) = Interop.mkprop "graphView.scrollOffset" value

// HexView
type hexView =
    // Properties
    static member inline allowEdits (value:bool) = Interop.mkprop "hexView.allowEdits" value
    static member inline displayStart (value:Int64) = Interop.mkprop "hexView.displayStart" value
    static member inline source (value:Stream) = Interop.mkprop "hexView.source" value
    // Events
    static member inline edited (handler:HexViewEditEventArgs->unit) = Interop.mkprop "hexView.edited" handler
    static member inline positionChanged (handler:HexViewEventArgs->unit) = Interop.mkprop "hexView.positionChanged" handler

// Label
type label =
    // Properties
    static member inline hotKeySpecifier (value:Rune) = Interop.mkprop "label.hotKeySpecifier" value
    static member inline text (value:string) = Interop.mkprop "label.text" value

// LegendAnnotation
// No properties or events LegendAnnotation

// Line
type line =
    // Properties
    static member inline orientation (value:Orientation) = Interop.mkprop "line.orientation" value
    // Events
    static member inline orientationChanged (handler:Orientation->unit) = Interop.mkprop "line.orientationChanged" handler
    static member inline orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "line.orientationChanging" handler

// LineView
type lineView =
    // Properties
    static member inline endingAnchor (value:Rune option) = Interop.mkprop "lineView.endingAnchor" value
    static member inline lineRune (value:Rune) = Interop.mkprop "lineView.lineRune" value
    static member inline orientation (value:Orientation) = Interop.mkprop "lineView.orientation" value
    static member inline startingAnchor (value:Rune option) = Interop.mkprop "lineView.startingAnchor" value

// ListView
type listView =
    // Properties
    static member inline allowsMarking (value:bool) = Interop.mkprop "listView.allowsMarking" value
    static member inline allowsMultipleSelection (value:bool) = Interop.mkprop "listView.allowsMultipleSelection" value
    static member inline leftItem (value:Int32) = Interop.mkprop "listView.leftItem" value
    static member inline selectedItem (value:Int32) = Interop.mkprop "listView.selectedItem" value
    static member inline source (value:string list) = Interop.mkprop "listView.source" value
    static member inline topItem (value:Int32) = Interop.mkprop "listView.topItem" value
    // Events
    static member inline collectionChanged (handler:NotifyCollectionChangedEventArgs->unit) = Interop.mkprop "listView.collectionChanged" handler
    static member inline openSelectedItem (handler:ListViewItemEventArgs->unit) = Interop.mkprop "listView.openSelectedItem" handler
    static member inline rowRender (handler:ListViewRowEventArgs->unit) = Interop.mkprop "listView.rowRender" handler
    static member inline selectedItemChanged (handler:ListViewItemEventArgs->unit) = Interop.mkprop "listView.selectedItemChanged" handler

// Margin
type margin =
    // Properties
    static member inline colorScheme (value:ColorScheme) = Interop.mkprop "margin.colorScheme" value
    static member inline shadowStyle (value:ShadowStyle) = Interop.mkprop "margin.shadowStyle" value

// MenuBar
type menuBar =
    // Properties
    static member inline key (value:Key) = Interop.mkprop "menuBar.key" value
    static member inline menus (value:MenuBarItem list) = Interop.mkprop "menuBar.menus" value
    static member inline menusBorderStyle (value:LineStyle) = Interop.mkprop "menuBar.menusBorderStyle" value
    static member inline useKeysUpDownAsKeysLeftRight (value:bool) = Interop.mkprop "menuBar.useKeysUpDownAsKeysLeftRight" value
    static member inline useSubMenusSingleFrame (value:bool) = Interop.mkprop "menuBar.useSubMenusSingleFrame" value
    static member inline visible (value:bool) = Interop.mkprop "menuBar.visible" value
    // Events
    static member inline menuAllClosed (handler:unit->unit) = Interop.mkprop "menuBar.menuAllClosed" handler
    static member inline menuClosing (handler:MenuClosingEventArgs->unit) = Interop.mkprop "menuBar.menuClosing" handler
    static member inline menuOpened (handler:MenuOpenedEventArgs->unit) = Interop.mkprop "menuBar.menuOpened" handler
    static member inline menuOpening (handler:MenuOpeningEventArgs->unit) = Interop.mkprop "menuBar.menuOpening" handler

// MenuBarv2
// No properties or events MenuBarv2

// Menuv2
// No properties or events Menuv2

// NumericUpDown`1
type numericUpDown<'a> =
    // Properties
    static member inline format (value:string) = Interop.mkprop "numericUpDown`1.format" value
    static member inline increment (value:'a) = Interop.mkprop "numericUpDown`1.increment" value
    static member inline value (value:'a) = Interop.mkprop "numericUpDown`1.value" value
    // Events
    static member inline formatChanged (handler:string->unit) = Interop.mkprop "numericUpDown`1.formatChanged" handler
    static member inline incrementChanged (handler:'a->unit) = Interop.mkprop "numericUpDown`1.incrementChanged" handler
    static member inline valueChanged (handler:'a->unit) = Interop.mkprop "numericUpDown`1.valueChanged" handler
    static member inline valueChanging (handler:CancelEventArgs<'a>->unit) = Interop.mkprop "numericUpDown`1.valueChanging" handler

// NumericUpDown
// No properties or events NumericUpDown

// OpenDialog
type openDialog =
    // Properties
    static member inline openMode (value:OpenMode) = Interop.mkprop "openDialog.openMode" value

// Padding
type padding =
    // Properties
    static member inline colorScheme (value:ColorScheme) = Interop.mkprop "padding.colorScheme" value

// ProgressBar
type progressBar =
    // Properties
    static member inline bidirectionalMarquee (value:bool) = Interop.mkprop "progressBar.bidirectionalMarquee" value
    static member inline fraction (value:Single) = Interop.mkprop "progressBar.fraction" value
    static member inline progressBarFormat (value:ProgressBarFormat) = Interop.mkprop "progressBar.progressBarFormat" value
    static member inline progressBarStyle (value:ProgressBarStyle) = Interop.mkprop "progressBar.progressBarStyle" value
    static member inline segmentCharacter (value:Rune) = Interop.mkprop "progressBar.segmentCharacter" value
    static member inline text (value:string) = Interop.mkprop "progressBar.text" value

// RadioGroup
type radioGroup =
    // Properties
    static member inline horizontalSpace (value:Int32) = Interop.mkprop "radioGroup.horizontalSpace" value
    static member inline orientation (value:Orientation) = Interop.mkprop "radioGroup.orientation" value
    static member inline radioLabels (value:string list) = Interop.mkprop "radioGroup.radioLabels" value
    static member inline selectedItem (value:Int32) = Interop.mkprop "radioGroup.selectedItem" value
    // Events
    static member inline orientationChanged (handler:Orientation->unit) = Interop.mkprop "radioGroup.orientationChanged" handler
    static member inline orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "radioGroup.orientationChanging" handler
    static member inline selectedItemChanged (handler:SelectedItemChangedArgs->unit) = Interop.mkprop "radioGroup.selectedItemChanged" handler

// SaveDialog
// No properties or events SaveDialog

// ScrollBarView
type scrollBarView =
    // Properties
    static member inline autoHideScrollBars (value:bool) = Interop.mkprop "scrollBarView.autoHideScrollBars" value
    static member inline isVertical (value:bool) = Interop.mkprop "scrollBarView.isVertical" value
    static member inline keepContentAlwaysInViewport (value:bool) = Interop.mkprop "scrollBarView.keepContentAlwaysInViewport" value
    static member inline otherScrollBarView (value:ScrollBarView) = Interop.mkprop "scrollBarView.otherScrollBarView" value
    static member inline position (value:Int32) = Interop.mkprop "scrollBarView.position" value
    static member inline showScrollIndicator (value:bool) = Interop.mkprop "scrollBarView.showScrollIndicator" value
    static member inline size (value:Int32) = Interop.mkprop "scrollBarView.size" value
    // Events
    static member inline changedPosition (handler:unit->unit) = Interop.mkprop "scrollBarView.changedPosition" handler

// ScrollView
type scrollView =
    // Properties
    static member inline autoHideScrollBars (value:bool) = Interop.mkprop "scrollView.autoHideScrollBars" value
    static member inline contentOffset (value:Point) = Interop.mkprop "scrollView.contentOffset" value
    static member inline keepContentAlwaysInViewport (value:bool) = Interop.mkprop "scrollView.keepContentAlwaysInViewport" value
    static member inline showHorizontalScrollIndicator (value:bool) = Interop.mkprop "scrollView.showHorizontalScrollIndicator" value
    static member inline showVerticalScrollIndicator (value:bool) = Interop.mkprop "scrollView.showVerticalScrollIndicator" value

// Shortcut
type shortcut =
    // Properties
    static member inline action (value:Action) = Interop.mkprop "shortcut.action" value
    static member inline alignmentModes (value:AlignmentModes) = Interop.mkprop "shortcut.alignmentModes" value
    static member inline colorScheme (value:ColorScheme) = Interop.mkprop "shortcut.colorScheme" value
    static member inline helpText (value:string) = Interop.mkprop "shortcut.helpText" value
    static member inline key (value:Key) = Interop.mkprop "shortcut.key" value
    static member inline keyBindingScope (value:KeyBindingScope) = Interop.mkprop "shortcut.keyBindingScope" value
    static member inline minimumKeyTextSize (value:Int32) = Interop.mkprop "shortcut.minimumKeyTextSize" value
    static member inline orientation (value:Orientation) = Interop.mkprop "shortcut.orientation" value
    static member inline text (value:string) = Interop.mkprop "shortcut.text" value
    // Events
    static member inline orientationChanged (handler:Orientation->unit) = Interop.mkprop "shortcut.orientationChanged" handler
    static member inline orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "shortcut.orientationChanging" handler

// Slider`1
type slider<'a> =
    // Properties
    static member inline allowEmpty (value:bool) = Interop.mkprop "slider`1.allowEmpty" value
    static member inline focusedOption (value:Int32) = Interop.mkprop "slider`1.focusedOption" value
    static member inline legendsOrientation (value:Orientation) = Interop.mkprop "slider`1.legendsOrientation" value
    static member inline minimumInnerSpacing (value:Int32) = Interop.mkprop "slider`1.minimumInnerSpacing" value
    static member inline options (value:SliderOption<'a> list) = Interop.mkprop "slider`1.options" value
    static member inline orientation (value:Orientation) = Interop.mkprop "slider`1.orientation" value
    static member inline rangeAllowSingle (value:bool) = Interop.mkprop "slider`1.rangeAllowSingle" value
    static member inline showEndSpacing (value:bool) = Interop.mkprop "slider`1.showEndSpacing" value
    static member inline showLegends (value:bool) = Interop.mkprop "slider`1.showLegends" value
    static member inline style (value:SliderStyle) = Interop.mkprop "slider`1.style" value
    static member inline text (value:string) = Interop.mkprop "slider`1.text" value
    static member inline ``type`` (value:SliderType) = Interop.mkprop "slider`1.``type``" value
    static member inline useMinimumSize (value:bool) = Interop.mkprop "slider`1.useMinimumSize" value
    // Events
    static member inline optionFocused (handler:SliderEventArgs<'a>->unit) = Interop.mkprop "slider`1.optionFocused" handler
    static member inline optionsChanged (handler:SliderEventArgs<'a>->unit) = Interop.mkprop "slider`1.optionsChanged" handler
    static member inline orientationChanged (handler:Orientation->unit) = Interop.mkprop "slider`1.orientationChanged" handler
    static member inline orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "slider`1.orientationChanging" handler

// Slider
// No properties or events Slider

// SpinnerView
type spinnerView =
    // Properties
    static member inline autoSpin (value:bool) = Interop.mkprop "spinnerView.autoSpin" value
    static member inline sequence (value:string list) = Interop.mkprop "spinnerView.sequence" value
    static member inline spinBounce (value:bool) = Interop.mkprop "spinnerView.spinBounce" value
    static member inline spinDelay (value:Int32) = Interop.mkprop "spinnerView.spinDelay" value
    static member inline spinReverse (value:bool) = Interop.mkprop "spinnerView.spinReverse" value
    static member inline style (value:SpinnerStyle) = Interop.mkprop "spinnerView.style" value

// StatusBar
// No properties or events StatusBar

// Tab
type tab =
    // Properties
    static member inline displayText (value:string) = Interop.mkprop "tab.displayText" value
    static member inline view (value:TerminalElement) = Interop.mkprop "tab.view" value

// TabView
type tabView =
    // Properties
    static member inline maxTabTextWidth (value:int) = Interop.mkprop "tabView.maxTabTextWidth" value
    static member inline selectedTab (value:Tab) = Interop.mkprop "tabView.selectedTab" value
    static member inline style (value:TabStyle) = Interop.mkprop "tabView.style" value
    static member inline tabScrollOffset (value:Int32) = Interop.mkprop "tabView.tabScrollOffset" value
    // Events
    static member inline selectedTabChanged (handler:TabChangedEventArgs->unit) = Interop.mkprop "tabView.selectedTabChanged" handler
    static member inline tabClicked (handler:TabMouseEventArgs->unit) = Interop.mkprop "tabView.tabClicked" handler

    static member inline tabs (value:TerminalElement list) = Interop.mkprop "tabView.tabs" value
            

// TableView
type tableView =
    // Properties
    static member inline cellActivationKey (value:KeyCode) = Interop.mkprop "tableView.cellActivationKey" value
    static member inline collectionNavigator (value:CollectionNavigatorBase) = Interop.mkprop "tableView.collectionNavigator" value
    static member inline columnOffset (value:Int32) = Interop.mkprop "tableView.columnOffset" value
    static member inline fullRowSelect (value:bool) = Interop.mkprop "tableView.fullRowSelect" value
    static member inline maxCellWidth (value:Int32) = Interop.mkprop "tableView.maxCellWidth" value
    static member inline minCellWidth (value:Int32) = Interop.mkprop "tableView.minCellWidth" value
    static member inline multiSelect (value:bool) = Interop.mkprop "tableView.multiSelect" value
    static member inline nullSymbol (value:string) = Interop.mkprop "tableView.nullSymbol" value
    static member inline rowOffset (value:Int32) = Interop.mkprop "tableView.rowOffset" value
    static member inline selectedColumn (value:Int32) = Interop.mkprop "tableView.selectedColumn" value
    static member inline selectedRow (value:Int32) = Interop.mkprop "tableView.selectedRow" value
    static member inline separatorSymbol (value:Char) = Interop.mkprop "tableView.separatorSymbol" value
    static member inline style (value:TableStyle) = Interop.mkprop "tableView.style" value
    static member inline table (value:ITableSource) = Interop.mkprop "tableView.table" value
    // Events
    static member inline cellActivated (handler:CellActivatedEventArgs->unit) = Interop.mkprop "tableView.cellActivated" handler
    static member inline cellToggled (handler:CellToggledEventArgs->unit) = Interop.mkprop "tableView.cellToggled" handler
    static member inline selectedCellChanged (handler:SelectedCellChangedEventArgs->unit) = Interop.mkprop "tableView.selectedCellChanged" handler

// TextField
type textField =
    // Properties
    static member inline autocomplete (value:IAutocomplete) = Interop.mkprop "textField.autocomplete" value
    static member inline caption (value:string) = Interop.mkprop "textField.caption" value
    static member inline captionColor (value:Color) = Interop.mkprop "textField.captionColor" value
    static member inline cursorPosition (value:Int32) = Interop.mkprop "textField.cursorPosition" value
    static member inline readOnly (value:bool) = Interop.mkprop "textField.readOnly" value
    static member inline secret (value:bool) = Interop.mkprop "textField.secret" value
    static member inline selectedStart (value:Int32) = Interop.mkprop "textField.selectedStart" value
    static member inline text (value:string) = Interop.mkprop "textField.text" value
    static member inline used (value:bool) = Interop.mkprop "textField.used" value
    // Events
    static member inline textChanging (handler:CancelEventArgs<string>->unit) = Interop.mkprop "textField.textChanging" handler

// TextValidateField
type textValidateField =
    // Properties
    static member inline provider (value:ITextValidateProvider) = Interop.mkprop "textValidateField.provider" value
    static member inline text (value:string) = Interop.mkprop "textValidateField.text" value

// TextView
type textView =
    // Properties
    static member inline allowsReturn (value:bool) = Interop.mkprop "textView.allowsReturn" value
    static member inline allowsTab (value:bool) = Interop.mkprop "textView.allowsTab" value
    static member inline cursorPosition (value:Point) = Interop.mkprop "textView.cursorPosition" value
    static member inline inheritsPreviousColorScheme (value:bool) = Interop.mkprop "textView.inheritsPreviousColorScheme" value
    static member inline isDirty (value:bool) = Interop.mkprop "textView.isDirty" value
    static member inline leftColumn (value:Int32) = Interop.mkprop "textView.leftColumn" value
    static member inline multiline (value:bool) = Interop.mkprop "textView.multiline" value
    static member inline readOnly (value:bool) = Interop.mkprop "textView.readOnly" value
    static member inline selecting (value:bool) = Interop.mkprop "textView.selecting" value
    static member inline selectionStartColumn (value:Int32) = Interop.mkprop "textView.selectionStartColumn" value
    static member inline selectionStartRow (value:Int32) = Interop.mkprop "textView.selectionStartRow" value
    static member inline tabWidth (value:Int32) = Interop.mkprop "textView.tabWidth" value
    static member inline text (value:string) = Interop.mkprop "textView.text" value
    static member inline topRow (value:Int32) = Interop.mkprop "textView.topRow" value
    static member inline used (value:bool) = Interop.mkprop "textView.used" value
    static member inline wordWrap (value:bool) = Interop.mkprop "textView.wordWrap" value
    // Events
    static member inline contentsChanged (handler:ContentsChangedEventArgs->unit) = Interop.mkprop "textView.contentsChanged" handler
    static member inline drawNormalColor (handler:RuneCellEventArgs->unit) = Interop.mkprop "textView.drawNormalColor" handler
    static member inline drawReadOnlyColor (handler:RuneCellEventArgs->unit) = Interop.mkprop "textView.drawReadOnlyColor" handler
    static member inline drawSelectionColor (handler:RuneCellEventArgs->unit) = Interop.mkprop "textView.drawSelectionColor" handler
    static member inline drawUsedColor (handler:RuneCellEventArgs->unit) = Interop.mkprop "textView.drawUsedColor" handler
    static member inline unwrappedCursorPosition (handler:PointEventArgs->unit) = Interop.mkprop "textView.unwrappedCursorPosition" handler

    static member inline textChanged (handler:string->unit) = Interop.mkprop "textView.textChanged" handler
            

// TileView
type tileView =
    // Properties
    static member inline lineStyle (value:LineStyle) = Interop.mkprop "tileView.lineStyle" value
    static member inline orientation (value:Orientation) = Interop.mkprop "tileView.orientation" value
    static member inline toggleResizable (value:KeyCode) = Interop.mkprop "tileView.toggleResizable" value
    // Events
    static member inline splitterMoved (handler:SplitterEventArgs->unit) = Interop.mkprop "tileView.splitterMoved" handler

// TimeField
type timeField =
    // Properties
    static member inline cursorPosition (value:Int32) = Interop.mkprop "timeField.cursorPosition" value
    static member inline isShortFormat (value:bool) = Interop.mkprop "timeField.isShortFormat" value
    static member inline time (value:TimeSpan) = Interop.mkprop "timeField.time" value
    // Events
    static member inline timeChanged (handler:DateTimeEventArgs<TimeSpan>->unit) = Interop.mkprop "timeField.timeChanged" handler

// Toplevel
type toplevel =
    // Properties
    static member inline isOverlappedContainer (value:bool) = Interop.mkprop "toplevel.isOverlappedContainer" value
    static member inline modal (value:bool) = Interop.mkprop "toplevel.modal" value
    static member inline running (value:bool) = Interop.mkprop "toplevel.running" value
    // Events
    static member inline activate (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.activate" handler
    static member inline allChildClosed (handler:unit->unit) = Interop.mkprop "toplevel.allChildClosed" handler
    static member inline childClosed (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.childClosed" handler
    static member inline childLoaded (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.childLoaded" handler
    static member inline childUnloaded (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.childUnloaded" handler
    static member inline closed (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.closed" handler
    static member inline closing (handler:ToplevelClosingEventArgs->unit) = Interop.mkprop "toplevel.closing" handler
    static member inline deactivate (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.deactivate" handler
    static member inline loaded (handler:unit->unit) = Interop.mkprop "toplevel.loaded" handler
    static member inline ready (handler:unit->unit) = Interop.mkprop "toplevel.ready" handler
    static member inline sizeChanging (handler:SizeChangedEventArgs->unit) = Interop.mkprop "toplevel.sizeChanging" handler
    static member inline unloaded (handler:unit->unit) = Interop.mkprop "toplevel.unloaded" handler

// TreeView`1
type treeView<'a when 'a : not struct> =
    // Properties
    static member inline allowLetterBasedNavigation (value:bool) = Interop.mkprop "treeView`1.allowLetterBasedNavigation" value
    static member inline aspectGetter (value:AspectGetterDelegate<'a>) = Interop.mkprop "treeView`1.aspectGetter" value
    static member inline colorGetter (value:Func<'a,ColorScheme>) = Interop.mkprop "treeView`1.colorGetter" value
    static member inline maxDepth (value:Int32) = Interop.mkprop "treeView`1.maxDepth" value
    static member inline multiSelect (value:bool) = Interop.mkprop "treeView`1.multiSelect" value
    static member inline objectActivationButton (value:MouseFlags option) = Interop.mkprop "treeView`1.objectActivationButton" value
    static member inline objectActivationKey (value:KeyCode) = Interop.mkprop "treeView`1.objectActivationKey" value
    static member inline scrollOffsetHorizontal (value:Int32) = Interop.mkprop "treeView`1.scrollOffsetHorizontal" value
    static member inline scrollOffsetVertical (value:Int32) = Interop.mkprop "treeView`1.scrollOffsetVertical" value
    static member inline selectedObject (value:'a) = Interop.mkprop "treeView`1.selectedObject" value
    static member inline style (value:TreeStyle) = Interop.mkprop "treeView`1.style" value
    static member inline treeBuilder (value:ITreeBuilder<'a>) = Interop.mkprop "treeView`1.treeBuilder" value
    // Events
    static member inline drawLine (handler:DrawTreeViewLineEventArgs<'a>->unit) = Interop.mkprop "treeView`1.drawLine" handler
    static member inline objectActivated (handler:ObjectActivatedEventArgs<'a>->unit) = Interop.mkprop "treeView`1.objectActivated" handler
    static member inline selectionChanged (handler:SelectionChangedEventArgs<'a>->unit) = Interop.mkprop "treeView`1.selectionChanged" handler

// TreeView
// No properties or events TreeView

// Window
// No properties or events Window

// Wizard
type wizard =
    // Properties
    static member inline currentStep (value:WizardStep) = Interop.mkprop "wizard.currentStep" value
    static member inline modal (value:bool) = Interop.mkprop "wizard.modal" value
    // Events
    static member inline cancelled (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.cancelled" handler
    static member inline finished (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.finished" handler
    static member inline movingBack (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.movingBack" handler
    static member inline movingNext (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.movingNext" handler
    static member inline stepChanged (handler:StepChangeEventArgs->unit) = Interop.mkprop "wizard.stepChanged" handler
    static member inline stepChanging (handler:StepChangeEventArgs->unit) = Interop.mkprop "wizard.stepChanging" handler

// WizardStep
type wizardStep =
    // Properties
    static member inline backButtonText (value:string) = Interop.mkprop "wizardStep.backButtonText" value
    static member inline helpText (value:string) = Interop.mkprop "wizardStep.helpText" value
    static member inline nextButtonText (value:string) = Interop.mkprop "wizardStep.nextButtonText" value

