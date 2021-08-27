namespace Terminal.Gui.Elmish

open Terminal.Gui

type ViewElementType =
    | PageElement
    | WindowElement
    | LabelElement
    | TextFieldElement
    | ButtonElement
    | TimeFieldElement
    | DateFieldElement
    | TextViewElement
    | FrameViewElement
    | HexViewElement
    | ListViewElement
    | ProgressBarElement
    | CheckBoxElement
    | RadioGroupElement
    | ScrollViewElement

type IProp = interface end

type IProp<'a> = 
    inherit IProp


type ViewElement = 
    {
        Type: ViewElementType
        Element: View option
        Props:IProp list
        Children: ViewElement list
    }

