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
        prop.title "My Tab View"
        prop.position.x.center
        prop.position.y.absolute 1
        prop.width.absolute 50
        prop.height.absolute 20
        tabView.maxTabTextWidth 15
        tabView.tabs [
            View.tab [
                tab.displayText "Tab 1"
                tab.view <| View.label [
                    prop.position.x.absolute 2
                    prop.position.y.absolute 2
                    label.text "This is the first tab"
                ]
            ]
            View.tab [
                tab.displayText "Tab 2"
                tab.view <| View.label [
                    prop.position.x.absolute 3
                    prop.position.y.absolute 3
                    label.text "This is the second tab"
                ]
            ]
        ]
    ]
]