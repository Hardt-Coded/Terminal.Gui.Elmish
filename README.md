# Terminal.Gui.Elmish

[![Build Status](https://travis-ci.org/DieselMeister/Terminal.Gui.Elmish.svg?branch=releases%2F0.1.3)](https://travis-ci.org/DieselMeister/Terminal.Gui.Elmish)

An elmish wrapper around Miguel de Icaza's 'Gui.cs' https://github.com/migueldeicaza/gui.cs including a fable like view DSL.

![anim gif](https://github.com/DieselMeister/Terminal.Gui.Elmish/raw/master/docsrc/files/img/lmish_console_gif4_lower_fps.gif)

Usage:
```fs
Program.mkProgram init update view  
|> Program.run
    
```

Some fable-elmish DSL:
```fs

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

Install via Nuget:

https://www.nuget.org/packages/Terminal.Gui.Elmish/0.1.0

```
dotnet add package Terminal.Gui.Elmish
```


A lot of Thanks to Miguel de Icaza. Nice Project!.
