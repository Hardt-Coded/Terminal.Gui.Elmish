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
        | :? ListProps<'a> as listProp ->
            match listProp with
            | Items items ->
                let objList = (items |> List.map (fun (item,name) -> (box item, name)))
                Items objList :> IProp<obj>
        | :? CommonProp<'a> as commonProp ->
            match commonProp with
            | Styles styles ->
                Styles styles :> IProp<obj>
            | Value a ->
                Value (box a) :> IProp<obj>
            | OnChanged func ->
                let objF (obj:obj) = func((obj :?> 'a))
                (OnChanged objF) :> IProp<obj>
                
        | _ -> failwith ("can not convert prop top obj prop")

