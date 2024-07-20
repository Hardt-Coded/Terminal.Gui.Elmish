module Start

open Terminal.Gui
open Terminal.Gui.Elmish

let view =
    [
        View.label [
            prop.position.x.absolute 0
            prop.position.y.absolute 1
            prop.width.fill 1
            prop.alignment.center
            label.text "Welcome to The Elmish Terminal Show"
        ] 

        View.label [
            prop.position.x.absolute 0
            prop.position.y.absolute 2
            prop.width.fill 1
            prop.alignment.start
            label.text "And Left"
        ] 

        View.label [
            prop.position.x.absolute 0
            prop.position.y.absolute 3
            prop.width.fill 1
            prop.alignment.``end``
            label.text "And Right"
        ] 

        View.label [
            prop.position.x.absolute 0
            prop.position.y.absolute 4
            prop.width.fill 1
            prop.alignment.center
            label.text "And Centered"
        ] 

        View.label [
            prop.position.x.absolute 0
            prop.position.y.absolute 5
            prop.width.fill 1
            prop.alignment.fill
            label.text "And some justified text alignment. Lorem ipsum dolor sit amet"
        ] 

        View.label [
            
            prop.position.x.absolute 0
            prop.position.y.absolute 9
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.BrightCyan ,Color.Magenta)
            label.text "And Colors"
        ] 

        View.label [
            prop.position.x.absolute 0
            prop.position.y.absolute 10
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.BrightGreen,Color.Red)
            label.text "And Colors"

        ] 

        View.label [
            prop.position.x.absolute 0
            prop.position.y.absolute 11
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.Magenta,Color.BrightYellow)
            label.text "And Colors"
        ] 

    ]
    

