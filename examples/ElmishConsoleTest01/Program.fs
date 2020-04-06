open System
open Terminal.Gui.Elmish


type Model = {foo: int}

type Msg = 
    | IncFoo

let sub =
    fun dispatch ->
        let rec loop () =
            async {
                do! Async.Sleep 5000
                dispatch IncFoo
                return! loop ()
            }
        loop () |> Async.StartImmediate
    |> Cmd.ofSub


//let sub dispatch = 
//    let rec loop () = 
//        async {
//            do! Async.Sleep 1000
//            dispatch IncFoo
//            return! loop ()
//        } 
//    loop () |> Async.RunSynchronously
    

let init () =
    {foo = 0}, Cmd.none //Cmd.none //Cmd.ofSub sub

let update msg model = 
    match msg with
    | IncFoo -> {model with foo = model.foo + 1}, Cmd.none

let view model dispatch = 
    page [
        label [Text (sprintf "foo = %d" model.foo)]
        button [
            Styles [
                Pos (AbsPos 0,AbsPos 2)
            ]
            
            OnClicked (fun () -> dispatch IncFoo)
            Text "Click me!"
        ] 
        button [
            Styles [
                Pos (AbsPos 0,AbsPos 3)
            ]
            
            OnClicked (fun () -> dispatch IncFoo)
            Text "Click me! 2"
        ] 
    ]

[<EntryPoint>]
let main argv =
    try 
        Program.mkProgram init update view
        |> Program.withSubscription (fun _ -> 
            let sub dispatch = 
                Terminal.Gui.Application
                    .MainLoop
                    .AddTimeout(
                        System.TimeSpan(0, 0, 5), 
                        (fun _ -> 
                            dispatch IncFoo
                            true
                        )
                    )
                    |> ignore
            Cmd.ofSub sub
        )
        |> Program.run
        0
    with
    | exc ->
        IO.File.WriteAllText("error.txt", sprintf "%A" exc)   
        printfn "%A" exc
        1 