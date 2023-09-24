module App

open System
open Terminal.Gui.Elmish

type Pages = 
    | Start
    | Counter
    | TextFields
    | RadioCheck
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
    View.page [
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
        ]
        
        prop.children [
            View.window [
                prop.position.x.at 0
                prop.position.y.at 0
                prop.width.filled
                prop.height.filled
                window.title $"Elmish Console Demo - {model.CurrentLocalTime:``yyyy-MM-dd HH:mm:ss.ms``}"
                window.children [

                    View.window [
                        prop.position.x.at 0
                        prop.position.y.at 0
                        prop.width.percent 20.0
                        prop.height.fill 2
                        window.title "Choose"
                        window.children [
                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 1
                                button.text "Start"
                                button.onClick (fun () -> dispatch (ChangePage Start))
                            ]
                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 2
                                button.text "Counter"
                                button.onClick (fun () -> dispatch (ChangePage Counter))
                            ] 
                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 3
                                button.text "TextFields"
                                button.onClick (fun () -> dispatch (ChangePage TextFields))
                            ] 

                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 4
                                button.text "Radio and Check"
                                button.onClick (fun () -> dispatch (ChangePage RadioCheck))
                            ] 

                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 5
                                button.text "Text File View"
                                button.onClick (fun () -> dispatch (ChangePage TextView))
                            ] 

                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 6
                                button.text "List View"
                                button.onClick (fun () -> dispatch (ChangePage ListView))
                            ]                
                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 7
                                button.text "Scroll View"
                                button.onClick (fun () -> dispatch (ChangePage ScrollView))
                            ] 
                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 8
                                button.text "Message Boxes"
                                button.onClick (fun () -> dispatch (ChangePage MessageBoxes))
                            ] 
                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 9
                                button.text "Wizard"
                                button.onClick (fun () -> dispatch (ChangePage Wizard))
                            ]
                            View.button [
                                prop.position.x.at 1
                                prop.position.y.at 10
                                button.text "Tab View"
                                button.onClick (fun () -> dispatch(ChangePage TabView))
                            ]
                        ]
                    ]

                    View.window [
                        prop.position.x.percent 25.0
                        prop.position.y.at 2
                        prop.width.fill 2
                        prop.height.fill 2
                        window.title "Demo"
                        window.children [
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