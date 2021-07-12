namespace Terminal.Gui.Elmish

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
    // Additional Colors
    | FocusColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
    | HotNormalColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
    | HotFocusedColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
    | DisabledColors of forground:Terminal.Gui.Color * background:Terminal.Gui.Color
    
    

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


open System

type ViewRef(onAttached, onDetached) =
    let handle = System.WeakReference<obj>(null)

    /// Check if the new target is the same than the previous one
    /// This is done to avoid triggering change events when nothing changes
    member private __.IsSameTarget(target) =
        match handle.TryGetTarget() with
        | true, res when res = target -> true
        | _ -> false

    member x.Set(target: obj) : unit =
        if not (x.IsSameTarget(target)) then
            handle.SetTarget(target)
            onAttached x target

    member x.Unset() : unit =
        if not (x.IsSameTarget(null)) then
            handle.SetTarget(null)
            onDetached x ()

    member __.TryValue =
        match handle.TryGetTarget() with
        | true, null -> None
        | true, res -> Some res
        | _ -> None

type ViewRef<'T when 'T : not struct>() =
    let attached = Event<EventHandler<'T>, 'T>()
    let detached = Event<EventHandler, EventArgs>()

    let onAttached sender target = attached.Trigger(sender, unbox target)
    let onDetached sender () = detached.Trigger(sender, EventArgs())

    let handle = ViewRef(onAttached, onDetached)

    [<CLIEvent>] member __.Attached = attached.Publish
    [<CLIEvent>] member __.Detached = detached.Publish

    member __.Value : 'T =
        match handle.TryValue with
        | Some res -> unbox res
        | None -> failwith "view reference target has been collected or was not set"

    member __.TryValue : 'T option =
        match handle.TryValue with
        | Some res -> Some (unbox res)
        | None -> None

    member __.Unbox = handle



type ViewElement(targetType: Type, create: (unit -> obj), update: (ViewElement voption -> ViewElement -> obj -> unit), props: Prop<obj> list) =

    
    

    static member Create(create: (unit -> 'T),
         update: (ViewElement voption -> ViewElement -> 'T -> unit),
         props: Prop<'v> list) =

            ViewElement(
                typeof<'T>, 
                (create >> box),
                (fun prev curr target -> update prev curr (unbox target)),
                props |> List.map unbox
            )


    /// Get the type created by the visual element
    member x.TargetType = targetType

    /// Differentially update a visual element and update the ViewRefs if present
    member private x.Update(prevOpt: ViewElement voption, curr: ViewElement, target: obj) =
        let prevViewRefOpt = match prevOpt with ValueNone -> ValueNone | ValueSome prev -> prev.TryGetAttributeKeyed(ViewElement.RefAttribKey)
        let currViewRefOpt = curr.TryGetAttributeKeyed(ViewElement.RefAttribKey)

        // To avoid triggering unwanted events, don't unset if prevOpt = None or ViewRef is the same instance for prev and curr
        match struct (prevViewRefOpt, currViewRefOpt) with
        | struct (ValueSome prevViewRef, ValueSome currViewRef) when Object.ReferenceEquals(prevViewRef, currViewRef) -> ()
        | struct (ValueSome prevViewRef, _) -> prevViewRef.Unset()
        | _ -> ()

        update prevOpt curr target

        match currViewRefOpt with
        | ValueNone -> ()
        | ValueSome currViewRef -> currViewRef.Set(target)

    /// Apply initial settings to a freshly created visual element
    member x.Update (target: obj) = x.Update(ValueNone, x, target)

    /// Differentially update a visual element given the previous settings
    member x.UpdateIncremental(prev: ViewElement, target: obj) = x.Update(ValueSome prev, x, target)


    /// Create the UI element from the view description
    member x.Create() : obj =
        Debug.WriteLine (sprintf "Create %O" x.TargetType)
        let target = create()
        x.Update(ValueNone, x, target)
        match x.TryGetAttributeKeyed(ViewElement.CreatedAttribKey) with
        | ValueSome f -> f target
        | ValueNone -> ()
        target


    override x.ToString() = sprintf "%s(...)@%d" x.TargetType.Name (x.GetHashCode())
    


