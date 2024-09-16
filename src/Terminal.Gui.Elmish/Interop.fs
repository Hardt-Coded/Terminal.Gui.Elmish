namespace Terminal.Gui.Elmish

open System.Linq.Expressions
open System.Text
open System
open Terminal.Gui

module internal EventHelpers =

    open System.Reflection
    
    type Expr = 
        static member Quote(e:Expression<System.Func<_, _>>) = e
        static member Quote(e:Expression<System.Action<_>>) = e

    let getEventDelegates (eventName:string) (o:obj) =
        //let eventInfo  = o.GetType().GetEvent(eventName, BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        //if eventInfo |> isNull then
        //    []
        //else
        
        let field = o.GetType().GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        let baseField = o.GetType().BaseType.GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        let eventDelegate = 
            if field |> isNull |> not then 
                field.GetValue(o) :?> MulticastDelegate 
            elif baseField |> isNull |> not then 
                baseField.GetValue(o) :?> MulticastDelegate 
            else null
        if (eventDelegate |> isNull) then
            []
        else
            eventDelegate.GetInvocationList() |> Array.toList

    let clearEventDelegates (eventName:string) (o:obj) =
        let eventInfo  = o.GetType().GetEvent(eventName, BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        if eventInfo |> isNull then
            ()
        else
            let field = o.GetType().GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic)
            let baseField = o.GetType().BaseType.GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
            let eventDelegate = 
                if field |> isNull |> not then 
                    field.GetValue(o) :?> MulticastDelegate 
                elif baseField |> isNull |> not then 
                    baseField.GetValue(o) :?> MulticastDelegate 
                else null
            if (eventDelegate |> isNull) then
                ()
            else
            
                eventDelegate.GetInvocationList() |> Array.iter (fun d -> eventInfo.RemoveEventHandler(o, d))

    let addEventDelegates (eventName:string) (delegates:Delegate list) (o:obj) =
        let eventInfo  = o.GetType().GetEvent(eventName, BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        if (eventInfo |> isNull) then
            ()
        else
            delegates |> List.iter (fun d -> eventInfo.AddEventHandler(o, d))




[<RequireQualifiedAccess>]
module Interop =
    let inline valueExists name (props: IProperty list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.exists (function | KeyValue (pname,_) -> name = pname)

    let inline getValueGeneric<'a, 'b when 'b :> IProperty> (name: string) (props: 'b list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.tryFind (function | KeyValue (pname,_) -> name = pname)
        |> Option.map (function | KeyValue (_,value) -> value :?> 'a)
        
    let inline checkValueGeneric<'a, 'b when 'b :> IProperty> (name: string) (props: 'b list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.tryFind (function | KeyValue (pname,_) -> name = pname)
        |> Option.map (function | KeyValue (_,value) -> value.GetType() = typeof<'a>)

    let inline getValueDefaultGeneric<'a, 'b when 'b :> IProperty> (name: string) (defaultValue: 'a) (props: 'b list) =
        props
        |> getValueGeneric<'a, 'b> name
        |> Option.defaultValue defaultValue

    let inline mkprop (name:string) (data:obj) : IProperty = KeyValue (name, data)
    
    let inline checkValue<'a> name props = checkValueGeneric<'a, IProperty> name props |> Option.defaultValue false
    let inline getValue<'a> name props = getValueGeneric<'a, IProperty> name props
    let inline getValueDefault<'a> name defaultVal (props:IProperty list) = getValueDefaultGeneric<'a, IProperty> name defaultVal props

    let inline mkMenuProp (name:string) (data:obj) : IMenuProperty = KeyValue (name, data)
    let inline getMenuValue<'a> name (props: IMenuProperty list) = getValueGeneric<'a, IMenuProperty> name props
    let inline getMenuValueDefault<'a> name defaultVal (props:IMenuProperty list) = getValueDefaultGeneric<'a, IMenuProperty> name defaultVal props

    let inline mkMenuBarProp (name:string) (data:obj) : IMenuBarProperty = KeyValue (name, data)
    let inline getMenuBarValue<'a> name (props:IMenuBarProperty list) = getValueGeneric<'a, IMenuBarProperty> name props
    let inline getMenuBarValueDefault<'a> name defaultVal (props:IMenuBarProperty list) = getValueDefaultGeneric<'a, IMenuBarProperty> name defaultVal props

    let mkTabProp (name: string) (data: obj) : ITabProperty = KeyValue (name, data)
    let getTabValue<'a> name (props: ITabProperty list) = getValueGeneric<'a, ITabProperty> name props

    let mkTabItemProp (name: string) (data: obj) : ITabItemProperty = KeyValue (name, data)
    let getTabItemValue<'a> name (props: ITabItemProperty list) = getValueGeneric<'a, ITabItemProperty> name props

    let inline mkstyle (name:string) (data:obj) : IStyle= KeyValue (name, data)
    
    let inline styleExists name (styles:IStyle list) =
        styles
        |> Seq.cast<KeyValue>
        |> Seq.exists (function | KeyValue (pname,_) -> name = pname)
    
    let inline getStyle<'a> name (styles:IStyle list) =
        styles
        |> Seq.cast<KeyValue>
        |> Seq.tryFind (function | KeyValue (pname,_) -> name = pname)
        |> Option.map (function | KeyValue (_,value) -> value :?> 'a)
    
    
    let inline getStyleDefault<'a> name defaultVal (styles:IStyle list) =
        styles
        |> Seq.cast<KeyValue>
        |> Seq.tryFind (function | KeyValue (pname,_) -> name = pname)
        |> Option.map (function | KeyValue (_,value) -> value :?> 'a)
        |> Option.defaultValue defaultVal


    let filterProps (oldprops:IProperty list) (newprops:IProperty list) =
        let get (KeyValue (a,b)) = (a,b)
        let changedProps = 
            ([],newprops)
            ||> List.fold (fun resultProps newProp ->
                let kv = newProp :?> KeyValue
                let (name,newValue) = kv |> get
                let oldValue = oldprops |> getValue name
                match oldValue with
                | None ->
                    resultProps @ [ newProp ]
                //| Some oldValue when name = "menubar" ->
                //    let oldValue = (oldValue :?> MenuBarElement) 
                //    let newValue = (newValue :?> MenuBarElement)
                //    resultProps
                | Some oldValue when name = "children" ->
                    resultProps
                | Some oldValue when oldValue = newValue ->
                    resultProps
                | Some _ ->
                    resultProps @ [ newProp ]
            )

        let removedProps =
            ([],oldprops)
            ||> List.fold (fun resultProps oldProp ->
                let op = oldProp :?> KeyValue
                let (name,_) = op |> get
                let newProp = newprops |> valueExists name
                match newProp with
                | true ->
                    resultProps
                | false ->
                    resultProps @ [ oldProp ]
                
            )
        (changedProps, removedProps)


    (*let ustr (s:string) = ustring.Make s

    let str (s:ustring) = s.ToString()*)


    let inline csharpList (list:'a list) = System.Linq.Enumerable.ToList list


    let rec getParent (view:View) =
        view.SuperView 
        |> Option.ofObj
        |> Option.bind (fun p ->
            if p.GetType().Name.Contains("Content") then
                getParent p
            else
                Some p
        )

    let cloneColorScheme (scheme:ColorScheme) =
        let disabled = scheme.Disabled
        let focus = scheme.Focus
        let hotFocus = scheme.HotFocus
        let hotNormal = scheme.HotNormal
        let normal = scheme.Normal
        let colorDisabled = Attribute(&disabled)
        let colorFocus = Attribute(&focus)
        let colorHotFocus = Attribute(&hotFocus)
        let colorHotNormal = Attribute(&hotNormal)
        let colorNormal = Attribute(&normal)
        ColorScheme(Disabled=colorDisabled,Focus=colorFocus,HotFocus=colorHotFocus,HotNormal=colorHotNormal,Normal=colorNormal)

    

    let removeEventHandlerIfNecessary evName element =
        let eventDel = EventHelpers.getEventDelegates evName element
        if (eventDel.Length > 0) then
            EventHelpers.clearEventDelegates evName element
            
    open Microsoft.FSharp.Quotations
    open Microsoft.FSharp.Quotations.Patterns
    open Microsoft.FSharp.Quotations.DerivedPatterns
    open FSharp.Linq.RuntimeHelpers
    
    
    /// Get the property name from an event expression
    /// i don'T like it, it's unnecessary complicated to get the name as sting from this particualr expression 
    let private getPropNameFromEvent (eventExpr: Expr<IEvent<'a,'b>>) =
        match eventExpr with
        | Call (a, mi, expr) ->
            if mi.Name = "CreateEvent" then
                if expr.Length < 1 then
                    None
                else
                    match expr[0] with
                    | Lambda (a, expr) ->
                        match expr with
                        | Call (a, mi, call) ->
                            Some (mi.Name.Replace("add_", "").Replace("remove_", ""))
                        | _ -> None
                    | _ -> None
            else
                None
        | _ -> None
    
    /// Set an event handler for an event on an element and remove the previous handler
    let setEventHandler (eventExpr: Expr<IEvent<'a,'b>>) (eventHandler:'b->unit) element  =
        let evName = getPropNameFromEvent eventExpr
        match evName with
        | None -> raise (ArgumentException "can not get property name from event expression")
        | Some evName ->
            let eventDel = EventHelpers.getEventDelegates evName element
            if (eventDel.Length > 0) then
                EventHelpers.clearEventDelegates evName element
            let event = unbox<IEvent<'a,'b>>(LeafExpressionConverter.EvaluateQuotation eventExpr)
            event.Add(eventHandler)
        
        
    let removeEventHandler (eventExpr: Expr<IEvent<'a,'b>>) element =
        let evName = getPropNameFromEvent eventExpr
        match evName with
        | None -> raise (ArgumentException "can not get property name from event expression")
        | Some evName ->
            let eventDel = EventHelpers.getEventDelegates evName element
            if (eventDel.Length > 0) then
                EventHelpers.clearEventDelegates evName element
            
        
        
        
        
    /// Set an event handler for an event on an element and remove the previous handler
    let setEventHandlerExpr (eventExpr: Expression<Func<_,IEvent<'a,'b>>>) eventHandler element =
        let evName =
            match eventExpr.Body with
            | :? MethodCallExpression as me ->
                let property = (me.Object :?> MemberExpression)
                property.Member.Name
            | _ -> raise (ArgumentException("Invalid event expression"))
            
        let eventDel = EventHelpers.getEventDelegates evName element
        if (eventDel.Length > 0) then
            EventHelpers.clearEventDelegates evName element
            
        let event = eventExpr.Compile()
        let e = event.Invoke() // :?> IEvent<'a,'b>
        e.Add(eventHandler)
            
        
        
    
[<RequireQualifiedAccess>]
module internal Checker =

    let textChanged (element:View) text =
        element.Text <> text