#r "nuget: Terminal.Gui, 2.0.0-v2-develop.2329"

open System
open System.IO
open System.Reflection
open Terminal.Gui

let getInheritedTypes (baseType: Type) =
    baseType.Assembly.GetTypes()
    //Assembly.GetExecutingAssembly().GetTypes()
    |> Array.filter (fun t -> t.IsSubclassOf(baseType))
    // remove internal
    |> Array.filter (fun t -> t.IsPublic)
    // sort, but type name with `1 should be before the type without `1
    |> Array.sortBy (fun t -> $"{t.Name}1".Replace("`1", "0"))

// Example usage
let inheritedTypes = getInheritedTypes typeof<View>
//inheritedTypes |> Array.iter (fun t -> printfn "%s" t.FullName)

// get all properties from a type, but not the inherited ones
let getPropertiesNotInherited (t: Type) =
    t.GetProperties(BindingFlags.DeclaredOnly ||| BindingFlags.Public ||| BindingFlags.Instance)
    // filter out properties, which have a private setter
    |> Array.filter (fun p -> p.CanWrite)
    |> Array.filter (fun p -> p.SetMethod |> isNull |> not && p.SetMethod.IsPublic)
    |> Array.filter (fun p ->
        match p.Name with
        | "SuperView" -> false
        | "CommandView" -> false
        | "Parent" -> false
        | _ -> true
        )
    // remove properties with init
    |> Array.filter (fun p ->
        let setMethodReturnParameterModifiers = p.SetMethod.ReturnParameter.GetRequiredCustomModifiers()
        setMethodReturnParameterModifiers |> Seq.exists (fun t -> t = (typeof<System.Runtime.CompilerServices.IsExternalInit>)) |> not
    )
    
    |> Array.map (fun p -> (p.Name, p.PropertyType, p.PropertyType.GetGenericArguments()))

let getProperties (t: Type) =
    t.GetProperties(BindingFlags.Public ||| BindingFlags.Instance)
    |> Array.filter (fun p -> p.CanWrite)
    |> Array.filter (fun p -> p.SetMethod |> isNull |> not && p.SetMethod.IsPublic)
    |> Array.filter (fun p ->
        match p.Name with
        | "SuperView" -> false
        | "CommandView" -> false
        | "Parent" -> false
        | _ -> true
        )
    // remove virtual properties
    //|> Array.filter (fun p -> p.SetMethod.IsVirtual |> not)
    // remove properties with init
    |> Array.filter (fun p ->
        let setMethodReturnParameterModifiers = p.SetMethod.ReturnParameter.GetRequiredCustomModifiers()
        setMethodReturnParameterModifiers |> Seq.exists (fun t -> t = (typeof<System.Runtime.CompilerServices.IsExternalInit>)) |> not
    )
    |> Array.map (fun p ->
        let name = p.Name    
        (name, p.PropertyType, p.PropertyType.GetGenericArguments())
    )
    
// get all events but not the inherited ones
let getEventsNotInherited (t: Type) =
    t.GetEvents(BindingFlags.DeclaredOnly ||| BindingFlags.Public ||| BindingFlags.Instance)
    |> Array.map (fun e ->
        // return name, type and generic sub type of eventhandler
        e.Name, e.EventHandlerType, e.EventHandlerType.GetGenericArguments()
    )

    
    
    
let getEvents (t: Type) =
    t.GetEvents()
    |> Array.map (fun e ->
        // return name, type and generic sub type of eventhandler
        let name = e.Name
        e.Name, e.EventHandlerType, e.EventHandlerType.GetGenericArguments()
    )    

let getAllNamespaces (types:Type array) =
    types
    |> Array.map (fun t -> t.Namespace)
    |> Array.distinct

// make string camal case
let toCamelCase (s: string) =
    let first = s.[0]
    let rest = s.[1..]
    $"{Char.ToLower(first)}{rest}"
    

  
let rec mapTypeAndSubTypeProperties (t: Type) (subtypes:Type array) =
    match t.Name, subtypes with
    | "T", _ -> "'a"
    | "IList`1", [| st |] 
    | "List`1", [| st |] 
    | "IEnumerable`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        $"{mapTypeAndSubTypeProperties st subprops} list"
    | "ITreeBuilder`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        $"ITreeBuilder<{mapTypeAndSubTypeProperties st subprops}>"
    | "Stack`1", [| st |] -> $"Stack<{st}>"
    | "IReadOnlyList`1", [| st |] -> $"{st} list"
    | "IReadOnlyCollection`1", [| st |] -> $"{st} list"
    | "IReadOnlyDictionary`1", [| st |] -> $"{st} dict"
    | "IReadOnlyDictionary`2", [| st1 ; st2 |] -> $"IReadOnlyDictionary<{st1},{st2}>"
    | "SliderEventArgs`1", _ -> $"SliderEventArgs<'a>"
    | "SliderOption`1", _ -> $"SliderOption<'a>"
    | "Func`2", [| st1 ; st2 |] ->
        let subprops1 = st1.GetGenericArguments()
        let subprops2 = st2.GetGenericArguments()
        $"Func<{mapTypeAndSubTypeProperties st1 subprops1},{mapTypeAndSubTypeProperties st2 subprops2}>"
    | "Nullable`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        $"{mapTypeAndSubTypeProperties st subprops} option"
    | "EventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"{mapTypeAndSubTypeProperties st subprops}"
    | "CancelEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"CancelEventArgs<{mapTypeAndSubTypeProperties st subprops}>"
    | "DateTimeEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"DateTimeEventArgs<{mapTypeAndSubTypeProperties st subprops}>"
    | "DrawTreeViewLineEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"DrawTreeViewLineEventArgs<{mapTypeAndSubTypeProperties st subprops}>"
    | "ObjectActivatedEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"ObjectActivatedEventArgs<{mapTypeAndSubTypeProperties st subprops}>"
    | "SelectionChangedEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"SelectionChangedEventArgs<{mapTypeAndSubTypeProperties st subprops}>"
        
    | "NotifyCollectionChangedEventHandler", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"SelectionChangedEventArgs<{mapTypeAndSubTypeProperties st subprops}>"
        
    | "AspectGetterDelegate`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"AspectGetterDelegate<{mapTypeAndSubTypeProperties st subprops}>"
        
    | "IListDataSource", [| |] -> $"string list"
    | "String", [| |] -> $"string"
    | "Boolean", [| |] -> $"bool"
    | "UInt32", [| |] -> $"int"
    | "System.Int64", [| |] -> $"long"
    | "System.UInt32", [| |] -> $"int"
    | "System.Byte", [| |] -> $"byte"
    | "String[]", [| |] -> $"string list"
    | "View", [| |] -> $"TerminalElement"
    | _ when t.IsArray && t.GetElementType().IsSubclassOf(typeof<View>) ->
        $"{t.GetElementType().Name} list"
    | _ when t.IsArray && t.GetElementType().IsSubclassOf(typeof<MenuItem>) ->
        $"{t.GetElementType().Name} list"
    | _ when t.IsArray ->
        $"{t.GetElementType().Name} list"
    
    | _ -> $"{t.Name}"
    

let mapTypeAndSubTypeEvents (t:Type) (subtypes:Type array) =
    match t.Name, subtypes with
    | "EventHandler", [| |] -> $"unit->unit", "v"
    | "EventArgs", [|  |] -> $"unit->unit", "v"
    | "EventHandler`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        let mapFunction =
            match subtypes |> Array.tryHead with
            | Some st1 when st1.Name = "EventArgs`1" -> "(fun arg -> v arg.CurrentValue)"
            | _ -> "v"
        $"{mapTypeAndSubTypeProperties st subprops}->unit", mapFunction
    | "NotifyCollectionChangedEventHandler", [| |] -> "NotifyCollectionChangedEventArgs->unit","v"
    | "SplitterEventHandler", [| |] -> "SplitterEventArgs->unit","v"
    | _ -> $"{t.Name}", "v"    

let getPropertyProps (properties: (string * Type * Type array) array) =
    properties
    |> Array.sortBy (fun (name,_,_) -> name)
    |> Array.map (fun (name,typ,subtypes) ->
        {|
            Name = name
            CamelName = if name = "Type" then "``type``" else toCamelCase name
            Type = mapTypeAndSubTypeProperties typ subtypes
            OrigType = typ.Name
        |}
    )


let getEventProps (events: (string * Type * Type array) array) =
    events
    |> Array.sortBy (fun (name,_,_) -> name)
    |> Array.map (fun (name,typ,subtypes) ->
        let t, mapFunction = mapTypeAndSubTypeEvents typ subtypes
        {|
            Name = name
            CamelName = toCamelCase name
            Type = t            
            MapFunction = mapFunction
        |}
    )

let printPropertiesProps (printfn:string->unit) (t:Type) (properties: {| Name:string; CamelName:string; Type:string; OrigType: string |} array) = 
    properties
    |> Array.iter (fun e ->
        printfn $"    static member inline {e.CamelName} (value:{e.Type}) = Interop.mkprop \"{toCamelCase t.Name}.{e.CamelName}\" value"
    )
    
let printEventsProps(printfn:string->unit) (t:Type) (events: {| Name:string; CamelName:string; Type:string; MapFunction: string |} array) =
    events
    |> Array.iter (fun e ->
        printfn $"    static member inline {e.CamelName} (handler:{e.Type}) = Interop.mkprop \"{toCamelCase t.Name}.{e.CamelName}\" handler"
    )
    
    
let printViewProps (printfn:string->unit) (view: Type) =
    let propsProps =
        if view.Name = "View" then
            getProperties view
            |> getPropertyProps
        else
            getPropertiesNotInherited view
            |> getPropertyProps
        
    let eventProps =
        if view.Name = "View" then
            getEvents view
            |> getEventProps
        else
            getEventsNotInherited view
            |> getEventProps
    
    
    printfn $"// {view.Name}"
    if propsProps.Length + eventProps.Length > 0 then
        
        // Header View
        let typeName = (if view.Name = "View" then "prop" else toCamelCase view.Name).Replace("`1", "<'a>")
        let typeName = if typeName = "treeView<'a>" then "treeView<'a when 'a : not struct>" else typeName
        if typeName = "prop" then
            printfn $"""
type {typeName} =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline ref (reference:View->unit) = Interop.mkprop "ref" reference
            """
        else
            printfn $"type {typeName} ="
        
        
        
        if typeName = "prop" then        
            printfn "    // Properties"
            propsProps
            |> printPropertiesProps printfn view
            printfn "    // Events"
            eventProps
            |> printEventsProps printfn view
            
            
            // additional props
            printfn $"""
    static member inline color (fg:Color, bg:Color) = Interop.mkprop "view.colorScheme" (
            let attrib = Attribute(&fg, &bg)
            ColorScheme(attrib)
        )            
            """
            // add addtional props for Position and Dim, extract the static methods from the Dim and Position Class
            printfn $"""

module prop =    
    module position =
        type x =
            static member inline absolute (position:int)                                        = Interop.mkprop "view.x" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "view.x" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd                                                      = Interop.mkprop "view.x" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.x" (Pos.AnchorEnd(offset))
            static member inline center                                                         = Interop.mkprop "view.x" (Pos.Center())
            static member inline func (f:unit -> int)                                           = Interop.mkprop "view.x" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "view.x" (Pos.Percent(percent))

        type y =
            static member inline absolute (position:int)                                        = Interop.mkprop "view.y" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "view.y" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd                                                      = Interop.mkprop "view.y" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.y" (Pos.AnchorEnd(offset))
            static member inline center                                                         = Interop.mkprop "view.y" (Pos.Center())
            static member inline func (f:unit -> int)                                           = Interop.mkprop "view.y" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "view.y" (Pos.Percent(percent))
        
    type width =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "view.width" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "view.width" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "view.width" (Dim.Fill(margin))
        static member inline func (f:unit->int)                                                                     = Interop.mkprop "view.width" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.width" (Dim.Percent(percent, mode))
        static member inline percent (percent:int)                                                                  = Interop.mkprop "view.width" (Dim.Percent(percent, DimPercentMode.ContentSize))

    type height =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "view.height" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "view.height" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "view.height" (Dim.Fill(margin))
        static member inline func (f:unit->int)                                                                     = Interop.mkprop "view.height" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.height" (Dim.Percent(percent, mode))
        static member inline percent (percent:int)                                                                  = Interop.mkprop "view.height" (Dim.Percent(percent, DimPercentMode.ContentSize))
    
    
    type alignment =
        static member inline center     =   Interop.mkprop "view.alignment" Alignment.Center
        static member inline ``end``    =   Interop.mkprop "view.alignment" Alignment.End
        static member inline start      =   Interop.mkprop "view.alignment" Alignment.Start
        static member inline fill       =   Interop.mkprop "view.alignment" Alignment.Fill

    type textDirection =
        static member inline bottomTop_leftRight = Interop.mkprop "view.textDirection" TextDirection.BottomTop_LeftRight
        static member inline bottomTop_rightLeft = Interop.mkprop "view.textDirection" TextDirection.BottomTop_RightLeft
        static member inline leftRight_bottomTop = Interop.mkprop "view.textDirection" TextDirection.LeftRight_BottomTop
        static member inline leftRight_topBottom = Interop.mkprop "view.textDirection" TextDirection.LeftRight_TopBottom
        static member inline rightLeft_bottomTop = Interop.mkprop "view.textDirection" TextDirection.RightLeft_BottomTop
        static member inline rightLeft_topBottom = Interop.mkprop "view.textDirection" TextDirection.RightLeft_TopBottom
        static member inline topBottom_leftRight = Interop.mkprop "view.textDirection" TextDirection.TopBottom_LeftRight

    type borderStyle =
        static member inline double = Interop.mkprop    "view.borderStyle" LineStyle.Double
        static member inline none = Interop.mkprop      "view.borderStyle" LineStyle.None
        static member inline rounded = Interop.mkprop   "view.borderStyle" LineStyle.Rounded
        static member inline single = Interop.mkprop    "view.borderStyle" LineStyle.Single

    type shadowStyle =
        static member inline none = Interop.mkprop          "view.shadowStyle" ShadowStyle.None
        static member inline opaque = Interop.mkprop        "view.shadowStyle" ShadowStyle.Opaque
        static member inline transparent = Interop.mkprop   "view.shadowStyle" ShadowStyle.Transparent
    
    """                    
        else
            if propsProps.Length > 0 then
                printfn "    // Properties"
                propsProps
                |> printPropertiesProps printfn view
                
            if eventProps.Length > 0 then                
                printfn "    // Events"
                eventProps
                |> printEventsProps printfn view
                
            // add possible additional props
            if view.Name = "CheckBox" then
                printfn $"""
    static member inline ischecked (value:bool) = Interop.mkprop "checkBox.checkedState" (if value then CheckState.Checked else CheckState.UnChecked)
            """
            if view.Name = "TextView" then
                printfn $"""
    static member inline textChanged (handler:string->unit) = Interop.mkprop "textView.textChanged" handler
            """
            if view.Name = "TabView" then
                printfn $"""
    static member inline tabs (value:TerminalElement list) = Interop.mkprop "tabView.tabs" value
            """
            
            
                
            
    else
        printfn $"// No properties or events {view.Name}"
        
    printfn ""
        
let printNamepaces (printfn:string->unit) =
    let namespaces = getAllNamespaces inheritedTypes
    // opens
    printfn "open System"
    printfn "open System.Text"
    printfn "open System.Drawing"
    printfn "open System.ComponentModel"
    printfn "open System.Collections.ObjectModel"
    printfn "open System.IO"
    printfn "open System.Collections.Generic"
    printfn "open System.Collections.Specialized"
    printfn "open System.Globalization"
    printfn "open Terminal.Gui.Elmish"
    printfn "open Terminal.Gui.Elmish.Elements"
    printfn "open Terminal.Gui.TextValidateProviders"
    namespaces |> Array.filter (fun ns -> String.IsNullOrWhiteSpace ns |> not)|> Array.iter (fun ns -> printfn $"open {ns}")




let generatePropsFs (printfn:string->unit) =
    printfn ""
    printfn "(*"
    printfn "#######################################"
    printfn "#            Props.fs              #"
    printfn "#######################################"
    printfn "*)"
    printfn ""
    printfn "namespace Terminal.Gui.Elmish"
    printNamepaces printfn
    // print props
    printViewProps printfn typeof<View>
    inheritedTypes
    |> Array.filter (fun t -> t.Name <> "View")
    |> Array.iter (printViewProps printfn)


if true then
    let file = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\Terminal.Gui.Elmish\PropsNew.fs")
    // empty file
    File.WriteAllText(file, "")
    let st = new StreamWriter(file)
    generatePropsFs (fun p -> st.WriteLine p)
    st.Flush()
    st.Close()

// #################################### Element.fs ###################


let printElementSetProps (printfn:string->unit) (t:Type) =
    let props =
        if t.Name = "View" then getProperties t else getPropertiesNotInherited t
        
    let events =
        if t.Name = "View" then getEvents t else getEventsNotInherited t
    
    if props.Length + events.Length = 0 then
        printfn $"        // No properties or events {t.Name}"
        printfn $"        ()"
    else
        if props.Length>0 then
            // print properties from view
            printfn "        // Properties"
            props
            |> getPropertyProps
            |> Array.sortBy (fun e -> e.Name)
            |> Array.iter (fun e ->
                match e.OrigType with
                | "IList`1"
                | "List`1" ->
                    printfn $"""        props |> Interop.getValue<{e.Type}> "{toCamelCase t.Name}.{e.CamelName}" |> Option.iter (fun v -> element.{e.Name} <- v.ToList())"""
                | "UInt32" ->
                    printfn $"""        props |> Interop.getValue<{e.Type}> "{toCamelCase t.Name}.{e.CamelName}" |> Option.iter (fun v -> element.{e.Name} <- (v |> uint32))"""
                | "IListDataSource" ->
                    printfn $"""        props |> Interop.getValue<string list> "{toCamelCase t.Name}.{e.CamelName}" |> Option.iter (fun v -> element.SetSource (ObservableCollection(v)))"""
                | "View" ->
                    printfn $"""
        props
        |> Interop.getValue<TerminalElement> "{toCamelCase t.Name}.{e.CamelName}"
        |> Option.iter (fun v -> 
            v.create (Some element)
            element.View <- v.element
        )"""
                | "String[]" 
                | "MenuBarItem[]" ->
                    printfn $"""        props |> Interop.getValue<{e.Type}> "{toCamelCase t.Name}.{e.CamelName}" |> Option.iter (fun v -> element.{e.Name} <- v |> List.toArray)"""
                | _ ->
                    printfn $"""        props |> Interop.getValue<{e.Type}> "{toCamelCase t.Name}.{e.CamelName}" |> Option.iter (fun v -> element.{e.Name} <- v {(if e.Type.Contains(" option") then " |> Option.toNullable" else "")})"""
            )
        
        if events.Length>0 then
            // print events from view
            printfn "        // Events"
            events
            |> getEventProps
            |> Array.sortBy (fun e -> e.Name)
            |> Array.iter (fun e ->
                if (e.Type = "unit->unit") then
                    printfn $"        props |> Interop.getValue<{e.Type}> \"{toCamelCase t.Name}.{e.CamelName}\" |> Option.iter (fun v -> Interop.setEventHandler <@ element.{e.Name} @> (fun _ -> v()) element)"
                else
                    printfn $"        props |> Interop.getValue<{e.Type}> \"{toCamelCase t.Name}.{e.CamelName}\" |> Option.iter (fun v -> Interop.setEventHandler <@ element.{e.Name} @> {e.MapFunction} element)"
            )
        
        
// print elements (Elements2.fs)
let printViewElementSetProps (printfn:string->unit) =
    printfn "    let setProps (element: View) props ="
    printfn "        // Properties"
    printfn "        props |> Interop.getValue<View->unit> \"ref\" |> Option.iter (fun v -> v element)"
    
    printElementSetProps printfn typeof<View>
    
    
    
    
    
let printElementRemoveProps (printfn:string->unit) (t:Type) =
    let props =
        if t.Name = "View" then getProperties t else getPropertiesNotInherited t
        
    let events =
        if t.Name = "View" then getEvents t else getEventsNotInherited t
    
    if props.Length + events.Length = 0 then
        printfn $"        // No properties or events {t.Name}"
        printfn $"        ()"
    else
        if props.Length>0 then
            // print properties from view
            printfn "        // Properties"
            props
            |> getPropertyProps
            |> Array.sortBy (fun e -> e.Name)
            |> Array.iter (fun e ->
                match e.OrigType with
                | "IListDataSource" ->
                    printfn $"        props |> Interop.getValue<{e.Type}> \"{toCamelCase t.Name}.{e.CamelName}\" |> Option.iter (fun _ -> element.SetSource Unchecked.defaultof<_>)"
                | _ ->
                    printfn $"        props |> Interop.getValue<{e.Type}> \"{toCamelCase t.Name}.{e.CamelName}\" |> Option.iter (fun _ -> element.{e.Name} <- Unchecked.defaultof<_>)"
            )
    
        if events.Length>0 then
            // print events from view
            printfn "        // Events"
            events
            |> getEventProps
            |> Array.sortBy (fun e -> e.Name)
            |> Array.iter (fun e ->
                printfn $"        props |> Interop.getValue<{e.Type}> \"{toCamelCase t.Name}.{e.CamelName}\" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.{e.Name} @> element)"
            )
    
let printViewElementRemoveProps (printfn:string->unit) =
    printfn "    let removeProps (element: View) props ="
    printfn "        // Properties"
    printfn "        props |> Interop.getValue<View->unit> \"ref\" |> Option.iter (fun _ -> ())"
    
    printElementRemoveProps printfn typeof<View>
    
    

let printViewElement (printfn:string->unit) =
    printfn "module ViewElement ="
    printfn ""
    printViewElementSetProps printfn
    printfn ""
    printViewElementRemoveProps printfn
    // print can update
    printfn """
    let canUpdate (view:View) props removedProps =
        let isPosCompatible (a:Pos) (b:Pos) =
            let nameA = a.GetType().Name
            let nameB = b.GetType().Name
            nameA = nameB ||
            (nameA = "PosAbsolute" && nameB = "PosAbsolute") ||
            (nameA <> "PosAbsolute" && nameB <> "PosAbsolute")

        let isDimCompatible (a:Dim) (b:Dim) =
            let nameA = a.GetType().Name
            let nameB = b.GetType().Name
            nameA = nameB ||
            (nameA = "DimAbsolute" && nameB = "DimAbsolute") ||
            (nameA <> "DimAbsolute" && nameB <> "DimAbsolute")

            
        let positionX = props |> Interop.getValue<Pos> "view.x"      |> Option.map (fun v -> isPosCompatible view.X v) |> Option.defaultValue true
        let positionY = props |> Interop.getValue<Pos> "view.y"      |> Option.map (fun v -> isPosCompatible view.Y v) |> Option.defaultValue true
        let width = props |> Interop.getValue<Dim> "view.width"      |> Option.map (fun v -> isDimCompatible view.Width v) |> Option.defaultValue true
        let height = props |> Interop.getValue<Dim> "view.height"      |> Option.map (fun v -> isDimCompatible view.Height v) |> Option.defaultValue true

        // in case width or height is removed!
        let widthNotRemoved  = removedProps |> Interop.valueExists "view.width"   |> not
        let heightNotRemoved = removedProps |> Interop.valueExists "view.height"  |> not

        [
            positionX
            positionY
            width
            height
            widthNotRemoved
            heightNotRemoved
        ]
        |> List.forall id
    """



let printElement (printfn:string->unit) (t:Type) =
    let name, isGeneric, onlyRef =
        match t.Name with
        | "TreeView`1" ->
            t.Name.Replace("`1", ""), true, true
        | x when x.Contains("`1") ->
            t.Name.Replace("`1", ""), true, false
        | _ -> t.Name, false, false
            
    let onlyRefNotation = if onlyRef then " when 'a : not struct" else ""            
            
    printfn $"""// {name}{(if isGeneric then ($"<'a{onlyRefNotation}>") else "")}"""
    printfn $"""type {name}Element{(if isGeneric then ($"<'a{onlyRefNotation}>") else "")}(props:IProperty list) ="""
    
    let name = $"""{name}{(if isGeneric then "<'a>" else "")}"""
    
    match name with
    |"TreeView" ->
        printfn $"    inherit {name}Element<ITreeNode>(props)"
        printfn ""
    |"NumericUpDown" ->
        printfn $"    inherit {name}Element<int>(props)"
        printfn ""
    | _ ->
        printfn $"    inherit TerminalElement(props)"
        printfn ""
        printfn $"""    let setProps (element: {name}) props ="""
        printElementSetProps printfn t
        // additional props
        if name = "TextView" then
            printfn $"""
            // Additional properties
        props |> Interop.getValue<string->unit> "textView.textChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentsChanged @> (fun _ -> v element.Text) element)
            """
        if name = "TabView" then
            printfn $"""
            // Additional properties
        props |> Interop.getValue<TerminalElement list> "tabView.tabs" |> Option.iter (fun v ->
            v
            |> List.iter (fun tabItems ->
                tabItems.create (Some element)
                element.AddTab ((tabItems.element :?> Tab), false)
                
                )
            )
            """
        
        printfn ""
        printfn $"""    let removeProps (element:{name}) props ="""
        printElementRemoveProps printfn t
        // additional props
        if name = "TextView" then
            printfn $"""
            // Additional properties
        props |> Interop.getValue<string->unit> "textView.textChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentsChanged @> element)
            """
        printfn ""
        printfn $"    override _.name = $\"{name}\""
        printfn ""
        printfn $"""
    override this.create parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{{this.name}} created!"
        #endif
        this.parent <- parent
        """
        printfn $"""
        let el = new {name}()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
        """
        printfn ""
        printfn """
    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
        """
        printfn ""
        printfn $"""
    override this.update prevElement oldProps = 
        let element = prevElement :?> {name}
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        ViewElement.setProps prevElement changedProps
        setProps element changedProps
        this.element <- prevElement    
        """
        printfn ""
        printfn ""




    
let generateElementsFs (printfn:string->unit) =    
    printfn ""
    printfn "(*"
    printfn "#######################################"
    printfn "#            Elements.fs              #"
    printfn "#######################################"
    printfn "*)"
    printfn ""
    printfn """
namespace Terminal.Gui.Elmish.Elements

open System
open System.ComponentModel
open System.Text
open System.Linq
open Terminal.Gui
open Terminal.Gui.Elmish


[<AbstractClass>]
type TerminalElement (props:IProperty list) =
    let mutable view: View = null
    let mutable p: View option = None
    let mutable addProps = []
    let c = props |> Interop.getValueDefault<TerminalElement list> "children" []

    member this.parent with get() = p and set v = p <- v
    member this.element with get() = view and set v = view <- v
    member this.additionalProps with get() = addProps and set v = addProps <- v
    member _.properties = props @ addProps
    member _.children   = c

    abstract create: parent:View option -> unit
    abstract update: prevElement:View -> oldProps:IProperty list -> unit
    abstract canUpdate: prevElement:View -> oldProps:IProperty list -> bool
    abstract name: string

    """
    printfn ""
    printNamepaces printfn
    printViewElement printfn
    inheritedTypes
    |> Array.filter (fun t -> t.Name <> "View")
    |> Array.iter (printElement printfn)
    
    
if true then
    let file = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\Terminal.Gui.Elmish\ElementsNew.fs")
    // empty file
    File.WriteAllText(file, "")
    let st = new StreamWriter(file)
    generateElementsFs (fun p -> st.WriteLine p)
    st.Flush()
    st.Close()



// ######################### View.fs ############################
let printViewType (printfn:string->unit) =
    inheritedTypes
    |> Array.sortBy (fun e -> e.Name)
    |> Array.map (fun e ->
        match e.Name with
        x when x.Contains("`1") ->
            e.Name.Replace("`1", ""), true
        | _ -> e.Name, false
    )
    |> Array.iter (fun (name, isGeneric)  ->
        let generic, notStruct =
            if isGeneric then
                match name with
                | "TreeView" -> "<'a>","<'a when 'a : not struct>"
                | _ -> "<'a>","<'a>"
            else
                "",""
        printfn $"""    /// <seealso cref="Terminal.Gui.{name}"/>"""
        printfn $"""    static member inline {toCamelCase name}{notStruct} (props:IProperty list) = {name}Element{generic}(props) :> TerminalElement"""
        printfn $"""    static member inline {toCamelCase name}{notStruct} (children:TerminalElement list) = 
        let props = [ prop.children children ]
        {name}Element{generic}(props) :> TerminalElement"""
        printfn $"""    static member inline {toCamelCase name}{notStruct} (x:int, y:int, title: string) = 
        let props = [ 
            prop.position.x.absolute x
            prop.position.y.absolute y
            prop.title title
        ]
        {name}Element{generic}(props) :> TerminalElement"""
        printfn ""
    )
     



let generateViewFs (printfn:string->unit) =    
    printfn ""
    printfn "(*"
    printfn "#######################################"
    printfn "#            View.fs                  #"
    printfn "#######################################"
    printfn "*)"
    printfn ""
    printfn """
namespace Terminal.Gui.Elmish

open System
open System.ComponentModel
open System.Linq.Expressions
open System.Text
open System.Linq
open Terminal.Gui
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Elmish.EventHelpers
    """
    
    printfn "type View ="
    printfn """
    static member inline topLevel (props:IProperty list) = ToplevelElement(props) :> TerminalElement
    static member inline topLevel (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ToplevelElement(props) :> TerminalElement

    """
    
    printViewType printfn
    
    printfn ""
    printfn """
module Dialogs =
    open System

    let showWizard (wizard:Wizard) =
        Application.Run(wizard)
        wizard.Dispose()
        ()


    let openFileDialog title =
        use dia = new OpenDialog(Title=title)
        Application.Run(dia)
        dia.Dispose()
        //Application.Top.Remove(dia) |> ignore
        if dia.Canceled then
            None
        else
            let file = 
                dia.FilePaths
                |> Seq.tryHead
                |> Option.bind (fun s ->
                    if String.IsNullOrEmpty(s) then None 
                    else Some s
                )
            file


    let messageBox (width:int) height title text (buttons:string list) =
        let result = MessageBox.Query(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ when result < 0 || result > buttons.Length - 1  -> ""
        | _ -> buttons.[result]


    let errorBox (width:int) height title text (buttons:string list) =
        let result = MessageBox.ErrorQuery(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ when result < 0 || result > buttons.Length - 1  -> ""
        | _ -> buttons.[result]


    """
    
    
if true then
    let file = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\Terminal.Gui.Elmish\ViewNew.fs")
    // empty file
    File.WriteAllText(file, "")
    let st = new StreamWriter(file)
    generateViewFs (fun p -> st.WriteLine p)
    st.Flush()
    st.Close()
