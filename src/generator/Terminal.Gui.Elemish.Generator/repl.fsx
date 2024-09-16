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
    | "T", _ -> "'a"
    | "IList`1", [| st |] 
    | "List`1", [| st |] 
    | "IEnumerable`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        $"{mapTypeAndSubTypeProperties st.Name subprops} list"
    | "ITreeBuilder`1", [| st |] ->
        $"ITreeBuilder<{st}>"
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
        $"Func<{mapTypeAndSubTypeProperties st1.Name subprops1},{mapTypeAndSubTypeProperties st2.Name subprops2}>"
    | "Nullable`1", [| st |] ->
        let subprops = st.GetGenericArguments()
        $"{mapTypeAndSubTypeProperties st.Name subprops} option"
    | "EventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"{mapTypeAndSubTypeProperties st.Name subprops}"
    | "CancelEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"CancelEventArgs<{mapTypeAndSubTypeProperties st.Name subprops}>"
    | "DateTimeEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"DateTimeEventArgs<{mapTypeAndSubTypeProperties st.Name subprops}>"
    | "DrawTreeViewLineEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"DrawTreeViewLineEventArgs<{mapTypeAndSubTypeProperties st.Name subprops}>"
    | "ObjectActivatedEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"ObjectActivatedEventArgs<{mapTypeAndSubTypeProperties st.Name subprops}>"
    | "SelectionChangedEventArgs`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"SelectionChangedEventArgs<{mapTypeAndSubTypeProperties st.Name subprops}>"
        
    | "AspectGetterDelegate`1", [| st |] -> 
        let subprops = st.GetGenericArguments()
        $"AspectGetterDelegate<{mapTypeAndSubTypeProperties st.Name subprops}>"
        
    | "String", [| |] -> $"string"
    | "Boolean", [| |] -> $"bool"
    | "Int32", [| |] -> $"int"
    | "System.Int64", [| |] -> $"long"
    | "System.Byte", [| |] -> $"byte"
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

let getPropertyProps (properties: (string * string * Type array) array) =
    properties
    |> Array.sortBy (fun (name,_,_) -> name)
    |> Array.map (fun (name,typ,subtypes) ->
        {|
            Name = name
            CamelName = if name = "Type" then "``type``" else toCamelCase name
            Type = mapTypeAndSubTypeProperties typ subtypes
        |}
    )


let getEventProps (events: (string * string * Type array) array) =
    events
    |> Array.sortBy (fun (name,_,_) -> name)
    |> Array.map (fun (name,typ,subtypes) ->
        {|
            Name = name
            CamelName = toCamelCase name
            Type = mapTypeAndSubTypeEvents typ subtypes
        |}
    )

let printPropertiesProps (printfn:string->unit) (properties: {| Name:string; CamelName:string; Type:string |} array) = 
    properties
    |> Array.iter (fun e ->
        printfn $"    static member inline {e.CamelName} (value:{e.Type}) = Interop.mkprop \"{e.CamelName}\" value"
    )
    
let printEventsProps(printfn:string->unit)  (events: {| Name:string; CamelName:string; Type:string |} array) =
    events
    |> Array.iter (fun e ->
        printfn $"    static member inline {e.CamelName} (handler:{e.Type}) = Interop.mkprop \"{e.CamelName}\" handler"
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
            |> printPropertiesProps printfn
            printfn "    // Events"
            eventProps
            |> printEventsProps printfn
        else
            if propsProps.Length > 0 then
                printfn "    // Properties"
                propsProps
                |> printPropertiesProps printfn
                
            if eventProps.Length > 0 then                
                printfn "    // Events"
                eventProps
                |> printEventsProps printfn
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
    printfn "open System.IO"
    printfn "open System.Collections.Generic"
    printfn "open System.Collections.Specialized"
    printfn "open System.Globalization"
    printfn "open Terminal.Gui.Elmish"
    printfn "open Terminal.Gui.Elmish.ElementsNew"
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
    printfn "namespace Terminal.Gui.Elmish.Props"
    printNamepaces printfn
    // print props
    printViewProps printfn typeof<View>
    inheritedTypes
    |> Array.filter (fun t -> t.Name <> "View")
    |> Array.iter (printViewProps printfn)


if true then
    let path = Path.Combine(__SOURCE_DIRECTORY__, "Props.fs")
    generatePropsFs (fun p -> printfn $"{p}")

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
                printfn $"        props |> Interop.getValue<{e.CamelName}> \"{e.CamelName}\" |> Option.iter (fun v -> element.{e.Name} <- v)"
            )
        
        if events.Length>0 then
            // print events from view
            printfn "        // Events"
            events
            |> getEventProps
            |> Array.sortBy (fun e -> e.Name)
            |> Array.iter (fun e ->
                if (e.Type = "unit->unit") then
                    printfn $"        props |> Interop.getValue<{e.CamelName}> \"{e.CamelName}\" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.{e.Name} @>) (fun _ -> v()) element)"
                else
                    printfn $"        props |> Interop.getValue<{e.CamelName}> \"{e.CamelName}\" |> Option.iter (fun v -> Interop.setEventHandler (<@ element.{e.Name} @>) v element)"
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
                printfn $"        props |> Interop.getValue<{e.CamelName}> \"{e.CamelName}\" |> Option.iter (fun _ -> element.{e.Name} <- Unchecked.defaultof<_>)"
            )
    
        if events.Length>0 then
            // print events from view
            printfn "        // Events"
            events
            |> getEventProps
            |> Array.sortBy (fun e -> e.Name)
            |> Array.iter (fun e ->
                printfn $"        props |> Interop.getValue<{e.CamelName}> \"{e.CamelName}\" |> Option.iter (fun v -> Interop.removeEventHandler (<@ element.{e.Name} @>) element)"
            )
    
let printViewElementRemoveProps (printfn:string->unit) =
    printfn "    let setProps (element: View) props ="
    printfn "        // Properties"
    printfn "        props |> Interop.getValue<View->unit> \"ref\" |> Option.iter (fun _ -> ())"
    
    printElementRemoveProps printfn typeof<View>    

let printViewElement (printfn:string->unit) =
    printfn "type ViewElement ="
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

            
        let positionX = props |> Interop.getValue<Pos> "x"      |> Option.map (fun v -> isPosCompatible view.X v) |> Option.defaultValue true
        let positionY = props |> Interop.getValue<Pos> "y"      |> Option.map (fun v -> isPosCompatible view.Y v) |> Option.defaultValue true
        let width = props |> Interop.getValue<Dim> "width"      |> Option.map (fun v -> isDimCompatible view.Width v) |> Option.defaultValue true
        let height = props |> Interop.getValue<Dim> "height"      |> Option.map (fun v -> isDimCompatible view.Height v) |> Option.defaultValue true

        // in case width or height is removed!
        let widthNotRemoved  = removedProps |> Interop.valueExists "width"   |> not
        let heightNotRemoved = removedProps |> Interop.valueExists "height"  |> not

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
    printfn $"// {t.Name}"
    printfn $"type {t.Name}Element(props:IProperty list) ="
    printfn $"    inherit TerminalElement(props)"
    printfn ""
    printfn "    let setProps (element: View) props ="
    printElementSetProps printfn t
    printfn ""
    printfn "    let removeProps (element:Button) props ="
    printElementRemoveProps printfn t
    printfn ""
    printfn $"    override _.name = $\"{t.Name}\""
    printfn ""
    printfn $"""
    override this.create parent =
        #if DEBUG
        Diagnostics.Debug.WriteLine ($"{{this.name}} created!")
        #endif
        this.parent <- parent
        System.Diagnostics.Debug.WriteLine($"button created!")
        let el = new {t.Name}()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        ViewElement.setProps el props
        setProps el props
        props |> Interop.getValue<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el
    """
    printfn ""
    printfn """
    override this.canUpdate prevElement oldProps =
        let (changedProps,removedProps) = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement
    """
    printfn ""
    printfn $"""
    override this.update prevElement oldProps = 
        let element = prevElement :?> {t.Name}
        let (changedProps,removedProps) = Interop.filterProps oldProps props
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
    open System
    open System.ComponentModel
    open System.Linq.Expressions
    open System.Text
    open Terminal.Gui
    open Terminal.Gui.Elmish.Elements
    open Terminal.Gui.Elmish.EventHelpers

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
    
    
if false then
    generateElementsFs (fun p -> printfn $"{p}")


