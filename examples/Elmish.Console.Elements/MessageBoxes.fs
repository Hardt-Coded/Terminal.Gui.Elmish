module MessageBoxes



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
    | ShowWizard
    | Reset

let init () : Model * Cmd<Msg> =
    let model = {
        Counter = 0
    }
    model, Cmd.none

let incrementCmd () =
    Cmd.OfFunc.perform (fun () ->
        let result = Dialogs.messageBox 60 10 "Increment" "How many do you want to Increment?" ["+10";"+5";"+1";"None"]
        match result with
        | "+10" -> 10
        | "+5" -> 5
        | "+1" -> 1
        | _ -> 0
    ) () Incremented

let decrementCmd () =
    Cmd.OfFunc.perform (fun () ->
        let result = Dialogs.errorBox 60 10 "Decrement" "How many do you want to Decrement?" ["-10";"-5";"-1";"None"]
        match result with
        | "-10" -> 10
        | "-5" -> 5
        | "-1" -> 1
        | _ -> 0
    ) () Decremented

let showWizardCmd =
    fun dispatch ->
        let wizard = new Wizard(Title=($"Setup Wizard"))
        // Add 1st step
        let firstStep = new WizardStep (Title="End User License Agreement")
        wizard.AddStep(firstStep);
        firstStep.NextButtonText <- "Accept!"
        firstStep.HelpText <- "This is the End User License Agreement."
        
        // Add 2nd step
        let secondStep = new WizardStep (Title="Second Step")
        wizard.AddStep(secondStep)
        secondStep.HelpText <- "This is the help text for the Second Step."
        let lbl = new Label (Text="Enter Number to Increase the Counter:", Width=Dim.Auto())
        secondStep.Add(lbl) |> ignore
        
        let number = new TextField (X = Pos.Right(lbl) + Pos.Absolute(1), Width = Dim.Fill() - Dim.Fill(1))
        secondStep.Add(number) |> ignore
        
        wizard.Finished.Add(
            fun ev ->
                match System.Int32.TryParse(number.Text.ToString()) with
                | false, _ -> 
                    MessageBox.Query("Wizard", $"Error. '{number.Text}' is not a number", "Ok") |> ignore
                    ev.Cancel <- true
                | true, num ->
                    MessageBox.Query("Wizard", $"Finished. The Number you entered is '{num}' and will be added to the counter!", "Ok") |> ignore
                    dispatch (Incremented num)
                
        )
 
        Dialogs.showWizard wizard
        
    |> Cmd.ofSub


let update (msg:Msg) (model:Model) =
    match msg with
    | Increment ->
        model, incrementCmd()
    | Decrement ->
        model, decrementCmd()
    | Incremented i ->
        {model with Counter = model.Counter + i}, Cmd.none
    | Decremented i ->
        {model with Counter = model.Counter - i}, Cmd.none
    | Reset ->
        {model with Counter = 0}, Cmd.none
    | ShowWizard ->
        model, showWizardCmd


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
            button.text "Up"
            prop.accept (fun ev -> dispatch Increment)
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
            button.text "Down"
            prop.accept (fun ev -> dispatch Decrement)
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.absolute 11
            button.text "Show Wizard"
            prop.accept (fun ev -> dispatch ShowWizard)
        ] 
    ]

