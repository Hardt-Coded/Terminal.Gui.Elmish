#r "nuget: Terminal.Gui, 2.0.0-v2-develop.2329"

open System
open System.Reflection
open Terminal.Gui

let getInheritedTypes (baseType: Type) =
    baseType.Assembly.GetTypes()
    //Assembly.GetExecutingAssembly().GetTypes()
    |> Array.filter (fun t -> t.IsSubclassOf(baseType))

// Example usage
let inheritedTypes = getInheritedTypes typeof<View>
//inheritedTypes |> Array.iter (fun t -> printfn "%s" t.FullName)

// get all properties from a type, but not the inherited ones
let getPropertiesNotInherited (t: Type) =
    t.GetProperties(BindingFlags.DeclaredOnly ||| BindingFlags.Public ||| BindingFlags.Instance)
    |> Array.map (fun p -> (p.Name, p.PropertyType.Name, p.PropertyType.GetGenericArguments()))

let getProperties (t: Type) =
    t.GetProperties()
    |> Array.map (fun p -> (p.Name, p.PropertyType.Name, p.PropertyType.GetGenericArguments()))
    
// get all events but not the inherited ones
let getEventsNotInherited (t: Type) =
    t.GetEvents(BindingFlags.DeclaredOnly ||| BindingFlags.Public ||| BindingFlags.Instance)
    |> Array.map (fun e ->
        // return name, type and generic sub type of eventhandler
        e.Name, e.EventHandlerType.Name, e.EventHandlerType.GetGenericArguments()
    )

    
    
    
let getEvents (t: Type) =
    t.GetEvents()
    |> Array.map (fun e ->
        // return name, type and generic sub type of eventhandler
        e.Name, e.EventHandlerType.Name, e.EventHandlerType.GetGenericArguments()
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
    

  
let rec mapTypeAndSubTypeProperties t (subtypes:Type array) =
    match t, subtypes with
    | "IList`1", [| st |] -> $"{st} list"
    | "Nullable`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        match subprops with
        //| [||] -> $"{st.Name} option"
        | _ ->
            $"{mapTypeAndSubTypeProperties st.Name subprops} option"
    | "EventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        match subprops with
        //| [||] -> $"{st.Name}"
        | _ ->
            $"{mapTypeAndSubTypeProperties st.Name subprops}"
    | "CancelEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        match subprops with
        //| [||] -> $"CancelEventArgs<{st.Name}>->unit"
        | _ -> $"CancelEventArgs<{mapTypeAndSubTypeProperties st.Name subprops}>"
    | "String", [| |] -> $"string"
    | _ -> $"{t}"
    
    
let mapTypeAndSubTypeEvents t (subtypes:Type array) =
    match t, subtypes with
    | "EventHandler", [| |] -> $"unit->unit"
    | "EventArgs", [|  |] -> $"unit->unit"
    | "EventHandler`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        match subprops with
        | [||] -> $"{st.Name}->unit"
        | _ ->
            $"{mapTypeAndSubTypeProperties st.Name subprops}->unit"
    | _ -> $"{t}"    


let printPropertiesProps (properties: (string * string * Type array) array) =
    properties
    |> Array.sortBy (fun (name,_,_) -> name)
    |> Array.iter (fun (name,typ,subtypes) ->
        let name = toCamelCase name
        printfn $"\tstatic member inline {name} (value:{mapTypeAndSubTypeProperties typ subtypes}) = Interop.mkprop \"{name}\" value"
    )
    
let printEventsProps (events: (string * string * Type array) array) =
    events
    |> Array.sortBy (fun (name,_,_) -> name)
    |> Array.iter (fun (name,typ,subtypes) ->
        let name = toCamelCase name
        printfn $"\tstatic member inline {name} (handler:{mapTypeAndSubTypeEvents typ subtypes}) = Interop.mkprop \"{name}\" handler"
    )
    
    
let printViewProps (view: Type) =
    printfn $"// {view.Name}"
    // Header View
    let typeName = if view.Name = "View" then "prop" else toCamelCase view.Name
    if typeName = "prop" then
        printfn $"""
type {typeName} =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline ref (reference:View->unit) = Interop.mkprop "ref" reference
        """
    else
        printfn $"type {typeName} ="
    
        
    if typeName = "prop" then        
        printfn "\t// Properties"
        getProperties view
        |> printPropertiesProps
        printfn "\t// Events"
        getEvents view
        |> printEventsProps
    else
        printfn "\t// Properties"
        getPropertiesNotInherited view
        |> printPropertiesProps
        printfn "\t// Events"
        getEventsNotInherited view
        |> printEventsProps
        


let namespaces = getAllNamespaces inheritedTypes
// opens
printfn "open System"
namespaces |> Array.iter (fun ns -> printfn $"open {ns}")

(*// Header View
printfn """
type prop =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline ref (reference:View->unit) = Interop.mkprop "ref" reference
"""

printfn ""
printfn "\t// Properties"
typedefof<View>
|> getProperties
|> Array.sortBy (fun (name,_,_) -> name)
|> Array.iter (fun (name,typ,subtypes) ->
    let name = toCamelCase name
    printfn $"\tstatic member inline {name} (value:{mapTypeAndSubTypeProperties typ subtypes}) = Interop.mkprop \"{name}\" value"
)

printfn ""
printfn "\t// Events"
typedefof<View>
|> getEvents
|> Array.sortBy (fun (name,_,_) -> name)
|> Array.iter (fun (name,typ,subtypes) ->
    let name = toCamelCase name
    printfn $"\tstatic member inline {name} (handler:{mapTypeAndSubTypeEvents typ subtypes}) = Interop.mkprop \"{name}\" handler"
)*)

printViewProps typeof<View>
inheritedTypes
|> Array.filter (fun t -> t.Name <> "View")
|> Array.iter printViewProps

//typedefof<Nullable<EventArgs<CancelEventArgs>>>

(*typedefof<View>.GetProperties()
|> Array.filter (fun p -> p.Name = "TabStop")
|> Array.iter (fun p -> printfn $"%A{p}")*)