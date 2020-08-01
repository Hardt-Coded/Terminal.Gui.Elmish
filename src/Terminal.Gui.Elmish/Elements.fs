namespace Terminal.Gui.Elmish

open System.Reflection
open System.Collections
open System




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
        | Value of 'TValue
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
        // Date And Time Field Stuff
        | IsShort

        

        

    
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

    let getAbsPosFromProps (props:Prop<'TValue> list) =
        tryGetStylesFromProps props
        |> Option.bind (
            function
            | Styles styles ->
                styles
                |> List.choose (fun i -> 
                    match i with 
                    | Pos (AbsPos x, AbsPos y) -> Some (x,y) 
                    | Pos (_, AbsPos y) -> Some (0,y) 
                    | Pos (AbsPos x, _) -> Some (x,0) 
                    | _ -> None)
                |> List.tryHead
            | _ ->
                Some (0,0)
        )
        |> Option.defaultValue (0,0)
        
    let hasShortProp (props:Prop<'TValue> list) =
        props
        |> List.exists (fun i -> match i with | IsShort _ -> true | _ -> false)
        

    let hasSecretInProps (props:Prop<'TValue> list) = 
        props
        |> List.exists (fun i -> match i with | Secret _ -> true | _ -> false)
        
        
       

    



[<AutoOpen>]
module Elements =

    open Terminal.Gui
    open NStack

    let ustr (x:string) = ustring.Make(x)


    let page (subViews:View list) =
        let top = Toplevel.Create()        
        subViews |> List.iter (fun v -> top.Add(v))
        //let state = Application.Begin(top)  
        //state
        top
       


    let window (props:Prop<'TValue> list) (subViews:View list) =        
        let title = getTitleFromProps props
        let window = Window(title |> ustr)
        subViews |> List.iter (fun v -> window.Add(v))        
        window
        |> addPossibleStylesFromProps props


    let button (props:Prop<'TValue> list) = 
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


    let label (props:Prop<'TValue> list) =   
        let text = getTextFromProps props
        let l = Label(text |> ustr)
        l
        |> addPossibleStylesFromProps props


    let textField (props:Prop<string> list) =        
        let value = 
            tryGetValueFromProps props
            |> Option.defaultValue ""
        
        let t = TextField(value |> ustr,Used = true)

        let changed = tryGetOnChangedFromProps props
        match changed with
        | Some changed ->
            t.Changed.AddHandler(fun o _ -> changed (((o:?>TextField).Text).ToString()))        
        | None -> ()

        let secret = hasSecretInProps props
        t.Secret <- secret

        t
        |> addPossibleStylesFromProps props


    /// DateField:
    /// Only AbsPos for Positioning 
    /// Exclusive 'isShort' Prop
    /// Currently in Version 0.81 of the Terminal.Gui this use a DateTime
    /// From the time beeing, the master branch uses TimeSpan
    let timeField (props:Prop<TimeSpan> list) =
        let value = 
            tryGetValueFromProps props
            |> Option.defaultValue TimeSpan.Zero

        let (x,y) =
            getAbsPosFromProps props

        let isShort =
            hasShortProp props
        

        let t = TimeField(x,y,System.DateTime.MinValue.Add(value),isShort)

        let changed = tryGetOnChangedFromProps props
        match changed with
        | Some changed ->
            let dtToTs (dt:DateTime) =
                TimeSpan(dt.Hour,dt.Minute,dt.Second)
            t.Changed.AddHandler(fun o _ -> changed ((o:?>TimeField).Time |> dtToTs))        
        | None -> ()
        
        t
        |> addPossibleStylesFromProps props


    /// DateField:
    /// Only AbsPos for Positioning 
    /// Exclusive 'isShort' Prop
    let dateField (props:Prop<DateTime> list) =
        let value = 
            tryGetValueFromProps props
            |> Option.defaultValue DateTime.MinValue

        let (x,y) =
            getAbsPosFromProps props

        let isShort =
            hasShortProp props
        
        let t = DateField(x,y,value,isShort)

        let changed = tryGetOnChangedFromProps props
        match changed with
        | Some changed ->
            t.Changed.AddHandler(fun o _ -> changed ((o:?>DateField).Date))        
        | None -> ()
        
        t
        |> addPossibleStylesFromProps props


    let textView (props:Prop<'TValue> list) =
        let text = getTextFromProps props
        let t = TextView()
        t.Text <- (text|> ustr)
        t
        |> addPossibleStylesFromProps props
   

    let frameView (props:Prop<'TValue> list) (subViews:View list) =
        let text = getTextFromProps props
        let f = FrameView(text |> ustr)
        subViews |> List.iter (fun v -> f.Add(v))
        f
        |> addPossibleStylesFromProps props


    let hexView (props:Prop<'TValue> list) stream =
        HexView(stream)
        |> addPossibleStylesFromProps props


    let inline listView (props:Prop<'TValue> list) = 
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
            

    let menuItem title help action = 
        MenuItem(title |> ustr,help ,(fun () -> action () ))


    let menuBarItem text (items:MenuItem list) = 
        MenuBarItem(text |> ustr,items |> List.toArray)


    let menuBar (items:MenuBarItem list) = 
        MenuBar (items |> List.toArray)


    let progressBar (props:Prop<float> list) = 
        let value = 
            tryGetValueFromProps props
            |> Option.defaultValue 0.0

        let pb = ProgressBar(Fraction = (value |> float32))        
        pb
        |> addPossibleStylesFromProps props


    let checkBox (props:Prop<bool> list) = 
        let isChecked = 
            tryGetValueFromProps props
            |> Option.defaultValue false

        let text = getTextFromProps props
        let cb = CheckBox(text |> ustr,isChecked)
        let onChanged = tryGetOnChangedFromProps props
        match onChanged with
        | Some onChanged ->
            cb.Toggled.AddHandler((fun o e -> (o:?>CheckBox).Checked |> onChanged))
        | None ->
            ()
        cb
        |> addPossibleStylesFromProps props
    

    let setCursorRadioGroup (x:int) (rg:RadioGroup) =
        rg.Cursor <- x
        rg


    let inline radioGroup (props:Prop<'TValue> list) =
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
            
    
    let scrollView (props:Prop<'TValue> list) (subViews:View list) =
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

            

            subViews |> List.iter (fun i -> sv.Add(i))
            sv
            |> addPossibleStylesFromProps props


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


    let statusBar (items:StatusItem list) =
        StatusBar(items |> List.toArray)


    let statusItem text (key:Terminal.Gui.Key) action =
        if key = Key.F9 then
            invalidArg "key" ("F9 is reserved to open a menu, sorry.")
        else
            StatusItem(key,text |> ustr, Action(action))

    


    

    