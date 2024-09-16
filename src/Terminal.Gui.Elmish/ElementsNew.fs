
(*
#######################################
#            Elements.fs              #
#######################################
*)


namespace Terminal.Gui.Elmish.Elements

open System
open System.ComponentModel
open System.Linq.Expressions
open System.Text
open System.Linq
open Terminal.Gui
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Elmish.EventHelpers

[<AbstractClass>]
type TerminalElement (props:IProperty list) =
    let mutable view: View = null
    let mutable p: View option = None
    let mutable addProps = []
    let c = props |> Interop.getValueDefault<TerminalElement list> "children" []

    member this.parent with get() = p and set v = p <- v
    member this.element with get() = view and set v = view <- v
    member this.additionalProps with get() = addProps and set v = addProps <- v
    member _.properties = props @ addProps
    member _.children   = c

    abstract create: parent:View option -> unit
    abstract update: prevElement:View -> oldProps:IProperty list -> unit
    abstract canUpdate: prevElement:View -> oldProps:IProperty list -> bool
    abstract name: string

    

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
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.TextValidateProviders
open Terminal.Gui
module ViewElement =

    let setProps (element: View) props =
        // Properties
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v element)
        // Properties
        props |> Interop.getValue<ViewArrangement> "view.arrangement" |> Option.iter (fun v -> element.Arrangement <- v )
        props |> Interop.getValue<LineStyle> "view.borderStyle" |> Option.iter (fun v -> element.BorderStyle <- v )
        props |> Interop.getValue<bool> "view.canFocus" |> Option.iter (fun v -> element.CanFocus <- v )
        props |> Interop.getValue<ColorScheme> "view.colorScheme" |> Option.iter (fun v -> element.ColorScheme <- v )
        props |> Interop.getValue<bool> "view.contentSizeTracksViewport" |> Option.iter (fun v -> element.ContentSizeTracksViewport <- v )
        props |> Interop.getValue<CursorVisibility> "view.cursorVisibility" |> Option.iter (fun v -> element.CursorVisibility <- v )
        props |> Interop.getValue<Object> "view.data" |> Option.iter (fun v -> element.Data <- v )
        props |> Interop.getValue<bool> "view.enabled" |> Option.iter (fun v -> element.Enabled <- v )
        props |> Interop.getValue<Rectangle> "view.frame" |> Option.iter (fun v -> element.Frame <- v )
        props |> Interop.getValue<bool> "view.hasFocus" |> Option.iter (fun v -> element.HasFocus <- v )
        props |> Interop.getValue<Dim> "view.height" |> Option.iter (fun v -> element.Height <- v )
        props |> Interop.getValue<HighlightStyle> "view.highlightStyle" |> Option.iter (fun v -> element.HighlightStyle <- v )
        props |> Interop.getValue<Key> "view.hotKey" |> Option.iter (fun v -> element.HotKey <- v )
        props |> Interop.getValue<Rune> "view.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Interop.getValue<string> "view.id" |> Option.iter (fun v -> element.Id <- v )
        props |> Interop.getValue<bool> "view.isInitialized" |> Option.iter (fun v -> element.IsInitialized <- v )
        props |> Interop.getValue<bool> "view.needsDisplay" |> Option.iter (fun v -> element.NeedsDisplay <- v )
        props |> Interop.getValue<bool> "view.preserveTrailingSpaces" |> Option.iter (fun v -> element.PreserveTrailingSpaces <- v )
        props |> Interop.getValue<ShadowStyle> "view.shadowStyle" |> Option.iter (fun v -> element.ShadowStyle <- v )
        props |> Interop.getValue<View> "view.superView" |> Option.iter (fun v -> element.SuperView <- v )
        props |> Interop.getValue<bool> "view.superViewRendersLineCanvas" |> Option.iter (fun v -> element.SuperViewRendersLineCanvas <- v )
        props |> Interop.getValue<TabBehavior option> "view.tabStop" |> Option.iter (fun v -> element.TabStop <- v  |> Option.toNullable)
        props |> Interop.getValue<string> "view.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Interop.getValue<Alignment> "view.textAlignment" |> Option.iter (fun v -> element.TextAlignment <- v )
        props |> Interop.getValue<TextDirection> "view.textDirection" |> Option.iter (fun v -> element.TextDirection <- v )
        props |> Interop.getValue<string> "view.title" |> Option.iter (fun v -> element.Title <- v )
        props |> Interop.getValue<bool> "view.validatePosDim" |> Option.iter (fun v -> element.ValidatePosDim <- v )
        props |> Interop.getValue<Alignment> "view.verticalTextAlignment" |> Option.iter (fun v -> element.VerticalTextAlignment <- v )
        props |> Interop.getValue<Rectangle> "view.viewport" |> Option.iter (fun v -> element.Viewport <- v )
        props |> Interop.getValue<ViewportSettings> "view.viewportSettings" |> Option.iter (fun v -> element.ViewportSettings <- v )
        props |> Interop.getValue<bool> "view.visible" |> Option.iter (fun v -> element.Visible <- v )
        props |> Interop.getValue<bool> "view.wantContinuousButtonPressed" |> Option.iter (fun v -> element.WantContinuousButtonPressed <- v )
        props |> Interop.getValue<bool> "view.wantMousePositionReports" |> Option.iter (fun v -> element.WantMousePositionReports <- v )
        props |> Interop.getValue<Dim> "view.width" |> Option.iter (fun v -> element.Width <- v )
        props |> Interop.getValue<Pos> "view.x" |> Option.iter (fun v -> element.X <- v )
        props |> Interop.getValue<Pos> "view.y" |> Option.iter (fun v -> element.Y <- v )
        // Events
        props |> Interop.getValue<HandledEventArgs->unit> "view.accept" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accept @> v element)
        props |> Interop.getValue<SuperViewChangedEventArgs->unit> "view.added" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Added @> v element)
        props |> Interop.getValue<CancelEventArgs<LineStyle>->unit> "view.borderStyleChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.BorderStyleChanging @> v element)
        props |> Interop.getValue<unit->unit> "view.canFocusChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CanFocusChanged @> (fun _ -> v()) element)
        props |> Interop.getValue<SizeChangedEventArgs->unit> "view.contentSizeChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentSizeChanged @> v element)
        props |> Interop.getValue<unit->unit> "view.disposing" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Disposing @> (fun _ -> v()) element)
        props |> Interop.getValue<DrawEventArgs->unit> "view.drawContent" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawContent @> v element)
        props |> Interop.getValue<DrawEventArgs->unit> "view.drawContentComplete" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawContentComplete @> v element)
        props |> Interop.getValue<unit->unit> "view.enabledChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.EnabledChanged @> (fun _ -> v()) element)
        props |> Interop.getValue<HasFocusEventArgs->unit> "view.hasFocusChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.HasFocusChanged @> v element)
        props |> Interop.getValue<HasFocusEventArgs->unit> "view.hasFocusChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.HasFocusChanging @> v element)
        props |> Interop.getValue<CancelEventArgs<HighlightStyle>->unit> "view.highlight" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Highlight @> v element)
        props |> Interop.getValue<KeyChangedEventArgs->unit> "view.hotKeyChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.HotKeyChanged @> v element)
        props |> Interop.getValue<unit->unit> "view.initialized" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Initialized @> (fun _ -> v()) element)
        props |> Interop.getValue<Key->unit> "view.invokingKeyBindings" |> Option.iter (fun v -> Interop.setEventHandler <@ element.InvokingKeyBindings @> v element)
        props |> Interop.getValue<Key->unit> "view.keyDown" |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyDown @> v element)
        props |> Interop.getValue<Key->unit> "view.keyUp" |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyUp @> v element)
        props |> Interop.getValue<LayoutEventArgs->unit> "view.layoutComplete" |> Option.iter (fun v -> Interop.setEventHandler <@ element.LayoutComplete @> v element)
        props |> Interop.getValue<LayoutEventArgs->unit> "view.layoutStarted" |> Option.iter (fun v -> Interop.setEventHandler <@ element.LayoutStarted @> v element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseClick" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseClick @> v element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseEnter" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseEnter @> v element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseEvent" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseEvent @> v element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseLeave" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseLeave @> v element)
        props |> Interop.getValue<Key->unit> "view.processKeyDown" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ProcessKeyDown @> v element)
        props |> Interop.getValue<SuperViewChangedEventArgs->unit> "view.removed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Removed @> v element)
        props |> Interop.getValue<unit->unit> "view.textChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TextChanged @> (fun _ -> v()) element)
        props |> Interop.getValue<string->unit> "view.titleChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TitleChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<CancelEventArgs<string>->unit> "view.titleChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TitleChanging @> v element)
        props |> Interop.getValue<DrawEventArgs->unit> "view.viewportChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ViewportChanged @> v element)
        props |> Interop.getValue<unit->unit> "view.visibleChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.VisibleChanged @> (fun _ -> v()) element)

    let removeProps (element: View) props =
        // Properties
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun _ -> ())
        // Properties
        props |> Interop.getValue<ViewArrangement> "view.arrangement" |> Option.iter (fun _ -> element.Arrangement <- Unchecked.defaultof<_>)
        props |> Interop.getValue<LineStyle> "view.borderStyle" |> Option.iter (fun _ -> element.BorderStyle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.canFocus" |> Option.iter (fun _ -> element.CanFocus <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ColorScheme> "view.colorScheme" |> Option.iter (fun _ -> element.ColorScheme <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.contentSizeTracksViewport" |> Option.iter (fun _ -> element.ContentSizeTracksViewport <- Unchecked.defaultof<_>)
        props |> Interop.getValue<CursorVisibility> "view.cursorVisibility" |> Option.iter (fun _ -> element.CursorVisibility <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Object> "view.data" |> Option.iter (fun _ -> element.Data <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.enabled" |> Option.iter (fun _ -> element.Enabled <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rectangle> "view.frame" |> Option.iter (fun _ -> element.Frame <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.hasFocus" |> Option.iter (fun _ -> element.HasFocus <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Dim> "view.height" |> Option.iter (fun _ -> element.Height <- Unchecked.defaultof<_>)
        props |> Interop.getValue<HighlightStyle> "view.highlightStyle" |> Option.iter (fun _ -> element.HighlightStyle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Key> "view.hotKey" |> Option.iter (fun _ -> element.HotKey <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rune> "view.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "view.id" |> Option.iter (fun _ -> element.Id <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.isInitialized" |> Option.iter (fun _ -> element.IsInitialized <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.needsDisplay" |> Option.iter (fun _ -> element.NeedsDisplay <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.preserveTrailingSpaces" |> Option.iter (fun _ -> element.PreserveTrailingSpaces <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ShadowStyle> "view.shadowStyle" |> Option.iter (fun _ -> element.ShadowStyle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<View> "view.superView" |> Option.iter (fun _ -> element.SuperView <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.superViewRendersLineCanvas" |> Option.iter (fun _ -> element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)
        props |> Interop.getValue<TabBehavior option> "view.tabStop" |> Option.iter (fun _ -> element.TabStop <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "view.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Alignment> "view.textAlignment" |> Option.iter (fun _ -> element.TextAlignment <- Unchecked.defaultof<_>)
        props |> Interop.getValue<TextDirection> "view.textDirection" |> Option.iter (fun _ -> element.TextDirection <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "view.title" |> Option.iter (fun _ -> element.Title <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.validatePosDim" |> Option.iter (fun _ -> element.ValidatePosDim <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Alignment> "view.verticalTextAlignment" |> Option.iter (fun _ -> element.VerticalTextAlignment <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rectangle> "view.viewport" |> Option.iter (fun _ -> element.Viewport <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ViewportSettings> "view.viewportSettings" |> Option.iter (fun _ -> element.ViewportSettings <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.visible" |> Option.iter (fun _ -> element.Visible <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.wantContinuousButtonPressed" |> Option.iter (fun _ -> element.WantContinuousButtonPressed <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "view.wantMousePositionReports" |> Option.iter (fun _ -> element.WantMousePositionReports <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Dim> "view.width" |> Option.iter (fun _ -> element.Width <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Pos> "view.x" |> Option.iter (fun _ -> element.X <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Pos> "view.y" |> Option.iter (fun _ -> element.Y <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<HandledEventArgs->unit> "view.accept" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Accept @> element)
        props |> Interop.getValue<SuperViewChangedEventArgs->unit> "view.added" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Added @> element)
        props |> Interop.getValue<CancelEventArgs<LineStyle>->unit> "view.borderStyleChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.BorderStyleChanging @> element)
        props |> Interop.getValue<unit->unit> "view.canFocusChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CanFocusChanged @> element)
        props |> Interop.getValue<SizeChangedEventArgs->unit> "view.contentSizeChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentSizeChanged @> element)
        props |> Interop.getValue<unit->unit> "view.disposing" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Disposing @> element)
        props |> Interop.getValue<DrawEventArgs->unit> "view.drawContent" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawContent @> element)
        props |> Interop.getValue<DrawEventArgs->unit> "view.drawContentComplete" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawContentComplete @> element)
        props |> Interop.getValue<unit->unit> "view.enabledChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.EnabledChanged @> element)
        props |> Interop.getValue<HasFocusEventArgs->unit> "view.hasFocusChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HasFocusChanged @> element)
        props |> Interop.getValue<HasFocusEventArgs->unit> "view.hasFocusChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HasFocusChanging @> element)
        props |> Interop.getValue<CancelEventArgs<HighlightStyle>->unit> "view.highlight" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Highlight @> element)
        props |> Interop.getValue<KeyChangedEventArgs->unit> "view.hotKeyChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HotKeyChanged @> element)
        props |> Interop.getValue<unit->unit> "view.initialized" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Initialized @> element)
        props |> Interop.getValue<Key->unit> "view.invokingKeyBindings" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.InvokingKeyBindings @> element)
        props |> Interop.getValue<Key->unit> "view.keyDown" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyDown @> element)
        props |> Interop.getValue<Key->unit> "view.keyUp" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyUp @> element)
        props |> Interop.getValue<LayoutEventArgs->unit> "view.layoutComplete" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.LayoutComplete @> element)
        props |> Interop.getValue<LayoutEventArgs->unit> "view.layoutStarted" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.LayoutStarted @> element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseClick" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseClick @> element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseEnter" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseEnter @> element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseEvent" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseEvent @> element)
        props |> Interop.getValue<MouseEventEventArgs->unit> "view.mouseLeave" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseLeave @> element)
        props |> Interop.getValue<Key->unit> "view.processKeyDown" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ProcessKeyDown @> element)
        props |> Interop.getValue<SuperViewChangedEventArgs->unit> "view.removed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Removed @> element)
        props |> Interop.getValue<unit->unit> "view.textChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TextChanged @> element)
        props |> Interop.getValue<string->unit> "view.titleChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TitleChanged @> element)
        props |> Interop.getValue<CancelEventArgs<string>->unit> "view.titleChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TitleChanging @> element)
        props |> Interop.getValue<DrawEventArgs->unit> "view.viewportChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ViewportChanged @> element)
        props |> Interop.getValue<unit->unit> "view.visibleChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.VisibleChanged @> element)

    let canUpdate (view:View) props removedProps =
        let isPosCompatible (a:Pos) (b:Pos) =
            let nameA = a.GetType().Name
            let nameB = b.GetType().Name
            nameA = nameB ||
            (nameA = "PosAbsolute" && nameB = "PosAbsolute") ||
            (nameA <> "PosAbsolute" && nameB <> "PosAbsolute")

        let isDimCompatible (a:Dim) (b:Dim) =
            let nameA = a.GetType().Name
            let nameB = b.GetType().Name
            nameA = nameB ||
            (nameA = "DimAbsolute" && nameB = "DimAbsolute") ||
            (nameA <> "DimAbsolute" && nameB <> "DimAbsolute")

            
        let positionX = props |> Interop.getValue<Pos> "view.x"      |> Option.map (fun v -> isPosCompatible view.X v) |> Option.defaultValue true
        let positionY = props |> Interop.getValue<Pos> "view.y"      |> Option.map (fun v -> isPosCompatible view.Y v) |> Option.defaultValue true
        let width = props |> Interop.getValue<Dim> "view.width"      |> Option.map (fun v -> isDimCompatible view.Width v) |> Option.defaultValue true
        let height = props |> Interop.getValue<Dim> "view.height"      |> Option.map (fun v -> isDimCompatible view.Height v) |> Option.defaultValue true

        // in case width or height is removed!
        let widthNotRemoved  = removedProps |> Interop.valueExists "view.width"   |> not
        let heightNotRemoved = removedProps |> Interop.valueExists "view.height"  |> not

        [
            positionX
            positionY
            width
            height
            widthNotRemoved
            heightNotRemoved
        ]
        |> List.forall id
    
// Adornment
type AdornmentElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Adornment) props =
        // Properties
        props |> Interop.getValue<View> "adornment.parent" |> Option.iter (fun v -> element.Parent <- v )
        props |> Interop.getValue<View> "adornment.superView" |> Option.iter (fun v -> element.SuperView <- v )
        props |> Interop.getValue<bool> "adornment.superViewRendersLineCanvas" |> Option.iter (fun v -> element.SuperViewRendersLineCanvas <- v )
        props |> Interop.getValue<Thickness> "adornment.thickness" |> Option.iter (fun v -> element.Thickness <- v )
        props |> Interop.getValue<Rectangle> "adornment.viewport" |> Option.iter (fun v -> element.Viewport <- v )
        // Events
        props |> Interop.getValue<unit->unit> "adornment.thicknessChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ThicknessChanged @> (fun _ -> v()) element)

    let removeProps (element:Adornment) props =
        // Properties
        props |> Interop.getValue<View> "adornment.parent" |> Option.iter (fun _ -> element.Parent <- Unchecked.defaultof<_>)
        props |> Interop.getValue<View> "adornment.superView" |> Option.iter (fun _ -> element.SuperView <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "adornment.superViewRendersLineCanvas" |> Option.iter (fun _ -> element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Thickness> "adornment.thickness" |> Option.iter (fun _ -> element.Thickness <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rectangle> "adornment.viewport" |> Option.iter (fun _ -> element.Viewport <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<unit->unit> "adornment.thicknessChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ThicknessChanged @> element)

    override _.name = $"Adornment"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Adornment()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Adornment
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Border
type BorderElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Border) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "border.colorScheme" |> Option.iter (fun v -> element.ColorScheme <- v )
        props |> Interop.getValue<LineStyle> "border.lineStyle" |> Option.iter (fun v -> element.LineStyle <- v )
        props |> Interop.getValue<BorderSettings> "border.settings" |> Option.iter (fun v -> element.Settings <- v )

    let removeProps (element:Border) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "border.colorScheme" |> Option.iter (fun _ -> element.ColorScheme <- Unchecked.defaultof<_>)
        props |> Interop.getValue<LineStyle> "border.lineStyle" |> Option.iter (fun _ -> element.LineStyle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<BorderSettings> "border.settings" |> Option.iter (fun _ -> element.Settings <- Unchecked.defaultof<_>)

    override _.name = $"Border"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Border()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Border
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Margin
type MarginElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Margin) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "margin.colorScheme" |> Option.iter (fun v -> element.ColorScheme <- v )
        props |> Interop.getValue<ShadowStyle> "margin.shadowStyle" |> Option.iter (fun v -> element.ShadowStyle <- v )

    let removeProps (element:Margin) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "margin.colorScheme" |> Option.iter (fun _ -> element.ColorScheme <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ShadowStyle> "margin.shadowStyle" |> Option.iter (fun _ -> element.ShadowStyle <- Unchecked.defaultof<_>)

    override _.name = $"Margin"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Margin()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Margin
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Padding
type PaddingElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Padding) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "padding.colorScheme" |> Option.iter (fun v -> element.ColorScheme <- v )

    let removeProps (element:Padding) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "padding.colorScheme" |> Option.iter (fun _ -> element.ColorScheme <- Unchecked.defaultof<_>)

    override _.name = $"Padding"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Padding()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Padding
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Bar
type BarElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Bar) props =
        // Properties
        props |> Interop.getValue<AlignmentModes> "bar.alignmentModes" |> Option.iter (fun v -> element.AlignmentModes <- v )
        props |> Interop.getValue<Orientation> "bar.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        // Events
        props |> Interop.getValue<Orientation->unit> "bar.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "bar.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    let removeProps (element:Bar) props =
        // Properties
        props |> Interop.getValue<AlignmentModes> "bar.alignmentModes" |> Option.iter (fun _ -> element.AlignmentModes <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Orientation> "bar.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<Orientation->unit> "bar.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "bar.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Bar"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Bar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Bar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Button
type ButtonElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Button) props =
        // Properties
        props |> Interop.getValue<Rune> "button.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Interop.getValue<bool> "button.isDefault" |> Option.iter (fun v -> element.IsDefault <- v )
        props |> Interop.getValue<bool> "button.noDecorations" |> Option.iter (fun v -> element.NoDecorations <- v )
        props |> Interop.getValue<bool> "button.noPadding" |> Option.iter (fun v -> element.NoPadding <- v )
        props |> Interop.getValue<string> "button.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Interop.getValue<bool> "button.wantContinuousButtonPressed" |> Option.iter (fun v -> element.WantContinuousButtonPressed <- v )

    let removeProps (element:Button) props =
        // Properties
        props |> Interop.getValue<Rune> "button.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "button.isDefault" |> Option.iter (fun _ -> element.IsDefault <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "button.noDecorations" |> Option.iter (fun _ -> element.NoDecorations <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "button.noPadding" |> Option.iter (fun _ -> element.NoPadding <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "button.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "button.wantContinuousButtonPressed" |> Option.iter (fun _ -> element.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    override _.name = $"Button"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Button()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Button
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// CheckBox
type CheckBoxElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: CheckBox) props =
        // Properties
        props |> Interop.getValue<bool> "checkBox.allowCheckStateNone" |> Option.iter (fun v -> element.AllowCheckStateNone <- v )
        props |> Interop.getValue<CheckState> "checkBox.checkedState" |> Option.iter (fun v -> element.CheckedState <- v )
        props |> Interop.getValue<Rune> "checkBox.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Interop.getValue<string> "checkBox.text" |> Option.iter (fun v -> element.Text <- v )
        // Events
        props |> Interop.getValue<CancelEventArgs<CheckState>->unit> "checkBox.checkedStateChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CheckedStateChanging @> v element)

    let removeProps (element:CheckBox) props =
        // Properties
        props |> Interop.getValue<bool> "checkBox.allowCheckStateNone" |> Option.iter (fun _ -> element.AllowCheckStateNone <- Unchecked.defaultof<_>)
        props |> Interop.getValue<CheckState> "checkBox.checkedState" |> Option.iter (fun _ -> element.CheckedState <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rune> "checkBox.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "checkBox.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<CancelEventArgs<CheckState>->unit> "checkBox.checkedStateChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CheckedStateChanging @> element)

    override _.name = $"CheckBox"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new CheckBox()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> CheckBox
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// ColorPicker
type ColorPickerElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: ColorPicker) props =
        // Properties
        props |> Interop.getValue<Color> "colorPicker.selectedColor" |> Option.iter (fun v -> element.SelectedColor <- v )
        props |> Interop.getValue<ColorPickerStyle> "colorPicker.style" |> Option.iter (fun v -> element.Style <- v )
        // Events
        props |> Interop.getValue<ColorEventArgs->unit> "colorPicker.colorChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ColorChanged @> v element)

    let removeProps (element:ColorPicker) props =
        // Properties
        props |> Interop.getValue<Color> "colorPicker.selectedColor" |> Option.iter (fun _ -> element.SelectedColor <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ColorPickerStyle> "colorPicker.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<ColorEventArgs->unit> "colorPicker.colorChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ColorChanged @> element)

    override _.name = $"ColorPicker"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ColorPicker()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> ColorPicker
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// ColorPicker16
type ColorPicker16Element(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: ColorPicker16) props =
        // Properties
        props |> Interop.getValue<int> "colorPicker16.boxHeight" |> Option.iter (fun v -> element.BoxHeight <- v )
        props |> Interop.getValue<int> "colorPicker16.boxWidth" |> Option.iter (fun v -> element.BoxWidth <- v )
        props |> Interop.getValue<Point> "colorPicker16.cursor" |> Option.iter (fun v -> element.Cursor <- v )
        props |> Interop.getValue<ColorName> "colorPicker16.selectedColor" |> Option.iter (fun v -> element.SelectedColor <- v )
        // Events
        props |> Interop.getValue<ColorEventArgs->unit> "colorPicker16.colorChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ColorChanged @> v element)

    let removeProps (element:ColorPicker16) props =
        // Properties
        props |> Interop.getValue<int> "colorPicker16.boxHeight" |> Option.iter (fun _ -> element.BoxHeight <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "colorPicker16.boxWidth" |> Option.iter (fun _ -> element.BoxWidth <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Point> "colorPicker16.cursor" |> Option.iter (fun _ -> element.Cursor <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ColorName> "colorPicker16.selectedColor" |> Option.iter (fun _ -> element.SelectedColor <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<ColorEventArgs->unit> "colorPicker16.colorChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ColorChanged @> element)

    override _.name = $"ColorPicker16"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ColorPicker16()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> ColorPicker16
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// ComboBox
type ComboBoxElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: ComboBox) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "comboBox.colorScheme" |> Option.iter (fun v -> element.ColorScheme <- v )
        props |> Interop.getValue<bool> "comboBox.hideDropdownListOnClick" |> Option.iter (fun v -> element.HideDropdownListOnClick <- v )
        props |> Interop.getValue<bool> "comboBox.readOnly" |> Option.iter (fun v -> element.ReadOnly <- v )
        props |> Interop.getValue<string> "comboBox.searchText" |> Option.iter (fun v -> element.SearchText <- v )
        props |> Interop.getValue<int> "comboBox.selectedItem" |> Option.iter (fun v -> element.SelectedItem <- v )
        props |> Interop.getValue<IListDataSource> "comboBox.source" |> Option.iter (fun v -> element.Source <- v )
        props |> Interop.getValue<string> "comboBox.text" |> Option.iter (fun v -> element.Text <- v )
        // Events
        props |> Interop.getValue<unit->unit> "comboBox.collapsed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Collapsed @> (fun _ -> v()) element)
        props |> Interop.getValue<unit->unit> "comboBox.expanded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Expanded @> (fun _ -> v()) element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "comboBox.openSelectedItem" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OpenSelectedItem @> v element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "comboBox.selectedItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)

    let removeProps (element:ComboBox) props =
        // Properties
        props |> Interop.getValue<ColorScheme> "comboBox.colorScheme" |> Option.iter (fun _ -> element.ColorScheme <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "comboBox.hideDropdownListOnClick" |> Option.iter (fun _ -> element.HideDropdownListOnClick <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "comboBox.readOnly" |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "comboBox.searchText" |> Option.iter (fun _ -> element.SearchText <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "comboBox.selectedItem" |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)
        props |> Interop.getValue<IListDataSource> "comboBox.source" |> Option.iter (fun _ -> element.Source <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "comboBox.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<unit->unit> "comboBox.collapsed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Collapsed @> element)
        props |> Interop.getValue<unit->unit> "comboBox.expanded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Expanded @> element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "comboBox.openSelectedItem" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OpenSelectedItem @> element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "comboBox.selectedItemChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

    override _.name = $"ComboBox"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ComboBox()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> ComboBox
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// DateField
type DateFieldElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: DateField) props =
        // Properties
        props |> Interop.getValue<CultureInfo> "dateField.culture" |> Option.iter (fun v -> element.Culture <- v )
        props |> Interop.getValue<int> "dateField.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Interop.getValue<DateTime> "dateField.date" |> Option.iter (fun v -> element.Date <- v )
        // Events
        props |> Interop.getValue<DateTimeEventArgs<DateTime>->unit> "dateField.dateChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DateChanged @> v element)

    let removeProps (element:DateField) props =
        // Properties
        props |> Interop.getValue<CultureInfo> "dateField.culture" |> Option.iter (fun _ -> element.Culture <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "dateField.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Interop.getValue<DateTime> "dateField.date" |> Option.iter (fun _ -> element.Date <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<DateTimeEventArgs<DateTime>->unit> "dateField.dateChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DateChanged @> element)

    override _.name = $"DateField"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new DateField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> DateField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// DatePicker
type DatePickerElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: DatePicker) props =
        // Properties
        props |> Interop.getValue<CultureInfo> "datePicker.culture" |> Option.iter (fun v -> element.Culture <- v )
        props |> Interop.getValue<DateTime> "datePicker.date" |> Option.iter (fun v -> element.Date <- v )

    let removeProps (element:DatePicker) props =
        // Properties
        props |> Interop.getValue<CultureInfo> "datePicker.culture" |> Option.iter (fun _ -> element.Culture <- Unchecked.defaultof<_>)
        props |> Interop.getValue<DateTime> "datePicker.date" |> Option.iter (fun _ -> element.Date <- Unchecked.defaultof<_>)

    override _.name = $"DatePicker"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new DatePicker()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> DatePicker
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Dialog
type DialogElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Dialog) props =
        // Properties
        props |> Interop.getValue<Alignment> "dialog.buttonAlignment" |> Option.iter (fun v -> element.ButtonAlignment <- v )
        props |> Interop.getValue<AlignmentModes> "dialog.buttonAlignmentModes" |> Option.iter (fun v -> element.ButtonAlignmentModes <- v )
        props |> Interop.getValue<bool> "dialog.canceled" |> Option.iter (fun v -> element.Canceled <- v )

    let removeProps (element:Dialog) props =
        // Properties
        props |> Interop.getValue<Alignment> "dialog.buttonAlignment" |> Option.iter (fun _ -> element.ButtonAlignment <- Unchecked.defaultof<_>)
        props |> Interop.getValue<AlignmentModes> "dialog.buttonAlignmentModes" |> Option.iter (fun _ -> element.ButtonAlignmentModes <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "dialog.canceled" |> Option.iter (fun _ -> element.Canceled <- Unchecked.defaultof<_>)

    override _.name = $"Dialog"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Dialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Dialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// FileDialog
type FileDialogElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: FileDialog) props =
        // Properties
        props |> Interop.getValue<IAllowedType list> "fileDialog.allowedTypes" |> Option.iter (fun v -> element.AllowedTypes <- v.ToList())
        props |> Interop.getValue<bool> "fileDialog.allowsMultipleSelection" |> Option.iter (fun v -> element.AllowsMultipleSelection <- v )
        props |> Interop.getValue<IFileOperations> "fileDialog.fileOperationsHandler" |> Option.iter (fun v -> element.FileOperationsHandler <- v )
        props |> Interop.getValue<bool> "fileDialog.mustExist" |> Option.iter (fun v -> element.MustExist <- v )
        props |> Interop.getValue<OpenMode> "fileDialog.openMode" |> Option.iter (fun v -> element.OpenMode <- v )
        props |> Interop.getValue<string> "fileDialog.path" |> Option.iter (fun v -> element.Path <- v )
        props |> Interop.getValue<ISearchMatcher> "fileDialog.searchMatcher" |> Option.iter (fun v -> element.SearchMatcher <- v )
        // Events
        props |> Interop.getValue<FilesSelectedEventArgs->unit> "fileDialog.filesSelected" |> Option.iter (fun v -> Interop.setEventHandler <@ element.FilesSelected @> v element)

    let removeProps (element:FileDialog) props =
        // Properties
        props |> Interop.getValue<IAllowedType list> "fileDialog.allowedTypes" |> Option.iter (fun _ -> element.AllowedTypes <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "fileDialog.allowsMultipleSelection" |> Option.iter (fun _ -> element.AllowsMultipleSelection <- Unchecked.defaultof<_>)
        props |> Interop.getValue<IFileOperations> "fileDialog.fileOperationsHandler" |> Option.iter (fun _ -> element.FileOperationsHandler <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "fileDialog.mustExist" |> Option.iter (fun _ -> element.MustExist <- Unchecked.defaultof<_>)
        props |> Interop.getValue<OpenMode> "fileDialog.openMode" |> Option.iter (fun _ -> element.OpenMode <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "fileDialog.path" |> Option.iter (fun _ -> element.Path <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ISearchMatcher> "fileDialog.searchMatcher" |> Option.iter (fun _ -> element.SearchMatcher <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<FilesSelectedEventArgs->unit> "fileDialog.filesSelected" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FilesSelected @> element)

    override _.name = $"FileDialog"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new FileDialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> FileDialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// FrameView
type FrameViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: FrameView) props =
        // No properties or events FrameView
        ()

    let removeProps (element:FrameView) props =
        // No properties or events FrameView
        ()

    override _.name = $"FrameView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new FrameView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> FrameView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// LegendAnnotation
type LegendAnnotationElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: LegendAnnotation) props =
        // No properties or events LegendAnnotation
        ()

    let removeProps (element:LegendAnnotation) props =
        // No properties or events LegendAnnotation
        ()

    override _.name = $"LegendAnnotation"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new LegendAnnotation()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> LegendAnnotation
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// GraphView
type GraphViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: GraphView) props =
        // Properties
        props |> Interop.getValue<HorizontalAxis> "graphView.axisX" |> Option.iter (fun v -> element.AxisX <- v )
        props |> Interop.getValue<VerticalAxis> "graphView.axisY" |> Option.iter (fun v -> element.AxisY <- v )
        props |> Interop.getValue<PointF> "graphView.cellSize" |> Option.iter (fun v -> element.CellSize <- v )
        props |> Interop.getValue<Attribute option> "graphView.graphColor" |> Option.iter (fun v -> element.GraphColor <- v  |> Option.toNullable)
        props |> Interop.getValue<UInt32> "graphView.marginBottom" |> Option.iter (fun v -> element.MarginBottom <- v )
        props |> Interop.getValue<UInt32> "graphView.marginLeft" |> Option.iter (fun v -> element.MarginLeft <- v )
        props |> Interop.getValue<PointF> "graphView.scrollOffset" |> Option.iter (fun v -> element.ScrollOffset <- v )

    let removeProps (element:GraphView) props =
        // Properties
        props |> Interop.getValue<HorizontalAxis> "graphView.axisX" |> Option.iter (fun _ -> element.AxisX <- Unchecked.defaultof<_>)
        props |> Interop.getValue<VerticalAxis> "graphView.axisY" |> Option.iter (fun _ -> element.AxisY <- Unchecked.defaultof<_>)
        props |> Interop.getValue<PointF> "graphView.cellSize" |> Option.iter (fun _ -> element.CellSize <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Attribute option> "graphView.graphColor" |> Option.iter (fun _ -> element.GraphColor <- Unchecked.defaultof<_>)
        props |> Interop.getValue<UInt32> "graphView.marginBottom" |> Option.iter (fun _ -> element.MarginBottom <- Unchecked.defaultof<_>)
        props |> Interop.getValue<UInt32> "graphView.marginLeft" |> Option.iter (fun _ -> element.MarginLeft <- Unchecked.defaultof<_>)
        props |> Interop.getValue<PointF> "graphView.scrollOffset" |> Option.iter (fun _ -> element.ScrollOffset <- Unchecked.defaultof<_>)

    override _.name = $"GraphView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new GraphView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> GraphView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// HexView
type HexViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: HexView) props =
        // Properties
        props |> Interop.getValue<bool> "hexView.allowEdits" |> Option.iter (fun v -> element.AllowEdits <- v )
        props |> Interop.getValue<Int64> "hexView.displayStart" |> Option.iter (fun v -> element.DisplayStart <- v )
        props |> Interop.getValue<Stream> "hexView.source" |> Option.iter (fun v -> element.Source <- v )
        // Events
        props |> Interop.getValue<HexViewEditEventArgs->unit> "hexView.edited" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Edited @> v element)
        props |> Interop.getValue<HexViewEventArgs->unit> "hexView.positionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.PositionChanged @> v element)

    let removeProps (element:HexView) props =
        // Properties
        props |> Interop.getValue<bool> "hexView.allowEdits" |> Option.iter (fun _ -> element.AllowEdits <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Int64> "hexView.displayStart" |> Option.iter (fun _ -> element.DisplayStart <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Stream> "hexView.source" |> Option.iter (fun _ -> element.Source <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<HexViewEditEventArgs->unit> "hexView.edited" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Edited @> element)
        props |> Interop.getValue<HexViewEventArgs->unit> "hexView.positionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PositionChanged @> element)

    override _.name = $"HexView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new HexView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> HexView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Label
type LabelElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Label) props =
        // Properties
        props |> Interop.getValue<Rune> "label.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Interop.getValue<string> "label.text" |> Option.iter (fun v -> element.Text <- v )

    let removeProps (element:Label) props =
        // Properties
        props |> Interop.getValue<Rune> "label.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "label.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    override _.name = $"Label"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Label()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Label
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Line
type LineElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Line) props =
        // Properties
        props |> Interop.getValue<Orientation> "line.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        // Events
        props |> Interop.getValue<Orientation->unit> "line.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "line.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    let removeProps (element:Line) props =
        // Properties
        props |> Interop.getValue<Orientation> "line.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<Orientation->unit> "line.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "line.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Line"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Line()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Line
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// LineView
type LineViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: LineView) props =
        // Properties
        props |> Interop.getValue<Rune option> "lineView.endingAnchor" |> Option.iter (fun v -> element.EndingAnchor <- v  |> Option.toNullable)
        props |> Interop.getValue<Rune> "lineView.lineRune" |> Option.iter (fun v -> element.LineRune <- v )
        props |> Interop.getValue<Orientation> "lineView.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Interop.getValue<Rune option> "lineView.startingAnchor" |> Option.iter (fun v -> element.StartingAnchor <- v  |> Option.toNullable)

    let removeProps (element:LineView) props =
        // Properties
        props |> Interop.getValue<Rune option> "lineView.endingAnchor" |> Option.iter (fun _ -> element.EndingAnchor <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rune> "lineView.lineRune" |> Option.iter (fun _ -> element.LineRune <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Orientation> "lineView.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rune option> "lineView.startingAnchor" |> Option.iter (fun _ -> element.StartingAnchor <- Unchecked.defaultof<_>)

    override _.name = $"LineView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new LineView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> LineView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// ListView
type ListViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: ListView) props =
        // Properties
        props |> Interop.getValue<bool> "listView.allowsMarking" |> Option.iter (fun v -> element.AllowsMarking <- v )
        props |> Interop.getValue<bool> "listView.allowsMultipleSelection" |> Option.iter (fun v -> element.AllowsMultipleSelection <- v )
        props |> Interop.getValue<int> "listView.leftItem" |> Option.iter (fun v -> element.LeftItem <- v )
        props |> Interop.getValue<int> "listView.selectedItem" |> Option.iter (fun v -> element.SelectedItem <- v )
        props |> Interop.getValue<IListDataSource> "listView.source" |> Option.iter (fun v -> element.Source <- v )
        props |> Interop.getValue<int> "listView.topItem" |> Option.iter (fun v -> element.TopItem <- v )
        // Events
        props |> Interop.getValue<NotifyCollectionChangedEventArgs->unit> "listView.collectionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CollectionChanged @> v element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "listView.openSelectedItem" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OpenSelectedItem @> v element)
        props |> Interop.getValue<ListViewRowEventArgs->unit> "listView.rowRender" |> Option.iter (fun v -> Interop.setEventHandler <@ element.RowRender @> v element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "listView.selectedItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)

    let removeProps (element:ListView) props =
        // Properties
        props |> Interop.getValue<bool> "listView.allowsMarking" |> Option.iter (fun _ -> element.AllowsMarking <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "listView.allowsMultipleSelection" |> Option.iter (fun _ -> element.AllowsMultipleSelection <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "listView.leftItem" |> Option.iter (fun _ -> element.LeftItem <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "listView.selectedItem" |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)
        props |> Interop.getValue<IListDataSource> "listView.source" |> Option.iter (fun _ -> element.Source <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "listView.topItem" |> Option.iter (fun _ -> element.TopItem <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<NotifyCollectionChangedEventArgs->unit> "listView.collectionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CollectionChanged @> element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "listView.openSelectedItem" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OpenSelectedItem @> element)
        props |> Interop.getValue<ListViewRowEventArgs->unit> "listView.rowRender" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.RowRender @> element)
        props |> Interop.getValue<ListViewItemEventArgs->unit> "listView.selectedItemChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

    override _.name = $"ListView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ListView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> ListView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// MenuBar
type MenuBarElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: MenuBar) props =
        // Properties
        props |> Interop.getValue<Key> "menuBar.key" |> Option.iter (fun v -> element.Key <- v )
        props |> Interop.getValue<MenuBarItem[]> "menuBar.menus" |> Option.iter (fun v -> element.Menus <- v )
        props |> Interop.getValue<LineStyle> "menuBar.menusBorderStyle" |> Option.iter (fun v -> element.MenusBorderStyle <- v )
        props |> Interop.getValue<bool> "menuBar.useKeysUpDownAsKeysLeftRight" |> Option.iter (fun v -> element.UseKeysUpDownAsKeysLeftRight <- v )
        props |> Interop.getValue<bool> "menuBar.useSubMenusSingleFrame" |> Option.iter (fun v -> element.UseSubMenusSingleFrame <- v )
        props |> Interop.getValue<bool> "menuBar.visible" |> Option.iter (fun v -> element.Visible <- v )
        // Events
        props |> Interop.getValue<unit->unit> "menuBar.menuAllClosed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MenuAllClosed @> (fun _ -> v()) element)
        props |> Interop.getValue<MenuClosingEventArgs->unit> "menuBar.menuClosing" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MenuClosing @> v element)
        props |> Interop.getValue<MenuOpenedEventArgs->unit> "menuBar.menuOpened" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MenuOpened @> v element)
        props |> Interop.getValue<MenuOpeningEventArgs->unit> "menuBar.menuOpening" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MenuOpening @> v element)

    let removeProps (element:MenuBar) props =
        // Properties
        props |> Interop.getValue<Key> "menuBar.key" |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)
        props |> Interop.getValue<MenuBarItem[]> "menuBar.menus" |> Option.iter (fun _ -> element.Menus <- Unchecked.defaultof<_>)
        props |> Interop.getValue<LineStyle> "menuBar.menusBorderStyle" |> Option.iter (fun _ -> element.MenusBorderStyle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "menuBar.useKeysUpDownAsKeysLeftRight" |> Option.iter (fun _ -> element.UseKeysUpDownAsKeysLeftRight <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "menuBar.useSubMenusSingleFrame" |> Option.iter (fun _ -> element.UseSubMenusSingleFrame <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "menuBar.visible" |> Option.iter (fun _ -> element.Visible <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<unit->unit> "menuBar.menuAllClosed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MenuAllClosed @> element)
        props |> Interop.getValue<MenuClosingEventArgs->unit> "menuBar.menuClosing" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MenuClosing @> element)
        props |> Interop.getValue<MenuOpenedEventArgs->unit> "menuBar.menuOpened" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MenuOpened @> element)
        props |> Interop.getValue<MenuOpeningEventArgs->unit> "menuBar.menuOpening" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MenuOpening @> element)

    override _.name = $"MenuBar"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new MenuBar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> MenuBar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// MenuBarv2
type MenuBarv2Element(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: MenuBarv2) props =
        // No properties or events MenuBarv2
        ()

    let removeProps (element:MenuBarv2) props =
        // No properties or events MenuBarv2
        ()

    override _.name = $"MenuBarv2"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new MenuBarv2()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> MenuBarv2
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Menuv2
type Menuv2Element(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Menuv2) props =
        // No properties or events Menuv2
        ()

    let removeProps (element:Menuv2) props =
        // No properties or events Menuv2
        ()

    override _.name = $"Menuv2"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Menuv2()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Menuv2
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// NumericUpDown<'a>
type NumericUpDownElement<'a>(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: NumericUpDown<'a>) props =
        // Properties
        props |> Interop.getValue<string> "numericUpDown`1.format" |> Option.iter (fun v -> element.Format <- v )
        props |> Interop.getValue<'a> "numericUpDown`1.increment" |> Option.iter (fun v -> element.Increment <- v )
        props |> Interop.getValue<'a> "numericUpDown`1.value" |> Option.iter (fun v -> element.Value <- v )
        // Events
        props |> Interop.getValue<string->unit> "numericUpDown`1.formatChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.FormatChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<'a->unit> "numericUpDown`1.incrementChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.IncrementChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<'a->unit> "numericUpDown`1.valueChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ValueChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<CancelEventArgs<'a>->unit> "numericUpDown`1.valueChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ValueChanging @> v element)

    let removeProps (element:NumericUpDown<'a>) props =
        // Properties
        props |> Interop.getValue<string> "numericUpDown`1.format" |> Option.iter (fun _ -> element.Format <- Unchecked.defaultof<_>)
        props |> Interop.getValue<'a> "numericUpDown`1.increment" |> Option.iter (fun _ -> element.Increment <- Unchecked.defaultof<_>)
        props |> Interop.getValue<'a> "numericUpDown`1.value" |> Option.iter (fun _ -> element.Value <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<string->unit> "numericUpDown`1.formatChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FormatChanged @> element)
        props |> Interop.getValue<'a->unit> "numericUpDown`1.incrementChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.IncrementChanged @> element)
        props |> Interop.getValue<'a->unit> "numericUpDown`1.valueChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ValueChanged @> element)
        props |> Interop.getValue<CancelEventArgs<'a>->unit> "numericUpDown`1.valueChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ValueChanging @> element)

    override _.name = $"NumericUpDown<'a>"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new NumericUpDown<'a>()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> NumericUpDown<'a>
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// NumericUpDown
type NumericUpDownElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: NumericUpDown) props =
        // No properties or events NumericUpDown
        ()

    let removeProps (element:NumericUpDown) props =
        // No properties or events NumericUpDown
        ()

    override _.name = $"NumericUpDown"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new NumericUpDown()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> NumericUpDown
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// OpenDialog
type OpenDialogElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: OpenDialog) props =
        // Properties
        props |> Interop.getValue<OpenMode> "openDialog.openMode" |> Option.iter (fun v -> element.OpenMode <- v )

    let removeProps (element:OpenDialog) props =
        // Properties
        props |> Interop.getValue<OpenMode> "openDialog.openMode" |> Option.iter (fun _ -> element.OpenMode <- Unchecked.defaultof<_>)

    override _.name = $"OpenDialog"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new OpenDialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> OpenDialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// ProgressBar
type ProgressBarElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: ProgressBar) props =
        // Properties
        props |> Interop.getValue<bool> "progressBar.bidirectionalMarquee" |> Option.iter (fun v -> element.BidirectionalMarquee <- v )
        props |> Interop.getValue<Single> "progressBar.fraction" |> Option.iter (fun v -> element.Fraction <- v )
        props |> Interop.getValue<ProgressBarFormat> "progressBar.progressBarFormat" |> Option.iter (fun v -> element.ProgressBarFormat <- v )
        props |> Interop.getValue<ProgressBarStyle> "progressBar.progressBarStyle" |> Option.iter (fun v -> element.ProgressBarStyle <- v )
        props |> Interop.getValue<Rune> "progressBar.segmentCharacter" |> Option.iter (fun v -> element.SegmentCharacter <- v )
        props |> Interop.getValue<string> "progressBar.text" |> Option.iter (fun v -> element.Text <- v )

    let removeProps (element:ProgressBar) props =
        // Properties
        props |> Interop.getValue<bool> "progressBar.bidirectionalMarquee" |> Option.iter (fun _ -> element.BidirectionalMarquee <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Single> "progressBar.fraction" |> Option.iter (fun _ -> element.Fraction <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ProgressBarFormat> "progressBar.progressBarFormat" |> Option.iter (fun _ -> element.ProgressBarFormat <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ProgressBarStyle> "progressBar.progressBarStyle" |> Option.iter (fun _ -> element.ProgressBarStyle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Rune> "progressBar.segmentCharacter" |> Option.iter (fun _ -> element.SegmentCharacter <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "progressBar.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    override _.name = $"ProgressBar"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ProgressBar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> ProgressBar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// RadioGroup
type RadioGroupElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: RadioGroup) props =
        // Properties
        props |> Interop.getValue<int> "radioGroup.horizontalSpace" |> Option.iter (fun v -> element.HorizontalSpace <- v )
        props |> Interop.getValue<Orientation> "radioGroup.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Interop.getValue<string list> "radioGroup.radioLabels" |> Option.iter (fun v -> element.RadioLabels <- v |> List.toArray)
        props |> Interop.getValue<int> "radioGroup.selectedItem" |> Option.iter (fun v -> element.SelectedItem <- v )
        // Events
        props |> Interop.getValue<Orientation->unit> "radioGroup.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "radioGroup.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)
        props |> Interop.getValue<SelectedItemChangedArgs->unit> "radioGroup.selectedItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)

    let removeProps (element:RadioGroup) props =
        // Properties
        props |> Interop.getValue<int> "radioGroup.horizontalSpace" |> Option.iter (fun _ -> element.HorizontalSpace <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Orientation> "radioGroup.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string list> "radioGroup.radioLabels" |> Option.iter (fun _ -> element.RadioLabels <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "radioGroup.selectedItem" |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<Orientation->unit> "radioGroup.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "radioGroup.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)
        props |> Interop.getValue<SelectedItemChangedArgs->unit> "radioGroup.selectedItemChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

    override _.name = $"RadioGroup"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new RadioGroup()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> RadioGroup
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// SaveDialog
type SaveDialogElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: SaveDialog) props =
        // No properties or events SaveDialog
        ()

    let removeProps (element:SaveDialog) props =
        // No properties or events SaveDialog
        ()

    override _.name = $"SaveDialog"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new SaveDialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> SaveDialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// ScrollBarView
type ScrollBarViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: ScrollBarView) props =
        // Properties
        props |> Interop.getValue<bool> "scrollBarView.autoHideScrollBars" |> Option.iter (fun v -> element.AutoHideScrollBars <- v )
        props |> Interop.getValue<bool> "scrollBarView.isVertical" |> Option.iter (fun v -> element.IsVertical <- v )
        props |> Interop.getValue<bool> "scrollBarView.keepContentAlwaysInViewport" |> Option.iter (fun v -> element.KeepContentAlwaysInViewport <- v )
        props |> Interop.getValue<ScrollBarView> "scrollBarView.otherScrollBarView" |> Option.iter (fun v -> element.OtherScrollBarView <- v )
        props |> Interop.getValue<int> "scrollBarView.position" |> Option.iter (fun v -> element.Position <- v )
        props |> Interop.getValue<bool> "scrollBarView.showScrollIndicator" |> Option.iter (fun v -> element.ShowScrollIndicator <- v )
        props |> Interop.getValue<int> "scrollBarView.size" |> Option.iter (fun v -> element.Size <- v )
        // Events
        props |> Interop.getValue<unit->unit> "scrollBarView.changedPosition" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ChangedPosition @> (fun _ -> v()) element)

    let removeProps (element:ScrollBarView) props =
        // Properties
        props |> Interop.getValue<bool> "scrollBarView.autoHideScrollBars" |> Option.iter (fun _ -> element.AutoHideScrollBars <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "scrollBarView.isVertical" |> Option.iter (fun _ -> element.IsVertical <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "scrollBarView.keepContentAlwaysInViewport" |> Option.iter (fun _ -> element.KeepContentAlwaysInViewport <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ScrollBarView> "scrollBarView.otherScrollBarView" |> Option.iter (fun _ -> element.OtherScrollBarView <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "scrollBarView.position" |> Option.iter (fun _ -> element.Position <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "scrollBarView.showScrollIndicator" |> Option.iter (fun _ -> element.ShowScrollIndicator <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "scrollBarView.size" |> Option.iter (fun _ -> element.Size <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<unit->unit> "scrollBarView.changedPosition" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ChangedPosition @> element)

    override _.name = $"ScrollBarView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ScrollBarView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> ScrollBarView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// ScrollView
type ScrollViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: ScrollView) props =
        // Properties
        props |> Interop.getValue<bool> "scrollView.autoHideScrollBars" |> Option.iter (fun v -> element.AutoHideScrollBars <- v )
        props |> Interop.getValue<Point> "scrollView.contentOffset" |> Option.iter (fun v -> element.ContentOffset <- v )
        props |> Interop.getValue<bool> "scrollView.keepContentAlwaysInViewport" |> Option.iter (fun v -> element.KeepContentAlwaysInViewport <- v )
        props |> Interop.getValue<bool> "scrollView.showHorizontalScrollIndicator" |> Option.iter (fun v -> element.ShowHorizontalScrollIndicator <- v )
        props |> Interop.getValue<bool> "scrollView.showVerticalScrollIndicator" |> Option.iter (fun v -> element.ShowVerticalScrollIndicator <- v )

    let removeProps (element:ScrollView) props =
        // Properties
        props |> Interop.getValue<bool> "scrollView.autoHideScrollBars" |> Option.iter (fun _ -> element.AutoHideScrollBars <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Point> "scrollView.contentOffset" |> Option.iter (fun _ -> element.ContentOffset <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "scrollView.keepContentAlwaysInViewport" |> Option.iter (fun _ -> element.KeepContentAlwaysInViewport <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "scrollView.showHorizontalScrollIndicator" |> Option.iter (fun _ -> element.ShowHorizontalScrollIndicator <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "scrollView.showVerticalScrollIndicator" |> Option.iter (fun _ -> element.ShowVerticalScrollIndicator <- Unchecked.defaultof<_>)

    override _.name = $"ScrollView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ScrollView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> ScrollView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Shortcut
type ShortcutElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Shortcut) props =
        // Properties
        props |> Interop.getValue<Action> "shortcut.action" |> Option.iter (fun v -> element.Action <- v )
        props |> Interop.getValue<AlignmentModes> "shortcut.alignmentModes" |> Option.iter (fun v -> element.AlignmentModes <- v )
        props |> Interop.getValue<ColorScheme> "shortcut.colorScheme" |> Option.iter (fun v -> element.ColorScheme <- v )
        props |> Interop.getValue<View> "shortcut.commandView" |> Option.iter (fun v -> element.CommandView <- v )
        props |> Interop.getValue<string> "shortcut.helpText" |> Option.iter (fun v -> element.HelpText <- v )
        props |> Interop.getValue<Key> "shortcut.key" |> Option.iter (fun v -> element.Key <- v )
        props |> Interop.getValue<KeyBindingScope> "shortcut.keyBindingScope" |> Option.iter (fun v -> element.KeyBindingScope <- v )
        props |> Interop.getValue<int> "shortcut.minimumKeyTextSize" |> Option.iter (fun v -> element.MinimumKeyTextSize <- v )
        props |> Interop.getValue<Orientation> "shortcut.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Interop.getValue<string> "shortcut.text" |> Option.iter (fun v -> element.Text <- v )
        // Events
        props |> Interop.getValue<Orientation->unit> "shortcut.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "shortcut.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    let removeProps (element:Shortcut) props =
        // Properties
        props |> Interop.getValue<Action> "shortcut.action" |> Option.iter (fun _ -> element.Action <- Unchecked.defaultof<_>)
        props |> Interop.getValue<AlignmentModes> "shortcut.alignmentModes" |> Option.iter (fun _ -> element.AlignmentModes <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ColorScheme> "shortcut.colorScheme" |> Option.iter (fun _ -> element.ColorScheme <- Unchecked.defaultof<_>)
        props |> Interop.getValue<View> "shortcut.commandView" |> Option.iter (fun _ -> element.CommandView <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "shortcut.helpText" |> Option.iter (fun _ -> element.HelpText <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Key> "shortcut.key" |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)
        props |> Interop.getValue<KeyBindingScope> "shortcut.keyBindingScope" |> Option.iter (fun _ -> element.KeyBindingScope <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "shortcut.minimumKeyTextSize" |> Option.iter (fun _ -> element.MinimumKeyTextSize <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Orientation> "shortcut.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "shortcut.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<Orientation->unit> "shortcut.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "shortcut.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Shortcut"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Shortcut()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Shortcut
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Slider
type SliderElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Slider) props =
        // No properties or events Slider
        ()

    let removeProps (element:Slider) props =
        // No properties or events Slider
        ()

    override _.name = $"Slider"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Slider()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Slider
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Slider<'a>
type SliderElement<'a>(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Slider<'a>) props =
        // Properties
        props |> Interop.getValue<bool> "slider`1.allowEmpty" |> Option.iter (fun v -> element.AllowEmpty <- v )
        props |> Interop.getValue<int> "slider`1.focusedOption" |> Option.iter (fun v -> element.FocusedOption <- v )
        props |> Interop.getValue<Orientation> "slider`1.legendsOrientation" |> Option.iter (fun v -> element.LegendsOrientation <- v )
        props |> Interop.getValue<int> "slider`1.minimumInnerSpacing" |> Option.iter (fun v -> element.MinimumInnerSpacing <- v )
        props |> Interop.getValue<SliderOption<'a> list> "slider`1.options" |> Option.iter (fun v -> element.Options <- v.ToList())
        props |> Interop.getValue<Orientation> "slider`1.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Interop.getValue<bool> "slider`1.rangeAllowSingle" |> Option.iter (fun v -> element.RangeAllowSingle <- v )
        props |> Interop.getValue<bool> "slider`1.showEndSpacing" |> Option.iter (fun v -> element.ShowEndSpacing <- v )
        props |> Interop.getValue<bool> "slider`1.showLegends" |> Option.iter (fun v -> element.ShowLegends <- v )
        props |> Interop.getValue<SliderStyle> "slider`1.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Interop.getValue<string> "slider`1.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Interop.getValue<SliderType> "slider`1.``type``" |> Option.iter (fun v -> element.Type <- v )
        props |> Interop.getValue<bool> "slider`1.useMinimumSize" |> Option.iter (fun v -> element.UseMinimumSize <- v )
        // Events
        props |> Interop.getValue<SliderEventArgs<'a>->unit> "slider`1.optionFocused" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OptionFocused @> v element)
        props |> Interop.getValue<SliderEventArgs<'a>->unit> "slider`1.optionsChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OptionsChanged @> v element)
        props |> Interop.getValue<Orientation->unit> "slider`1.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.CurrentValue) element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "slider`1.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    let removeProps (element:Slider<'a>) props =
        // Properties
        props |> Interop.getValue<bool> "slider`1.allowEmpty" |> Option.iter (fun _ -> element.AllowEmpty <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "slider`1.focusedOption" |> Option.iter (fun _ -> element.FocusedOption <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Orientation> "slider`1.legendsOrientation" |> Option.iter (fun _ -> element.LegendsOrientation <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "slider`1.minimumInnerSpacing" |> Option.iter (fun _ -> element.MinimumInnerSpacing <- Unchecked.defaultof<_>)
        props |> Interop.getValue<SliderOption<'a> list> "slider`1.options" |> Option.iter (fun _ -> element.Options <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Orientation> "slider`1.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "slider`1.rangeAllowSingle" |> Option.iter (fun _ -> element.RangeAllowSingle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "slider`1.showEndSpacing" |> Option.iter (fun _ -> element.ShowEndSpacing <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "slider`1.showLegends" |> Option.iter (fun _ -> element.ShowLegends <- Unchecked.defaultof<_>)
        props |> Interop.getValue<SliderStyle> "slider`1.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "slider`1.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Interop.getValue<SliderType> "slider`1.``type``" |> Option.iter (fun _ -> element.Type <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "slider`1.useMinimumSize" |> Option.iter (fun _ -> element.UseMinimumSize <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<SliderEventArgs<'a>->unit> "slider`1.optionFocused" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OptionFocused @> element)
        props |> Interop.getValue<SliderEventArgs<'a>->unit> "slider`1.optionsChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OptionsChanged @> element)
        props |> Interop.getValue<Orientation->unit> "slider`1.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Interop.getValue<CancelEventArgs<Orientation>->unit> "slider`1.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Slider<'a>"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Slider<'a>()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Slider<'a>
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// SpinnerView
type SpinnerViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: SpinnerView) props =
        // Properties
        props |> Interop.getValue<bool> "spinnerView.autoSpin" |> Option.iter (fun v -> element.AutoSpin <- v )
        props |> Interop.getValue<string list> "spinnerView.sequence" |> Option.iter (fun v -> element.Sequence <- v |> List.toArray)
        props |> Interop.getValue<bool> "spinnerView.spinBounce" |> Option.iter (fun v -> element.SpinBounce <- v )
        props |> Interop.getValue<int> "spinnerView.spinDelay" |> Option.iter (fun v -> element.SpinDelay <- v )
        props |> Interop.getValue<bool> "spinnerView.spinReverse" |> Option.iter (fun v -> element.SpinReverse <- v )
        props |> Interop.getValue<SpinnerStyle> "spinnerView.style" |> Option.iter (fun v -> element.Style <- v )

    let removeProps (element:SpinnerView) props =
        // Properties
        props |> Interop.getValue<bool> "spinnerView.autoSpin" |> Option.iter (fun _ -> element.AutoSpin <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string list> "spinnerView.sequence" |> Option.iter (fun _ -> element.Sequence <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "spinnerView.spinBounce" |> Option.iter (fun _ -> element.SpinBounce <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "spinnerView.spinDelay" |> Option.iter (fun _ -> element.SpinDelay <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "spinnerView.spinReverse" |> Option.iter (fun _ -> element.SpinReverse <- Unchecked.defaultof<_>)
        props |> Interop.getValue<SpinnerStyle> "spinnerView.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)

    override _.name = $"SpinnerView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new SpinnerView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> SpinnerView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// StatusBar
type StatusBarElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: StatusBar) props =
        // No properties or events StatusBar
        ()

    let removeProps (element:StatusBar) props =
        // No properties or events StatusBar
        ()

    override _.name = $"StatusBar"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new StatusBar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> StatusBar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Tab
type TabElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Tab) props =
        // Properties
        props |> Interop.getValue<string> "tab.displayText" |> Option.iter (fun v -> element.DisplayText <- v )
        props |> Interop.getValue<View> "tab.view" |> Option.iter (fun v -> element.View <- v )

    let removeProps (element:Tab) props =
        // Properties
        props |> Interop.getValue<string> "tab.displayText" |> Option.iter (fun _ -> element.DisplayText <- Unchecked.defaultof<_>)
        props |> Interop.getValue<View> "tab.view" |> Option.iter (fun _ -> element.View <- Unchecked.defaultof<_>)

    override _.name = $"Tab"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Tab()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Tab
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TableView
type TableViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TableView) props =
        // Properties
        props |> Interop.getValue<KeyCode> "tableView.cellActivationKey" |> Option.iter (fun v -> element.CellActivationKey <- v )
        props |> Interop.getValue<CollectionNavigatorBase> "tableView.collectionNavigator" |> Option.iter (fun v -> element.CollectionNavigator <- v )
        props |> Interop.getValue<int> "tableView.columnOffset" |> Option.iter (fun v -> element.ColumnOffset <- v )
        props |> Interop.getValue<bool> "tableView.fullRowSelect" |> Option.iter (fun v -> element.FullRowSelect <- v )
        props |> Interop.getValue<int> "tableView.maxCellWidth" |> Option.iter (fun v -> element.MaxCellWidth <- v )
        props |> Interop.getValue<int> "tableView.minCellWidth" |> Option.iter (fun v -> element.MinCellWidth <- v )
        props |> Interop.getValue<bool> "tableView.multiSelect" |> Option.iter (fun v -> element.MultiSelect <- v )
        props |> Interop.getValue<string> "tableView.nullSymbol" |> Option.iter (fun v -> element.NullSymbol <- v )
        props |> Interop.getValue<int> "tableView.rowOffset" |> Option.iter (fun v -> element.RowOffset <- v )
        props |> Interop.getValue<int> "tableView.selectedColumn" |> Option.iter (fun v -> element.SelectedColumn <- v )
        props |> Interop.getValue<int> "tableView.selectedRow" |> Option.iter (fun v -> element.SelectedRow <- v )
        props |> Interop.getValue<Char> "tableView.separatorSymbol" |> Option.iter (fun v -> element.SeparatorSymbol <- v )
        props |> Interop.getValue<TableStyle> "tableView.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Interop.getValue<ITableSource> "tableView.table" |> Option.iter (fun v -> element.Table <- v )
        // Events
        props |> Interop.getValue<CellActivatedEventArgs->unit> "tableView.cellActivated" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CellActivated @> v element)
        props |> Interop.getValue<CellToggledEventArgs->unit> "tableView.cellToggled" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CellToggled @> v element)
        props |> Interop.getValue<SelectedCellChangedEventArgs->unit> "tableView.selectedCellChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedCellChanged @> v element)

    let removeProps (element:TableView) props =
        // Properties
        props |> Interop.getValue<KeyCode> "tableView.cellActivationKey" |> Option.iter (fun _ -> element.CellActivationKey <- Unchecked.defaultof<_>)
        props |> Interop.getValue<CollectionNavigatorBase> "tableView.collectionNavigator" |> Option.iter (fun _ -> element.CollectionNavigator <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "tableView.columnOffset" |> Option.iter (fun _ -> element.ColumnOffset <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "tableView.fullRowSelect" |> Option.iter (fun _ -> element.FullRowSelect <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "tableView.maxCellWidth" |> Option.iter (fun _ -> element.MaxCellWidth <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "tableView.minCellWidth" |> Option.iter (fun _ -> element.MinCellWidth <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "tableView.multiSelect" |> Option.iter (fun _ -> element.MultiSelect <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "tableView.nullSymbol" |> Option.iter (fun _ -> element.NullSymbol <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "tableView.rowOffset" |> Option.iter (fun _ -> element.RowOffset <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "tableView.selectedColumn" |> Option.iter (fun _ -> element.SelectedColumn <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "tableView.selectedRow" |> Option.iter (fun _ -> element.SelectedRow <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Char> "tableView.separatorSymbol" |> Option.iter (fun _ -> element.SeparatorSymbol <- Unchecked.defaultof<_>)
        props |> Interop.getValue<TableStyle> "tableView.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ITableSource> "tableView.table" |> Option.iter (fun _ -> element.Table <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<CellActivatedEventArgs->unit> "tableView.cellActivated" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CellActivated @> element)
        props |> Interop.getValue<CellToggledEventArgs->unit> "tableView.cellToggled" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CellToggled @> element)
        props |> Interop.getValue<SelectedCellChangedEventArgs->unit> "tableView.selectedCellChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedCellChanged @> element)

    override _.name = $"TableView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TableView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TableView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TabView
type TabViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TabView) props =
        // Properties
        props |> Interop.getValue<UInt32> "tabView.maxTabTextWidth" |> Option.iter (fun v -> element.MaxTabTextWidth <- v )
        props |> Interop.getValue<Tab> "tabView.selectedTab" |> Option.iter (fun v -> element.SelectedTab <- v )
        props |> Interop.getValue<TabStyle> "tabView.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Interop.getValue<int> "tabView.tabScrollOffset" |> Option.iter (fun v -> element.TabScrollOffset <- v )
        // Events
        props |> Interop.getValue<TabChangedEventArgs->unit> "tabView.selectedTabChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedTabChanged @> v element)
        props |> Interop.getValue<TabMouseEventArgs->unit> "tabView.tabClicked" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TabClicked @> v element)

    let removeProps (element:TabView) props =
        // Properties
        props |> Interop.getValue<UInt32> "tabView.maxTabTextWidth" |> Option.iter (fun _ -> element.MaxTabTextWidth <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Tab> "tabView.selectedTab" |> Option.iter (fun _ -> element.SelectedTab <- Unchecked.defaultof<_>)
        props |> Interop.getValue<TabStyle> "tabView.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "tabView.tabScrollOffset" |> Option.iter (fun _ -> element.TabScrollOffset <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<TabChangedEventArgs->unit> "tabView.selectedTabChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedTabChanged @> element)
        props |> Interop.getValue<TabMouseEventArgs->unit> "tabView.tabClicked" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TabClicked @> element)

    override _.name = $"TabView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TabView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TabView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TextField
type TextFieldElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TextField) props =
        // Properties
        props |> Interop.getValue<IAutocomplete> "textField.autocomplete" |> Option.iter (fun v -> element.Autocomplete <- v )
        props |> Interop.getValue<string> "textField.caption" |> Option.iter (fun v -> element.Caption <- v )
        props |> Interop.getValue<Color> "textField.captionColor" |> Option.iter (fun v -> element.CaptionColor <- v )
        props |> Interop.getValue<int> "textField.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Interop.getValue<bool> "textField.readOnly" |> Option.iter (fun v -> element.ReadOnly <- v )
        props |> Interop.getValue<bool> "textField.secret" |> Option.iter (fun v -> element.Secret <- v )
        props |> Interop.getValue<int> "textField.selectedStart" |> Option.iter (fun v -> element.SelectedStart <- v )
        props |> Interop.getValue<string> "textField.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Interop.getValue<bool> "textField.used" |> Option.iter (fun v -> element.Used <- v )
        // Events
        props |> Interop.getValue<CancelEventArgs<string>->unit> "textField.textChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TextChanging @> v element)

    let removeProps (element:TextField) props =
        // Properties
        props |> Interop.getValue<IAutocomplete> "textField.autocomplete" |> Option.iter (fun _ -> element.Autocomplete <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "textField.caption" |> Option.iter (fun _ -> element.Caption <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Color> "textField.captionColor" |> Option.iter (fun _ -> element.CaptionColor <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "textField.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textField.readOnly" |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textField.secret" |> Option.iter (fun _ -> element.Secret <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "textField.selectedStart" |> Option.iter (fun _ -> element.SelectedStart <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "textField.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textField.used" |> Option.iter (fun _ -> element.Used <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<CancelEventArgs<string>->unit> "textField.textChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TextChanging @> element)

    override _.name = $"TextField"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TextField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TextField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TextValidateField
type TextValidateFieldElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TextValidateField) props =
        // Properties
        props |> Interop.getValue<ITextValidateProvider> "textValidateField.provider" |> Option.iter (fun v -> element.Provider <- v )
        props |> Interop.getValue<string> "textValidateField.text" |> Option.iter (fun v -> element.Text <- v )

    let removeProps (element:TextValidateField) props =
        // Properties
        props |> Interop.getValue<ITextValidateProvider> "textValidateField.provider" |> Option.iter (fun _ -> element.Provider <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "textValidateField.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    override _.name = $"TextValidateField"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TextValidateField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TextValidateField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TextView
type TextViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TextView) props =
        // Properties
        props |> Interop.getValue<bool> "textView.allowsReturn" |> Option.iter (fun v -> element.AllowsReturn <- v )
        props |> Interop.getValue<bool> "textView.allowsTab" |> Option.iter (fun v -> element.AllowsTab <- v )
        props |> Interop.getValue<Point> "textView.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Interop.getValue<bool> "textView.inheritsPreviousColorScheme" |> Option.iter (fun v -> element.InheritsPreviousColorScheme <- v )
        props |> Interop.getValue<bool> "textView.isDirty" |> Option.iter (fun v -> element.IsDirty <- v )
        props |> Interop.getValue<int> "textView.leftColumn" |> Option.iter (fun v -> element.LeftColumn <- v )
        props |> Interop.getValue<bool> "textView.multiline" |> Option.iter (fun v -> element.Multiline <- v )
        props |> Interop.getValue<bool> "textView.readOnly" |> Option.iter (fun v -> element.ReadOnly <- v )
        props |> Interop.getValue<bool> "textView.selecting" |> Option.iter (fun v -> element.Selecting <- v )
        props |> Interop.getValue<int> "textView.selectionStartColumn" |> Option.iter (fun v -> element.SelectionStartColumn <- v )
        props |> Interop.getValue<int> "textView.selectionStartRow" |> Option.iter (fun v -> element.SelectionStartRow <- v )
        props |> Interop.getValue<int> "textView.tabWidth" |> Option.iter (fun v -> element.TabWidth <- v )
        props |> Interop.getValue<string> "textView.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Interop.getValue<int> "textView.topRow" |> Option.iter (fun v -> element.TopRow <- v )
        props |> Interop.getValue<bool> "textView.used" |> Option.iter (fun v -> element.Used <- v )
        props |> Interop.getValue<bool> "textView.wordWrap" |> Option.iter (fun v -> element.WordWrap <- v )
        // Events
        props |> Interop.getValue<ContentsChangedEventArgs->unit> "textView.contentsChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentsChanged @> v element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawNormalColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawNormalColor @> v element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawReadOnlyColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawReadOnlyColor @> v element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawSelectionColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawSelectionColor @> v element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawUsedColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawUsedColor @> v element)
        props |> Interop.getValue<PointEventArgs->unit> "textView.unwrappedCursorPosition" |> Option.iter (fun v -> Interop.setEventHandler <@ element.UnwrappedCursorPosition @> v element)

        // Additional properties
        props |> Interop.getValue<string->unit> "textView.textChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentsChanged @> (fun _ -> v element.Text) element)
        

    let removeProps (element:TextView) props =
        // Properties
        props |> Interop.getValue<bool> "textView.allowsReturn" |> Option.iter (fun _ -> element.AllowsReturn <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.allowsTab" |> Option.iter (fun _ -> element.AllowsTab <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Point> "textView.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.inheritsPreviousColorScheme" |> Option.iter (fun _ -> element.InheritsPreviousColorScheme <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.isDirty" |> Option.iter (fun _ -> element.IsDirty <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "textView.leftColumn" |> Option.iter (fun _ -> element.LeftColumn <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.multiline" |> Option.iter (fun _ -> element.Multiline <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.readOnly" |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.selecting" |> Option.iter (fun _ -> element.Selecting <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "textView.selectionStartColumn" |> Option.iter (fun _ -> element.SelectionStartColumn <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "textView.selectionStartRow" |> Option.iter (fun _ -> element.SelectionStartRow <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "textView.tabWidth" |> Option.iter (fun _ -> element.TabWidth <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "textView.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "textView.topRow" |> Option.iter (fun _ -> element.TopRow <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.used" |> Option.iter (fun _ -> element.Used <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "textView.wordWrap" |> Option.iter (fun _ -> element.WordWrap <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<ContentsChangedEventArgs->unit> "textView.contentsChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentsChanged @> element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawNormalColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawNormalColor @> element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawReadOnlyColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawReadOnlyColor @> element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawSelectionColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawSelectionColor @> element)
        props |> Interop.getValue<RuneCellEventArgs->unit> "textView.drawUsedColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawUsedColor @> element)
        props |> Interop.getValue<PointEventArgs->unit> "textView.unwrappedCursorPosition" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.UnwrappedCursorPosition @> element)

        // Additional properties
        props |> Interop.getValue<string->unit> "textView.textChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentsChanged @> element)
        

    override _.name = $"TextView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TextView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TextView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TileView
type TileViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TileView) props =
        // Properties
        props |> Interop.getValue<LineStyle> "tileView.lineStyle" |> Option.iter (fun v -> element.LineStyle <- v )
        props |> Interop.getValue<Orientation> "tileView.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Interop.getValue<KeyCode> "tileView.toggleResizable" |> Option.iter (fun v -> element.ToggleResizable <- v )
        // Events
        props |> Interop.getValue<SplitterEventArgs->unit> "tileView.splitterMoved" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SplitterMoved @> v element)

    let removeProps (element:TileView) props =
        // Properties
        props |> Interop.getValue<LineStyle> "tileView.lineStyle" |> Option.iter (fun _ -> element.LineStyle <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Orientation> "tileView.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Interop.getValue<KeyCode> "tileView.toggleResizable" |> Option.iter (fun _ -> element.ToggleResizable <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<SplitterEventArgs->unit> "tileView.splitterMoved" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SplitterMoved @> element)

    override _.name = $"TileView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TileView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TileView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TimeField
type TimeFieldElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TimeField) props =
        // Properties
        props |> Interop.getValue<int> "timeField.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Interop.getValue<bool> "timeField.isShortFormat" |> Option.iter (fun v -> element.IsShortFormat <- v )
        props |> Interop.getValue<TimeSpan> "timeField.time" |> Option.iter (fun v -> element.Time <- v )
        // Events
        props |> Interop.getValue<DateTimeEventArgs<TimeSpan>->unit> "timeField.timeChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TimeChanged @> v element)

    let removeProps (element:TimeField) props =
        // Properties
        props |> Interop.getValue<int> "timeField.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "timeField.isShortFormat" |> Option.iter (fun _ -> element.IsShortFormat <- Unchecked.defaultof<_>)
        props |> Interop.getValue<TimeSpan> "timeField.time" |> Option.iter (fun _ -> element.Time <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<DateTimeEventArgs<TimeSpan>->unit> "timeField.timeChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TimeChanged @> element)

    override _.name = $"TimeField"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TimeField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TimeField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Toplevel
type ToplevelElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Toplevel) props =
        // Properties
        props |> Interop.getValue<bool> "toplevel.isOverlappedContainer" |> Option.iter (fun v -> element.IsOverlappedContainer <- v )
        props |> Interop.getValue<bool> "toplevel.modal" |> Option.iter (fun v -> element.Modal <- v )
        props |> Interop.getValue<bool> "toplevel.running" |> Option.iter (fun v -> element.Running <- v )
        // Events
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.activate" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Activate @> v element)
        props |> Interop.getValue<unit->unit> "toplevel.allChildClosed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.AllChildClosed @> (fun _ -> v()) element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.childClosed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ChildClosed @> v element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.childLoaded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ChildLoaded @> v element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.childUnloaded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ChildUnloaded @> v element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.closed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Closed @> v element)
        props |> Interop.getValue<ToplevelClosingEventArgs->unit> "toplevel.closing" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Closing @> v element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.deactivate" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Deactivate @> v element)
        props |> Interop.getValue<unit->unit> "toplevel.loaded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Loaded @> (fun _ -> v()) element)
        props |> Interop.getValue<unit->unit> "toplevel.ready" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Ready @> (fun _ -> v()) element)
        props |> Interop.getValue<SizeChangedEventArgs->unit> "toplevel.sizeChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SizeChanging @> v element)
        props |> Interop.getValue<unit->unit> "toplevel.unloaded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Unloaded @> (fun _ -> v()) element)

    let removeProps (element:Toplevel) props =
        // Properties
        props |> Interop.getValue<bool> "toplevel.isOverlappedContainer" |> Option.iter (fun _ -> element.IsOverlappedContainer <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "toplevel.modal" |> Option.iter (fun _ -> element.Modal <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "toplevel.running" |> Option.iter (fun _ -> element.Running <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.activate" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Activate @> element)
        props |> Interop.getValue<unit->unit> "toplevel.allChildClosed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.AllChildClosed @> element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.childClosed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ChildClosed @> element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.childLoaded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ChildLoaded @> element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.childUnloaded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ChildUnloaded @> element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.closed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Closed @> element)
        props |> Interop.getValue<ToplevelClosingEventArgs->unit> "toplevel.closing" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Closing @> element)
        props |> Interop.getValue<ToplevelEventArgs->unit> "toplevel.deactivate" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Deactivate @> element)
        props |> Interop.getValue<unit->unit> "toplevel.loaded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Loaded @> element)
        props |> Interop.getValue<unit->unit> "toplevel.ready" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Ready @> element)
        props |> Interop.getValue<SizeChangedEventArgs->unit> "toplevel.sizeChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SizeChanging @> element)
        props |> Interop.getValue<unit->unit> "toplevel.unloaded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Unloaded @> element)

    override _.name = $"Toplevel"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Toplevel()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Toplevel
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TreeView
type TreeViewElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TreeView) props =
        // No properties or events TreeView
        ()

    let removeProps (element:TreeView) props =
        // No properties or events TreeView
        ()

    override _.name = $"TreeView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TreeView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TreeView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// TreeView<'a when 'a : not struct>
type TreeViewElement<'a when 'a : not struct>(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: TreeView<'a>) props =
        // Properties
        props |> Interop.getValue<bool> "treeView`1.allowLetterBasedNavigation" |> Option.iter (fun v -> element.AllowLetterBasedNavigation <- v )
        props |> Interop.getValue<AspectGetterDelegate<'a>> "treeView`1.aspectGetter" |> Option.iter (fun v -> element.AspectGetter <- v )
        props |> Interop.getValue<Func<'a,ColorScheme>> "treeView`1.colorGetter" |> Option.iter (fun v -> element.ColorGetter <- v )
        props |> Interop.getValue<int> "treeView`1.maxDepth" |> Option.iter (fun v -> element.MaxDepth <- v )
        props |> Interop.getValue<bool> "treeView`1.multiSelect" |> Option.iter (fun v -> element.MultiSelect <- v )
        props |> Interop.getValue<MouseFlags option> "treeView`1.objectActivationButton" |> Option.iter (fun v -> element.ObjectActivationButton <- v  |> Option.toNullable)
        props |> Interop.getValue<KeyCode> "treeView`1.objectActivationKey" |> Option.iter (fun v -> element.ObjectActivationKey <- v )
        props |> Interop.getValue<int> "treeView`1.scrollOffsetHorizontal" |> Option.iter (fun v -> element.ScrollOffsetHorizontal <- v )
        props |> Interop.getValue<int> "treeView`1.scrollOffsetVertical" |> Option.iter (fun v -> element.ScrollOffsetVertical <- v )
        props |> Interop.getValue<'a> "treeView`1.selectedObject" |> Option.iter (fun v -> element.SelectedObject <- v )
        props |> Interop.getValue<TreeStyle> "treeView`1.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Interop.getValue<ITreeBuilder<'a>> "treeView`1.treeBuilder" |> Option.iter (fun v -> element.TreeBuilder <- v )
        // Events
        props |> Interop.getValue<DrawTreeViewLineEventArgs<'a>->unit> "treeView`1.drawLine" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawLine @> v element)
        props |> Interop.getValue<ObjectActivatedEventArgs<'a>->unit> "treeView`1.objectActivated" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ObjectActivated @> v element)
        props |> Interop.getValue<SelectionChangedEventArgs<'a>->unit> "treeView`1.selectionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectionChanged @> v element)

    let removeProps (element:TreeView<'a>) props =
        // Properties
        props |> Interop.getValue<bool> "treeView`1.allowLetterBasedNavigation" |> Option.iter (fun _ -> element.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)
        props |> Interop.getValue<AspectGetterDelegate<'a>> "treeView`1.aspectGetter" |> Option.iter (fun _ -> element.AspectGetter <- Unchecked.defaultof<_>)
        props |> Interop.getValue<Func<'a,ColorScheme>> "treeView`1.colorGetter" |> Option.iter (fun _ -> element.ColorGetter <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "treeView`1.maxDepth" |> Option.iter (fun _ -> element.MaxDepth <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "treeView`1.multiSelect" |> Option.iter (fun _ -> element.MultiSelect <- Unchecked.defaultof<_>)
        props |> Interop.getValue<MouseFlags option> "treeView`1.objectActivationButton" |> Option.iter (fun _ -> element.ObjectActivationButton <- Unchecked.defaultof<_>)
        props |> Interop.getValue<KeyCode> "treeView`1.objectActivationKey" |> Option.iter (fun _ -> element.ObjectActivationKey <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "treeView`1.scrollOffsetHorizontal" |> Option.iter (fun _ -> element.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)
        props |> Interop.getValue<int> "treeView`1.scrollOffsetVertical" |> Option.iter (fun _ -> element.ScrollOffsetVertical <- Unchecked.defaultof<_>)
        props |> Interop.getValue<'a> "treeView`1.selectedObject" |> Option.iter (fun _ -> element.SelectedObject <- Unchecked.defaultof<_>)
        props |> Interop.getValue<TreeStyle> "treeView`1.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Interop.getValue<ITreeBuilder<'a>> "treeView`1.treeBuilder" |> Option.iter (fun _ -> element.TreeBuilder <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<DrawTreeViewLineEventArgs<'a>->unit> "treeView`1.drawLine" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawLine @> element)
        props |> Interop.getValue<ObjectActivatedEventArgs<'a>->unit> "treeView`1.objectActivated" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ObjectActivated @> element)
        props |> Interop.getValue<SelectionChangedEventArgs<'a>->unit> "treeView`1.selectionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectionChanged @> element)

    override _.name = $"TreeView<'a>"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TreeView<'a>()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> TreeView<'a>
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Window
type WindowElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Window) props =
        // No properties or events Window
        ()

    let removeProps (element:Window) props =
        // No properties or events Window
        ()

    override _.name = $"Window"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Window()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Window
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// Wizard
type WizardElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: Wizard) props =
        // Properties
        props |> Interop.getValue<WizardStep> "wizard.currentStep" |> Option.iter (fun v -> element.CurrentStep <- v )
        props |> Interop.getValue<bool> "wizard.modal" |> Option.iter (fun v -> element.Modal <- v )
        // Events
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.cancelled" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Cancelled @> v element)
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.finished" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Finished @> v element)
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.movingBack" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MovingBack @> v element)
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.movingNext" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MovingNext @> v element)
        props |> Interop.getValue<StepChangeEventArgs->unit> "wizard.stepChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.StepChanged @> v element)
        props |> Interop.getValue<StepChangeEventArgs->unit> "wizard.stepChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.StepChanging @> v element)

    let removeProps (element:Wizard) props =
        // Properties
        props |> Interop.getValue<WizardStep> "wizard.currentStep" |> Option.iter (fun _ -> element.CurrentStep <- Unchecked.defaultof<_>)
        props |> Interop.getValue<bool> "wizard.modal" |> Option.iter (fun _ -> element.Modal <- Unchecked.defaultof<_>)
        // Events
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.cancelled" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Cancelled @> element)
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.finished" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Finished @> element)
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.movingBack" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MovingBack @> element)
        props |> Interop.getValue<WizardButtonEventArgs->unit> "wizard.movingNext" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MovingNext @> element)
        props |> Interop.getValue<StepChangeEventArgs->unit> "wizard.stepChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.StepChanged @> element)
        props |> Interop.getValue<StepChangeEventArgs->unit> "wizard.stepChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.StepChanging @> element)

    override _.name = $"Wizard"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Wizard()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> Wizard
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


// WizardStep
type WizardStepElement(props:IProperty list) =
    inherit TerminalElement(props)

    let setProps (element: WizardStep) props =
        // Properties
        props |> Interop.getValue<string> "wizardStep.backButtonText" |> Option.iter (fun v -> element.BackButtonText <- v )
        props |> Interop.getValue<string> "wizardStep.helpText" |> Option.iter (fun v -> element.HelpText <- v )
        props |> Interop.getValue<string> "wizardStep.nextButtonText" |> Option.iter (fun v -> element.NextButtonText <- v )

    let removeProps (element:WizardStep) props =
        // Properties
        props |> Interop.getValue<string> "wizardStep.backButtonText" |> Option.iter (fun _ -> element.BackButtonText <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "wizardStep.helpText" |> Option.iter (fun _ -> element.HelpText <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "wizardStep.nextButtonText" |> Option.iter (fun _ -> element.NextButtonText <- Unchecked.defaultof<_>)

    override _.name = $"WizardStep"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new WizardStep()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    


    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    


    override this.update prevElement oldProps = 
        let element = prevElement :?> WizardStep
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
    


