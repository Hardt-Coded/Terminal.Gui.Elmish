# Terminal.Gui.Elmish

[![Build Status](https://travis-ci.org/DieselMeister/Terminal.Gui.Elmish.svg?branch=master)](https://travis-ci.org/DieselMeister/Terminal.Gui.Elmish)

An elmish wrapper around Miguel de Icaza's 'Gui.cs' https://github.com/migueldeicaza/gui.cs including a Feliz-like like view DSL.

![20220614_terminal_gui_2](https://user-images.githubusercontent.com/13096516/173627457-eb4d5e71-9819-4c9f-aa13-a037846745a4.gif)

# Major Changes

I decided to rework the DSL to a Feliz-style. Thank you Zaid Ajaj (https://github.com/Zaid-Ajaj/Feliz) for that awesome idea! You can leverage now more the Intellisense of your IDE.  
I also introduced a diffing mechanism, so that the elements are not recreated on every cycle. I try actually to update the current elements.

This can be end up sometimes in some weird behavior or exceptions. I try to find all the quirks, but help me out and open an issue if you find something.


# Documentation

It's missing again!

Almost all properties from the View-Elements should be available. Some events I extended. For example the Toggled-Event from a checkbox returns the old value in the event.  
I mapped the event to return both.



# Example

In the examples you find the old project, which I converted to the new DSL


# Usage:



```fs
Program.mkProgram init update view  
|> Program.run
    
```

Some fable-elmish DSL:
```fs

module Counter

open Terminal.Gui
open Terminal.Gui.Elmish
open System

type Model = {
    Counter:int
    IsSpinning: bool
}

type Msg =
    | Increment
    | Decrement
    | Reset
    | StartSpin
    | StopSpin
    | Spinned

let init () : Model * Cmd<Msg> =
    let model = {
        Counter = 0
        IsSpinning = false
    }
    model, Cmd.none


module Commands =
    let startSpinning isSpinning =
        fun dispatch ->
            async {
                do! Async.Sleep 20
                if isSpinning then
                    dispatch Increment
                    dispatch Spinned
            }
            |> Async.StartImmediate
        |> Cmd.ofSub

let update (msg:Msg) (model:Model) =
    match msg with
    | Increment ->
        {model with Counter = model.Counter + 1}, Cmd.none
    | Decrement ->
        {model with Counter = model.Counter - 1}, Cmd.none
    | Reset ->
        {model with Counter = 0}, Cmd.none
    | StartSpin ->
        {model with IsSpinning = true}, Commands.startSpinning true
    | StopSpin ->
        {model with IsSpinning = false}, Cmd.none
    | Spinned ->
        model, Commands.startSpinning model.IsSpinning
        


let view (model:Model) (dispatch:Msg->unit) =
    View.page [
        page.menuBar [
            menubar.menus [
                menu.menuBarItem [
                    menu.prop.title "Menu 1"
                    menu.prop.children [
                        menu.submenuItem [
                            menu.prop.title "Sub Menu 1"
                            menu.prop.children [
                                menu.menuItem ("Sub Item 1", (fun () -> System.Diagnostics.Debug.WriteLine($"Sub menu 1 triggered")))
                                menu.menuItem [
                                    menu.prop.title "Sub Item 2"
                                    menu.item.action (fun () -> System.Diagnostics.Debug.WriteLine($"Sub menu 2 triggered"))
                                    menu.item.itemstyle.check
                                    menu.item.isChecked true
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
        prop.children [
            View.label [
                prop.position.x.center
                prop.position.y.at 1
                prop.textAlignment.centered
                prop.color (Color.BrightYellow, Color.Green)
                prop.text "'F#ncy' Counter!"
            ] 

            View.button [
                prop.position.x.center
                prop.position.y.at 5
                prop.text "Up"
                button.onClick (fun () -> dispatch Increment)
            ] 

            View.label [
                let c = (model.Counter |> float) / 100.0
                let x = (16.0 * Math.Cos(c)) |> int 
                let y = (8.0 * Math.Sin(c)) |> int

                prop.position.x.at (x + 20)
                prop.position.y.at (y + 10)
                prop.textAlignment.centered
                prop.color (Color.Magenta, Color.BrightYellow)
                prop.text $"The Count of 'Fancyness' is {model.Counter}"
            ] 


            View.button [
                prop.position.x.center
                prop.position.y.at 7
                prop.text "Down"
                button.onClick (fun () -> dispatch Decrement)
            ] 

            View.button [
                prop.position.x.center
                prop.position.y.at 9
                prop.text "Start Spinning"
                button.onClick (fun () -> dispatch StartSpin)
            ] 

            View.button [
                prop.position.x.center
                prop.position.y.at 11
                prop.text "Stop Spinning"
                button.onClick (fun () -> dispatch StopSpin)
            ] 

            View.button [
                prop.position.x.center
                prop.position.y.at 13
                prop.text "Reset"
                button.onClick (fun () -> dispatch Reset)
            ]
        ]
    ]



```

Install via Nuget:

https://www.nuget.org/packages/Terminal.Gui.Elmish

```
dotnet add package Terminal.Gui.Elmish
```

#Referencing the underlying Element

You can reference the underlying element. Also use this to influcence further setting when the element is created!  

```
            View.button [
                prop.position.x.center
                prop.position.y.at 13
                prop.text "Reset"
                button.onClick (fun () -> dispatch Reset)
                prop.ref (fun view -> myButtonRef <- (view :?> Terminal.Gui.Button).xxxx // do your stuff here)
            ]

```

A lot of Thanks to Miguel de Icaza. Nice Project!.
