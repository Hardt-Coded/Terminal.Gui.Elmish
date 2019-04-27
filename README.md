# Terminal.Gui.Elmish


An elmish wrapper around Miguel de Icaza's 'Gui.cs' https://github.com/migueldeicaza/gui.cs including a fable like view DSL.


Usage:
```
Program.mkConsoleProgram init update view  
    |> Program.run

```

Some fable-elmish DSL:
```

window [
    Styles [
        Pos (PercentPos 20.0,PercentPos 10.0)
        Dim (PercentDim 30.0,AbsDim 15)
    ]
	Title "Demo 1"
] [
    button [
        Styles [
            Pos (AbsPos 1, AbsPos 1)
        ]
        Text "Counter Up"
        OnClicked (fun () -> dispatch Inc)                    
    ] 

    button [
        Styles [
            Pos (AbsPos 1, AbsPos 2)
        ]
        Text "Counter Down"
        OnClicked (fun () -> dispatch Dec)                    
    ] 

                
]

```

