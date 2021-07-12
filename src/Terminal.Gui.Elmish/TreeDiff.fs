module TreeDiff

open Terminal.Gui
open NStack

type ViewElementType =
    | PageElement
    | WindowElement
    | TextElement
    | TextBoxElement


type IProp = interface end

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
    
    type CommonProp =
        | Styles of Style list
        | Value of obj
        | OnChanged of (obj -> unit)
        interface IProp

    type TextProps =
        | Text of string
        | OnTextChanged of (string -> unit)
        | Secret
        interface IProp

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
        interface IProp

    type ListProps =
        | Items of (obj * string) list
        interface IProp

    type ButtonProp =
        | OnClicked of (unit -> unit)
        interface IProp
    

type ViewElement = 
    {
        Type: ViewElementType
        Element: View option
        Props:IProp list
        Children: ViewElement list
    }



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
    


let setPropsToElement (props:IProp list) (element:View) =
    // here type specialty
    props
    |> List.iter (fun p ->
        match p with
        | :? CommonProp as p ->
            match p with
            | Styles styles ->
                element |> setStylesToElement styles
            
            | OnChanged changed ->
                match element with
                | :? TextField as tf ->
                    tf.add_TextChanged(fun ustr -> changed(ustr.ToString()))
                | _ ->
                    ()
        | :? TextProps as p ->
            match p with
            | Text text ->
                match element with
                | :? TextField as tf ->
                    tf.Text <- ustring.Make(text) 
                    tf.CursorPosition <- text.Length
                    // weird hack, because after set text and cursor pos the text is shifted left "out" of th box
                    tf.ProcessKey(KeyEvent(Key.Home,KeyModifiers())) |> ignore
                    tf.ProcessKey(KeyEvent(Key.End,KeyModifiers()))  |> ignore 
                    ()
                | :? Label as l ->
                    l.Text <- ustring.Make(text)
                | _ ->
                    ()
            | OnTextChanged changed ->
                match element with
                | :? TextField as tf ->
                    tf.add_TextChanging(fun ev -> changed(ev.NewText.ToString()))
                    
                | _ ->
                    ()
        | :? WindowProps as p ->
            match p with
            | Title title ->
                match element with
                | :? Window as w ->
                    w.Title <- ustring.Make(title)
                | _ ->
                    ()
        | :? ScrollBarProps as p ->
            match p with
            | _ -> ()
        | :? DateTimeProps as p ->
            match p with
            | _ -> ()
        | :? ListProps as p ->
            match p with
            | _ -> ()
        | :? ButtonProp as p ->
            match p with
            | _ -> ()
        | _ ->
            ()
    )


let removePropsToElement (props:IProp list) (element:View) =
    // Maybe todo !
    ()



let rec processElementObjects (parent:View option) (element:ViewElement) =
    match element with
    | { Type = PageElement; Props = props; Element = elem; Children = children }  ->
        match elem with
        | None ->
            let newElement = Toplevel.Create()
            newElement |> setPropsToElement props
            parent |> Option.iter (fun p -> p.Subviews.Add newElement)
            {
                element with
                    Element = Some (newElement :> View)
                    Children = (children |> List.map (fun c -> processElementObjects (Some (newElement :> View)) c)) 
            }
        | Some el ->
            el |> setPropsToElement props
            { element with 
                Children = (children |> List.map (fun c -> processElementObjects (Some el) c)) 
            }

    | { Type = WindowElement; Props = props; Element = elem; Children = children } ->
        match elem with
        | None ->
            let newElement = Window(NStack.ustring.Make(""))
            newElement |> setPropsToElement props
            parent |> Option.iter (fun p -> p.Add(newElement))
            {
                element with
                    Element = Some (newElement :> View)
                    Children = (children |> List.map (fun c -> processElementObjects (Some (newElement :> View)) c)) 
            }
        | Some el ->
            el |> setPropsToElement props
            { element with 
                Children = (children |> List.map (fun c -> processElementObjects (Some el) c)) 
            }

    | { Type = TextBoxElement; Props = props; Element = elem; Children = children } ->
        match elem with
        | None ->
            let newElement = TextField(NStack.ustring.Make(""))
            newElement |> setPropsToElement props
            parent |> Option.iter (fun p -> p.Add newElement)
            {
                element with
                    Element = Some (newElement :> View)
                    Children = (children |> List.map (fun c -> processElementObjects (Some (newElement :> View)) c)) 
            }
        | Some el ->
            el |> setPropsToElement props
            { element with 
                Children = (children |> List.map (fun c -> processElementObjects (Some el) c)) 
            }

    | { Type = TextElement; Props = props; Element = elem; Children = children } ->
        match elem with
        | None ->
            let newElement = Label(NStack.ustring.Make(""))
            newElement |> setPropsToElement props
            parent |> Option.iter (fun p -> p.Add newElement)
            {
                element with
                    Element = Some (newElement :> View)
                    Children = (children |> List.map (fun c -> processElementObjects (Some (newElement :> View)) c)) 
            }
        | Some el ->
            el |> setPropsToElement props
            { element with 
                Children = (children |> List.map (fun c -> processElementObjects (Some el) c)) 
            }
            

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
    let prop2Types = props1 |> List.map (fun i -> i.GetType().Name)
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
                | :? CommonProp as rp', (:? CommonProp as newProp') ->
                    match rp', newProp' with
                    | OnChanged _, OnChanged _ ->
                        (false, rp)
                    | _, _ ->
                        (true, newProp)
                | :? TextProps as rp', (:? TextProps as newProp') ->
                    match rp', newProp' with
                    | OnTextChanged _, OnTextChanged _ ->
                        (false, rp)
                    | _, _ ->
                        (true, newProp)
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