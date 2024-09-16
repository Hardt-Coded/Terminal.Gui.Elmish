module TabView

open Terminal.Gui
open Terminal.Gui.Elmish

let view = [
    View.label [
        prop.position.x.center
        prop.position.y.absolute 0
        label.text "Tab view example"
    ]
    View.tabView [
        prop.position.x.center
        prop.position.y.absolute 1
        prop.width.fill 50
        prop.height.fill 100
        prop.children [
            (*tab.create [
                tabItem.title "Tab 1"
                tabItem.view <| View.label [
                    prop.position.x.absolute 0
                    prop.position.y.absolute 0
                    label.text "This is the first tab"
                ]
            ]
            tab.create [
                tabItem.title "Tab 1"
                tabItem.view <| View.label [
                    prop.position.x.absolute 0
                    prop.position.y.absolute 0
                    label.text "This is the second tab"
                ]
            ]*)
        ]
    ]
]