module ScrollView


open System.Drawing
open Terminal.Gui
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish

type Model = {
    Text:string 
}


let text = """
Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore 
et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. 
Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, 
consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, 
sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea 
takimata sanctus est Lorem ipsum dolor sit amet.
Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore 
et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. 
Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, 
consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, 
sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea 
takimata sanctus est Lorem ipsum dolor sit amet.
Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore 
et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. 
Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, 
consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, 
sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea 
takimata sanctus est Lorem ipsum dolor sit amet.
Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore 
et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. 
Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, 
consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, 
sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea 
takimata sanctus est Lorem ipsum dolor sit amet.
"""

type Msg =
    | ChangeText of string
    
    

let init () : Model * Cmd<Msg> =
    let model = {
        Text = text           
    }
    model, Cmd.none

    

let update (msg:Msg) (model:Model) =
    match msg with
    | ChangeText txt ->
        {model with Text = txt}, Cmd.none   


let view (model:Model) (dispatch:Msg -> unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.absolute 1
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.BrightYellow, Color.Green)
            label.text "Scrollbars"
        ] 

        

        View.frameView [
            prop.position.x.absolute 1
            prop.position.y.absolute 4
            prop.width.fill 1
            prop.height.fill 5
            prop.title "ScrollView"
            prop.children [
        
                View.scrollView [
                    prop.position.x.absolute 0
                    prop.position.y.absolute 0
                    prop.width.fill 0
                    prop.height.fill 0
                    scrollView.contentOffset(Point(0,0))
                    scrollView.showHorizontalScrollIndicator true
                    scrollView.showVerticalScrollIndicator true
                    prop.children [
                        View.label [
                            prop.position.x.absolute 0
                            prop.position.y.absolute 0
                            prop.width.absolute 120
                            prop.height.absolute 120
                            prop.color (Terminal.Gui.Color.BrightMagenta,Terminal.Gui.Color.Blue)
                            label.text model.Text
                        ]
                    ]
                ]
            ]
        ]
    ]
