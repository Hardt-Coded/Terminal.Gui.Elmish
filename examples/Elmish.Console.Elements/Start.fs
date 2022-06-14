module Start

open Terminal.Gui
open Terminal.Gui.Elmish

let view =
    [
        View.label [
            prop.position.x.at 0
            prop.position.y.at 1
            prop.width.fill 1
            prop.textAlignment.centered
            label.text "Welcome to The Elmish Terminal Show"
        ] 

        View.label [
            prop.position.x.at 0
            prop.position.y.at 2
            prop.width.fill 1
            prop.textAlignment.left
            label.text "And Left"
        ] 

        View.label [
            prop.position.x.at 0
            prop.position.y.at 3
            prop.width.fill 1
            prop.textAlignment.right
            label.text "And Right"
        ] 

        View.label [
            prop.position.x.at 0
            prop.position.y.at 4
            prop.width.fill 1
            prop.textAlignment.centered
            label.text "And Centered"
        ] 

        View.label [
            prop.position.x.at 0
            prop.position.y.at 5
            prop.width.fill 1
            prop.textAlignment.justified
            label.text "And some justified text alignment. Lorem ipsum dolor sit amet"
        ] 

        View.label [
            
            prop.position.x.at 0
            prop.position.y.at 9
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.BrightCyan ,Color.Magenta)
            label.text "And Colors"
        ] 

        View.label [
            prop.position.x.at 0
            prop.position.y.at 10
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.BrightGreen,Color.Red)
            label.text "And Colors"

        ] 

        View.label [
            prop.position.x.at 0
            prop.position.y.at 11
            prop.width.fill 1
            prop.textAlignment.centered
            prop.color (Color.Magenta,Color.BrightYellow)
            label.text "And Colors"
        ] 

    ]
    

