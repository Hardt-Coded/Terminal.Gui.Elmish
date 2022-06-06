namespace Terminal.Gui.Elmish

open NStack
open System

module internal EventHelpers =

    open System.Reflection

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

    let inline mkprop (name:string) (data:obj) : IProperty = KeyValue (name, data)

    let inline valueExists name (props:IProperty list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.exists (function | KeyValue (pname,_) -> name = pname)

    let inline getValue<'a> name (props:IProperty list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.tryFind (function | KeyValue (pname,_) -> name = pname)
        |> Option.map (function | KeyValue (_,value) -> value :?> 'a)


    let inline getValueDefault<'a> name defaultVal (props:IProperty list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.tryFind (function | KeyValue (pname, _) -> name = pname)
        |> Option.map (function | KeyValue (_,value) -> value :?> 'a)
        |> Option.defaultValue defaultVal



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


    let inline filterProps (oldprops:IProperty list) (newprops:IProperty list) =
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


    let ustr (s:string) = ustring.Make s

    let str (s:ustring) = s.ToString()


    let removeEventHandlerIfNecessary evName element =
        let eventDel = EventHelpers.getEventDelegates evName element
        if (eventDel.Length > 0) then
            EventHelpers.clearEventDelegates evName element