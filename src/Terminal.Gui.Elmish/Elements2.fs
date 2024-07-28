module Terminal.Gui.Elmish.Elements2

open System.Linq.Expressions
open System.Text
open Terminal.Gui
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Elmish.EventHelpers



type ButtonElement2(props:IProperty list) =
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
        System.Diagnostics.Debug.WriteLine($"button created!")
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
        this.element <- prevElement