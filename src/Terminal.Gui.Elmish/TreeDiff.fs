namespace Terminal.Gui.Elmish

module Differ =

    open Terminal.Gui

    let (|OnlyPropsChanged|_|) (ve1:TerminalElement,ve2:TerminalElement) =
        let cve1 = ve1.children |> List.map (fun e -> e.name) |> List.sort
        let cve2 = ve2.children |> List.map (fun e -> e.name) |> List.sort
        if cve1 <> cve2 then None else Some ()

    let (|ChildsDifferent|_|) (ve1:TerminalElement,ve2:TerminalElement) =
        let cve1 = ve1.children |> List.map (fun e -> e.name) |> List.sort
        let cve2 = ve2.children |> List.map (fun e -> e.name) |> List.sort
        if cve1 <> cve2 then Some () else None


    let rec initializeTree (parent:View option) (tree:TerminalElement) =
        tree.create()
        match parent  with
        | None ->
            ()
        | Some parent ->
            parent.Add tree.element

        match tree.children with
        | [] ->
            ()
        | head::rest ->
            (initializeTree (Some tree.element) head)
            rest |> List.iter (fun e -> initializeTree (Some tree.element) e)



    let rec update (rootTree:TerminalElement) (newTree:TerminalElement) =
        match rootTree, newTree with
        | rt, nt when rt.name <> nt.name ->
            initializeTree None newTree
        | OnlyPropsChanged ->
            newTree.update rootTree.element rootTree.properties
            let sortedRootChildren = rootTree.children |> List.sortBy (fun v -> v.name)
            let sortedNewChildren = newTree.children |> List.sortBy (fun v -> v.name)
            (sortedRootChildren,sortedNewChildren) ||> List.iter2 (fun rt nt -> update rt nt)
        | ChildsDifferent ->
            newTree.update rootTree.element rootTree.properties
            let sortedRootChildren = rootTree.children |> List.sortBy (fun v -> v.name)
            let sortedNewChildren = newTree.children |> List.sortBy (fun v -> v.name)
            let groupedRootType = sortedRootChildren |> List.map (fun v -> v.name) |> List.distinct
            let groupedNewType = sortedNewChildren |> List.map (fun v -> v.name) |> List.distinct
            let allTypes = groupedRootType @ groupedNewType |> List.distinct

        
            allTypes
            |> List.iter (fun et ->
                let rootElements = sortedRootChildren |> List.filter (fun e -> e.name = et)
                let newElements = sortedNewChildren |> List.filter (fun e -> e.name = et)
                if (newElements.Length > rootElements.Length) then
                    newElements
                    |> List.iteri (fun idx ne ->
                        if (idx+1 <= rootElements.Length) then
                            update rootElements.[idx] ne
                        else
                            let newElem = initializeTree (Some rootTree.element) ne
                            newElem
                
                    )
                else
                    rootElements
                    |> List.iteri (fun idx re ->
                        if (idx+1 <= newElements.Length) then
                            update re newElements.[idx]
                        else
                            // the rest we remove
                            re.element |> rootTree.element.Remove |> ignore
                            ()
                    
                    )
            )
        | _ ->
            printfn "other"
            ()