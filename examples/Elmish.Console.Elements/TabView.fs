module TabView

open Terminal.Gui
open Terminal.Gui.Elmish

let view = [
    View.label [
        prop.position.x.at 0
        prop.position.y.at 0
        label.text "Tab view example"
    ]
    View.tabView [
        prop.position.x.center
        prop.position.y.at 1
        prop.width.filled
        tabView.tabs [
            tab.create [
                tabItem.title "Tab 1"
                tabItem.view <| View.label [
                    prop.position.x.at 0
                    prop.position.y.at 0
                    label.text "This is the first tab"
                ]
            ]
            tab.create [
                tabItem.title "Tab 1"
                tabItem.view <| View.label [
                    prop.position.x.at 0
                    prop.position.y.at 0
                    label.text "This is the second tab"
                ]
            ]
        ]
    ]
]