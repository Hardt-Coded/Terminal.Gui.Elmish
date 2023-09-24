namespace Terminal.Gui.Elmish.Elements

open System.Reflection
open System.Collections
open System
open Terminal.Gui
open Terminal.Gui.Elmish
open System.Data
open Terminal.Gui.Trees

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

        //props |> Interop.getValue<string> "text"                    |> Option.iter (fun v -> if Checker.textChanged view v then view.Text <- v)
        props |> Interop.getValue<TextAlignment> "textAlignment"    |> Option.iter (fun v -> view.TextAlignment <- v)
        props |> Interop.getValue<TextDirection> "textDirection"    |> Option.iter (fun v -> view.TextDirection <- v)
            
        props |> Interop.getValue<int> "tabIndex"   |> Option.iter (fun v -> view.TabIndex <- v)
        props |> Interop.getValue<bool> "autoSize"  |> Option.iter (fun v -> view.AutoSize <- v)
        props |> Interop.getValue<bool> "tabStop"   |> Option.iter (fun v -> view.TabStop <- v)

        props |> Interop.getValue<bool> "enabled" |> Option.iter (fun v -> view.Enabled <- v)
        props |> Interop.getValue<ColorScheme> "colorScheme" |> Option.iter (fun v -> view.ColorScheme <- v)
        props 
        |> Interop.getValue<Attribute> "colorDisabled" 
        |> Option.iter (fun v -> 
            let colorScheme =
                view.ColorScheme |> Option.ofObj |> Option.defaultValue Colors.TopLevel
            let cloned = Interop.cloneColorScheme colorScheme
            cloned.Normal <- v
            view.ColorScheme <- cloned
        )
        props 
        |> Interop.getValue<Attribute> "colorFocus" 
        |> Option.iter (fun v -> 
            let colorScheme =
                view.ColorScheme |> Option.ofObj |> Option.defaultValue Colors.TopLevel
            let cloned = Interop.cloneColorScheme colorScheme
            cloned.Normal <- v
            view.ColorScheme <- cloned
        )
        props 
        |> Interop.getValue<Attribute> "colorHotFocus" 
        |> Option.iter (fun v -> 
            let colorScheme =
                view.ColorScheme |> Option.ofObj |> Option.defaultValue Colors.TopLevel
            let cloned = Interop.cloneColorScheme colorScheme
            cloned.Normal <- v
            view.ColorScheme <- cloned
        )
        props 
        |> Interop.getValue<Attribute> "colorHotNormal" 
        |> Option.iter (fun v -> 
            let colorScheme =
                view.ColorScheme |> Option.ofObj |> Option.defaultValue Colors.TopLevel
            let cloned = Interop.cloneColorScheme colorScheme
            cloned.Normal <- v
            view.ColorScheme <- cloned
        )
        props 
        |> Interop.getValue<Attribute> "colorNormal" 
        |> Option.iter (fun v -> 
            let colorScheme =
                view.ColorScheme |> Option.ofObj |> Option.defaultValue Colors.TopLevel
            let cloned = Interop.cloneColorScheme colorScheme
            cloned.Normal <- v
            view.ColorScheme <- cloned
        )

        props 
        |> Interop.getValue<Attribute> "color" 
        |> Option.iter (fun v -> 
            let colorScheme =
                view.ColorScheme |> Option.ofObj |> Option.defaultValue Colors.TopLevel
            let cloned = Interop.cloneColorScheme colorScheme
            cloned.Focus <- v
            cloned.Normal <- v
            view.ColorScheme <- cloned
        )

        
        


    let removeEvents (view:View) props =
        // all bool properties musst be set to default, when removed
        props |> Interop.getValue<bool> "autoSize"  |> Option.iter (fun v -> view.AutoSize <- false)
        props |> Interop.getValue<bool> "tabStop"   |> Option.iter (fun v -> view.TabStop <- false)
        props |> Interop.getValue<bool> "enabled" |> Option.iter (fun v -> view.Enabled <- true)
        props |> Interop.getValue<Dim> "x"  |> Option.iter (fun v -> view.AutoSize <- true)
        props |> Interop.getValue<Dim> "y"  |> Option.iter (fun v -> view.AutoSize <- true)
        props |> Interop.getValue<Dim> "width"  |> Option.iter (fun v -> view.AutoSize <- true)
        props |> Interop.getValue<Dim> "height"  |> Option.iter (fun v -> view.AutoSize <- true)

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
        props |> Interop.getValue<bool> "enabled" |> Option.iter (fun v -> view.Enabled <- true)
        props |> Interop.getValue<ColorScheme> "colorScheme" |> Option.iter (fun v -> view.ColorScheme <- null)
        props |> Interop.getValue<TextAlignment> "textAlignment"    |> Option.iter (fun v -> view.TextAlignment <- TextAlignment.Left)
        props |> Interop.getValue<TextDirection> "textDirection"    |> Option.iter (fun v -> view.TextDirection <- TextDirection.LeftRight_TopBottom)
            
        props |> Interop.getValue<bool> "autoSize"  |> Option.iter (fun v -> view.AutoSize <- true)
        props |> Interop.getValue<bool> "tabStop"   |> Option.iter (fun v -> view.TabStop <- true)

        props |> Interop.getValue<Pos> "x"      |> Option.iter (fun v -> view.X <- 0)
        props |> Interop.getValue<Pos> "y"      |> Option.iter (fun v -> view.Y <- 0)
        props |> Interop.getValue<Dim> "width"  |> Option.iter (fun v -> view.Width <- Dim.Sized(1); view.IsInitialized <- true)
        props |> Interop.getValue<Dim> "height" |> Option.iter (fun v -> view.Height <- Dim.Sized(1); view.IsInitialized <- true)

        props 
        |> Interop.getValue<Attribute> "colorDisabled" 
        |> Option.iter (fun v -> view.ColorScheme <- null)
        props 
        |> Interop.getValue<Attribute> "colorFocus" 
        |> Option.iter (fun v -> view.ColorScheme <- null)
        props 
        |> Interop.getValue<Attribute> "colorHotFocus" 
        |> Option.iter (fun v -> view.ColorScheme <- null)
        props 
        |> Interop.getValue<Attribute> "colorHotNormal" 
        |> Option.iter (fun v -> view.ColorScheme <- null)
        props 
        |> Interop.getValue<Attribute> "colorNormal" 
        |> Option.iter (fun v -> view.ColorScheme <- null)

        props 
        |> Interop.getValue<Attribute> "color" 
        |> Option.iter (fun v -> view.ColorScheme <- null)


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
        



[<RequireQualifiedAccess>]
module MenuElements =
    
    [<AbstractClass>]
    type MenuElement (props:IMenuProperty list) =
        let mutable view: MenuItem = null
    
        member this.element with get() = view and set v = view <- v
    
        abstract create: parent:MenuItem option -> unit
        abstract name: string

    type MenuItemElement(props:IMenuProperty list) =
        inherit MenuElement(props) 
        
    
        let setProps (element:MenuItem) props =
            props |> Interop.getMenuValue<string> "title" |> Option.iter (fun v -> element.Title <- v)
            props |> Interop.getMenuValue<unit->unit> "action" |> Option.iter (fun v -> element.Action <- v)
            props |> Interop.getMenuValue<MenuItemCheckStyle> "itemstyle" |> Option.iter (fun v -> element.CheckType <- v)
            props |> Interop.getMenuValue<bool> "checked" |> Option.iter (fun v -> element.Checked <- v)
            props |> Interop.getMenuValue<Key> "shortcut" |> Option.iter (fun v -> element.Shortcut <- v)
            
           
    
        override _.name = $"MenuItem"
    
    
        override this.create parent =
            #if DEBUG
            Diagnostics.Debug.WriteLine ($"{this.name} created!")
            #endif
            let el = new MenuItem()
            
            setProps el props
            this.element <- el   


    type MenuBarItemElement(props:IMenuProperty list) as self =
        inherit MenuElement(props) 
        
    
        let setProps (element:MenuBarItem) props =
            props 
            |> Interop.getMenuValue<IMenu list> "children" 
            |> Option.iter (fun elements -> 
                if elements.Length = 0 then
                    failwith("menu bar items should have at least one sub item")
                let menus = 
                    elements 
                    |> Seq.cast<KeyValue>
                    |> Seq.map (function | KeyValue (name,elementProperties) -> (name, elementProperties :?> IMenuProperty list))
                    |> Seq.map (fun (name,properties) -> 
                        match name with
                        | "submenuItem" ->
                            let el = MenuBarItemElement(properties)
                            el.create (Some self.element)
                            Some el.element
                        | "menuItem" ->
                            let el = MenuItemElement(properties)
                            el.create (Some self.element)
                            Some el.element
                        | _ ->
                            None
                    )
                    |> Seq.choose id
                    |> Seq.toArray
                element.Children <- menus
            )
            props |> Interop.getMenuValue<string> "title" |> Option.iter (fun v -> element.Title <- v)
           
    
        override _.name = $"MenuBarItem"
    
    
        override this.create parent =
            #if DEBUG
            Diagnostics.Debug.WriteLine ($"{this.name} created!")
            #endif
            let el = 
                match parent with
                | None ->
                    new MenuBarItem()    
                | Some parent ->
                    new MenuBarItem("", [||],parent)
            setProps el props            
            this.element <- el
    
        


type MenuBarElement(props:IMenuBarProperty list) =
    inherit TerminalElement(props |> Seq.cast<IProperty> |> Seq.toList) 
    
    let setProps (element:MenuBar) props =
        props 
        |> Interop.getMenuBarValue<IMenuBarItem list> "menus" 
        |> Option.iter (fun elements -> 
            let menus = 
                elements 
                |> Seq.cast<KeyValue>
                |> Seq.map (function | KeyValue (_,elementProperties) -> elementProperties :?> IMenuProperty list)
                |> Seq.map (fun menuBarProperties -> 
                    let el = MenuElements.MenuBarItemElement(menuBarProperties)
                    el.create None; el.element :?> MenuBarItem)
                |> Seq.toArray
                
            element.Menus <- (menus)
        )
        props |> Interop.getMenuBarValue<bool> "useKeysUpDownAsKeysLeftRight" |> Option.iter (fun v -> element.UseKeysUpDownAsKeysLeftRight <- v)
        props |> Interop.getMenuBarValue<bool> "useSubMenusSingleFrame" |> Option.iter (fun v -> element.UseSubMenusSingleFrame <- v)

        // onMenuAllClosed
        props 
        |> Interop.getMenuBarValue<unit->unit> "onMenuAllClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuAllClosed" element
            element.add_MenuAllClosed v
        )
        // onMenuClosing
        props 
        |> Interop.getMenuBarValue<MenuClosingEventArgs->unit> "onMenuClosing" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuClosing" element
            element.add_MenuClosing v
        )
        // onMenuOpened
        props 
        |> Interop.getMenuBarValue<MenuItem->unit> "onMenuOpened" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuOpened" element
            element.add_MenuOpened v
        )
        // onMenuOpening
        props 
        |> Interop.getMenuBarValue<MenuOpeningEventArgs->unit> "onMenuOpening" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuOpening" element
            element.add_MenuOpening v
        )

    let removeProps (element:MenuBar) props =
        props |> Interop.getMenuBarValue<MenuBarItem list> "menus" |> Option.iter (fun v -> element.Menus <- Array.empty)
        props |> Interop.getMenuBarValue<bool> "useKeysUpDownAsKeysLeftRight" |> Option.iter (fun v -> element.UseKeysUpDownAsKeysLeftRight <- false)
        props |> Interop.getMenuBarValue<bool> "useSubMenusSingleFrame" |> Option.iter (fun v -> element.UseSubMenusSingleFrame <- false)
        
        // onMenuAllClosed
        props 
        |> Interop.getMenuBarValue<unit->unit> "onMenuAllClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuAllClosed" element
        )
        // onMenuClosing
        props 
        |> Interop.getMenuBarValue<MenuClosingEventArgs->unit> "onMenuClosing" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuClosing" element
        )
        // onMenuOpened
        props 
        |> Interop.getMenuBarValue<MenuItem->unit> "onMenuOpened" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuOpened" element
        )
        // onMenuOpening
        props 
        |> Interop.getMenuBarValue<MenuOpeningEventArgs->unit> "onMenuOpening" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "MenuOpening" element
        )

    override _.name = $"MenuBar"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new MenuBar()
        parent |> Option.iter (fun p -> p.Add el)
        setProps el props
        this.element <- el


    override this.canUpdate prevElement oldProps =
        true

    override this.update prevElement oldProps = 
        ()

    

type PageElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:Toplevel) props =
        props 
        |> Interop.getValue<IMenuBarProperty list> "menuBar" 
        |> Option.iter (fun menubarProperties -> 
            let menubar = MenuBarElement(menubarProperties)
            menubar.create (Some element)
            //element.MenuBar <- (menubar.element :?> MenuBar)
            //element.Add (menubar.element :?> MenuBar)
        )
        
        props |> Interop.getValue<bool> "running"         |> Option.iter (fun v -> element.Running <- v)
        props |> Interop.getValue<bool> "modal"         |> Option.iter (fun v -> element.Modal <- v)
        // Todo:
        props |> Interop.getValue<StatusBar> "statusBar"         |> Option.iter (fun v -> element.StatusBar <- v)
        props |> Interop.getValue<bool> "isMdiContainer"         |> Option.iter (fun v -> element.IsMdiContainer <- v)
        
        // onLoaded
        props 
        |> Interop.getValue<unit->unit> "onLoaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Loaded" element
            element.add_Loaded v
        )
        
        // onReady
        props 
        |> Interop.getValue<unit->unit> "onReady" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Ready" element
            element.add_Ready v
        )
        
        // onUnloaded
        props 
        |> Interop.getValue<unit->unit> "onUnloaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Unloaded" element
            element.add_Unloaded v
        )
        
        // onActivate
        props 
        |> Interop.getValue<Toplevel->unit> "onActivate" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Activate" element
            element.add_Activate v
        )
        
        // onDeactivate
        props 
        |> Interop.getValue<Toplevel->unit> "onDeactivate" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Deactivate" element
            element.add_Deactivate v
        )
        
        // onChildClosed
        props 
        |> Interop.getValue<Toplevel->unit> "onChildClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ChildClosed" element
            element.add_ChildClosed v
        )
        
        // onAllChildClosed
        props 
        |> Interop.getValue<unit->unit> "onAllChildClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "AllChildClosed" element
            element.add_AllChildClosed v
        )
        
        // onClosing
        props 
        |> Interop.getValue<ToplevelClosingEventArgs->unit> "onClosing" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Closing" element
            element.add_Closing v
        )
        
        // onClosed
        props 
        |> Interop.getValue<Toplevel->unit> "onClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Closed" element
            element.add_Closed v
        )
        
        // onChildLoaded
        props 
        |> Interop.getValue<Toplevel->unit> "onChildLoaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ChildLoaded" element
            element.add_ChildLoaded v
        )
        
        // onChildUnloaded
        props 
        |> Interop.getValue<Toplevel->unit> "onChildUnloaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ChildUnloaded" element
            element.add_ChildUnloaded v
        )
        
        // onResized
        props 
        |> Interop.getValue<Size->unit> "onResized" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Resized" element
            element.add_Resized v
        )
        
        // onAlternateForwardKeyChanged
        props 
        |> Interop.getValue<Key->unit> "onAlternateForwardKeyChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "AlternateForwardKeyChanged" element
            element.add_AlternateForwardKeyChanged v
        )
        
        // onAlternateBackwardKeyChanged
        props 
        |> Interop.getValue<Key->unit> "onAlternateBackwardKeyChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "AlternateBackwardKeyChanged" element
            element.add_AlternateBackwardKeyChanged v
        )
        
        // onQuitKeyChanged
        props 
        |> Interop.getValue<Key->unit> "onQuitKeyChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "QuitKeyChanged" element
            element.add_QuitKeyChanged v
        )
        

    let removeProps (element:Toplevel) props =
        props 
        |> Interop.getValue<IMenuBarProperty list> "menuBar" 
        |> Option.iter (fun menubarProperties -> 
            element.MenuBar <- null
        )
        // onLoaded
        props 
        |> Interop.getValue<unit->unit> "onLoaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Loaded" element
        )
        
        // onReady
        props 
        |> Interop.getValue<unit->unit> "onReady" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Ready" element
        )
        
        // onUnloaded
        props 
        |> Interop.getValue<unit->unit> "onUnloaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Unloaded" element
        )
        
        // onActivate
        props 
        |> Interop.getValue<Toplevel->unit> "onActivate" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Activate" element
        )
        
        // onDeactivate
        props 
        |> Interop.getValue<Toplevel->unit> "onDeactivate" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Deactivate" element
        )
        
        // onChildClosed
        props 
        |> Interop.getValue<Toplevel->unit> "onChildClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ChildClosed" element
        )
        
        // onAllChildClosed
        props 
        |> Interop.getValue<unit->unit> "onAllChildClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "AllChildClosed" element
        )
        
        // onClosing
        props 
        |> Interop.getValue<ToplevelClosingEventArgs->unit> "onClosing" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Closing" element
        )
        
        // onClosed
        props 
        |> Interop.getValue<Toplevel->unit> "onClosed" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Closed" element
        )
        
        // onChildLoaded
        props 
        |> Interop.getValue<Toplevel->unit> "onChildLoaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ChildLoaded" element
        )
        
        // onChildUnloaded
        props 
        |> Interop.getValue<Toplevel->unit> "onChildUnloaded" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ChildUnloaded" element
        )
        
        // onResized
        props 
        |> Interop.getValue<Size->unit> "onResized" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Resized" element
        )
        
        // onAlternateForwardKeyChanged
        props 
        |> Interop.getValue<Key->unit> "onAlternateForwardKeyChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "AlternateForwardKeyChanged" element
        )
        
        // onAlternateBackwardKeyChanged
        props 
        |> Interop.getValue<Key->unit> "onAlternateBackwardKeyChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "AlternateBackwardKeyChanged" element
        )
        
        // onQuitKeyChanged
        props 
        |> Interop.getValue<Key->unit> "onQuitKeyChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "QuitKeyChanged" element
        )

    override _.name = "Page"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = Toplevel.Create()
        parent |> Option.iter (fun p -> p.Add el)
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
    

    let setProps (element:Window) props =
        props |> Interop.getValue<string> "title" |> Option.map Interop.ustr |> Option.iter (fun v -> element.Title <- v)
        props |> Interop.getValue<string> "text" |> Option.map Interop.ustr |> Option.iter (fun v -> element.Text <- v)
        props |> Interop.getValue<BorderStyle> "borderStyle" |> Option.iter (fun v -> element.Border.BorderStyle <- v)
        props |> Interop.getValue<bool> "effect3D" |> Option.iter (fun v -> element.Border.Effect3D <- v)

        
    let removeProps (element:Window) props =
        props |> Interop.getValue<bool> "effect3D" |> Option.iter (fun v -> element.Border.Effect3D <- false)

    override _.name = "Window"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Window(title |> Interop.ustr)
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<string> "text" |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
        
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
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Label(text |> Interop.ustr)
        parent |> Option.iter (fun p -> p.Add el)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
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
        props |> Interop.getValue<string> "text"            |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
        props |> Interop.getValue<bool> "isDefault"         |> Option.iter (fun v -> element.IsDefault <- v)
        props |> Interop.getValue<Key> "hotKey"             |> Option.iter (fun v -> element.HotKey <- v)
        props |> Interop.getValue<Rune> "hotKeySpecifier"   |> Option.iter (fun v -> element.HotKeySpecifier <- v)
        props |> Interop.getValue<bool> "autoSize"          |> Option.iter (fun v -> element.AutoSize <- v)
        // onCLick
        props 
        |> Interop.getValue<unit->unit> "onClick" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Clicked" element
            element.add_Clicked v
        )
        
            

    let removeProps (element:Button) props =
        props |> Interop.getValue<bool> "isDefault"         |> Option.iter (fun v -> element.IsDefault <- false)
        // onCLick
        props 
        |> Interop.getValue<unit->unit> "onClick" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Clicked" element
        )

    override _.name = $"Button"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        System.Diagnostics.Debug.WriteLine($"button created!")
        let el = new Button(text |> Interop.ustr)
        parent |> Option.iter (fun p -> p.Add el)
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


type CheckBoxElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:CheckBox) props =
        props |> Interop.getValue<string> "text" |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
        props |> Interop.getValue<bool> "checked" |> Option.iter (fun v -> element.Checked <- v)
        // onToggled
        props 
        |> Interop.getValue<{| previous:bool; current:bool |}->unit> "toggled" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Toggled" element
            let action previous = v ({| previous = previous; current = element.Checked |})
            element.add_Toggled action
        )

    let removeProps (element:CheckBox) props =
        props |> Interop.getValue<bool> "checked" |> Option.iter (fun v -> element.Checked <- false)
        // onToggled
        props 
        |> Interop.getValue<{| previous:bool; current:bool |}->unit> "toggled" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "Toggled" element
        )

    override _.name = $"CheckBox"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let text = props |> Interop.getValue<string> "text" |> Option.defaultValue ""
        let el = new CheckBox(text)
        parent |> Option.iter (fun p -> p.Add el)
        setProps el props
        ViewElement.setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
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
        // yes named title, but into the text property
        props |> Interop.getValue<string> "title" |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ColorPicker()
        parent |> Option.iter (fun p -> p.Add el)
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
        props 
        |> Interop.getValue<string list> "source" 
        |> Option.iter (fun v -> 
            element.SetSource(v |> System.Linq.Enumerable.ToList)
        )
        props |> Interop.getValue<string> "text"        |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
        props |> Interop.getValue<int> "selectedItem"   |> Option.iter (fun v -> element.SelectedItem <- v)
        props |> Interop.getValue<bool> "readonly"      |> Option.iter (fun v -> element.ReadOnly <- v)
        
        
        
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
        props |> Interop.getValue<int> "readonly" |> Option.iter (fun v -> element.ReadOnly <- false)

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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let dropDownHeight = props |> Interop.getValueDefault<int> "dropdownHeight" 5
        let source = props |> Interop.getValueDefault<string list> "source" [] |> Linq.Enumerable.ToList
        let width = source |> Seq.map (fun i -> i.Length)|> Seq.max
        let el = new ComboBox(Rect(0,0,width + 2,dropDownHeight), source)
        parent |> Option.iter (fun p -> p.Add el)
        el.ColorScheme <- Colors.Base
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = 
            match props |> Interop.getValue<DateTime> "date" with
            | None ->
                new DateField()
            | Some date -> 
                new DateField(date)
        parent |> Option.iter (fun p -> p.Add el)
        ViewElement.setProps el props
        setProps el props
        // cursor to the end
        el.CursorPosition <- 10
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el


    override this.canUpdate prevElement oldProps =
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
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
        props |> Interop.getValue<BorderStyle> "borderStyle"    |> Option.iter (fun v -> element.Border.BorderStyle <- v)
        props |> Interop.getValue<bool> "effect3D"              |> Option.iter (fun v -> element.Border.Effect3D <- v)
        props |> Interop.getValue<string> "title"               |> Option.iter (fun v -> element.Title <- v)
        props |> Interop.getValue<string> "text"                |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
        props |> Interop.getValue<TextAlignment> "textAlignment"|> Option.iter (fun v -> element.TextAlignment <- v)

    let removeProps (element:FrameView) props =
        props |> Interop.getValue<bool> "effect3D" |> Option.iter (fun v -> element.Border.Effect3D <- false)
        props |> Interop.getValue<string> "text"                |> Option.iter (fun v -> element.Text <- "")
        props |> Interop.getValue<TextAlignment> "textAlignment"|> Option.iter (fun v -> element.TextAlignment <- TextAlignment.Left)
        props |> Interop.getValue<BorderStyle> "borderStyle"    |> Option.iter (fun v -> element.Border.BorderStyle <- BorderStyle.Single)
        props |> Interop.getValue<string> "title"               |> Option.iter (fun v -> element.Title <- "")

    override _.name = $"FrameView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new FrameView()
        parent |> Option.iter (fun p -> p.Add el)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new GraphView()
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<bool> "allowEdits"    |> Option.iter (fun v -> element.AllowEdits <- false)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new HexView()
        parent |> Option.iter (fun p -> p.Add el)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new LineView()
        parent |> Option.iter (fun p -> p.Add el)
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
        props 
        |> Interop.getValue<string list> "source" 
        |> Option.iter (fun v -> 
            element.SelectedItem <- 0
            element.SetSource(v |> System.Linq.Enumerable.ToList)
        )
        props |> Interop.getValue<string> "text"                    |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
        props |> Interop.getValue<bool> "allowsMultipleSelection"   |> Option.iter (fun v -> element.AllowsMultipleSelection <- v)
        props |> Interop.getValue<bool> "allowsMarking"             |> Option.iter (fun v -> element.AllowsMarking <- v)
        props |> Interop.getValue<int> "selectedItem"               |> Option.iter (fun v -> element.SelectedItem <- v)
        props |> Interop.getValue<int> "leftItem"                   |> Option.iter (fun v -> element.LeftItem <- v)
        props |> Interop.getValue<int> "topItem"                    |> Option.iter (fun v -> element.TopItem <- v)
        
        // onOpenSelectedItem
        props 
        |> Interop.getValue<ListViewItemEventArgs->unit> "onOpenSelectedItem" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "OpenSelectedItem" element
            element.add_OpenSelectedItem v
        )
        // onSelectedItemChanged
        props 
        |> Interop.getValue<ListViewItemEventArgs->unit> "onSelectedItemChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedItemChanged" element
            element.add_SelectedItemChanged v
        )
        // onRowRender
        props 
        |> Interop.getValue<ListViewRowEventArgs->unit> "onRowRender" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "RowRender" element
            element.add_RowRender v
        )

    let removeProps (element:ListView) props =
        props |> Interop.getValue<int> "selectedItem" |> Option.iter (fun v -> element.SelectedItem <- 0)
        props |> Interop.getValue<bool> "allowsMultipleSelection" |> Option.iter (fun v -> element.AllowsMultipleSelection <- false)
        props |> Interop.getValue<bool> "allowsMarking" |> Option.iter (fun v -> element.AllowsMarking <- false)
        // onOpenSelectedItem
        props 
        |> Interop.getValue<ListViewItemEventArgs->unit> "onOpenSelectedItem" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "OpenSelectedItem" element
        )
        // onSelectedItemChanged
        props 
        |> Interop.getValue<ListViewItemEventArgs->unit> "onSelectedItemChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedItemChanged" element
        )
        // onRowRender
        props 
        |> Interop.getValue<ListViewRowEventArgs->unit> "onRowRender" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "RowRender" element
        )

    override _.name = $"ListView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = 
            match props |> Interop.getValue<string list> "source" with
            | None ->
                new ListView()
            | Some source ->
                new ListView(source |> System.Linq.Enumerable.ToList)
        
        parent |> Option.iter (fun p -> p.Add el)
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
        let element = prevElement :?> ListView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


type PanelViewElement(props:IProperty list) as self =
    inherit TerminalElement(props)

    let createChild (element:PanelView) =
        props 
        |> Interop.getValue<TerminalElement> "child" 
        |> Option.iter (fun child -> 
            self.additionalProps <- [
                Interop.mkprop "child" child
            ]
            child.create (Some element)
            
            child.properties |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v child.element)
            element.Child <- child.element
        )

    let updateChild (element:PanelView) oldProps =
        props 
        |> Interop.getValue<TerminalElement> "child" 
        |> Option.iter (fun child -> 
            let prevChild = 
                oldProps 
                |> Interop.getValue<TerminalElement> "child"

            match prevChild with
            | None ->
                element.Child.Dispose()
                createChild element
            | Some prevChild when prevChild.name <> child.name ->
                element.Child.Dispose()
                createChild element
            | Some prevChild ->
                if child.canUpdate prevChild.element prevChild.properties then
                    child.update prevChild.element prevChild.properties
                else
                    element.Child.Dispose()
                    createChild element
                
        )


    let setProps (element:PanelView) props =
        props |> Interop.getValue<BorderStyle> "borderStyle" |> Option.iter (fun v -> 
            element.Border.BorderStyle <- v)
        props |> Interop.getValue<bool> "usePanelFrame" |> Option.iter (fun v -> element.UsePanelFrame <- v)
        props |> Interop.getValue<bool> "effect3D" |> Option.iter (fun v -> 
            element.Border.Effect3D <- v)

    let removeProps (element:PanelView) props =
        props |> Interop.getValue<bool> "usePanelFrame" |> Option.iter (fun v -> element.UsePanelFrame <- false)
        props |> Interop.getValue<bool> "effect3D" |> Option.iter (fun v -> element.Border.Effect3D <- false)

    override _.name = $"PanelView"

    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new PanelView()
        parent |> Option.iter (fun p -> p.Add el)
        
        createChild el
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
        let element = prevElement :?> PanelView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        updateChild element oldProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        
        this.element <- prevElement



type ProgressBarElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ProgressBar) props =
        props |> Interop.getValue<bool> "bidirectionalMarquee"          |> Option.iter (fun v -> element.BidirectionalMarquee <- v)
        props |> Interop.getValue<float> "fraction"                     |> Option.iter (fun v -> element.Fraction <- (v |> float32))
        props |> Interop.getValue<ProgressBarFormat> "progressBarFormat"|> Option.iter (fun v -> element.ProgressBarFormat <- v)
        props |> Interop.getValue<ProgressBarStyle> "progressBarStyle"  |> Option.iter (fun v -> element.ProgressBarStyle <- v)
        props |> Interop.getValue<Rune> "segmentCharacter"              |> Option.iter (fun v -> element.SegmentCharacter <- v)
        props |> Interop.getValue<string> "text"                        |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)

    let removeProps (element:ProgressBar) props =
        props |> Interop.getValue<bool> "bidirectionalMarquee" |> Option.iter (fun v -> element.BidirectionalMarquee <- false)

    override _.name = $"ProgressBar"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ProgressBar()
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<#seq<string>> "radioLabels"       |> Option.iter (fun v -> element.RadioLabels <- (v |> Seq.map Interop.ustr |> Seq.toArray) )
        
        props |> Interop.getValue<DisplayModeLayout> "displayMode"  |> Option.iter (fun v -> element.DisplayMode <- v)
        props |> Interop.getValue<int> "horizontalSpace"            |> Option.iter (fun v -> element.HorizontalSpace <- v)
        
        // onSelectedItemChanged
        props 
        |> Interop.getValue<SelectedItemChangedArgs->unit> "onSelectedItemChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedItemChanged" element
            element.add_SelectedItemChanged v
        )
        props |> Interop.getValue<int> "selectedItem"               |> Option.iter (fun v -> element.SelectedItem <- v)

    let removeProps (element:RadioGroup) props =
        props 
        |> Interop.getValue<SelectedItemChangedArgs->unit> "onSelectedItemChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedItemChanged" element
        )

    override _.name = $"RadioGroup"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new RadioGroup()
        parent |> Option.iter (fun p -> p.Add el)
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
        let element = prevElement :?> RadioGroup
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


// not working yet
//type ScrollBarViewElement(props:IProperty list) =
//    inherit TerminalElement(props) 

//    let setProps (element:ScrollBarView) props =
//        props |> Interop.getValue<bool> "autoHideScrollBars"         |> Option.iter (fun v -> element.AutoHideScrollBars <- v)
//        props |> Interop.getValue<bool> "isVertical"                 |> Option.iter (fun v -> element.IsVertical <- v)
//        props |> Interop.getValue<bool> "keepContentAlwaysInViewport"|> Option.iter (fun v -> element.KeepContentAlwaysInViewport <- v)
//        //props |> Interop.getValue<int>  "otherScrollBarView"         |> Option.iter (fun v -> element.OtherScrollBarView <- v)
//        props |> Interop.getValue<int>  "position"                   |> Option.iter (fun v -> element.Position <- v)
//        props |> Interop.getValue<bool> "showScrollIndicator"        |> Option.iter (fun v -> element.ShowScrollIndicator <- v)
//        props |> Interop.getValue<int>  "size"                       |> Option.iter (fun v -> element.Size <- v)
        
//        // onChangedPosition
//        props 
//        |> Interop.getValue<unit->unit> "onChangedPosition" 
//        |> Option.iter (fun v -> 
//            Interop.removeEventHandlerIfNecessary "ChangedPosition" element
//            element.add_ChangedPosition v
//        )

//    let removeProps (element:ScrollBarView) props =
//        props |> Interop.getValue<bool> "autoHideScrollBars"         |> Option.iter (fun v -> element.AutoHideScrollBars <- true)
//        props |> Interop.getValue<bool> "isVertical"                 |> Option.iter (fun v -> element.IsVertical <- false)
//        props |> Interop.getValue<bool> "keepContentAlwaysInViewport"|> Option.iter (fun v -> element.KeepContentAlwaysInViewport <- true)
//        props |> Interop.getValue<bool> "showScrollIndicator"        |> Option.iter (fun v -> element.ShowScrollIndicator <- false)
//        // onChangedPosition
//        props 
//        |> Interop.getValue<unit->unit> "onChangedPosition" 
//        |> Option.iter (fun v -> 
//            Interop.removeEventHandlerIfNecessary "ChangedPosition" element
//        )

//    override _.name = $"ScrollBarView"


//    override this.create parent =
//        #if DEBUG
//        Diagnostics.Debug.WriteLine ($"{this.name} created!")
//        #endif
//        let el = new ScrollBarView()
//        parent |> Option.iter (fun p -> p.Add el)
//        ViewElement.setProps el props
//        setProps el props
//        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
//        this.element <- el


//    override this.canUpdate prevElement oldProps =
//        let (changedProps,removedProps) = Interop.filterProps oldProps props
//        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
//        let canUpdateElement =
//            true

//        canUpdateView && canUpdateElement

//    override this.update prevElement oldProps = 
//        let element = prevElement :?> ScrollBarView
//        let (changedProps,removedProps) = Interop.filterProps oldProps props
//        ViewElement.removeProps prevElement removedProps
//        removeProps element removedProps
//        ViewElement.setProps prevElement changedProps
//        setProps element changedProps
//        this.element <- prevElement



type ScrollViewElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:ScrollView) props =
        props |> Interop.getValue<bool> "autoHideScrollBars"         |> Option.iter (fun v -> element.AutoHideScrollBars <- v)
        props |> Interop.getValue<bool> "keepContentAlwaysInViewport"|> Option.iter (fun v -> element.KeepContentAlwaysInViewport <- v)
        props |> Interop.getValue<bool> "showVerticalScrollIndicator"   |> Option.iter (fun v -> element.ShowVerticalScrollIndicator <- v)
        props |> Interop.getValue<bool> "showHorizontalScrollIndicator" |> Option.iter (fun v -> element.ShowHorizontalScrollIndicator <- v)
        props |> Interop.getValue<Size> "contentSize"                   |> Option.iter (fun v -> element.ContentSize <- v)
        props |> Interop.getValue<Point>"contentOffset"                 |> Option.iter (fun v -> element.ContentOffset <- v)

    let removeProps (element:ScrollView) props =
        props |> Interop.getValue<bool> "autoHideScrollBars"         |> Option.iter (fun v -> element.AutoHideScrollBars <- true)
        props |> Interop.getValue<bool> "keepContentAlwaysInViewport"|> Option.iter (fun v -> element.KeepContentAlwaysInViewport <- true)
        props |> Interop.getValue<bool> "showVerticalScrollIndicator"   |> Option.iter (fun v -> element.ShowVerticalScrollIndicator <- false)
        props |> Interop.getValue<bool> "showHorizontalScrollIndicator" |> Option.iter (fun v -> element.ShowHorizontalScrollIndicator <- false)

    override _.name = $"ScrollView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new ScrollView()
        parent |> Option.iter (fun p -> p.Add el)
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
        let element = prevElement :?> ScrollView
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement


//Todo
type StatusBarElement(props:IProperty list) =
    inherit TerminalElement(props) 

    let setProps (element:StatusBar) props =
        ()

    let removeProps (element:StatusBar) props =
        ()

    override _.name = $"StatusBar"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new StatusBar()
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<DataTable> "table"            |> Option.iter (fun v -> element.Table <- v)
        props |> Interop.getValue<TableView.TableStyle> "style" |> Option.iter (fun v -> element.Style <- v)
        props |> Interop.getValue<bool> "fullRowSelect"         |> Option.iter (fun v -> element.FullRowSelect <- v)
        props |> Interop.getValue<bool> "multiSelect"           |> Option.iter (fun v -> element.MultiSelect <- v)
        props |> Interop.getValue<int> "columnOffset"           |> Option.iter (fun v -> element.ColumnOffset <- v)
        props |> Interop.getValue<int> "rowOffset"              |> Option.iter (fun v -> element.RowOffset <- v)
        props |> Interop.getValue<int> "selectedColumn"         |> Option.iter (fun v -> element.SelectedColumn <- v)
        props |> Interop.getValue<int> "selectedRow"            |> Option.iter (fun v -> element.SelectedRow <- v)
        props |> Interop.getValue<int> "maxCellWidth"           |> Option.iter (fun v -> element.MaxCellWidth <- v)
        props |> Interop.getValue<string> "nullSymbol"          |> Option.iter (fun v -> element.NullSymbol <- v)
        props |> Interop.getValue<char> "separatorSymbol"       |> Option.iter (fun v -> element.SeparatorSymbol <- v)
        
        // onSelectedCellChanged
        props 
        |> Interop.getValue<TableView.SelectedCellChangedEventArgs->unit> "onSelectedCellChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedCellChanged" element
            element.add_SelectedCellChanged v
        )
        
        // onCellActivated
        props 
        |> Interop.getValue<TableView.CellActivatedEventArgs->unit> "onCellActivated" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "CellActivated" element
            element.add_CellActivated v
        )
        props |> Interop.getValue<Key> "cellActivationKey" |> Option.iter (fun v -> element.CellActivationKey <- v)
        

    let removeProps (element:TableView) props =
        props |> Interop.getValue<bool> "fullRowSelect" |> Option.iter (fun v -> element.FullRowSelect <- false)
        props |> Interop.getValue<bool> "multiSelect"   |> Option.iter (fun v -> element.MultiSelect <- false)
        // onSelectedCellChanged
        props 
        |> Interop.getValue<TableView.SelectedCellChangedEventArgs->unit> "onSelectedCellChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedCellChanged" element
        )
        
        // onCellActivated
        props 
        |> Interop.getValue<TableView.CellActivatedEventArgs->unit> "onCellActivated" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "CellActivated" element
        )

    override _.name = $"TableView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TableView()
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<int> "tabScrollOffset"    |> Option.iter (fun v -> element.TabScrollOffset <- v)
        props |> Interop.getValue<UInt32> "maxTabTextWidth" |> Option.iter (fun v -> element.MaxTabTextWidth <- v)
        props |> Interop.getValue<TabView.Tab> "selectedTab"|> Option.iter (fun v -> element.SelectedTab <- v)
        props |> Interop.getValue<TabView.TabStyle> "style" |> Option.iter (fun v -> element.Style <- v)

        props |> Interop.getValue<ITabProperty list> "tabs" |> Option.iter (fun v ->
            v
            |> List.choose (fun tabProp -> Interop.getTabValue<ITabItemProperty list> "tabItems" [tabProp])
            |> List.iter (fun tabItems ->
                let title = Interop.getTabItemValue<string> "title" tabItems
                let view = Interop.getTabItemValue<TerminalElement> "view" tabItems
                (title, view)
                ||> Option.map2 (fun title view ->
                    view.create None
                    TabView.Tab(title, view.element))
                |> Option.iter (fun tab -> element.AddTab(tab, false))))
        
        // onSelectedTabChanged
        props 
        |> Interop.getValue<EventHandler<TabView.TabChangedEventArgs>> "onSelectedTabChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedTabChanged" element
            element.add_SelectedTabChanged v
        )

    let removeProps (element:TabView) props =
        // onSelectedTabChanged
        props 
        |> Interop.getValue<EventHandler<TabView.TabChangedEventArgs>> "onSelectedTabChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectedTabChanged" element
        )

    override _.name = $"TabView"


    override this.canUpdate prevElement oldProps =
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TabView()
        parent |> Option.iter (fun p -> p.Add el)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        
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
        props |> Interop.getValue<string> "text"        |> Option.iter (fun v -> if element.Text.ToString()<>v then element.Text <- v)
        props |> Interop.getValue<bool> "used"          |> Option.iter (fun v -> element.Used <- v)
        props |> Interop.getValue<bool> "readOnly"      |> Option.iter (fun v -> element.ReadOnly <- v)
        props |> Interop.getValue<Rect> "frame"         |> Option.iter (fun v -> element.Frame <- v)
        props |> Interop.getValue<bool> "secret"        |> Option.iter (fun v -> element.Secret <- v)
        props |> Interop.getValue<int> "cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v)
        props |> Interop.getValue<bool> "canFocus"      |> Option.iter (fun v -> element.CanFocus <- v)
        props |> Interop.getValue<int> "selectedStart"  |> Option.iter (fun v -> element.SelectedStart <- v)
        props |> Interop.getValue<CursorVisibility> "desiredCursorVisibility"   |> Option.iter (fun v -> element.DesiredCursorVisibility <- v)
        // onTextChanging
        props 
        |> Interop.getValue<TextChangingEventArgs->unit> "onTextChanging" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "TextChanging" element
            element.add_TextChanging v
        )
        // onTextChanging
        props 
        |> Interop.getValue<string->unit> "onTextChangingString" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "TextChanging" element
            let action (ev:TextChangingEventArgs) = v (ev.NewText |> Interop.str); ev.Cancel <- false
            element.add_TextChanging action
        )
        // onTextChanged
        props 
        |> Interop.getValue<string->unit> "onTextChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "TextChanged" element
            let action ustr = v (ustr |> Interop.str)
            element.add_TextChanged action
        )

    let removeProps (element:TextField) props =
        props |> Interop.getValue<bool> "used"          |> Option.iter (fun v -> element.Used <- false)
        props |> Interop.getValue<bool> "readOnly"      |> Option.iter (fun v -> element.ReadOnly <- false)
        props |> Interop.getValue<bool> "secret"        |> Option.iter (fun v -> element.Secret <- false)
        props |> Interop.getValue<bool> "canFocus"      |> Option.iter (fun v -> element.CanFocus <- true)

        // onTextChanging
        props 
        |> Interop.getValue<TextChangingEventArgs->unit> "onTextChanging" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "TextChanging" element
        )
        
        // onTextChanged
        props 
        |> Interop.getValue<string->unit> "onTextChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "TextChanged" element
        )

    override _.name = $"TextField"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let text = props |> Interop.getValue<string> "text" |> Option.defaultValue ""
        let el = new TextField(text)
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<TextValidateProviders.ITextValidateProvider> "provider" |> Option.iter (fun v -> element.Provider <- v)
        props |> Interop.getValue<string> "text"|> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)

    let removeProps (element:TextValidateField) props =
        ()

    override _.name = $"TextValidateField"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TextValidateField()
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<string> "text"                |> Option.iter (fun v -> if Checker.textChanged element v then element.Text <- v)
        props |> Interop.getValue<Rect> "frame"                 |> Option.iter (fun v -> element.Frame <- v)
        props |> Interop.getValue<int> "topRow"                 |> Option.iter (fun v -> element.TopRow <- v)
        props |> Interop.getValue<int> "leftColumn"             |> Option.iter (fun v -> element.LeftColumn <- v)
        props |> Interop.getValue<Point> "cursorPosition"       |> Option.iter (fun v -> element.CursorPosition <- v)
        props |> Interop.getValue<int> "selectionStartColumn"   |> Option.iter (fun v -> element.SelectionStartColumn <- v)
        props |> Interop.getValue<int> "selectionStartRow"      |> Option.iter (fun v -> element.SelectionStartRow <- v)
        props |> Interop.getValue<int> "bottomOffset"           |> Option.iter (fun v -> element.BottomOffset <- v)
        props |> Interop.getValue<int> "rightOffset"            |> Option.iter (fun v -> element.RightOffset <- v)
        props |> Interop.getValue<int> "tabWidth"               |> Option.iter (fun v -> element.TabWidth <- v)
        props |> Interop.getValue<bool> "allowsReturn"          |> Option.iter (fun v -> element.AllowsReturn <- v)
        props |> Interop.getValue<bool> "allowsTab"             |> Option.iter (fun v -> element.AllowsTab <- v)
        props |> Interop.getValue<bool> "multiline"             |> Option.iter (fun v -> element.Multiline <- v)
        props |> Interop.getValue<bool> "readOnly"              |> Option.iter (fun v -> element.ReadOnly <- v)
        props |> Interop.getValue<bool> "selecting"             |> Option.iter (fun v -> element.Selecting <- v)
        props |> Interop.getValue<bool> "wordWrap"              |> Option.iter (fun v -> element.WordWrap <- v)
        props |> Interop.getValue<bool> "canFocus"              |> Option.iter (fun v -> element.CanFocus <- v)
        props |> Interop.getValue<bool> "used"                  |> Option.iter (fun v -> element.Used <- v)
        props |> Interop.getValue<CursorVisibility> "desiredCursorVisibility" |> Option.iter (fun v -> element.DesiredCursorVisibility <- v)
        // onTextChanged
        props 
        |> Interop.getValue<string->unit> "onTextChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "TextChanged" element
            let action () = v (element.Text |> Interop.str)
            element.add_TextChanged action
        )

    let removeProps (element:TextView) props =
        props |> Interop.getValue<bool> "allowsReturn"          |> Option.iter (fun v -> element.AllowsReturn <- true)
        props |> Interop.getValue<bool> "allowsTab"             |> Option.iter (fun v -> element.AllowsTab <- true)
        props |> Interop.getValue<bool> "multiline"             |> Option.iter (fun v -> element.Multiline <- true)
        props |> Interop.getValue<bool> "readOnly"              |> Option.iter (fun v -> element.ReadOnly <- false)
        props |> Interop.getValue<bool> "selecting"             |> Option.iter (fun v -> element.Selecting <- false)
        props |> Interop.getValue<bool> "wordWrap"              |> Option.iter (fun v -> element.WordWrap <- false)
        props |> Interop.getValue<bool> "canFocus"              |> Option.iter (fun v -> element.CanFocus <- true)
        props |> Interop.getValue<bool> "used"                  |> Option.iter (fun v -> element.Used <- true)
        props |> Interop.getValue<int> "tabWidth"               |> Option.iter (fun v -> element.TabWidth <- 4)
        // onTextChanged
        props 
        |> Interop.getValue<unit->unit> "onTextChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "TextChanged" element
        )

    override _.name = $"TextView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TextView()
        parent |> Option.iter (fun p -> p.Add el)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = 
            match props |> Interop.getValue<TimeSpan> "time" with
            | None ->
                new TimeField()
            | Some time -> 
                new TimeField(time)
        
        parent |> Option.iter (fun p -> p.Add el)
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
        props |> Interop.getValue<ITreeBuilder<Trees.ITreeNode>> "treeBuilder"      |> Option.iter (fun v -> element.TreeBuilder <- v)
        props |> Interop.getValue<TreeStyle> "style"                                |> Option.iter (fun v -> element.Style <- v)
        props |> Interop.getValue<bool> "multiSelect"                               |> Option.iter (fun v -> element.MultiSelect <- v)
        props |> Interop.getValue<bool> "allowLetterBasedNavigation"                |> Option.iter (fun v -> element.AllowLetterBasedNavigation <- v)
        props |> Interop.getValue<ITreeNode> "selectedObject"                       |> Option.iter (fun v -> element.SelectedObject <- v)
        props |> Interop.getValue<Key> "objectActivationKey"                        |> Option.iter (fun v -> element.ObjectActivationKey <- v)
        props |> Interop.getValue<Nullable<MouseFlags>> "objectActivationButton"    |> Option.iter (fun v -> element.ObjectActivationButton <- v)
        props |> Interop.getValue<ITreeNode->ColorScheme> "colorGetter"             |> Option.iter (fun v -> element.ColorGetter <- v)
        props |> Interop.getValue<int> "scrollOffsetVertical"                       |> Option.iter (fun v -> element.ScrollOffsetVertical <- v)
        props |> Interop.getValue<int> "scrollOffsetHorizontal"                     |> Option.iter (fun v -> element.ScrollOffsetHorizontal <- v)
        props |> Interop.getValue<AspectGetterDelegate<ITreeNode>> "aspectGetter"   |> Option.iter (fun v -> element.AspectGetter <- v)
        props |> Interop.getValue<CursorVisibility> "desiredCursorVisibility"       |> Option.iter (fun v -> element.DesiredCursorVisibility <- v)
        props |> Interop.getValue<string list> "items"                              |> Option.iter (fun v -> v |> List.iter (fun text -> element.AddObject(TreeNode(text))))
        props |> Interop.getValue<ITreeNode list> "nodes"                           |> Option.iter (fun v -> v |> List.iter element.AddObject)
        // onObjectActivated
        props 
        |> Interop.getValue<ObjectActivatedEventArgs<ITreeNode>->unit> "onObjectActivated" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "ObjectActivated" element
            element.add_ObjectActivated v
        )
        // onSelectionChanged
        props 
        |> Interop.getValue<SelectionChangedEventArgs<ITreeNode>->unit> "onSelectionChanged" 
        |> Option.iter (fun v -> 
            Interop.removeEventHandlerIfNecessary "SelectionChanged" element
            element.SelectionChanged.Add v
        )

    let removeProps (element:TreeView) props =
        ()

    override _.name = $"TreeView"


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new TreeView()
        parent |> Option.iter (fun p -> p.Add el)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new Dialog()
        parent |> Option.iter (fun p -> p.Add el)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new FileDialog()
        parent |> Option.iter (fun p -> p.Add el)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new SaveDialog()
        parent |> Option.iter (fun p -> p.Add el)
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


    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{this.name} created!")
        #endif
        this.parent <- parent
        let el = new OpenDialog()
        parent |> Option.iter (fun p -> p.Add el)
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
        let element = prevElement :?> OpenDialog
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement



