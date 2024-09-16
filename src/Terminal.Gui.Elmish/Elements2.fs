module Terminal.Gui.Elmish.Elements2
(*
open System
open System.ComponentModel
open System.Linq.Expressions
open System.Text
open Terminal.Gui
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
    

module ViewElement =

    let setProps (element: View) props =
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v element)
        props |> Interop.getValue<HandledEventArgs->unit> "accept" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.Accept @>) v element)
        props |> Interop.getValue<obj> "data" |> Option.iter (fun v -> element.Data <- v)
        props |> Interop.getValue<string> "id" |> Option.iter (fun v -> element.Id <- v)
        props |> Interop.getValue<unit->unit> "initialized" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.Initialized @>) v element)
        props |> Interop.getValue<bool> "enabled" |> Option.iter (fun v -> element.Enabled <- v)
        props |> Interop.getValue<unit->unit> "enabledChanged" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.EnabledChanged @>) (fun _ -> v()) element)
        props |> Interop.getValue<bool> "visible" |> Option.iter (fun v -> element.Visible <- v)
        props |> Interop.getValue<unit->unit> "visibleChanged" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.VisibleChanged @>) (fun _ -> v()) element)
        props |> Interop.getValue<bool> "clearOnVisibleFalse" |> Option.iter (fun v -> element.ClearOnVisibleFalse <- v)
        props |> Interop.getValue<string> "title" |> Option.iter (fun v -> element.Title <- v)
        props |> Interop.getValue<EventArgs<string>->unit> "titleChanged" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.TitleChanged @>) v element)
        props |> Interop.getValue<CancelEventArgs<string>->unit> "titleChanging" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.TitleChanging @>) v element)
        props |> Interop.getValue<Key> "hotKey" |> Option.iter (fun v -> element.HotKey <- v)
        props |> Interop.getValue<Rune> "hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v)
        props |> Interop.getValue<int> "tabIndex" |> Option.iter (fun v -> element.TabIndex <- v)
        props |> Interop.getValue<bool> "tabStop" |> Option.iter (fun v -> element.TabStop <- v)
        props |> Interop.getValue<Key->unit> "keyDown" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.KeyDown @>) v element)
        props |> Interop.getValue<Key->unit> "processKeyDown" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.ProcessKeyDown @>) v element)
        props |> Interop.getValue<Key->unit> "keyUp" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.KeyUp @>) v element)
        props |> Interop.getValue<Key->unit> "invokingKeyBindings" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.InvokingKeyBindings @>) v element)
        props |> Interop.getValue<KeyBindings> "keyBindings" |> Option.iter (fun v -> element.KeyBindings <- v)


    let removeProps (element: View) props =
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun _ -> ())
        props |> Interop.getValue<HandledEventArgs->unit> "accept" |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.Accept @>) element)
        props |> Interop.getValue<obj> "data" |> Option.iter (fun _ -> element.Data <- Unchecked.defaultof<_>)
        props |> Interop.getValue<string> "id" |> Option.iter (fun _ -> element.Id <- null)
        props |> Interop.getValue<unit->unit> "initialized" |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.Initialized @>) element)
        props |> Interop.getValue<bool> "enabled" |> Option.iter (fun _ -> element.Enabled <- Unchecked.defaultof<_>)
        props |> Interop.getValue<unit->unit> "enabledChanged" |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.EnabledChanged @>) element)
        props |> Interop.getValue<bool> "visible" |> Option.iter (fun _ -> element.Visible <- false)
        props |> Interop.getValue<unit->unit> "visibleChanged" |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.VisibleChanged @>) element)
        props |> Interop.getValue<bool> "clearOnVisibleFalse" |> Option.iter (fun _ -> element.ClearOnVisibleFalse <- false)
        props |> Interop.getValue<string> "title" |> Option.iter (fun _ -> element.Title <- null)
        props |> Interop.getValue<EventArgs<string>->unit> "titleChanged" |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.TitleChanged @>) element)
        props |> Interop.getValue<CancelEventArgs<string>->unit> "titleChanging" |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.TitleChanging @>) element)

    

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

            
        let positionX = props |> Interop.getValue<Pos> "x"      |> Option.map (fun v -> isPosCompatible view.X v) |> Option.defaultValue true
        let positionY = props |> Interop.getValue<Pos> "y"      |> Option.map (fun v -> isPosCompatible view.Y v) |> Option.defaultValue true
        let width = props |> Interop.getValue<Dim> "width"      |> Option.map (fun v -> isDimCompatible view.Width v) |> Option.defaultValue true
        let height = props |> Interop.getValue<Dim> "height"      |> Option.map (fun v -> isDimCompatible view.Height v) |> Option.defaultValue true

        // in case width or height is removed!
        let widthNotRemoved  = removedProps |> Interop.valueExists "width"   |> not
        let heightNotRemoved = removedProps |> Interop.valueExists "height"  |> not

        [
            positionX
            positionY
            width
            height
            widthNotRemoved
            heightNotRemoved
        ]
        |> List.forall id

    


type ButtonElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let text = props |> Interop.getValueDefault "text" ""

    let setProps (element:Button) props =
        props |> Interop.getValue<string> "text"            |> Option.iter (fun v -> if element.Text <> v then element.Text <- v)
        props |> Interop.getValue<bool> "isDefault"         |> Option.iter (fun v -> element.IsDefault <- v)
        props |> Interop.getValue<Key> "hotKey"             |> Option.iter (fun v -> element.HotKey <- v)
        props |> Interop.getValue<Rune> "hotKeySpecifier"   |> Option.iter (fun v -> element.HotKeySpecifier <- v)
        
        props |> Interop.getValue<string> "text"                            |> Option.iter (fun v -> element.Text <- v)
        props |> Interop.getValue<bool> "isDefault"                         |> Option.iter (fun v -> element.IsDefault <- v)
        props |> Interop.getValue<bool> "wantContinuousButtonPressed"       |> Option.iter (fun v -> element.WantContinuousButtonPressed <- v)
        props |> Interop.getValue<Rune> "hotKeySpecifier"                   |> Option.iter (fun v -> element.HotKeySpecifier <- v)
        props |> Interop.getValue<bool> "noDecorations"                     |> Option.iter (fun v -> element.NoDecorations <- v)
        props |> Interop.getValue<bool> "noPadding"                         |> Option.iter (fun v -> element.NoPadding <- v)
        props |> Interop.getValue<EventArgs<string> -> unit> "titleChanged" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.TitleChanged @>) v element)
        props |> Interop.getValue<MouseEventEventArgs -> unit> "mouseClick" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.MouseClick @>) v element)
        //props |> Interop.getValue<unit -> unit> "onAccept"                  |> Option.iter (fun v -> Interop.setEventHandler <@ fun () -> element.Accept @> (fun _ -> v()) element)
        props |> Interop.getValue<unit -> unit> "onAccept"                  |> Option.iter (fun v -> Interop.setEventHandler (<@ element.Accept @>) (fun _ -> v()) element)
        
            

    let removeProps (element:Button) props =
        props |> Interop.getValue<bool> "isDefault"         |> Option.iter (fun v -> element.IsDefault <- false)
        props |> Interop.getValue<unit -> unit> "onAccept"                  |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.Accept @>) element)
        

    override _.name = $"Button"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        System.Diagnostics.Trace.WriteLine($"button created!")
        let el = new Button(Text=text)
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
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
        this.element <- prevElement*)