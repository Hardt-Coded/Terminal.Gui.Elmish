module App

open Elmish
open Terminal.Gui.Elmish

type Pages = 
    | Start
    | Counter
    | TextFields
    | RadioCheck

type Model = {
    Page: Pages

    CounterModel:Counter.Model option
    TextFieldsModel:TextFields.Model option
    RadioCheckModel:RadioCheck.Model option
}


type Msg = 
    | ChangePage of Pages

    | CounterMsg of Counter.Msg
    | TextFieldsMsg of TextFields.Msg
    | RadioCheckMsg of RadioCheck.Msg


let init () =
    let model = {
        Page=Start
        CounterModel = None
        TextFieldsModel = None
        RadioCheckModel = None
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




        
        

let view (model:Model) (dispatch:Msg->unit) =
    page [
        menuBar [
            menuBarItem "Demo" [
                menuItem "Start" "" (fun () -> dispatch (ChangePage Start))
                menuItem "Counter" "" (fun () -> dispatch (ChangePage Counter))
                menuItem "TextFields" "" (fun () -> dispatch (ChangePage TextFields))
                menuItem "Radio and Check" "" (fun () -> dispatch (ChangePage RadioCheck))
            ]
        ]

        window [
            Styles [
                Pos (AbsPos 0,AbsPos 1)
                Dim (Fill,Fill)
            ]
            Title "Elmish Console Demo"
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


            ]


        ]
    ]



