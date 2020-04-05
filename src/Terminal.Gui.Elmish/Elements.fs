namespace Terminal.Gui.Elmish

open System.Reflection
open System.Collections
open System
open Terminal.Gui




[<AutoOpen>]
module StyleHelpers =
    
    open Terminal.Gui

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



    type Prop<'TValue> =
        | Styles of Style list
        | Value of ^TValue
        | Text of string
        | Title of string
        | OnChanged of ('TValue -> unit)
        | OnClicked of (unit -> unit)
        | Items of ('TValue * string) list
        | Secret
        // Scrollbar Stuff
        | ScrollContentSize of int * int
        | ScrollOffset of int * int
        | ScrollBar of ScrollBar
        | Frame of (int * int * int * int)






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

    

    let addStyleToView (view:View) (style:Style) =
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
                label.TextColor <- colorScheme
            | _ -> 
                ()
        | Colors (fg,bg) ->
            let color = Terminal.Gui.Attribute.Make(fg,bg)
            match view with
            | :? Label as label ->                
                label.TextColor <- color
            | _ -> 
                ()
            
            
    
    let addStyles (styles:Style list) (view:View)=
        styles
        |> List.iter (fun si ->
            si |> addStyleToView view                    
        )

    let tryGetStylesFromProps (props:Prop<'TValue> list) =
        props
        |> List.tryFind (fun i -> match i with | Styles _ -> true | _ -> false)

    let inline addPossibleStylesFromProps (props:Prop<'TValue> list) (view:'T when 'T :> View) =
        let styles = tryGetStylesFromProps props
        match styles with
        | None ->
            view
        | Some (Styles styles) ->
            view |> addStyles styles
            view
        | Some _ ->
            view

    
    let getTitleFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | Title _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | Title t -> t | _ -> "")
        |> Option.defaultValue ""

    let getTextFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | Text _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | Text t -> t | _ -> "")
        |> Option.defaultValue ""

    let tryGetValueFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | Value _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | Value t -> t | _ -> failwith "What?No!Never should this happen!")

    let tryGetFrameFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | Frame _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | Frame t -> t | _ -> failwith "What?No!Never should this happen!")

    let tryGetOnChangedFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | OnChanged _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | OnChanged t -> t | _ -> failwith "What?No!Never should this happen!")

    let tryGetOnClickedFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | OnClicked _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | OnClicked t -> t | _ -> failwith "What?No!Never should this happen!")

    let getItemsFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | Items _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | Items t -> t | _ -> failwith "What?No!Never should this happen!")
        |> Option.defaultValue []

    let getScrollBarFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | ScrollBar _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | ScrollBar t -> t | _ -> failwith "What?No!Never should this happen!")
        |> Option.defaultValue NoBars

    let getScrollContentSizeFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | ScrollContentSize _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | ScrollContentSize (w,h) -> (w,h) | _ -> failwith "What?No!Never should this happen!")

    let getScrollOffsetFromProps (props:Prop<'TValue> list) = 
        props
        |> List.tryFind (fun i -> match i with | ScrollOffset _ -> true | _ -> false)
        |> Option.map (fun i -> match i with | ScrollOffset (w,h) -> (w,h) | _ -> failwith "What?No!Never should this happen!")
        

    let hasSecretInProps (props:Prop<'TValue> list) = 
        props
        |> List.exists (fun i -> match i with | Secret _ -> true | _ -> false)
        
        
       

    



[<AutoOpen>]
module Elements =

    

    module Args =

        type MenuItemArg = {
            Title:string; 
            Help:string; 
            OnClicked: (unit -> unit) 
        }

        type MenuBarItemArg = { 
            Children: MenuItemArg list; 
            Text: string;  
        }



    [<CustomEquality; NoComparison>]
    type Control<'TValue> = 
        | Nothing
        | Page
        | Window        of Prop<'TValue> list
        | Button        of Prop<'TValue> list
        | Label         of Prop<'TValue> list
        | TextField     of Prop<'TValue> list
        | TextView      of Prop<'TValue> list
        | FrameView     of Prop<'TValue> list
        | ScrollView    of Prop<'TValue> list
        | CheckBox      of Prop<'TValue> list
        | RadioGroup    of Prop<'TValue> list
        | ProgressBar   of Prop<'TValue> list
        | HexView       of (Prop<'TValue> list * IO.Stream)
        | ListView      of Prop<'TValue> list

        override x.Equals(yobj) =
            match yobj with
            | :? Control<'TValue> as y -> true
            | _ -> false

        override x.GetHashCode() = 0


    type Change<'T> =
        | Unchanged
        | Inserted
        | Deleted
        | Updated of 'T


    
    type Node<'T> = 
        {
            Id : string
            Value : 'T
            Ref: View ref 
            Modified : Change<'T>
            Children : Node<'T> list
            Parent: Node<'T> option
        }

    let EmptyRootPage<'TValue> = 
        {
            Id = "Root"
            Value = Nothing
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }
       
    
    type ControlNode<'TValue> = Node<Control<'TValue>>

    module Tree =
        

        /// Nodes are equal in all but Value
        let (==) node1 node2 =
            node1.Id = node2.Id 
            && node1.Children = node2.Children 
            && node1.Value <> node2.Value

        /// Combines the children of two nodes into a single list
        let combine node1 node2 =
            let first = node1.Children |> List.map (fun x -> (Some x, node2.Children |> List.tryFind (fun y -> y.Id = x.Id)))
            let second = node2.Children |> List.map (fun x -> (node1.Children |> List.tryFind (fun y -> y.Id = x.Id)), Some x)
            first @ second |> List.distinct

        let rec markNode change node =
            { node with
                Modified = change
                Children = node.Children |> List.map (markNode change) }

        let insert node = markNode Inserted node

        let delete node = markNode Deleted node

        let update node value =
            { node with 
                Modified = Updated value }

        let rec diff (oldNodeOpt, newNodeOpt)  =
            match oldNodeOpt, newNodeOpt with
            | Some oldNode, None -> delete oldNode
            | None, Some newNode -> insert newNode
            | Some oldNode, Some newNode when oldNode = newNode -> oldNode
            | Some oldNode, Some newNode when oldNode == newNode -> update oldNode newNode.Value
            | Some oldNode, Some newNode ->
                { oldNode with
                    Modified = if oldNode.Value = newNode.Value then Unchanged else Updated newNode.Value
                    Children =
                        combine oldNode newNode 
                        |> List.map diff
                        |> List.map (fun x -> {x with Parent = Some oldNode })
                }
            | None, None -> failwith "Must have at least one node to compare."

        /// Compares two trees and returns a tree with the differences labeled
        let compare oldTree newTree = diff (Some oldTree, Some newTree)
                       
    open Terminal.Gui
    open NStack
    
    
    module ViewElements =

        let ustr (x:string) = ustring.Make(x)

        let rec createView (node:ControlNode<'TValue>) : View =
            match node.Value with
            | Nothing ->
                failwith ("the empty tree is not for creating view elements")
            | Page ->
                let top = Toplevel.Create()        
                node.Children |> List.iter (fun v -> top.Add(createView v))
                top :> View
            | Window props ->
                let title = getTitleFromProps props
                let window = Window(title |> ustr)
                node.Children |> List.iter (fun v -> window.Add(createView v))        
                window
                |> addPossibleStylesFromProps props
                :> View
            | Button props ->
                let text = getTextFromProps props
                let b = Button(text |> ustr)
                let clicked = tryGetOnClickedFromProps props
                match clicked with
                | Some clicked ->
                    b.Clicked <- Action((fun () -> clicked() ))
                | None ->
                    ()
                b
                |> addPossibleStylesFromProps props
                :> View
            | Label  props ->
                let text = getTextFromProps props
                let l = Label(text |> ustr)
                l
                |> addPossibleStylesFromProps props
                :> View
            | TextField props ->
                let value = 
                    tryGetValueFromProps props
                
                let t = 
                    match value with
                    | None ->
                        TextField("",Used = true)
                    | Some value ->
                        TextField(value |> unbox<string> |> ustr,Used = true)
            
            

                let changed = tryGetOnChangedFromProps props
                match changed with
                | Some changed ->
                    t.Changed.AddHandler(fun o _ -> changed (((o:?>TextField).Text).ToString() |> unbox))        
                | None -> ()

                let secret = hasSecretInProps props
                t.Secret <- secret

                t
                |> addPossibleStylesFromProps props
                :> View
            | TextView props ->
                let text = getTextFromProps props
                let t = TextView()
                t.Text <- (text|> ustr)
                t
                |> addPossibleStylesFromProps props
                :> View
            | FrameView props ->
                let text = getTextFromProps props
                let f = FrameView(text |> ustr)
                node.Children |> List.iter (fun v -> f.Add(createView v))
                f
                |> addPossibleStylesFromProps props
                :> View
            | ScrollView props ->
                let frame = tryGetFrameFromProps props
                match frame with
                | None ->
                    failwith "Scrollview need a Frame Prop"
                | Some (x,y,w,h) ->
                    let sv = ScrollView(Rect(x,y,w,h))
                    // Scroll bars
                    let bars = getScrollBarFromProps props
                    match bars with
                    | NoBars ->
                        ()
                    | HorizontalBar ->
                        sv.ShowHorizontalScrollIndicator <- true
                    | VerticalBar ->
                        sv.ShowVerticalScrollIndicator <- true
                    | BothBars ->
                        sv.ShowHorizontalScrollIndicator <- true
                        sv.ShowVerticalScrollIndicator <- true

                    // ContentSize
                    let contentSize = getScrollContentSizeFromProps props
                    match contentSize with
                    | None -> ()
                    | Some (w,h) ->
                        sv.ContentSize <- Size(w,h)

                    // Offset
                    let offset = getScrollOffsetFromProps props
                    match offset with
                    | None -> ()
                    | Some (x,y) ->
                        sv.ContentOffset <- Point(x,y)

                

                    node.Children |> List.iter (fun i -> sv.Add(createView i))
                    sv
                    |> addPossibleStylesFromProps props
                :> View
            | CheckBox props ->
                let isChecked = 
                    tryGetValueFromProps props
                
                let isChecked =
                    match isChecked with
                    | None -> false
                    | Some x -> x |> unbox<bool>

                let text = getTextFromProps props
                let cb = CheckBox(text |> ustr,isChecked)
                let onChanged = tryGetOnChangedFromProps props
                match onChanged with
                | Some onChanged ->
                    cb.Toggled.AddHandler((fun o e -> (o:?>CheckBox).Checked |> unbox |> onChanged))
                | None ->
                    ()
                cb
                |> addPossibleStylesFromProps props
                :> View
            | RadioGroup props ->
                let items = getItemsFromProps props        
                let value = tryGetValueFromProps props
                let displayValues = items |> List.map (snd) |> List.toArray
                let idxItem = 
                    value
                    |> Option.bind (fun value ->
                        items |> List.tryFindIndex (fun (v,_) -> v = value)
                    )

                let addSelectedChanged (rg:RadioGroup) =
                    let onChange =
                        tryGetOnChangedFromProps props
                    match onChange with
                    | Some onChange ->
                        let action = Action<int>((fun idx -> 
                            let (value,disp) = items.[idx]
                            onChange (value)
                        ))
                        rg.SelectionChanged <- action
                        rg
                    | None ->
                        rg

                let setCursorRadioGroup (x:int) (rg:RadioGroup) =
                    rg.Cursor <- x
                    rg

                match idxItem with
                | None ->
                    RadioGroup(displayValues)
                    |> addSelectedChanged
                    |> addPossibleStylesFromProps props
                | Some idx ->
                    RadioGroup(displayValues,idx)
                    |> setCursorRadioGroup idx
                    |> addSelectedChanged
                    |> addPossibleStylesFromProps props
                :> View
            | ProgressBar props ->
                let value = 
                    tryGetValueFromProps props
                
                let value =
                    match value with
                    | Some x -> x |> unbox<float>
                    | None -> 0.0

                let pb = ProgressBar(Fraction = (value |> float32))        
                pb
                |> addPossibleStylesFromProps props
                :> View
            | HexView (props,stream) ->
                HexView(stream)
                |> addPossibleStylesFromProps props
                :> View
            | ListView props ->
                let items = getItemsFromProps props
                let displayValues = items |> List.map (snd) |> List.toArray :> IList
                let value = tryGetValueFromProps props
                let selectedIdx = 
                    value
                    |> Option.bind (fun value ->
                        items |> List.tryFindIndex (fun (v,_) -> v = value) 
                    )
                
                let lv = 
                    ListView(displayValues)
                    |> addPossibleStylesFromProps props
                let addSelectedChanged (lv:ListView) =
                    let onChange =
                        tryGetOnChangedFromProps props
                    match onChange with
                    | Some onChange ->
                        let action = Action((fun () -> 
                            let (value,disp) = items.[lv.SelectedItem]
                            onChange (value)
                        ))
                        lv.add_SelectedChanged(action)
                        lv
                    | None ->
                        lv
            
                match selectedIdx with
                | None ->
                    lv
                    |> addSelectedChanged
                    |> addPossibleStylesFromProps props
                | Some idx ->
                    lv.SelectedItem <- idx
                    lv
                    |> addSelectedChanged
                    |> addPossibleStylesFromProps props
                :> View


        let rec inline updateView (view:View) (node:ControlNode<'TValue>) : View =
            match node.Value with
            | Nothing ->
                failwith ("the empty tree is not for updating view elements")
            | Page ->
                view
            | Window props ->
                let title = getTitleFromProps props
                let window = view :?> Window
                window.Title <- title |> ustr
                window
                |> addPossibleStylesFromProps props
                :> View
            | Button props ->
                let text = getTextFromProps props
                let b =  view :?> Button
                b.Text <- text |> ustr
                let clicked = tryGetOnClickedFromProps props
                match clicked with
                | Some clicked ->
                    b.Clicked <- Action((fun () -> clicked() ))
                | None ->
                    ()
                b
                |> addPossibleStylesFromProps props
                :> View
            | Label  props ->
                let text = getTextFromProps props
                let l =  view :?> Label
                l.Text <- text |> ustr
                l
                |> addPossibleStylesFromProps props
                :> View
            | TextField props ->
                let value = 
                    tryGetValueFromProps props
                
                let t = view :?> TextField
                match value with
                | None ->
                    t.Text <- "" |> ustr
                | Some value ->
                    t.Text <- value |> unbox<string> |> ustr
            

                let changed = tryGetOnChangedFromProps props
                match changed with
                | Some changed ->
                    t.Changed.AddHandler(fun o _ -> changed (((o:?>TextField).Text).ToString() |> unbox))        
                | None -> ()

                let secret = hasSecretInProps props
                t.Secret <- secret

                t
                |> addPossibleStylesFromProps props
                :> View
            | TextView props ->
                let text = getTextFromProps props
                let t =  view :?> TextView
                t.Text <- (text |> ustr)
                t
                |> addPossibleStylesFromProps props
                :> View
            | FrameView props ->
                let title = getTitleFromProps props
                let f =  view :?> FrameView
                f.Title <- (title |> ustr)
                f
                |> addPossibleStylesFromProps props
                :> View
            | ScrollView props ->
                let frame = tryGetFrameFromProps props
                match frame with
                | None ->
                    failwith "Scrollview need a Frame Prop"
                | Some (x,y,w,h) ->
                    let sv =  view :?> ScrollView
                    sv.Frame <- Rect(x,y,w,h)
                    // Scroll bars
                    let bars = getScrollBarFromProps props
                    match bars with
                    | NoBars ->
                        ()
                    | HorizontalBar ->
                        sv.ShowHorizontalScrollIndicator <- true
                    | VerticalBar ->
                        sv.ShowVerticalScrollIndicator <- true
                    | BothBars ->
                        sv.ShowHorizontalScrollIndicator <- true
                        sv.ShowVerticalScrollIndicator <- true

                    // ContentSize
                    let contentSize = getScrollContentSizeFromProps props
                    match contentSize with
                    | None -> ()
                    | Some (w,h) ->
                        sv.ContentSize <- Size(w,h)

                    // Offset
                    let offset = getScrollOffsetFromProps props
                    match offset with
                    | None -> ()
                    | Some (x,y) ->
                        sv.ContentOffset <- Point(x,y)

                    sv
                    |> addPossibleStylesFromProps props
                :> View
            | CheckBox props ->
                let isChecked = 
                    tryGetValueFromProps props
                
                let isChecked =
                    match isChecked with
                    | None -> false
                    | Some x -> x |> unbox<bool>

                let text = getTextFromProps props
                let cb =  view :?> CheckBox
                cb.Text <- text |> ustr
                cb.Checked <- isChecked
                let onChanged = tryGetOnChangedFromProps props
                match onChanged with
                | Some onChanged ->
                    cb.Toggled.AddHandler((fun o e -> (o:?>CheckBox).Checked |> unbox |> onChanged))
                | None ->
                    ()
                cb
                |> addPossibleStylesFromProps props
                :> View
            | RadioGroup props ->
                let items = getItemsFromProps props        
                let value = tryGetValueFromProps props
                let displayValues = items |> List.map (snd) |> List.toArray
                let idxItem = 
                    value
                    |> Option.bind (fun value ->
                        items |> List.tryFindIndex (fun (v,_) -> v = value)
                    )

                let addSelectedChanged (rg:RadioGroup) =
                    let onChange =
                        tryGetOnChangedFromProps props
                    match onChange with
                    | Some onChange ->
                        let action = Action<int>((fun idx -> 
                            let (value,disp) = items.[idx]
                            onChange (value)
                        ))
                        rg.SelectionChanged <- action
                        rg
                    | None ->
                        rg

                let rb = view :?> RadioGroup
                rb.Selected <- idxItem |> Option.defaultValue -1
                rb
                |> addSelectedChanged
                |> addPossibleStylesFromProps props
                :> View
            | ProgressBar props ->
                let value = 
                    tryGetValueFromProps props
                
                let value =
                    match value with
                    | Some x -> x |> unbox<float>
                    | None -> 0.0

                let pb = view :?> ProgressBar       
                pb.Fraction <- (value |> float32)
                pb
                |> addPossibleStylesFromProps props
                :> View
            | HexView (props,stream) ->
                let hf = view :?> HexView
                hf.Source <- stream
                hf
                |> addPossibleStylesFromProps props
                :> View
            | ListView props ->
                let items = getItemsFromProps props
                let displayValues = items |> List.map (snd) |> List.toArray :> IList
                let value = tryGetValueFromProps props
                let selectedIdx = 
                    value
                    |> Option.bind (fun value ->
                        items |> List.tryFindIndex (fun (v,_) -> v = value) 
                    )
                
                let lv = view :?> ListView

                lv.SetSource(displayValues)

                lv.SelectedItem <- selectedIdx |> Option.defaultValue -1

                let addSelectedChanged (lv:ListView) =
                    let onChange =
                        tryGetOnChangedFromProps props
                    match onChange with
                    | Some onChange ->
                        let action = Action((fun () -> 
                            let (value,disp) = items.[lv.SelectedItem]
                            onChange (value)
                        ))
                        lv.add_SelectedChanged(action)
                        lv
                    | None ->
                        lv
            
            
                lv
                |> addSelectedChanged
                |> addPossibleStylesFromProps props
                :> View



    let processNode node =
        match node.Modified with
        | Unchanged ->
            node
        | Inserted ->
            node.Ref :=  ViewElements.createView node
            match node.Parent with
            | None ->
                ()
            | Some parent ->
                (!parent.Ref).Subviews.Add(!node.Ref)
            node
        | Deleted ->
            match node.Parent with
            | None ->
                ()
            | Some parent ->
                (!parent.Ref).Subviews.Remove(!node.Ref) |> ignore
            node
        | Updated newData ->
            let view = !node.Ref
            let _ = ViewElements.updateView view node
            node


    let updateTree oldTree newTree =
        let treeDiff = Tree.compare oldTree newTree
        let rec traverse (node:ControlNode<'a>) =
            processNode node |> ignore
            treeDiff.Children 
            |> List.iter (fun c -> traverse c)
        traverse treeDiff
            
        






    let inline page (subViews:ControlNode<'TValue> list) : ControlNode<'TValue> =
        {
            Id = "Root"
            Value = Page
            Ref = ref null
            Modified = Unchanged
            Children = subViews
            Parent = None
        }
       

    let inline window (props:Prop<'TValue> list) (subViews:ControlNode<'TValue> list) : ControlNode<'TValue> =        
        {
            Id = "Window"
            Value = Control.Window props
            Ref = ref null
            Modified = Unchanged
            Children = subViews
            Parent = None
        }


    let inline button (props:Prop<'TValue> list) = 
        {
            Id = "Button"
            Value = Control.Button props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }

    let inline label (props:Prop<'TValue> list)  : ControlNode<'TValue> =   
        {
            Id = "Label"
            Value = Control.Label props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }

    let inline textField (props:Prop<string> list)  : Node<Control<string>> =        
        {
            Id = "TextField"
            Value = Control.TextField props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }

    let inline textView (props:Prop<'TValue> list)  : ControlNode<'TValue> =
        {
            Id = "TextView"
            Value = Control.TextView props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }

   

    let inline frameView (props:Prop<'TValue> list) (subViews:ControlNode<'TValue> list) : ControlNode<'TValue> =
        {
            Id = "Window"
            Value = Control.FrameView props
            Ref = ref null
            Modified = Unchanged
            Children = subViews
            Parent = None
        }

    let inline hexView (props:Prop<'TValue> list) stream  : ControlNode<'TValue> =
        {
            Id = "HexView"
            Value = Control.HexView stream
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }

    let inline listView (props:Prop<'TValue> list)  : ControlNode<'TValue> = 
        {
            Id = "ListView"
            Value = Control.ListView props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }


    open ViewElements


    let menuItem title help action = 
        MenuItem(title |> ustr,help ,(fun () -> action () ))

    let menuBarItem text (items:MenuItem list) = 
        MenuBarItem(text |> ustr,items |> List.toArray)

    let menuBar (items:MenuBarItem list) = 
        MenuBar (items |> List.toArray)


    let inline progressBar (props:Prop<float> list)  : Node<Control<float>> = 
        {
            Id = "ProgressBar"
            Value = Control.ProgressBar props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }


    let inline checkBox (props:Prop<bool> list) = 
        {
            Id = "CheckBox"
            Value = Control.CheckBox props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }


    let inline radioGroup (props:Prop<'TValue> list) =
        {
            Id = "RadioGroup"
            Value = Control.RadioGroup props
            Ref = ref null
            Modified = Unchanged
            Children = []
            Parent = None
        }

    
    let inline scrollView (props:Prop<'TValue> list) (subViews:ControlNode<'TValue> list) : ControlNode<'TValue> =
        {
            Id = "ScrollView"
            Value = Control.ScrollView props
            Ref = ref null
            Modified = Unchanged
            Children = subViews
            Parent = None
        }




    open ViewElements

    let fileDialog title prompt nameFieldLabel message =
        let dia = FileDialog(title |> ustr,prompt |> ustr,nameFieldLabel |> ustr,message |> ustr)
        Application.Run(dia)
        let file = 
            dia.FilePath
            |> Option.ofObj 
            |> Option.map string
            |> Option.bind (fun s ->
                if String.IsNullOrEmpty(s) then None 
                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
            )
        file

    let openDialog title message =
        let dia = OpenDialog(title |> ustr,message |> ustr)                
        Application.Run(dia)
        let file = 
            dia.FilePath
            |> Option.ofObj 
            |> Option.map string
            |> Option.bind (fun s ->
                if String.IsNullOrEmpty(s) then None 
                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
            )
        file
        
            
        
        
        

    let saveDialog title message =
        let dia = SaveDialog(title |> ustr,message |> ustr)        
        Application.Run(dia)
        let file = 
            dia.FileName
            |> Option.ofObj 
            |> Option.map string
            |> Option.bind (fun s ->
                if String.IsNullOrEmpty(s) then None 
                else Some (System.IO.Path.Combine((dia.DirectoryPath |> string),s))
            )
        file


    let messageBox width height title text (buttons:string list) =
        let result = MessageBox.Query(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ -> buttons.[result]

    let errorBox width height title text (buttons:string list) =
        let result = MessageBox.ErrorQuery(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ -> buttons.[result]
            

    


    

    