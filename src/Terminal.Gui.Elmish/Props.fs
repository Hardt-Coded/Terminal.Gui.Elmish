namespace Terminal.Gui.Elmish

open Terminal.Gui


type style =
    static member inline color (colorscheme:ColorScheme) = Interop.mkstyle "color" colorscheme


type prop =
    static member inline style (children:IStyle list) = Interop.mkprop "style" children
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children

module prop =

    module position =

        type x =
            static member inline at (i:int) = Interop.mkprop "x" (Pos.At i)
            static member inline bottom = Interop.mkprop "x" Pos.Bottom
            static member inline center = Interop.mkprop "x" Pos.Center
            static member inline percent (i:double) = Interop.mkprop "x" (Pos.Percent (i |> float32))

        type y =
            static member inline at (i:int) = Interop.mkprop "y" (Pos.At i)
            static member inline bottom = Interop.mkprop "y" Pos.Bottom
            static member inline center = Interop.mkprop "y" Pos.Center
            static member inline percent (i:double) = Interop.mkprop "y" (Pos.Percent (i |> float32))

    
    type width =
        static member inline sized (i:int) = Interop.mkprop "width" (Dim.Sized i)
        static member inline filled = Interop.mkprop "width" (Dim.Fill 0)
        static member inline fill (margin:int) = Interop.mkprop "width" (Dim.Fill margin)

    type height =
        static member inline sized (i:int) = Interop.mkprop "height" (Dim.Sized i)
        static member inline filled = Interop.mkprop "height" (Dim.Fill 0)
        static member inline fill (margin:int) = Interop.mkprop "height" (Dim.Fill margin)









type window =
    static member inline title (p:string) = Interop.mkprop "title" p

