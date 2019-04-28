module Start

open Terminal.Gui
open Terminal.Gui.Elmish

let view : View list =
    [
        label [
            Styles [
                Pos (AbsPos 0,AbsPos 1)
                Dim (FillMargin 1,AbsDim 1)
                TextAlignment Centered
                
            ]
            
            Text "Welcome to The Elmish Terminal Show"
        ] 

        label [
            Styles [
                Pos (AbsPos 0,AbsPos 2)
                Dim (FillMargin 1,AbsDim 1)
                TextAlignment Left                
            ]
            
            Text "And Left"
        ] 

        label [
            Styles [
                Pos (AbsPos 0,AbsPos 3)
                Dim (FillMargin 1,AbsDim 1)
                TextAlignment Right
                
            ]
            
            Text "And Right"
        ] 

        label [
            Styles [
                Pos (AbsPos 0,AbsPos 4)
                Dim (FillMargin 1,AbsDim 1)
                TextAlignment Centered
                
            ]
            
            Text "And Centered"
        ] 

        label [
            Styles [
                Pos (AbsPos 0,AbsPos 5)
                Dim (FillMargin 1,AbsDim 3)
                TextAlignment Justified
                
            ]
            
            Text "And some justified text alignment. Lorem ipsum dolor sit amet"
        ] 

        label [
            Styles [
                Pos (AbsPos 0,AbsPos 9)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Color.BrighCyan,Color.Magenta)
            ]
            
            Text "And Colors"
        ] 

        label [
            Styles [
                Pos (AbsPos 0,AbsPos 10)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Color.BrightGreen,Color.Red)
            ]
            
            Text "And Colors"
        ] 

        label [
            Styles [
                Pos (AbsPos 0,AbsPos 11)
                Dim (Fill,AbsDim 1)
                TextAlignment Centered
                Colors (Color.Magenta,Color.BrightYellow)
                
            ]
            
            Text "And Colors"
        ] 

    ]
    

