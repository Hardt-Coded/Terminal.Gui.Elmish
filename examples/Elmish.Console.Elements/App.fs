module App

open System
open Terminal.Gui
open Terminal.Gui.Elmish

type Pages = 
    | Start
    | Counter
    | TextFields
    | RadioCheck
    | Combo
    | TextView
    | ListView
    | ScrollView
    | MessageBoxes
    | Wizard
    | TabView

type Model = {
    Page: Pages

    CounterModel:Counter.Model option
    TextFieldsModel:TextFields.Model option
    RadioCheckModel:RadioCheck.Model option
    ComboModel:Combo.Model option
    TextViewModel:TextView.Model option
    ListViewModel:ListView.Model option
    ScrollViewModel:ScrollView.Model option
    MessageBoxesModel:MessageBoxes.Model option
    WizardModel:Wizard.Model option
    CurrentLocalTime:DateTime
}


type Msg = 
    | ChangePage of Pages
    | ExitApp

    | CounterMsg of Counter.Msg
    | TextFieldsMsg of TextFields.Msg
    | RadioCheckMsg of RadioCheck.Msg
    | ComboMsg of Combo.Msg
    | TextViewMsg of TextView.Msg
    | ListViewMsg of ListView.Msg
    | ScrollViewMsg of ScrollView.Msg
    | MessageBoxesMsg of MessageBoxes.Msg
    | WizardMsg of Wizard.Msg

    | UpdateTime


let timerSubscription dispatch =
    let rec loop () =
        async {
            do! Async.Sleep 20
            dispatch UpdateTime
            return! loop ()
        }
    loop () |> Async.Start



let init () =
    let model = {
        Page=Start
        CounterModel = None
        TextFieldsModel = None
        RadioCheckModel = None
        ComboModel = None
        TextViewModel = None
        ListViewModel = None
        ScrollViewModel = None
        MessageBoxesModel = None
        WizardModel = None
        CurrentLocalTime = DateTime.Now
    }
    model, Cmd.none


let update (msg:Msg) (model:Model) =
    match msg with
    | ChangePage page ->
        match page with
        | Start ->
            {model with Page = page}, Cmd.none
        | Counter ->
            match model.CounterModel with
            | None ->
                let m,c = Counter.init()
                let cmd =
                    c |> Cmd.map CounterMsg
                {model with CounterModel = Some m; Page = Counter}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | TextFields ->
            match model.TextFieldsModel with
            | None ->
                let m,c = TextFields.init()
                let cmd =
                    c |> Cmd.map TextFieldsMsg
                {model with TextFieldsModel = Some m; Page = TextFields}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | RadioCheck ->
            match model.RadioCheckModel with
            | None ->
                let m,c = RadioCheck.init()
                let cmd =
                    c |> Cmd.map RadioCheckMsg
                {model with RadioCheckModel = Some m; Page = RadioCheck}, cmd
            | _ ->
                {model with Page = page}, Cmd.none

        | Combo ->
            match model.RadioCheckModel with
            | None ->
                let m,c = Combo.init()
                let cmd =
                    c |> Cmd.map ComboMsg
                {model with ComboModel =  Some m; Page = Combo}, cmd
            | _ ->
                {model with Page = page}, Cmd.none

        | TextView ->
            match model.TextViewModel with
            | None ->
                let m,c = TextView.init()
                let cmd =
                    c |> Cmd.map TextViewMsg
                {model with TextViewModel = Some m; Page = TextView}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | ListView ->
            match model.ListViewModel with
            | None ->
                let m,c = ListView.init()
                let cmd =
                    c |> Cmd.map ListViewMsg
                {model with ListViewModel = Some m; Page = ListView}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | ScrollView ->
            match model.ScrollViewModel with
            | None ->
                let m,c = ScrollView.init()
                let cmd =
                    c |> Cmd.map ScrollViewMsg
                {model with ScrollViewModel = Some m; Page = ScrollView}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | MessageBoxes ->
            match model.MessageBoxesModel with
            | None ->
                let m,c = MessageBoxes.init()
                let cmd =
                    c |> Cmd.map MessageBoxesMsg
                {model with MessageBoxesModel = Some m; Page = MessageBoxes}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | Wizard ->
            match model.WizardModel with
            | None ->
                let m,c = Wizard.init()
                let cmd =
                    c |> Cmd.map WizardMsg
                {model with WizardModel = Some m; Page = Wizard}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | TabView ->
            { model with Page = TabView }, Cmd.none

    | ExitApp ->
        Program.quit()
        model, Cmd.none
        
    | CounterMsg cmsg ->
        match model.CounterModel with
        | None ->
            model, Cmd.none
        | Some cmodel ->
            let m,c = Counter.update cmsg cmodel
            let cmd =
                c |> Cmd.map CounterMsg
            {model with CounterModel = Some m}, cmd


    | TextFieldsMsg tfmsg ->
        match model.TextFieldsModel with
        | None ->
            model, Cmd.none
        | Some tfmodel ->
            let m,c = TextFields.update tfmsg tfmodel
            let cmd =
                c |> Cmd.map TextFieldsMsg
            {model with TextFieldsModel = Some m}, cmd

    | RadioCheckMsg rcmsg ->
        match model.RadioCheckModel with
        | None ->
            model, Cmd.none
        | Some rcmodel ->
            let m,c = RadioCheck.update rcmsg rcmodel
            let cmd =
                c |> Cmd.map RadioCheckMsg
            {model with RadioCheckModel = Some m}, cmd

    | ComboMsg msg ->
        match model.ComboModel with
        | None ->
            model, Cmd.none
        | Some cmodel ->
            let m,c = Combo.update msg cmodel
            let cmd =
                c |> Cmd.map ComboMsg
            { model with ComboModel =  Some m}, cmd

    | TextViewMsg tfmsg ->
           match model.TextViewModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let m,c = TextView.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map TextViewMsg
               {model with TextViewModel = Some m}, cmd
    | ListViewMsg tfmsg ->
           match model.ListViewModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let m,c = ListView.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map ListViewMsg
               {model with ListViewModel = Some m}, cmd
    | ScrollViewMsg tfmsg ->
           match model.ScrollViewModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let m,c = ScrollView.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map ScrollViewMsg
               {model with ScrollViewModel = Some m}, cmd
    | MessageBoxesMsg tfmsg ->
           match model.MessageBoxesModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let m,c = MessageBoxes.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map MessageBoxesMsg
               {model with MessageBoxesModel = Some m}, cmd

    | WizardMsg tfmsg ->
        match model.WizardModel with
        | None ->
            model, Cmd.none
        | Some tfmodel ->
            let m,c = Wizard.update tfmsg tfmodel
            let cmd =
                c |> Cmd.map WizardMsg
            {model with WizardModel = Some m}, cmd

    | UpdateTime ->
        {model with CurrentLocalTime = DateTime.Now}, Cmd.none




        
        

let view (model:Model) (dispatch:Msg->unit) =
    (*View.window [
        
        page.menuBar [
            menubar.menus [
                menu.menuBarItem [
                    menu.prop.title "Application"
                    menu.prop.children [
                        menu.submenuItem [
                             menu.prop.title "Page"
                             menu.prop.children [
                                 menu.menuItem ("Start", (fun () -> dispatch (ChangePage Start)))
                                 menu.menuItem ("Counter", (fun () -> dispatch (ChangePage Counter)))
                                 menu.menuItem ("TextFields", (fun () -> dispatch (ChangePage TextFields)))
                                 menu.menuItem ("RadioCheck", (fun () -> dispatch (ChangePage RadioCheck)))
                                 menu.menuItem ("TextView", (fun () -> dispatch (ChangePage TextView)))
                                 menu.menuItem ("ListView", (fun () -> dispatch (ChangePage ListView)))
                                 menu.menuItem ("ScrollView", (fun () -> dispatch (ChangePage ScrollView)))
                                 menu.menuItem ("MessageBoxes", (fun () -> dispatch (ChangePage MessageBoxes)))
                                 menu.menuItem ("Wizard", (fun () -> dispatch (ChangePage Wizard)))
                                 menu.menuItem ("TabView", (fun () -> dispatch (ChangePage TabView)))
                             ]
                        ]
                        menu.menuItem ("Exit", (fun () -> dispatch (ExitApp)))
                    ]
                ]
            ]
        ]*)
    View.topLevel [
        prop.children [
            View.menuBar [
                menuBar.menus [
                    MenuBarItem("Test", [|
                        MenuItem ("Start","", (fun () -> dispatch (ChangePage Start)))
                        MenuItem ("Counter","", (fun () -> dispatch (ChangePage Counter)))
                        MenuItem ("TextFields","", (fun () -> dispatch (ChangePage TextFields)))
                        MenuItem ("RadioCheck","", (fun () -> dispatch (ChangePage RadioCheck)))
                        MenuItem ("Combo","", (fun () -> dispatch (ChangePage Combo)))
                        MenuItem ("TextView","", (fun () -> dispatch (ChangePage TextView)))
                        MenuItem ("ListView","", (fun () -> dispatch (ChangePage ListView)))
                        MenuItem ("ScrollView","", (fun () -> dispatch (ChangePage ScrollView)))
                        MenuItem ("MessageBoxes","", (fun () -> dispatch (ChangePage MessageBoxes)))
                        MenuItem ("Wizard","", (fun () -> dispatch (ChangePage Wizard)))
                        MenuItem ("TabView","", (fun () -> dispatch (ChangePage TabView)))
                        MenuItem("Exit","", fun () -> dispatch ExitApp)
                    |])
                ]
            ]
            
            View.window [
                prop.position.x.absolute 0
                prop.position.y.absolute 0
                prop.width.fill 0
                prop.height.fill 0
                prop.title $"Elmish Console Demo - {model.CurrentLocalTime:``yyyy-MM-dd HH:mm:ss.ms``}"
                prop.children [
                    
                    View.window [
                        prop.position.x.absolute 0
                        prop.position.y.absolute 0
                        prop.width.percent 20
                        prop.height.fill 2
                        prop.title "Choose"
                        prop.children [
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 1
                                button.text "Start"
                                prop.accept (fun ev -> dispatch (ChangePage Start))
                            ]
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 2
                                button.text "Counter"
                                prop.accept (fun ev -> dispatch (ChangePage Counter))
                            ] 
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 3
                                button.text "TextFields"
                                prop.accept (fun ev -> dispatch (ChangePage TextFields))
                            ] 

                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 4
                                button.text "Radio and Check"
                                prop.accept (fun ev -> dispatch (ChangePage RadioCheck))
                            ]
                            
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 5
                                button.text "Combo und Co"
                                prop.accept (fun ev -> dispatch (ChangePage Combo))
                            ]

                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 6
                                button.text "Text File View"
                                prop.accept (fun ev -> dispatch (ChangePage TextView))
                            ] 

                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 7
                                button.text "List View"
                                prop.accept (fun ev -> dispatch (ChangePage ListView))
                            ]                
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 8
                                button.text "Scroll View"
                                prop.accept (fun ev -> dispatch (ChangePage ScrollView))
                            ] 
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 9
                                button.text "Message Boxes"
                                prop.accept (fun ev -> dispatch (ChangePage MessageBoxes))
                            ] 
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 10
                                button.text "Wizard"
                                prop.accept (fun ev -> dispatch (ChangePage Wizard))
                            ]
                            View.button [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 11
                                button.text "Tab View"
                                prop.accept (fun ev -> dispatch(ChangePage TabView))
                            ]
                        ]
                    ]

                    View.window [
                        prop.position.x.percent 25
                        prop.position.y.absolute 2
                        prop.width.fill 2
                        prop.height.fill 2
                        prop.title "Demo"
                        prop.children [
                            match model.Page with
                            | Start ->
                                yield! Start.view
                            | Counter ->
                                match model.CounterModel with
                                | None -> ()
                                | Some cmodel ->
                                    yield! Counter.view cmodel (CounterMsg >> dispatch)
                            | TextFields ->
                                match model.TextFieldsModel with
                                | None -> ()
                                | Some tfmodel ->
                                    yield! TextFields.view tfmodel (TextFieldsMsg >> dispatch)
                            | RadioCheck ->
                                match model.RadioCheckModel with
                                | None -> ()
                                | Some rcmodel ->
                                    yield! RadioCheck.view rcmodel (RadioCheckMsg >> dispatch)
                            | Combo ->
                                match model.ComboModel with
                                | None -> ()
                                | Some cmodel ->
                                yield! Combo.view cmodel (ComboMsg >> dispatch)
                            | TextView ->
                                match model.TextViewModel with
                                | None -> ()
                                | Some tvmodel ->
                                    yield! TextView.view tvmodel (TextViewMsg >> dispatch)
                            | ListView ->
                                match model.ListViewModel with
                                | None -> ()
                                | Some tvmodel ->
                                    yield! ListView.view tvmodel (ListViewMsg >> dispatch)
                            | ScrollView ->
                                match model.ScrollViewModel with
                                | None -> ()
                                | Some svmodel ->
                                    yield! ScrollView.view svmodel (ScrollViewMsg >> dispatch)
                            | MessageBoxes ->
                                match model.MessageBoxesModel with
                                | None -> ()
                                | Some svmodel ->
                                    yield! MessageBoxes.view svmodel (MessageBoxesMsg >> dispatch)
                            | Wizard ->
                                match model.WizardModel with
                                | None -> ()
                                | Some svmodel ->
                                    yield! Wizard.view svmodel (WizardMsg >> dispatch)
                            | TabView ->
                                yield! TabView.view 
                        ]
                    ]
                ]
            ]
        ]
    ]