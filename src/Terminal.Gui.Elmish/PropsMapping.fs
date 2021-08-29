namespace Terminal.Gui.Elmish

open Terminal.Gui
open NStack
open System
open System.Collections

[<AutoOpen>]
module internal PropsMappings =

    [<AutoOpen>]
    module Styles =

        let private convDim (dim:Dimension) =
            match dim with
            | Fill -> Dim.Fill()
            | FillMargin m -> Dim.Fill(m)
            | AbsDim i -> Dim.Sized(i)
            | PercentDim p -> Dim.Percent(p |> float32)


        let private convPos (dim:Position) =
            match dim with
            | Position.AbsPos i -> Pos.At(i)
            | Position.PercentPos p -> Pos.Percent(p |> float32)
            | Position.CenterPos -> Pos.Center()
    
    
        let addTextAlignmentToView (view:View) (alignment:Props.TextAlignment) =
            match alignment with
            | Left -> view.TextAlignment <- Terminal.Gui.TextAlignment.Left
            | Right -> view.TextAlignment <- Terminal.Gui.TextAlignment.Right
            | Centered -> view.TextAlignment <- Terminal.Gui.TextAlignment.Centered
            | Justified -> view.TextAlignment <- Terminal.Gui.TextAlignment.Justified
    
    
        let (|IsDimAbsolute|_|) (o:obj) =
            let n = o.GetType().Name
            if n = "DimAbsolute" then Some () else None


        let getTopLevelColorScheme () =
            new Terminal.Gui.ColorScheme(Normal=Application.Top.ColorScheme.Normal,
                Focus=Application.Top.ColorScheme.Focus,
                HotNormal=Application.Top.ColorScheme.HotNormal,
                HotFocus=Application.Top.ColorScheme.HotFocus,
                Disabled=Application.Top.ColorScheme.Disabled
            )
            

        let private addStyleToView (view:View) (style:Style) =
            view.SetNeedsDisplay()
            match style with
            | Pos (x,y) ->
                match x,y with
                | AbsPos _, AbsPos _ ->
                    view.LayoutStyle <- LayoutStyle.Absolute
                    view.X <- x |> convPos
                    view.Y <- y |> convPos
                | AbsPos _, _ ->
                    view.LayoutStyle <- LayoutStyle.Absolute
                    view.X <- x |> convPos
                    view.LayoutStyle <- LayoutStyle.Computed
                    view.Y <- y |> convPos
                | _, AbsPos _ ->
                    view.LayoutStyle <- LayoutStyle.Absolute
                    view.Y <- y |> convPos
                    view.LayoutStyle <- LayoutStyle.Computed
                    view.X <- x |> convPos
                    
                | _ ->
                    view.LayoutStyle <- LayoutStyle.Computed
                    view.X <- x |> convPos
                    view.Y <- y |> convPos
                //view.X <- x |> convPos
                //view.Y <- y |> convPos
    
            | Dim (width,height) ->
                
                match width,height with
                | AbsDim _, AbsDim _ ->
                    view.LayoutStyle <- LayoutStyle.Absolute
                    view.Width <- width |> convDim
                    view.Height <- height |> convDim
                | AbsDim _, _ ->
                    view.LayoutStyle <- LayoutStyle.Absolute
                    view.Width <- width |> convDim
                    view.LayoutStyle <- LayoutStyle.Computed
                    view.Height <- height |> convDim
                | _, AbsDim _ ->
                    view.LayoutStyle <- LayoutStyle.Absolute
                    view.Height <- height |> convDim
                    view.LayoutStyle <- LayoutStyle.Computed
                    view.Width <- width |> convDim
                    
                | _ ->
                    view.LayoutStyle <- LayoutStyle.Computed
                    view.Width <- width |> convDim
                    view.Height <- height |> convDim
                //view.Width <- width |> convDim
                //view.Height <- height |> convDim

            | TextAlignment alignment ->
                match view with
                | :? Label as label ->                
                    alignment |> addTextAlignmentToView label
                | _ -> 
                    ()
    
            | TextColorScheme color ->
                let colorScheme =
                    let s = 
                        getTopLevelColorScheme ()
                    match color with
                    | Normal -> s.Normal
                    | Focus -> s.Focus
                    | HotNormal -> s.HotNormal
                    | HotFocus -> s.HotFocus

                
                match view with
                | :? Label as label ->         
                    if label.ColorScheme = null then
                        label.ColorScheme <- getTopLevelColorScheme ()
                    label.ColorScheme.Normal <- colorScheme
                | _ -> 
                    ()
    
            | Colors (fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                view.ColorScheme <- getTopLevelColorScheme ()
                view.ColorScheme.Normal <- color
    
            | FocusColors (fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                view.ColorScheme <- getTopLevelColorScheme ()
                view.ColorScheme.Focus <- color
    
            | HotNormalColors(fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                view.ColorScheme <- getTopLevelColorScheme ()
                view.ColorScheme.HotNormal <- color
    
            | HotFocusedColors(fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                view.ColorScheme <- getTopLevelColorScheme ()
                view.ColorScheme.HotFocus <- color
    
            | DisabledColors(fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                view.ColorScheme <- getTopLevelColorScheme ()
                view.ColorScheme.Disabled <- color
            

        
        let private resetAllStyleOnView (view:View) (style:Style) =
                Left |> addTextAlignmentToView view
                view.ColorScheme <- null
    


        let setStylesToElement (styles: Style list) (element:View) =
            styles |> List.iter (fun s -> addStyleToView element s)

        let resetStylesOnElement (styles: Style list) (element:View) =
            styles |> List.iter (fun s -> resetAllStyleOnView element s)


    [<AutoOpen>]
    module Props =

        let ustr (x:string) = ustring.Make(x)

        //let inline setCommonObjStyleOnly (prop:View) (element:View) =
        //    match prop with
        //    | Styles styles ->
        //        element |> setStylesToElement styles
        //    | _ ->
        //        ()


        //let inline resetCommonObjStyleOnly (prop:CommonProp<'a>) (element:View) =
        //    match prop with
        //    | Styles styles ->
        //        element |> resetStylesOnElement styles
        //    | _ ->
        //        ()

        let removeEventHandlerIfNecessary evName element =
            let eventDel = EventHelpers.getEventDelegates evName element
            if (eventDel.Length > 0) then
                EventHelpers.clearEventDelegates evName element

        module Setters =


            let inline setViewPropToBaseElement<'a> (prop:ViewProp<'a>) (element:View) =
                match prop with
                | Styles styles ->
                    match element with
                    | :? DateField ->
                        if styles |> List.map (fun i -> i.GetType().Name) |> List.contains ("Dim") then
                            failwith ("Dim is not allowed in a date field")
                        element |> setStylesToElement styles
                    | :? TimeField ->
                        if styles |> List.map (fun i -> i.GetType().Name) |> List.contains ("Dim") then
                            failwith ("Dim is not allowed in a time field")
                        element |> setStylesToElement styles
                    |_ ->
                        element |> setStylesToElement styles
                | OnEntered func            -> 
                    element |> removeEventHandlerIfNecessary "Enter"
                    element.add_Enter(Action<View.FocusEventArgs>(func))
                | OnLeft func               -> 
                    element |> removeEventHandlerIfNecessary "Leave"
                    element.add_Leave(Action<View.FocusEventArgs>(func))
                | OnMouseEntered func       -> 
                    element |> removeEventHandlerIfNecessary "MouseEnter"
                    element.add_MouseEnter(Action<View.MouseEventArgs>(func))
                | OnMouseLeft func          -> 
                    element |> removeEventHandlerIfNecessary "MouseLeave"
                    element.add_MouseLeave(Action<View.MouseEventArgs>(func))
                | OnMouseClicked func       -> 
                    element |> removeEventHandlerIfNecessary "MouseClick"
                    element.add_MouseClick(Action<View.MouseEventArgs>(func))
                | OnCanFocusChanged func    -> 
                    element |> removeEventHandlerIfNecessary "CanFocusChanged"
                    element.add_CanFocusChanged(Action(func))
                | OnEnabledChanged func     -> 
                    element |> removeEventHandlerIfNecessary "EnabledChanged"
                    element.add_EnabledChanged(Action(func))
                | OnVisibleChanged func     -> 
                    element |> removeEventHandlerIfNecessary "VisibleChanged"
                    element.add_VisibleChanged(Action(func))
                | HotKey key                -> 
                    element.HotKey <- key
                | ShortCut key              -> 
                    element.Shortcut <- key
                | TabIndex idx              -> 
                    element.TabIndex <- idx
                | TabStop tabStop           -> 
                    element.TabStop <- tabStop
                | Text text ->
                    let eventDel = EventHelpers.getEventDelegates "TextChanged" element
                    if (eventDel.Length > 0) then
                        EventHelpers.clearEventDelegates "TextChanged" element

                    match element with
                    | :? Button as e ->
                        e.Text <- ustring.Make(text)
                    | :? Label as e ->
                        e.Text <- ustring.Make(text)
                    | :? CheckBox as e ->
                        e.Text <- ustring.Make(text)
                    | :? TextField as e ->
                        e.Text <- ustring.Make(text)
                    | :? ComboBox as e ->
                        e.Text <- ustring.Make(text)
                    |_ ->
                        element.Text <- ustring.Make(text) 
                    EventHelpers.addEventDelegates "TextChanged" eventDel element

            let setPropToBaseViewElement (prop:IProp) (element:View) =
                match prop with
                | :? ViewProp<string> as prop ->
                    setViewPropToBaseElement prop element
                | :? ViewProp<DateTime> as prop ->
                    setViewPropToBaseElement prop element
                | :? ViewProp<TimeSpan> as prop ->
                    setViewPropToBaseElement prop element
                | :? ViewProp<obj> as prop ->
                    setViewPropToBaseElement prop element
                | _ -> ()


            let setPropToWindowElement (prop:IProp) (element:Window) =
                match prop with 
                | :? WindowProps as prop->
                    match prop with
                    | Title title ->
                        element.Title <- ustring.Make(title)
                
                | _ -> ()
            
        
            let setPropsToTextElement (prop:IProp) (element:Label) =
                ()
        
            let setPropToTextFieldElement (prop:IProp) (element:TextField) =
                match prop with
                | :? ViewProp<obj> as prop ->
                    match prop with
                    | Text text ->
                        element.CursorPosition <- text.Length
                        // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                        element.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                        element.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 
                    | _ ->
                        ()

                | :? CommonProp<string> as prop -> 
                    match prop with
                    | OnChanged changed ->
                        EventHelpers.clearEventDelegates "TextChanging" element
                        element.add_TextChanging(fun ev -> changed(ev.NewText.ToString()))
                    | Value value ->
                        // remove change event handler to avoid endless loop
                        let eventDel = EventHelpers.getEventDelegates "TextChanging" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "TextChanging" element
                        element.Text <- ustring.Make(value) 
                        EventHelpers.addEventDelegates "TextChanging" eventDel element
                        

                        element.CursorPosition <- value.Length
                        // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                        element.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                        element.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 
                    
                | :? TextFieldProps as prop ->
                    match prop with
                    | OnTextChanged changed ->
                        let eventDel = EventHelpers.getEventDelegates "TextChanging" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "TextChanging" element
                        element.add_TextChanging(fun ev -> changed(ev.NewText.ToString()))

                    | Secret ->
                        element.Secret <- true
                | _ -> ()
        
            let setPropToButtonElement (prop:IProp) (element:Button) =
                match prop with
                | :? TextFieldProps as prop ->
                    match prop with
                    | _ -> ()

                | :? ButtonProp as prop ->
                    match prop with
                    | OnClicked f ->
                        let eventDel = EventHelpers.getEventDelegates "Clicked" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "Clicked" element
                        element.add_Clicked(Action(f))
                | _ -> ()
                
        
            let setPropToDateFieldElement (prop:IProp) (element:DateField) =
                match prop with
                | :? CommonProp<DateTime> as prop ->
                    match prop with
                    | OnChanged changed ->
                        let eventDel = EventHelpers.getEventDelegates "DateChanged" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "DateChanged" element
                        element.add_DateChanged(fun dateEv -> changed(dateEv.NewValue))
                    
                    | Value value ->
                        // remove change event handler to avoid endless loop
                        let eventDel = EventHelpers.getEventDelegates "DateChanged" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "DateChanged" element
                        element.Date <- value
                        EventHelpers.addEventDelegates "DateChanged" eventDel element
                        
                        // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                        element.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                        element.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 
                | :? DateTimeProps as prop ->
                    match prop with
                    | IsShort -> 
                        element.IsShortFormat <- true
                | _ -> ()
        
            let setPropToTimeFieldElement (prop:IProp) (element:TimeField) =
                match prop with
                | :? CommonProp<TimeSpan> as prop ->
                    match prop with
                    | OnChanged changed ->
                        let eventDel = EventHelpers.getEventDelegates "TimeChanged" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "TimeChanged" element
                        element.add_TimeChanged(fun timeEv -> changed(timeEv.NewValue))

                    | Value value ->
                        // remove change event handler to avoid endless loop
                        let eventDel = EventHelpers.getEventDelegates "TimeChanged" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "TimeChanged" element
                        element.Time <- value
                        EventHelpers.addEventDelegates "TimeChanged" eventDel element
                        
                        // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                        element.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                        element.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 

                | :? DateTimeProps as prop ->
                    match prop with
                    | IsShort -> 
                        element.IsShortFormat <- true
                    
                | _ -> ()
        
            let setPropToTextViewElement (prop:IProp) (element:TextView) =
                match prop with
                | :? ViewProp<string> as prop ->
                    match prop with
                    | Text text ->
                        ()
                    | _ -> ()
                | _ -> ()
        
            let setPropToFrameViewElement (prop:IProp) (element:FrameView) =
                match prop with
                | _ -> ()
        
            let setPropToHexViewElement (prop:IProp) (element:HexView) =
                match prop with
                | _ -> ()
        
            let setPropToListViewElement (prop:IProp) (element:ListView) =
                match prop with
                | :? CommonProp<obj> as prop ->
                    match prop with
                    | Value value ->
                        if element.Data |> isNull |> not then
                            let items = unbox<(obj * string) list> element.Data
                            let idx = items |> List.findIndex (fun (o,_)-> o = value)

                            // remove change event handler to avoid endless loop
                            let eventDel = EventHelpers.getEventDelegates "SelectedItemChanged" element
                            if (eventDel.Length > 0) then
                                EventHelpers.clearEventDelegates "SelectedItemChanged" element
                            element.SelectedItem <- idx
                            EventHelpers.addEventDelegates "SelectedItemChanged" eventDel element
                        
                    | OnChanged onChanged ->
                        if element.Data |> isNull |> not then
                            let eventDel = EventHelpers.getEventDelegates "SelectedItemChanged" element
                            if (eventDel.Length > 0) then
                                EventHelpers.clearEventDelegates "SelectedItemChanged" element
                            element.add_SelectedItemChanged(fun ev -> 
                                let items = unbox<(obj * string) list> element.Data
                                let (item,_) = items.[ev.Item]
                                onChanged(item)
                            )
                | :? ListProps<obj> as prop ->
                    match prop with
                    | Items items ->
                        
                        // Selected Item will be fired when set the RadioLabels
                        let eventDel = EventHelpers.getEventDelegates "SelectedItemChanged" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "SelectedItemChanged" element
                        let entries = (items |> List.map snd |> List.toArray) :> IList
                        element.SetSource(entries)
                        element.Data <- (box items)
                
                    
                | _ -> ()
        
            let setPropToProgressBarElement (prop:IProp) (element:ProgressBar) =
                match prop with
                | _ -> ()
        
            let setPropToCheckBoxElement (prop:IProp) (element:CheckBox) =
                match prop with
                | _ -> ()
        
            let setPropToRadioGroupElement (prop:IProp) (element:RadioGroup) =
                match prop with
                | :? CommonProp<obj> as prop ->
                    match prop with
                    | Value value ->
                        if element.Data |> isNull |> not then
                            let items = unbox<(obj * string) list> element.Data
                            let idx = items |> List.findIndex (fun (o,_)-> o = value)

                            // remove change event handler to avoid endless loop
                            let eventDel = EventHelpers.getEventDelegates "SelectedItemChanged" element
                            if (eventDel.Length > 0) then
                                EventHelpers.clearEventDelegates "SelectedItemChanged" element
                            element.SelectedItem <- idx
                            EventHelpers.addEventDelegates "SelectedItemChanged" eventDel element
                        
                    | OnChanged onChanged ->
                        if element.Data |> isNull |> not then
                            let eventDel = EventHelpers.getEventDelegates "SelectedItemChanged" element
                            if (eventDel.Length > 0) then
                                EventHelpers.clearEventDelegates "SelectedItemChanged" element
                            element.add_SelectedItemChanged(fun ev -> 
                                if (ev.PreviousSelectedItem <> ev.SelectedItem) then
                                    let items = unbox<(obj * string) list> element.Data
                                    let (item,_) = items.[ev.SelectedItem]
                                    onChanged(item)
                            )
                | :? ListProps<obj> as prop ->
                    match prop with
                    | Items items ->
                        // Selected Item will be fired when set the RadioLabels
                        let eventDel = EventHelpers.getEventDelegates "SelectedItemChanged" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "SelectedItemChanged" element
                        let labels = items |> List.map (fun (_,s) -> ustr (s)) |> List.toArray
                        element.RadioLabels <- labels
                        element.Data <- (box items)
                
                    
                | _ -> ()

            
        
        
        module Remove =
        

            let removePropFromBaseViewElement (prop:IProp) (element:View) =
                match prop with
                | :? ViewProp<obj> as prop ->
                    match prop with
                    | Styles styles ->
                        element |> resetStylesOnElement styles
                    | OnEntered func            -> 
                        element |> removeEventHandlerIfNecessary "Enter"
                    | OnLeft func               -> 
                        element |> removeEventHandlerIfNecessary "Leave"
                    | OnMouseEntered func       -> 
                        element |> removeEventHandlerIfNecessary "MouseEnter"
                    | OnMouseLeft func          -> 
                        element |> removeEventHandlerIfNecessary "MouseLeave"
                    | OnMouseClicked func       -> 
                        element |> removeEventHandlerIfNecessary "MouseClick"
                    | OnCanFocusChanged func    -> 
                        element |> removeEventHandlerIfNecessary "CanFocusChanged"
                    | OnEnabledChanged func     -> 
                        element |> removeEventHandlerIfNecessary "EnabledChanged"
                    | OnVisibleChanged func     -> 
                        element |> removeEventHandlerIfNecessary "VisibleChanged"
                    | HotKey key                -> 
                        element.HotKey <- Key.Unknown
                    | ShortCut key              -> 
                        element.Shortcut <- Key.Unknown
                    | TabIndex idx              -> 
                        element.TabIndex <- 0
                    | TabStop tabStop           -> 
                        element.TabStop <- false
                    | Text text ->
                        element.Text <- ustr ""
                | _ -> ()


            let removePropFromWindowElement (prop:IProp) (element:Window) =
                match prop with 
                | :? WindowProps as prop->
                    match prop with
                    | Title title ->
                        element.Title <- ustring.Make(title)
                        
                | _ -> ()
                    
                
            let removePropsFromTextElement (prop:IProp) (element:Label) =
                match prop with
                | _ -> ()
                
            let removePropFromTextFieldElement (prop:IProp) (element:TextField) =
                match prop with
                | :? CommonProp<string> as prop ->
                    match prop with
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "TextChanged" element
                    | _ ->
                        ()
                        

                | :? TextFieldProps as prop ->
                    match prop with
                    | OnTextChanged _ ->
                        EventHelpers.clearEventDelegates "TextChanged" element
                    | Secret ->
                        element.Secret <- false
                | _ -> ()
                
            let removePropFromButtonElement (prop:IProp) (element:Button) =
                match prop with
                | :? ButtonProp as prop ->
                    match prop with
                    | OnClicked _ ->
                        EventHelpers.clearEventDelegates "Clicked" element
                | _ -> ()
                
            let removePropFromDateFieldElement (prop:IProp) (element:DateField) =
                match prop with
                | :? CommonProp<DateTime> as prop ->
                    match prop with
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "DateChanged" element
                    | _ ->
                        ()
                | :? DateTimeProps as prop ->
                    match prop with

                    | IsShort -> 
                        element.IsShortFormat <- false
                | _ -> ()
                
            let removePropFromTimeFieldElement (prop:IProp) (element:TimeField) =
                match prop with
                | :? CommonProp<TimeSpan> as prop ->
                    match prop with
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "TimeChanged" element
                    | _ ->
                        ()
                | :? DateTimeProps as prop ->
                    match prop with
                    | IsShort -> 
                        element.IsShortFormat <- false
                            
                | _ -> ()
                
            let removePropFromTextViewElement (prop:IProp) (element:TextView) =
                match prop with
                | _ -> ()
                
            let removePropFromFrameViewElement (prop:IProp) (element:FrameView) =
                match prop with
                | _ -> ()
                
            let removePropFromHexViewElement (prop:IProp) (element:HexView) =
                match prop with
                | _ -> ()
                
            let removePropFromListViewElement (prop:IProp) (element:ListView) =
                match prop with
                | :? CommonProp<obj> as prop -> 
                    match prop with
                    | Value a ->
                        element.SelectedItem <- 0
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "SelectedItemChanged" element
                    
                | :? ListProps<obj> as listProp ->
                    
                    match listProp with
                    | Items items ->
                        element.Data <- null
                        element.SetSource(null)
                | _ -> ()
                
            let removePropFromProgressBarElement (prop:IProp) (element:ProgressBar) =
                match prop with
                | _ -> ()
                
            let removePropFromCheckBoxElement (prop:IProp) (element:CheckBox) =
                match prop with
                | _ -> ()
                
            let removePropFromRadioGroupElement (prop:IProp) (element:RadioGroup) =
                match prop with
                | :? CommonProp<obj> as prop -> 
                    match prop with
                    | Value a ->
                        element.SelectedItem <- 0
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "SelectedItemChanged" element
                    
                | :? ListProps<obj> as listProp ->
                    
                    match listProp with
                    | Items items ->
                        element.Data <- null
                        element.RadioLabels <- [||]
                | _ -> ()

        open Setters
        
        let setPropsToElement (props:IProp list) (element:View) =
            // Styles in the end, but text at first
            props 
            |> List.sortWith (
                fun e1 e2 -> 
                    match e1 with 
                    | :? ViewProp<obj> as prop -> 
                        match prop with | Text _ -> 1 | _ -> -1
                    | _ -> -1
            ) 
            |> List.iter (fun p -> 
                setPropToBaseViewElement p element
            )

            match element with
            | :? Window as element ->       props |> List.iter (fun p -> setPropToWindowElement p element)
            | :? Label as element ->        props |> List.iter (fun p -> setPropsToTextElement p element)
            | :? Button as element ->       props |> List.iter (fun p -> setPropToButtonElement p element)
            | :? DateField as element ->    props |> List.iter (fun p -> setPropToDateFieldElement p element)
            | :? TimeField as element ->    props |> List.iter (fun p -> setPropToTimeFieldElement p element)
            | :? TextField as element ->    props |> List.iter (fun p -> setPropToTextFieldElement p element)
            | :? TextView as element ->     props |> List.iter (fun p -> setPropToTextViewElement p element)
            | :? FrameView as element ->    props |> List.iter (fun p -> setPropToFrameViewElement p element)
            | :? HexView as element ->      props |> List.iter (fun p -> setPropToHexViewElement p element)
            | :? ListView as element ->     
                props 
                |> List.sortWith (
                    fun e1 e2 -> 
                        match e1 with 
                        | :? ListProps<obj> as prop -> 
                            match prop with | Items _ -> -1 
                        | _ -> 1
                )
                |> List.iter (fun p -> setPropToListViewElement p element)
            | :? ProgressBar as element ->  props |> List.iter (fun p -> setPropToProgressBarElement p element)
            | :? CheckBox as element ->     props |> List.iter (fun p -> setPropToCheckBoxElement p element)
            // Force here to process the Items at first!
            | :? RadioGroup as element ->   
                props 
                |> List.sortWith (
                    fun e1 e2 -> 
                        match e1 with 
                        | :? ListProps<obj> as prop -> 
                            match prop with | Items _ -> -1 
                        | _ -> 1
                ) 
                |> List.iter (fun p -> setPropToRadioGroupElement p element)

            | _ -> ()
            
                
        
        open Remove

        let removePropsToElement (props:IProp list) (element:View) =
            props |> List.iter (fun p -> removePropFromBaseViewElement p element)
            match element with
            | :? Window as element ->       props |> List.iter (fun p -> removePropFromWindowElement p element)
            | :? Label as element ->        props |> List.iter (fun p -> removePropsFromTextElement p element)
            | :? Button as element ->       props |> List.iter (fun p -> removePropFromButtonElement p element)
            | :? DateField as element ->    props |> List.iter (fun p -> removePropFromDateFieldElement p element)
            | :? TimeField as element ->    props |> List.iter (fun p -> removePropFromTimeFieldElement p element)
            | :? TextField as element ->    props |> List.iter (fun p -> removePropFromTextFieldElement p element)
            | :? TextView as element ->     props |> List.iter (fun p -> removePropFromTextViewElement p element)
            | :? FrameView as element ->    props |> List.iter (fun p -> removePropFromFrameViewElement p element)
            | :? HexView as element ->      props |> List.iter (fun p -> removePropFromHexViewElement p element)
            | :? ListView as element ->     props |> List.iter (fun p -> removePropFromListViewElement p element)
            | :? ProgressBar as element ->  props |> List.iter (fun p -> removePropFromProgressBarElement p element)
            | :? CheckBox as element ->     props |> List.iter (fun p -> removePropFromCheckBoxElement p element)
            | :? RadioGroup as element ->   props |> List.iter (fun p -> removePropFromRadioGroupElement p element)

            | _ -> ()

