namespace Terminal.Gui.Elmish

open System

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
        | Value of 'a
        | OnChanged of ('a -> unit)
        interface IProp<'a>


    open Terminal.Gui

    type ViewProp<'a> =
        | Styles of Style list
        | OnEntered of (View.FocusEventArgs -> unit)
        | OnLeft of  (View.FocusEventArgs -> unit)
        | OnMouseEntered of (View.MouseEventArgs -> unit)
        | OnMouseLeft of (View.MouseEventArgs -> unit)
        | OnMouseClicked of (View.MouseEventArgs -> unit)
        | OnCanFocusChanged of (unit -> unit)
        | OnEnabledChanged of (unit -> unit)
        | OnVisibleChanged of (unit -> unit)
        | HotKey of Key
        | ShortCut of Key
        | TabIndex  of int
        | TabStop of Boolean
        | Text of string

        interface IProp<'a>
        


    type TextFieldProps =
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

    type ListProps<'a> =
        | Items of ('a * string) list
        interface IProp<'a>

    type ButtonProp =
        | OnClicked of (unit -> unit)
        interface IProp
    

    open Microsoft.FSharp.Reflection

    let GetUnionCaseName (x:'a) = 
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name

    let inline toObjProp<'a> (input:IProp<'a>) : IProp<obj> =
        match input with
        | :? ViewProp<'a> as viewProp ->
            match viewProp with
            | Styles styles ->
                Styles styles :> IProp<obj>
            | OnEntered func            -> 
                OnEntered func :> IProp<obj>
            | OnLeft func               -> 
                OnLeft func :> IProp<obj>
            | OnMouseEntered func       -> 
                OnMouseEntered func :> IProp<obj>
            | OnMouseLeft func          -> 
                OnMouseLeft func  :> IProp<obj>
            | OnMouseClicked func       -> 
                OnMouseClicked func :> IProp<obj>
            | OnCanFocusChanged func    -> 
                OnCanFocusChanged func :> IProp<obj>
            | OnEnabledChanged func     -> 
                OnEnabledChanged func :> IProp<obj>
            | OnVisibleChanged func     -> 
                OnVisibleChanged func  :> IProp<obj>
            | HotKey key                -> 
                HotKey key  :> IProp<obj>
            | ShortCut key              -> 
                ShortCut key  :> IProp<obj>
            | TabIndex idx              -> 
                TabIndex idx :> IProp<obj>
            | TabStop tabStop           -> 
                TabStop tabStop  :> IProp<obj>
            | Text text ->
                Text text :> IProp<obj>

        | :? ListProps<'a> as listProp ->
            match listProp with
            | Items items ->
                let objList = (items |> List.map (fun (item,name) -> (box item, name)))
                Items objList :> IProp<obj>

        | :? CommonProp<'a> as commonProp ->
            match commonProp with
            | Value a ->
                Value (box a) :> IProp<obj>
            | OnChanged func ->
                let objF (obj:obj) = func((obj :?> 'a))
                (OnChanged objF) :> IProp<obj>
                
        | _ -> failwith ("can not convert prop top obj prop")

