namespace Terminal.Gui.Elmish


open Terminal.Gui
open NStack
open System
open System.Data

type ViewElementType =
    | PageElement
    | WindowElement
    | LabelElement
    | TextFieldElement
    | ButtonElement
    | TimeFieldElement
    | DateFieldElement
    | TextViewElement
    | FrameViewElement
    | HexViewElement
    | ListViewElement
    | ProgressBarElement
    | CheckBoxElement
    | RadioGroupElement
    | ScrollViewElement



type IProp = interface end

type IProp<'a> = 
    inherit IProp

[<AutoOpen>]
module Props =

    type Position =
        | AbsPos of int
        | PercentPos of float
        | CenterPos

    type Dimension =
        | Fill
        | FillMargin of int
        | AbsDim of int
        | PercentDim of float

    type TextAlignment =
        | Left 
        | Right
        | Centered
        | Justified

    type ColorScheme =
        | Normal
        | Focus
        | HotNormal
        | HotFocus

    type ScrollBar =
        | NoBars
        | HorizontalBar
        | VerticalBar
        | BothBars

    type Style =
        | Pos of x:Position * y:Position
        | Dim of width:Dimension * height:Dimension
        | TextAlignment of TextAlignment
        | TextColorScheme of ColorScheme
        | Colors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
        // Additional Colors
        | FocusColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
        | HotNormalColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
        | HotFocusedColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
        | DisabledColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
    
    type CommonProp<'a> =
        | Styles of Style list
        | Value of 'a
        | OnChanged of ('a -> unit)
        interface IProp<'a>

    type TextFieldProps =
        | Text of string
        | OnTextChanged of (string -> unit)
        | Secret
        interface IProp<string>

    type WindowProps =
        | Title of string
        interface IProp

    type ScrollBarProps =
        | ScrollContentSize of int * int
        | ScrollOffset of int * int
        | ScrollBar of ScrollBar
        | Frame of (int * int * int * int)
        interface IProp


    type DateTimeProps =
        | IsShort
        interface IProp<DateTime>
        interface IProp<TimeSpan>

    type ListProps =
        | Items of (obj * string) list
        interface IProp

    type ButtonProp =
        | OnClicked of (unit -> unit)
        interface IProp
    



[<AutoOpen>]
module PropsMappings =

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
    
    
        let addTextAlignmentToLabel (label:Label) (alignment:TextAlignment) =
            match alignment with
            | Left -> label.TextAlignment <- Terminal.Gui.TextAlignment.Left
            | Right -> label.TextAlignment <- Terminal.Gui.TextAlignment.Right
            | Centered -> label.TextAlignment <- Terminal.Gui.TextAlignment.Centered
            | Justified -> label.TextAlignment <- Terminal.Gui.TextAlignment.Justified
    
    
        let private addStyleToView (view:View) (style:Style) =
            match style with
            | Pos (x,y) ->
                view.X <- x |> convPos
                view.Y <- y |> convPos
    
            | Dim (width,height) ->
                
                match width, height with
                | AbsDim _, AbsDim _
                | AbsDim _, _
                | _, AbsDim _ -> 
                    view.LayoutStyle <- LayoutStyle.Absolute
                | _, _ ->
                    view.LayoutStyle <- LayoutStyle.Computed
                 

                view.Width <- width |> convDim
                view.Height <- height |> convDim

    
            | TextAlignment alignment ->
                match view with
                | :? Label as label ->                
                    alignment |> addTextAlignmentToLabel label
                | _ -> 
                    ()
    
            | TextColorScheme color ->
                let colorScheme =
                    let s = 
                        if Application.Current<> null then
                            Application.Current.ColorScheme
                        else
                            Terminal.Gui.ColorScheme()
                    match color with
                    | Normal -> s.Normal
                    | Focus -> s.Focus
                    | HotNormal -> s.HotNormal
                    | HotFocus -> s.HotFocus
                match view with
                | :? Label as label ->         
                    if label.ColorScheme = null then
                        label.ColorScheme <- Terminal.Gui.ColorScheme()
                    label.ColorScheme.Normal <- colorScheme
                | _ -> 
                    ()
    
            | Colors (fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                match view with
                | :? Label as label -> 
                    if label.ColorScheme = null then
                        label.ColorScheme <- Terminal.Gui.ColorScheme()
                    label.ColorScheme.Normal <- color
                | _ as view ->
                    if view.ColorScheme = null then
                        view.ColorScheme <- Terminal.Gui.ColorScheme()
                    else
                        view.ColorScheme.Normal <- color
    
            | FocusColors (fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                if view.ColorScheme = null then
                    view.ColorScheme <- Terminal.Gui.ColorScheme()
                else
                    view.ColorScheme.Focus <- color
    
            | HotNormalColors(fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                if view.ColorScheme = null then
                    view.ColorScheme <- Terminal.Gui.ColorScheme()
                else
                    view.ColorScheme.HotNormal <- color
    
            | HotFocusedColors(fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                if view.ColorScheme = null then
                    view.ColorScheme <- Terminal.Gui.ColorScheme()
                else
                    view.ColorScheme.HotFocus <- color
    
            | DisabledColors(fg,bg) ->
                let color = Terminal.Gui.Attribute.Make(fg,bg)
                if view.ColorScheme = null then
                    view.ColorScheme <- Terminal.Gui.ColorScheme()
                else
                    view.ColorScheme.Disabled <- color
    
    
    
        let setStylesToElement (styles: Style list) (element:View) =
            styles |> List.iter (fun s -> addStyleToView element s)


    [<AutoOpen>]
    module Props =

        let inline setCommonObjStyleOnly (prop:CommonProp<'a>) (element:View) =
            match prop with
            | Styles styles ->
                element |> setStylesToElement styles
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
                        element.add_TextChanging(fun ev -> changed(ev.NewText.ToString()))
                    | Value value ->
                        element.Text <- ustring.Make(value) 
                        element.CursorPosition <- value.Length
                        // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                        element.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                        element.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 
                    
                | :? TextFieldProps as prop ->
                    match prop with
                    | Text text ->
                        element.Text <- ustring.Make(text) 
                        element.CursorPosition <- text.Length
                        // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                        element.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                        element.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 
                    | OnTextChanged changed ->
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
                        element.add_DateChanged(fun dateEv -> changed(dateEv.NewValue))
                    
                    | Value value ->
                        element.Date <- value
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
                        //element.add_TimeChanged(fun timeEv -> changed(timeEv.NewValue))
                        element.add_TextChanging(fun t -> printfn "%A" t)
                    | Value value ->
                        element.Time <- value
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
                | _ -> ()
        
            let setPropToFrameViewElement (prop:IProp) (element:FrameView) =
                match prop with
                | _ -> ()
        
            let setPropToHexViewElement (prop:IProp) (element:HexView) =
                match prop with
                | _ -> ()
        
            let setPropToListViewElement (prop:IProp) (element:ListView) =
                match prop with
                | _ -> ()
        
            let setPropToProgressBarElement (prop:IProp) (element:ProgressBar) =
                match prop with
                | _ -> ()
        
            let setPropToCheckBoxElement (prop:IProp) (element:CheckBox) =
                match prop with
                | _ -> ()
        
            let setPropToRadioGroupElement (prop:IProp) (element:RadioGroup) =
                match prop with
                | _ -> ()
        
        
        module Remove =
        
            let removePropToWindowElement (prop:IProp) (element:Window) =
                match prop with 
                | :? CommonProp<_> as prop -> 
                    setCommonObjStyleOnly prop element 
                | :? WindowProps as prop->
                    match prop with
                    | Title title ->
                        element.Title <- ustring.Make(title)
                        
                | _ -> ()
                    
                
            let removePropsToTextElement (prop:IProp) (element:Label) =
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
                
            let removePropToTextFieldElement (prop:IProp) (element:TextField) =
                match prop with
                | :? TextFieldProps as prop ->
                    match prop with
                    | Secret ->
                        element.Secret <- false
                    | _ ->
                        ()
                | _ -> ()
                
            let removePropToButtonElement (prop:IProp) (element:Button) =
                match prop with
                | _ -> ()
                
            let removePropToDateFieldElement (prop:IProp) (element:DateField) =
                match prop with
                | :? DateTimeProps as prop ->
                    match prop with
                    | IsShort -> 
                        element.IsShortFormat <- false
                | _ -> ()
                
            let removePropToTimeFieldElement (prop:IProp) (element:TimeField) =
                match prop with
                | :? DateTimeProps as prop ->
                    match prop with
                    | IsShort -> 
                        element.IsShortFormat <- false
                            
                | _ -> ()
                
            let removePropToTextViewElement (prop:IProp) (element:TextView) =
                match prop with
                | _ -> ()
                
            let removePropToFrameViewElement (prop:IProp) (element:FrameView) =
                match prop with
                | _ -> ()
                
            let removePropToHexViewElement (prop:IProp) (element:HexView) =
                match prop with
                | _ -> ()
                
            let removePropToListViewElement (prop:IProp) (element:ListView) =
                match prop with
                | _ -> ()
                
            let removePropToProgressBarElement (prop:IProp) (element:ProgressBar) =
                match prop with
                | _ -> ()
                
            let removePropToCheckBoxElement (prop:IProp) (element:CheckBox) =
                match prop with
                | _ -> ()
                
            let removePropToRadioGroupElement (prop:IProp) (element:RadioGroup) =
                match prop with
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
            | :? ListView as element ->     props |> List.iter (fun p -> setPropToListViewElement p element)
            | :? ProgressBar as element ->  props |> List.iter (fun p -> setPropToProgressBarElement p element)
            | :? CheckBox as element ->     props |> List.iter (fun p -> setPropToCheckBoxElement p element)
            | :? RadioGroup as element ->   props |> List.iter (fun p -> setPropToRadioGroupElement p element)

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

        



type ViewElement = 
    {
        Type: ViewElementType
        Element: View option
        Props:IProp list
        Children: ViewElement list
    }



module TreeProcessing =

    let processElementObject (parent:View option) processElementObjects (createView:unit -> View)   (currentView:View option) (viewElement:ViewElement) =
        match currentView with
        | None ->
            let newElement = createView()
            newElement |> setPropsToElement viewElement.Props
            parent |> Option.iter (fun p -> p.Add newElement)
            {
                viewElement with
                    Element = Some newElement
                    Children = (viewElement.Children |> List.map (fun c -> processElementObjects (Some (newElement :> View)) c)) 
            }
        | Some el ->
            el |> setPropsToElement viewElement.Props
            { viewElement with 
                Children = (viewElement.Children |> List.map (fun c -> processElementObjects (Some el) c)) 
            }


    let rec processElementObjects (parent:View option) (element:ViewElement) =
        match element with
        | { Type = PageElement; Props = props; Element = elem; Children = children }  ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Toplevel.Create() :> View)
                elem
                element
        

        | { Type = WindowElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Window(NStack.ustring.Make("")) :> View)
                elem
                element
        

        | { Type = TextFieldElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> TextField(NStack.ustring.Make("")) :> View)
                elem
                element
       

        | { Type = ButtonElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Button() :> View)
                elem
                element

        | { Type = LabelElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Label(NStack.ustring.Make("")) :> View)
                elem
                element
        

        | { Type = DateFieldElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> DateField(IsShortFormat = false) :> View)
                elem
                element
        

        | { Type = TimeFieldElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> TimeField(IsShortFormat = false) :> View)
                elem
                element
        
        | { Type = TextViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> TextView() :> View)
                elem
                element
        
        | { Type = FrameViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> FrameView() :> View)
                elem
                element
        
        | { Type = HexViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> HexView() :> View)
                elem
                element
        
        | { Type = ListViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> ListView() :> View)
                elem
                element
        
        | { Type = ProgressBarElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> ProgressBar() :> View)
                elem
                element
        
        | { Type = CheckBoxElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> CheckBox() :> View)
                elem
                element
        
        | { Type = RadioGroupElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> RadioGroup() :> View)
                elem
                element
        
        | { Type = ScrollViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> ScrollView() :> View)
                elem
                element
        
   
            

    let initializeTree (tree:ViewElement) =
        processElementObjects None tree


    let (|OnlyPropsChanged|_|) (ve1,ve2) =
        let cve1 = ve1.Children |> List.map (fun e -> e.Type) |> List.sort
        let cve2 = ve2.Children |> List.map (fun e -> e.Type) |> List.sort
        if cve1 <> cve2 then None else Some ()

    let (|ChildsDifferent|_|) (ve1,ve2) =
        let cve1 = ve1.Children |> List.map (fun e -> e.Type) |> List.sort
        let cve2 = ve2.Children |> List.map (fun e -> e.Type) |> List.sort
        if cve1 <> cve2 then Some () else None

    let processProps (props1:IProp list) (props2:IProp list) (element:View) =
        let prop1Types = props1 |> List.map (fun i -> i.GetType().Name)
        let prop2Types = props2 |> List.map (fun i -> i.GetType().Name)
        let addedPropTypes = prop2Types |> List.except prop1Types
        let deletedPropTypes = prop1Types |> List.except prop2Types
        let modifyedPropTypes = prop1Types |> List.except deletedPropTypes
        let addedProps =
            addedPropTypes
            |> List.map (fun name -> 
                props2 |> List.tryFind (fun p2 -> p2.GetType().Name = name)
            )
            |> List.choose id

        // add new Props
        setPropsToElement addedProps element


        let deletedProps =
            props1
            |> List.filter (fun e -> (deletedPropTypes |> List.contains (e.GetType().Name)))

    
        removePropsToElement deletedProps element

        let remainProps =
            props1
            |> List.filter (fun e -> (deletedPropTypes |> List.contains (e.GetType().Name) |> not))

        let changedRemainProps =
            remainProps
            |> List.map (fun rp ->
                props2 
                |> List.tryFind (fun np -> np.GetType().Name = rp.GetType().Name)
                |> Option.map (fun newProp ->
                    match rp, newProp with
                    | :? CommonProp<string> as rp', (:? CommonProp<string> as newProp') ->
                        match rp', newProp' with
                        | OnChanged _, OnChanged _ ->
                            (false, rp)
                        | Styles s1, Styles s2 ->
                            let changed = s1 <> s2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? CommonProp<DateTime> as rp', (:? CommonProp<DateTime> as newProp') ->
                        match rp', newProp' with
                        | OnChanged _, OnChanged _ ->
                            (false, rp)
                        | Styles s1, Styles s2 ->
                            let changed = s1 <> s2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? CommonProp<TimeSpan> as rp', (:? CommonProp<TimeSpan> as newProp') ->
                        match rp', newProp' with
                        | OnChanged _, OnChanged _ ->
                            (false, rp)
                        | Styles s1, Styles s2 ->
                            let changed = s1 <> s2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? CommonProp<_> as rp', (:? CommonProp<_> as newProp') ->
                        match rp', newProp' with
                        | OnChanged _, OnChanged _ ->
                            (false, rp)
                        | Styles s1, Styles s2 ->
                            let changed = s1 <> s2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? TextFieldProps as rp', (:? TextFieldProps as newProp') ->
                        match rp', newProp' with
                        | OnTextChanged _, OnTextChanged _ ->
                            (false, rp)
                        | _, _ ->
                            (true, newProp)

                    | :? ButtonProp as rp', (:? ButtonProp as newProp') ->
                        match rp', newProp' with
                        | OnClicked _, OnClicked _ ->
                            (false, rp)

                    | _, _ ->
                        (true, newProp)
                )
            )
            |> List.choose id

        let toUpdate =
            changedRemainProps 
            |> List.filter (fun (b,_) -> b)
            |> List.map ( fun (_,e)-> e)

        setPropsToElement toUpdate element

        let newModified =
            changedRemainProps 
            |> List.map ( fun (_,e)-> e)

        newModified @ addedProps

    

  


  
        



    let rec updateTree rootTree newTree =
        match rootTree.Element with
        | None ->
            failwith ("root element must be initialized")
        | Some rootElement ->
            match rootTree, newTree with
            | rt,nt when rt.Type <> nt.Type ->
                initializeTree newTree
            | OnlyPropsChanged ->
                let newProps = processProps rootTree.Props newTree.Props rootElement
                let sortedRootChildren = rootTree.Children |> List.sortBy (fun v -> v.Type)
                let sortedNewChildren = newTree.Children |> List.sortBy (fun v -> v.Type)
                let newChildList = (sortedRootChildren,sortedNewChildren) ||> List.map2 (fun rt nt -> updateTree rt nt)
                { rootTree with Props = newProps; Children = newChildList }
            | ChildsDifferent ->
                let sortedRootChildren = rootTree.Children |> List.sortBy (fun v -> v.Type)
                let sortedNewChildren = newTree.Children |> List.sortBy (fun v -> v.Type)
                let groupedRootType = sortedRootChildren |> List.map (fun v -> v.Type) |> List.distinct
                let groupedNewType = sortedNewChildren |> List.map (fun v -> v.Type) |> List.distinct
                let allTypes = groupedRootType @ groupedNewType |> List.distinct
            
                // find out new added existing types and mix it with existing elements and set the new props
                let newChildList = 
                    allTypes
                    |> List.map (fun et ->
                        let rootElements = sortedRootChildren |> List.filter (fun e -> e.Type = et)
                        let newElements = sortedNewChildren |> List.filter (fun e -> e.Type = et)
                        if (newElements.Length > rootElements.Length) then
                            newElements
                            |> List.mapi (fun idx ne ->
                                if (idx+1 <= rootElements.Length) then
                                    updateTree rootElements.[idx] ne
                                else
                                    let newElem = initializeTree ne
                                    // add elementobj to children
                                    newElem.Element |> Option.iter rootElement.Add 
                                    newElem
                        
                            )
                        else
                            rootElements
                            |> List.mapi (fun idx re ->
                                if (idx+1 <= newElements.Length) then
                                    Some (updateTree re newElements.[idx])
                                else
                                    // the res we remove
                                    re.Element |> Option.map rootElement.Remove |> ignore
                                    None
                            
                            )
                            |> List.choose id
                    )
                    |> List.collect id
            
                // Update Props

                let newProps = processProps rootTree.Props newTree.Props rootElement

                let res = { rootTree with Props = newProps; Children = newChildList }
                res
      
            | _ ->
                failwith "not implemented yet"