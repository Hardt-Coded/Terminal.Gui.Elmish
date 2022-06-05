namespace Terminal.Gui.Elmish

open NStack


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
        ([],newprops)
        ||> List.fold (fun resultProps newProp ->
            let kv = newProp :?> KeyValue
            let get (KeyValue (a,b)) = (a,b)
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


    let ustr (s:string) = ustring.Make s

    let str (s:ustring) = s.ToString()


    let removeEventHandlerIfNecessary evName element =
        let eventDel = EventHelpers.getEventDelegates evName element
        if (eventDel.Length > 0) then
            EventHelpers.clearEventDelegates evName element