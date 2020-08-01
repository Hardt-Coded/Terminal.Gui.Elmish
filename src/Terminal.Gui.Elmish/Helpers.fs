namespace Terminal.Gui.Elmish

module Helpers =

    open System    
    open Terminal.Gui

    let findFocused (view:View) =
        view
        |> Seq.cast<View>            
        |> Seq.collect (fun x -> x |> Seq.cast<View>)
        |> Seq.tryFind (fun x -> x.HasFocus)

    let getAllElements (view:View) =
        let rec collectLoop (view:View) =
            match view.Subviews |> Seq.toList with
            | [] -> []
            | l ->
                l 
                |> List.collect (
                    fun i -> 
                        let subv = 
                            i.Subviews 
                            |> Seq.toList
                            |> List.collect collectLoop
                        i::subv
                )
        view::(collectLoop view)

    let getFocusedElements (view:View) =
        getAllElements view
        |> List.filter (fun e -> e.HasFocus)
        
    

    type View with
        // terminal gui is not immutable so i have to calculate a unique identifier
        member this.GetId() = 
            let typeName = this.GetType().Name
            let bounds = this.Bounds.GetHashCode()
            let parentName = 
                if this.SuperView <> null then 
                    this.SuperView.GetId()
                else
                    "root"
            let titleOrText =
                if (typeName = "Window") then
                    "window"
                else
                    let properties = 
                        this.GetType().GetProperties()                   
                        |> Array.filter (fun pi -> pi.Name = "Title" || pi.Name= "Text")
                        |> Array.map( fun pi -> pi.GetValue(this)|> string)
                        |> Array.collect (fun s -> s.ToCharArray())                    
                    String(properties)
                

                
            sprintf "%s%i%s%s" typeName bounds parentName titleOrText




    module StateSychronizer =
        

        type ViewElement =
            | TextField of id:string * cursorpos:int
            | TextView of id:string * row:int * col:int
            | Focused of id:string
            | Menu of barId:string * currentselected:int
            | NotRelevant
            
            

        let getViewElementState (view:View) =
            view
            |> getAllElements
            |> List.collect (fun i ->
                let id = i.GetId()
                let specialType =
                    match i with
                    | :? TextField as tf ->
                        TextField (id,tf.CursorPosition)
                    | :? TextView as tv ->
                        TextView (id,tv.CurrentRow,tv.CurrentColumn)
                    | _ when i.GetType().Name = "Menu" ->
                        let fields = i.GetType().GetFields(System.Reflection.BindingFlags.NonPublic ||| System.Reflection.BindingFlags.Instance)
                        
                        let barItemsField = 
                            fields |> Array.tryFind (fun i -> i.Name = "barItems")
                        
                        let currentField =
                            fields |> Array.tryFind (fun i -> i.Name = "current")
                        
                        match barItemsField,currentField with
                        | Some barItemsField, Some currentField ->
                            let barItem =
                                barItemsField.GetValue(i) :?> MenuBarItem
                            let currentSelected =
                                currentField.GetValue(i) :?> int

                            let bid = barItem.Title |> string
                            Menu (bid,currentSelected)
                        | _ ->
                            NotRelevant
                    | _ ->
                        NotRelevant
                
                                               
                if (i.HasFocus) then
                    [ specialType; Focused id ]
                else
                    [ specialType ]
            )
            |> List.filter (fun i -> i <> NotRelevant)


        let setViewElementState (states:ViewElement list) (view:View) =
            let allElements =
                view
                |> getAllElements
                |> List.map (fun i -> (i.GetId(),i))
                |> Map

            states
            |> List.iter (fun state ->
                match state with
                | TextField (id,cp) ->
                    allElements
                    |> Map.tryFind id
                    |> Option.iter (fun element -> (element :?> TextField).CursorPosition <- cp)
                | TextView (id,row,col) ->
                    allElements
                    |> Map.tryFind id
                    |> Option.iter (fun element -> 
                        let tv = (element :?> TextView)
                        [1..row] |> List.iter (fun _ -> element.ProcessKey(KeyEvent(Key.CursorDown)) |> ignore)
                        [1..col] |> List.iter (fun _ -> element.ProcessKey(KeyEvent(Key.CursorRight)) |> ignore)
                    )
                | Menu (barTitle,selectedEntry) ->
                    view
                    |> getAllElements
                    |> List.filter (fun i -> i :? MenuBar)
                    |> List.map (fun i -> i :?> MenuBar)
                    |> List.tryFind (fun i -> i.Menus |> Array.exists (fun mi -> (mi.Title |> string) =  barTitle))
                    |> Option.iter (fun menuBar ->
                        let barIndex =
                            menuBar.Menus 
                            |> Array.findIndex (fun mi -> (mi.Title |> string) =  barTitle)

                        // activate menu
                        menuBar.ProcessHotKey(KeyEvent(Key.F9)) |> ignore
                        [1..barIndex] |> List.iter (fun _ -> menuBar.SuperView.ProcessKey(KeyEvent(Key.CursorRight)) |> ignore)
                        [1..selectedEntry] |> List.iter (fun _ -> menuBar.SuperView.ProcessKey(KeyEvent(Key.CursorDown)) |> ignore)
                        
                    )
                    
                | Focused id ->
                    allElements
                    |> Map.tryFind id
                    |> Option.iter (fun element -> if element.SuperView<> null then element.SuperView.SetFocus(element))
                | NotRelevant ->
                    ()

            )

            // bring status bar in front
            let statusBar =
                view.Subviews
                |> Seq.tryFind (fun e -> e :? StatusBar)
                |> Option.map (fun e -> e :?> StatusBar)

            statusBar
            |> Option.iter (fun sb ->
                sb.Redraw(Rect(0,0,1000,1000))
                view.BringSubviewToFront(sb)
            )

            
            


        


