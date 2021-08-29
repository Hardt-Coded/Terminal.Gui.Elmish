namespace Terminal.Gui.Elmish


open Terminal.Gui
open System


module internal TreeProcessing =

    let processElementObject (parent:View option) processElementObjects (createView:unit -> View)   (currentView:View option) (viewElement:ViewElement) =
        match currentView with
        | None ->
            let newElement = createView()
            newElement |> setPropsToElement viewElement.Props
            parent |> Option.iter (fun p -> p.Add newElement)
            {
                viewElement with
                    Element = Some newElement
                    Children = (viewElement.Children |> List.map (fun c -> processElementObjects (Some (newElement :> View)) c)) 
            }
        | Some el ->
            el |> setPropsToElement viewElement.Props
            { viewElement with 
                Children = (viewElement.Children |> List.map (fun c -> processElementObjects (Some el) c)) 
            }


    let rec processElementObjects (parent:View option) (element:ViewElement) =
        match element with
        | { Type = PageElement; Props = props; Element = elem; Children = children }  ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Toplevel.Create() :> View)
                elem
                element
        

        | { Type = WindowElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Window(NStack.ustring.Make("")) :> View)
                elem
                element
        

        | { Type = TextFieldElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> TextField(NStack.ustring.Make("")) :> View)
                elem
                element
       

        | { Type = ButtonElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Button() :> View)
                elem
                element

        | { Type = LabelElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> Label(NStack.ustring.Make("")) :> View)
                elem
                element
        

        | { Type = DateFieldElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> DateField(IsShortFormat = false) :> View)
                elem
                element
        

        | { Type = TimeFieldElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> TimeField(IsShortFormat = false) :> View)
                elem
                element
        
        | { Type = TextViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> TextView() :> View)
                elem
                element
        
        | { Type = FrameViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> FrameView() :> View)
                elem
                element
        
        | { Type = HexViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> HexView() :> View)
                elem
                element
        
        | { Type = ListViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> ListView() :> View)
                elem
                element
        
        | { Type = ProgressBarElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> ProgressBar() :> View)
                elem
                element
        
        | { Type = CheckBoxElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> CheckBox() :> View)
                elem
                element
        
        | { Type = RadioGroupElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> RadioGroup() :> View)
                elem
                element
        
        | { Type = ScrollViewElement; Props = props; Element = elem; Children = children } ->
            processElementObject 
                parent 
                processElementObjects 
                (fun () -> ScrollView() :> View)
                elem
                element
        
   
            

    let initializeTree (tree:ViewElement) =
        processElementObjects None tree


    let (|OnlyPropsChanged|_|) (ve1,ve2) =
        let cve1 = ve1.Children |> List.map (fun e -> e.Type) |> List.sort
        let cve2 = ve2.Children |> List.map (fun e -> e.Type) |> List.sort
        if cve1 <> cve2 then None else Some ()

    let (|ChildsDifferent|_|) (ve1,ve2) =
        let cve1 = ve1.Children |> List.map (fun e -> e.Type) |> List.sort
        let cve2 = ve2.Children |> List.map (fun e -> e.Type) |> List.sort
        if cve1 <> cve2 then Some () else None


    let checkReplacement (props1:IProp list) (props2:IProp list) =
        let prop1Types = props1 |> List.map (fun i -> i.GetType().Name)
        let prop2Types = props2 |> List.map (fun i -> i.GetType().Name)
        let addedPropTypes = prop2Types |> List.except prop1Types
        let deletedPropTypes = prop1Types |> List.except prop2Types
        let modifiedPropTypes = prop1Types |> List.except deletedPropTypes
        let modifiedProps =
            modifiedPropTypes
            |> List.map (fun name -> 
                let np = props2 |> List.tryFind (fun p2 -> p2.GetType().Name = name)
                let op = props1 |> List.tryFind (fun p2 -> p2.GetType().Name = name)
                match (np,op) with
                | Some np, Some op ->
                    {| 
                        NewProps = np
                        OldProps = op
                    |} |> Some
                | _, _ ->
                    None
            )
            |> List.choose id
            

        let checkStyles oldStyles newStyles =
            false
            

        modifiedProps
        |> List.exists (fun p ->
            match (p.NewProps,p.OldProps) with
            | (:? ViewProp<obj> as p1), (:? ViewProp<obj> as p2) ->
                match p1,p2 with
                | Styles oldStyles, Styles newStyles ->
                    checkStyles oldStyles newStyles
                | _ ->
                    false
            | _ -> false

                
        )


    let checkViewProps<'a> (rp:ViewProp<'a>) (newProp:ViewProp<'a>) =
        match rp, newProp with
        | Styles s1, Styles s2 ->
            let changed = s1 <> s2
            (changed, if changed then newProp :> IProp else rp :> IProp)
        | Text t1, Text t2 ->
            let changed = t1 <> t2
            (changed, if changed then newProp :> IProp else rp :> IProp)
        | HotKey k1,  HotKey k2 ->
            let changed = k1 <> k2
            (changed, if changed then newProp :> IProp else rp :> IProp)
        | ShortCut k1, ShortCut k2 -> 
            let changed = k1 <> k2
            (changed, if changed then newProp :> IProp else rp :> IProp)
        | TabIndex idx1, TabIndex idx2  ->
            let changed = idx1 <> idx2
            (changed, if changed then newProp :> IProp else rp :> IProp)
        | TabStop b1, TabStop b2 ->
            let changed = b1 <> b2
            (changed, if changed then newProp :> IProp else rp :> IProp)
        | _ ->
            (true, newProp :> IProp)


    let processProps (props1:IProp list) (props2:IProp list) (element:View) =
        let prop1Types = props1 |> List.map (fun i -> i.GetType().Name)
        let prop2Types = props2 |> List.map (fun i -> i.GetType().Name)
        let addedPropTypes = prop2Types |> List.except prop1Types
        let deletedPropTypes = prop1Types |> List.except prop2Types
        let modifyedPropTypes = prop1Types |> List.except deletedPropTypes
        let addedProps =
            addedPropTypes
            |> List.map (fun name -> 
                props2 |> List.tryFind (fun p2 -> p2.GetType().Name = name)
            )
            |> List.choose id

        // add new Props
        setPropsToElement addedProps element


        let deletedProps =
            props1
            |> List.filter (fun e -> (deletedPropTypes |> List.contains (e.GetType().Name)))

        if (deletedProps |> List.isEmpty |> not) then
            removePropsToElement deletedProps element

        let remainProps =
            props1
            |> List.filter (fun e -> (deletedPropTypes |> List.contains (e.GetType().Name) |> not))


        

        let changedRemainProps =
            remainProps
            |> List.map (fun rp ->
                props2 
                |> List.tryFind (fun np -> np.GetType().Name = rp.GetType().Name)
                |> Option.map (fun newProp ->
                    match rp, newProp with
                    | :? ViewProp<string> as rp', (:? ViewProp<string> as newProp') ->
                        checkViewProps rp' newProp'
                    | :? ViewProp<DateTime> as rp', (:? ViewProp<DateTime> as newProp') ->
                        checkViewProps rp' newProp'
                    | :? ViewProp<TimeSpan> as rp', (:? ViewProp<TimeSpan> as newProp') ->
                        checkViewProps rp' newProp'
                    | :? ViewProp<obj> as rp', (:? ViewProp<obj> as newProp') ->
                        checkViewProps rp' newProp'

                    | :? CommonProp<string> as rp', (:? CommonProp<string> as newProp') ->
                        match rp', newProp' with
                        | Value v1, Value v2 ->
                            let changed = v1 <> v2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? CommonProp<DateTime> as rp', (:? CommonProp<DateTime> as newProp') ->
                        match rp', newProp' with
                        | Value v1, Value v2 ->
                            let changed = v1 <> v2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? CommonProp<TimeSpan> as rp', (:? CommonProp<TimeSpan> as newProp') ->
                        match rp', newProp' with
                        | Value v1, Value v2 ->
                            let changed = v1 <> v2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? CommonProp<obj> as rp', (:? CommonProp<obj> as newProp') ->
                        match rp', newProp' with
                        | Value v1, Value v2 ->
                            let changed = v1 <> v2
                            (changed, if changed then newProp else rp)
                        | _, _ ->
                            (true, newProp)

                    | :? TextFieldProps as rp', (:? TextFieldProps as newProp') ->
                        match rp', newProp' with
                        | _, _ ->
                            (true, newProp)

                    | :? ListProps<obj> as rp', (:? ListProps<obj> as newProp') ->
                        match rp', newProp' with
                        | Items i1, Items i2 ->
                            let changed = i1 <> i2
                            (changed, if changed then newProp else rp)
                        
                    | :? WindowProps as rp', (:? WindowProps as newProp') ->
                        match rp', newProp' with
                        | Title t1, Title t2 ->
                            let changed = t1 <> t2
                            (changed, if changed then newProp else rp)

                    //| :? ButtonProp as rp', (:? ButtonProp as newProp') ->
                    //    match rp', newProp' with
                    //    | OnClicked _, OnClicked _ ->
                    //        (false, rp)


                    | _, _ ->
                        (true, newProp)
                )
            )
            |> List.choose id

        let toUpdate =
            changedRemainProps 
            |> List.filter (fun (b,_) -> b)
            |> List.map ( fun (_,e)-> e)

        // reset here the styles before setting them new
        toUpdate
        |> List.iter (fun prop ->
            match prop with
            | :? ViewProp<string> as p ->
                match p with
                | Styles styles ->
                    resetStylesOnElement styles element
                | _ ->
                    ()
            | :? ViewProp<DateTime> as p ->
                match p with
                | Styles styles ->
                    resetStylesOnElement styles element
                | _ ->
                    ()
            | :? ViewProp<TimeSpan> as p ->
                match p with
                | Styles styles ->
                    resetStylesOnElement styles element
                | _ ->
                    ()
            | :? ViewProp<obj> as p ->
                match p with
                | Styles styles ->
                    resetStylesOnElement styles element
                | _ ->
                    ()
            | _ ->
                ()
                
        )
        

        setPropsToElement toUpdate element



        let newModified =
            changedRemainProps 
            |> List.map ( fun (_,e)-> e)

        newModified @ addedProps



    let rec updateTree rootTree newTree (parentElement:View Option) : ViewElement =
        match rootTree.Element with
        | None ->
            failwith ("root element must be initialized")
        | Some rootElement ->
            match rootTree, newTree with
            | rt,nt when rt.Type <> nt.Type ->
                processElementObjects parentElement newTree
            | OnlyPropsChanged ->
                let replaceFullElement = checkReplacement rootTree.Props newTree.Props
                if replaceFullElement then
                    parentElement 
                    |> Option.iter (fun parentElement ->
                        parentElement.Remove(rootElement);
                    )
                    rootElement.Dispose()
                    processElementObjects parentElement newTree
                else
                    let newProps = processProps rootTree.Props newTree.Props rootElement
                    let sortedRootChildren = rootTree.Children |> List.sortBy (fun v -> v.Type)
                    let sortedNewChildren = newTree.Children |> List.sortBy (fun v -> v.Type)
                    let newChildList = (sortedRootChildren,sortedNewChildren) ||> List.map2 (fun rt nt -> updateTree rt nt rootTree.Element)
                    { rootTree with Props = newProps; Children = newChildList }
            | ChildsDifferent ->
                let replaceFullElement = checkReplacement rootTree.Props newTree.Props
                if replaceFullElement then
                    parentElement 
                    |> Option.iter (fun parentElement ->
                        parentElement.Remove(rootElement);
                    )
                    rootElement.Dispose()
                    processElementObjects parentElement newTree
                else
                    let sortedRootChildren = rootTree.Children |> List.sortBy (fun v -> v.Type)
                    let sortedNewChildren = newTree.Children |> List.sortBy (fun v -> v.Type)
                    let groupedRootType = sortedRootChildren |> List.map (fun v -> v.Type) |> List.distinct
                    let groupedNewType = sortedNewChildren |> List.map (fun v -> v.Type) |> List.distinct
                    let allTypes = groupedRootType @ groupedNewType |> List.distinct
            
                    // find out new added existing types and mix it with existing elements and set the new props
                    let newChildList = 
                        allTypes
                        |> List.map (fun et ->
                            let rootElements = sortedRootChildren |> List.filter (fun e -> e.Type = et)
                            let newElements = sortedNewChildren |> List.filter (fun e -> e.Type = et)
                            if (newElements.Length > rootElements.Length) then
                                newElements
                                |> List.mapi (fun idx ne ->
                                    if (idx+1 <= rootElements.Length) then
                                        updateTree rootElements.[idx] ne rootTree.Element
                                    else
                                        let newElem = processElementObjects (Some rootElement) ne
                                        // add elementobj to children
                                        //newElem.Element |> Option.iter rootElement.Add 
                                        newElem
                        
                                )
                            else
                                rootElements
                                |> List.mapi (fun idx re ->
                                    if (idx+1 <= newElements.Length) then
                                        Some (updateTree re newElements.[idx] rootTree.Element)
                                    else
                                        // the res we remove
                                        re.Element 
                                        |> Option.iter (fun el -> 
                                            rootElement.Remove(el)
                                            el.Dispose()
                                        )
                                        None
                            
                                )
                                |> List.choose id
                        )
                        |> List.collect id
            
                    // Update Props
                    let newProps = processProps rootTree.Props newTree.Props rootElement

                    let res = { rootTree with Props = newProps; Children = newChildList }
                    res
      
            | _ ->
                failwith "not implemented yet"