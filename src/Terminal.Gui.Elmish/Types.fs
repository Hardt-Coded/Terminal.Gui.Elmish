namespace Terminal.Gui.Elmish

type IProperty = interface end
type IStyle = interface end

type KeyValue = 
    KeyValue of (string * obj) 
        interface IProperty
        interface IStyle

