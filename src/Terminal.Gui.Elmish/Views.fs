namespace Terminal.Gui.Elmish

open Terminal.Gui



type View =

    static member inline page (props:IProperty list) = PageElement(props) :> TerminalElement
    static member inline page (children:TerminalElement list) = 
        let props = [ prop.children children ]
        PageElement(props) :> TerminalElement

    static member inline window (props:IProperty list) = WindowElement(props) :> TerminalElement
    static member inline window (children:TerminalElement list) = 
        let props = [ prop.children children ]
        WindowElement(props) :> TerminalElement

    static member inline label (props:IProperty list) = LabelElement(props) :> TerminalElement
    static member inline label (children:TerminalElement list) = 
        let props = [ prop.children children ]
        LabelElement(props) :> TerminalElement

    static member inline button (props:IProperty list) = ButtonElement(props) :> TerminalElement
    static member inline button (children:TerminalElement list) = 
        let props = [ prop.children children ]
        ButtonElement(props) :> TerminalElement





