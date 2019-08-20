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

type Model = {
    Page: Pages

    CounterModel:Counter.Model option
    TextFieldsModel:TextFields.Model option
    RadioCheckModel:RadioCheck.Model option
    TextViewModel:TextView.Model option
    ListViewModel:ListView.Model option
    ScrollViewModel:ScrollView.Model option
    MessageBoxesModel:MessageBoxes.Model option
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

    | UpdateTime


let timerSubscription dispatch =
    let rec loop () =
        async {
            do! Async.Sleep 1000
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
                let (m,c) = Counter.init()
                let cmd =
                    c |> Cmd.map (CounterMsg)
                {model with CounterModel = Some m; Page = Counter}, cmd
            | _ ->
                {model with Page = page}, Cmd.none



        | TextFields ->
            match model.TextFieldsModel with
            | None ->
                let (m,c) = TextFields.init()
                let cmd =
                    c |> Cmd.map (TextFieldsMsg)
                {model with TextFieldsModel = Some m; Page = TextFields}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | RadioCheck ->
            match model.RadioCheckModel with
            | None ->
                let (m,c) = RadioCheck.init()
                let cmd =
                    c |> Cmd.map (RadioCheckMsg)
                {model with RadioCheckModel = Some m; Page = RadioCheck}, cmd
            | _ ->
                {model with Page = page}, Cmd.none

        | TextView ->
            match model.TextViewModel with
            | None ->
                let (m,c) = TextView.init()
                let cmd =
                    c |> Cmd.map (TextViewMsg)
                {model with TextViewModel = Some m; Page = TextView}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | ListView ->
            match model.ListViewModel with
            | None ->
                let (m,c) = ListView.init()
                let cmd =
                    c |> Cmd.map (ListViewMsg)
                {model with ListViewModel = Some m; Page = ListView}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | ScrollView ->
            match model.ScrollViewModel with
            | None ->
                let (m,c) = ScrollView.init()
                let cmd =
                    c |> Cmd.map (ScrollViewMsg)
                {model with ScrollViewModel = Some m; Page = ScrollView}, cmd
            | _ ->
                {model with Page = page}, Cmd.none
        | MessageBoxes ->
            match model.ScrollViewModel with
            | None ->
                let (m,c) = MessageBoxes.init()
                let cmd =
                    c |> Cmd.map (MessageBoxesMsg)
                {model with MessageBoxesModel = Some m; Page = MessageBoxes}, cmd
            | _ ->
                {model with Page = page}, Cmd.none


    | ExitApp ->
        Program.quit()
        model, Cmd.none
    | CounterMsg cmsg ->
        match model.CounterModel with
        | None ->
            model, Cmd.none
        | Some cmodel ->
            let (m,c) = Counter.update cmsg cmodel
            let cmd =
                c |> Cmd.map (CounterMsg)
            {model with CounterModel = Some m}, cmd


    | TextFieldsMsg tfmsg ->
        match model.TextFieldsModel with
        | None ->
            model, Cmd.none
        | Some tfmodel ->
            let (m,c) = TextFields.update tfmsg tfmodel
            let cmd =
                c |> Cmd.map (TextFieldsMsg)
            {model with TextFieldsModel = Some m}, cmd

    | RadioCheckMsg rcmsg ->
        match model.RadioCheckModel with
        | None ->
            model, Cmd.none
        | Some rcmodel ->
            let (m,c) = RadioCheck.update rcmsg rcmodel
            let cmd =
                c |> Cmd.map (RadioCheckMsg)
            {model with RadioCheckModel = Some m}, cmd

    | TextViewMsg tfmsg ->
           match model.TextViewModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let (m,c) = TextView.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map (TextViewMsg)
               {model with TextViewModel = Some m}, cmd
    | ListViewMsg tfmsg ->
           match model.ListViewModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let (m,c) = ListView.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map (ListViewMsg)
               {model with ListViewModel = Some m}, cmd
    | ScrollViewMsg tfmsg ->
           match model.ScrollViewModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let (m,c) = ScrollView.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map (ScrollViewMsg)
               {model with ScrollViewModel = Some m}, cmd
    | MessageBoxesMsg tfmsg ->
           match model.MessageBoxesModel with
           | None ->
               model, Cmd.none
           | Some tfmodel ->
               let (m,c) = MessageBoxes.update tfmsg tfmodel
               let cmd =
                   c |> Cmd.map (MessageBoxesMsg)
               {model with MessageBoxesModel = Some m}, cmd

    | UpdateTime ->
        {model with CurrentLocalTime = DateTime.Now}, Cmd.none




        
        

let view (model:Model) (dispatch:Msg->unit) =
    page [
        menuBar [
            menuBarItem "Demo" [
                menuItem "Start" "" (fun () -> dispatch (ChangePage Start))
                menuItem "Counter" "" (fun () -> dispatch (ChangePage Counter))
                menuItem "TextFields" "" (fun () -> dispatch (ChangePage TextFields))
                menuItem "Radio and Check" "" (fun () -> dispatch (ChangePage RadioCheck))
                menuItem "Text File View" "" (fun () -> dispatch (ChangePage TextView))
                menuItem "List View" "" (fun () -> dispatch (ChangePage ListView))
                menuItem "Scroll View" "" (fun () -> dispatch (ChangePage ScrollView))
                menuItem "Message Boxes" "" (fun () -> dispatch (ChangePage MessageBoxes))
                menuItem "E_xit" "" (fun () -> dispatch (ExitApp))
            ]
        ]

        window [
            Styles [
                Pos (AbsPos 0,AbsPos 1)
                Dim (Fill,Fill)
            ]
            Title (sprintf "Elmish Console Demo - %s" <| model.CurrentLocalTime.ToString("yyyy-MM-dd HH:mm:ss"))
        ] [
            window [
                Styles [
                    Pos (AbsPos 2,AbsPos 2)
                    Dim (PercentDim 20.0,FillMargin 2)
                ]
                Title "Choose"
            ] [
                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 1)                        
                    ]
                    Text "Start"
                    OnClicked (fun () -> dispatch (ChangePage Start))
                ]
                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 2)
                    ]
                    Text "Counter"
                    OnClicked (fun () -> dispatch (ChangePage Counter))
                ] 
                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 3)
                    ]
                    Text "TextFields"
                    OnClicked (fun () -> dispatch (ChangePage TextFields))
                ] 

                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 4)
                    ]
                    Text "Radio and Check"
                    OnClicked (fun () -> dispatch (ChangePage RadioCheck))
                ] 

                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 5)
                    ]
                    Text "Text File View"
                    OnClicked (fun () -> dispatch (ChangePage TextView))
                ] 

                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 6)
                    ]
                    Text "List View"
                    OnClicked (fun () -> dispatch (ChangePage ListView))
                ]                
                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 7)
                    ]
                    Text "Scroll View"
                    OnClicked (fun () -> dispatch (ChangePage ScrollView))
                ] 
                button [
                    Styles [
                        Pos (AbsPos 1, AbsPos 8)
                    ]
                    Text "Message Boxes"
                    OnClicked (fun () -> dispatch (ChangePage MessageBoxes))
                ] 
            ]

            window [
                Styles [
                    Pos (PercentPos 25.0,AbsPos 2)
                    Dim (FillMargin 2,FillMargin 2)
                ]
                Title "Demo"
            ] [
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


            ]


        ]
    ]



