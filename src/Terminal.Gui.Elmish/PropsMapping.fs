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

        let inline setCommonObjStyleOnly (prop:CommonProp<'a>) (element:View) =
            match prop with
            | Styles styles ->
                element |> setStylesToElement styles
            | _ ->
                ()


        let inline resetCommonObjStyleOnly (prop:CommonProp<'a>) (element:View) =
            match prop with
            | Styles styles ->
                element |> resetStylesOnElement styles
            | _ ->
                ()

        module Setters =

            let setPropToWindowElement (prop:IProp) (element:Window) =
                match prop with 
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | :? WindowProps as prop->
                    match prop with
                    | Title title ->
                        element.Title <- ustring.Make(title)
                
                | _ -> ()
            
        
            let setPropsToTextElement (prop:IProp) (element:Label) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | :? TextFieldProps as prop ->
                    match prop with
                    | Text text ->
                        element.Text <- ustring.Make(text)
                    | _ ->
                        ()
                | _ -> ()
        
            let setPropToTextFieldElement (prop:IProp) (element:TextField) =
                match prop with
                | :? CommonProp<string> as prop -> 
                    match prop with
                    | Styles styles ->
                        element |> setStylesToElement styles
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
                    | Text text ->
                        // remove change event handler to avoid endless loop
                        let eventDel = EventHelpers.getEventDelegates "TextChanged" element
                        if (eventDel.Length > 0) then
                            EventHelpers.clearEventDelegates "TextChanged" element
                        element.Text <- ustring.Make(text) 
                        EventHelpers.addEventDelegates "TextChanged" eventDel element
                        element.CursorPosition <- text.Length
                        // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                        element.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                        element.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 
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
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | :? TextFieldProps as prop ->
                    match prop with
                    | Text text ->
                        element.Text <- ustring.Make(text) 
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
                    | Styles styles ->
                        if styles |> List.map (fun i -> i.GetType().Name) |> List.contains ("Dim") then
                            failwith ("Dim is not allowed in a date field")
                        element |> setStylesToElement styles
                
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
                    | Styles styles ->
                        if styles |> List.map (fun i -> i.GetType().Name) |> List.contains ("Dim") then
                            failwith ("Dim is not allowed in a time field")
                        element |> setStylesToElement styles
                
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
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | _ -> ()
        
            let setPropToFrameViewElement (prop:IProp) (element:FrameView) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | _ -> ()
        
            let setPropToHexViewElement (prop:IProp) (element:HexView) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | _ -> ()
        
            let setPropToListViewElement (prop:IProp) (element:ListView) =
                match prop with
                | :? CommonProp<obj> as prop ->
                    match prop with
                    | Styles _ ->
                        setCommonObjStyleOnly prop element 
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
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | _ -> ()
        
            let setPropToCheckBoxElement (prop:IProp) (element:CheckBox) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | _ -> ()
        
            let setPropToRadioGroupElement (prop:IProp) (element:RadioGroup) =
                
                match prop with
                | :? CommonProp<obj> as prop ->
                    match prop with
                    | Styles _ ->
                        setCommonObjStyleOnly prop element 
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
        
            let removePropToWindowElement (prop:IProp) (element:Window) =
                match prop with 
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | :? WindowProps as prop->
                    match prop with
                    | Title title ->
                        element.Title <- ustring.Make(title)
                        
                | _ -> ()
                    
                
            let removePropsToTextElement (prop:IProp) (element:Label) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | :? TextFieldProps as prop ->
                    match prop with
                    | Text text ->
                        element.Text <- ustring.Make(text)
                    
                    | _ ->
                        ()
                | _ -> ()
                
            let removePropToTextFieldElement (prop:IProp) (element:TextField) =
                match prop with
                | :? CommonProp<string> as prop ->
                    match prop with
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "TextChanged" element
                    | Styles _ ->
                        resetCommonObjStyleOnly prop element
                    | _ ->
                        ()
                        

                | :? TextFieldProps as prop ->
                    match prop with
                    | OnTextChanged _ ->
                        EventHelpers.clearEventDelegates "TextChanged" element
                    | Secret ->
                        element.Secret <- false
                    | _ ->
                        ()

                | _ -> ()
                
            let removePropToButtonElement (prop:IProp) (element:Button) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | :? ButtonProp as prop ->
                    match prop with
                    | OnClicked _ ->
                        EventHelpers.clearEventDelegates "Clicked" element
                | _ -> ()
                
            let removePropToDateFieldElement (prop:IProp) (element:DateField) =
                match prop with
                | :? CommonProp<DateTime> as prop ->
                    match prop with
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "DateChanged" element
                    | Styles _ ->
                        resetCommonObjStyleOnly prop element
                    | _ ->
                        ()
                | :? DateTimeProps as prop ->
                    match prop with

                    | IsShort -> 
                        element.IsShortFormat <- false
                | _ -> ()
                
            let removePropToTimeFieldElement (prop:IProp) (element:TimeField) =
                match prop with
                | :? CommonProp<TimeSpan> as prop ->
                    match prop with
                    | OnChanged _ ->
                        EventHelpers.clearEventDelegates "TimeChanged" element
                    | Styles _ ->
                        resetCommonObjStyleOnly prop element
                    | _ ->
                        ()
                | :? DateTimeProps as prop ->
                    match prop with
                    | IsShort -> 
                        element.IsShortFormat <- false
                            
                | _ -> ()
                
            let removePropToTextViewElement (prop:IProp) (element:TextView) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | _ -> ()
                
            let removePropToFrameViewElement (prop:IProp) (element:FrameView) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | _ -> ()
                
            let removePropToHexViewElement (prop:IProp) (element:HexView) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | _ -> ()
                
            let removePropToListViewElement (prop:IProp) (element:ListView) =
                match prop with
                | :? CommonProp<obj> as prop -> 
                    match prop with
                    | Styles styles ->
                        resetCommonObjStyleOnly prop element 
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
                
            let removePropToProgressBarElement (prop:IProp) (element:ProgressBar) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | _ -> ()
                
            let removePropToCheckBoxElement (prop:IProp) (element:CheckBox) =
                match prop with
                | :? CommonProp<_> as prop -> 
                    resetCommonObjStyleOnly prop element 
                | _ -> ()
                
            let removePropToRadioGroupElement (prop:IProp) (element:RadioGroup) =
                match prop with
                | :? CommonProp<obj> as prop -> 
                    match prop with
                    | Styles styles ->
                        resetCommonObjStyleOnly prop element 
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
            match element with
            | :? Window as element ->       props |> List.iter (fun p -> removePropToWindowElement p element)
            | :? Label as element ->        props |> List.iter (fun p -> removePropsToTextElement p element)
            | :? Button as element ->       props |> List.iter (fun p -> removePropToButtonElement p element)
            | :? DateField as element ->    props |> List.iter (fun p -> removePropToDateFieldElement p element)
            | :? TimeField as element ->    props |> List.iter (fun p -> removePropToTimeFieldElement p element)
            | :? TextField as element ->    props |> List.iter (fun p -> removePropToTextFieldElement p element)
            | :? TextView as element ->     props |> List.iter (fun p -> removePropToTextViewElement p element)
            | :? FrameView as element ->    props |> List.iter (fun p -> removePropToFrameViewElement p element)
            | :? HexView as element ->      props |> List.iter (fun p -> removePropToHexViewElement p element)
            | :? ListView as element ->     props |> List.iter (fun p -> removePropToListViewElement p element)
            | :? ProgressBar as element ->  props |> List.iter (fun p -> removePropToProgressBarElement p element)
            | :? CheckBox as element ->     props |> List.iter (fun p -> removePropToCheckBoxElement p element)
            | :? RadioGroup as element ->   props |> List.iter (fun p -> removePropToRadioGroupElement p element)

            | _ -> ()

