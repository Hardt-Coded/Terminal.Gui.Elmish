namespace Terminal.Gui.Elmish

open NStack
open System
open Terminal.Gui

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
    let inline valueExists name (props: IProperty list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.exists (function | KeyValue (pname,_) -> name = pname)

    let inline getValueGeneric<'a, 'b when 'b :> IProperty> (name: string) (props: 'b list) =
        props
        |> Seq.cast<KeyValue>
        |> Seq.tryFind (function | KeyValue (pname,_) -> name = pname)
        |> Option.map (function | KeyValue (_,value) -> value :?> 'a)

    let inline getValueDefaultGeneric<'a, 'b when 'b :> IProperty> (name: string) (defaultValue: 'a) (props: 'b list) =
        props
        |> getValueGeneric<'a, 'b> name
        |> Option.defaultValue defaultValue

    let inline mkprop (name:string) (data:obj) : IProperty = KeyValue (name, data)
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
        let colorDisabled = Attribute.Make(scheme.Disabled.Foreground, scheme.Disabled.Background)
        let colorFocus = Attribute.Make(scheme.Focus.Foreground, scheme.Focus.Background)
        let colorHotFocus = Attribute.Make(scheme.HotFocus.Foreground, scheme.HotFocus.Background)
        let colorHotNormal = Attribute.Make(scheme.HotNormal.Foreground, scheme.HotNormal.Background)
        let colorNormal = Attribute.Make(scheme.Normal.Foreground, scheme.Normal.Background)
        ColorScheme(Disabled=colorDisabled,Focus=colorFocus,HotFocus=colorHotFocus,HotNormal=colorHotNormal,Normal=colorNormal)

    

    let removeEventHandlerIfNecessary evName element =
        let eventDel = EventHelpers.getEventDelegates evName element
        if (eventDel.Length > 0) then
            EventHelpers.clearEventDelegates evName element

[<RequireQualifiedAccess>]
module internal Checker =

    let textChanged (element:View) text =
        element.Text <> (text |> Interop.ustr)