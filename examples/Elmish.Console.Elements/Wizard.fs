module Wizard



open Terminal.Gui
open Terminal.Gui.Elmish

type Model = {
    Counter:int
}

type Msg =
    | Increment
    | Decrement
    | Incremented of int
    | Decremented of int
    | Reset

let init () : Model * Cmd<Msg> =
    let model = {
        Counter = 0
    }
    model, Cmd.none


let showWizardCmd case =
    fun dispatch ->
        let wizard = new Wizard (Title="Setup Wizard")
        
        // Add 1st step
        let firstStep = new WizardStep (Title="End User License Agreement")
        wizard.AddStep(firstStep);
        firstStep.NextButtonText <- "Accept!"
        firstStep.HelpText <- "This is the End User License Agreement."
        
        // Add 2nd step
        let secondStep = new WizardStep (Title="Second Step")
        wizard.AddStep(secondStep)
        secondStep.HelpText <- "This is the help text for the Second Step."
        let lbl = new Label (Text="Enter Number to Change the Counter:", Width=Dim.Auto())
        secondStep.Add(lbl) |> ignore
        
        let number = new TextField (X = Pos.Right(lbl) + Pos.Absolute(1), Width = Dim.Fill(1))
        number.ColorScheme <- Application.Top.ColorScheme
        secondStep.Add(number) |> ignore
        
        wizard.Finished.Add(
            fun ev ->
                match System.Int32.TryParse(number.Text.ToString()) with
                | false, _ -> 
                    MessageBox.Query("Wizard", $"Error. '{number.Text}' is not a number", "Ok") |> ignore
                    ev.Cancel <- true
                | true, num ->
                    MessageBox.Query("Wizard", $"Finished. The Number you entered is '{num}'!", "Ok") |> ignore
                    dispatch (case num)
                
        )
 
        Dialogs.showWizard wizard
        
    |> Cmd.ofSub


let update (msg:Msg) (model:Model) =
    match msg with
    | Increment ->
        model, showWizardCmd Incremented
    | Decrement ->
        model, showWizardCmd Decremented
    | Incremented i ->
        {model with Counter = model.Counter + i}, Cmd.none
    | Decremented i ->
        {model with Counter = model.Counter - i}, Cmd.none
    | Reset ->
        {model with Counter = 0}, Cmd.none
    


let view (model:Model) (dispatch:Msg->unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.absolute 1
            prop.width.fill 1
            prop.alignment.center
            prop.color (Color.BrightYellow, Color.Green)
            label.text "Counter with Message Boxes!"
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.absolute 5
            button.text "Wizard Up"
            button.onAccept (fun () -> dispatch Increment)
        ] 

        View.label [
            prop.position.x.center
            prop.position.y.absolute 6
            prop.alignment.center
            prop.color (Color.Magenta, Color.BrightYellow)
            label.text $"The Count of 'Fancyness' is {model.Counter}"
        ] 


        View.button [
            prop.position.x.center
            prop.position.y.absolute 7
            button.text "Wizard Down"
            button.onAccept (fun () -> dispatch Decrement)
        ] 

        
    ]

