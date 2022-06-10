namespace Terminal.Gui.Elmish

type IProperty = interface end
type IMenuBarProperty = interface inherit IProperty end
type IMenuProperty = interface inherit IProperty end
type IStyle = interface end

type IMenuBar = interface end
type IMenuBarItem = interface end
type IMenu = interface end

type KeyValue = 
    KeyValue of (string * obj) 
        interface IProperty
        interface IMenuBarProperty
        interface IMenuProperty
        interface IMenuBarItem
        interface IMenuBar
        interface IMenu
        interface IStyle

