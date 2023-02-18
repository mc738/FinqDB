namespace FinqDB.App

[<AutoOpen>]
module Common =

    open System    
    
    let cprintf (color: ConsoleColor) (str: string) =
        Console.ForegroundColor <- color
        Console.WriteLine str
        Console.ResetColor()
        
        
    let printInfo str = cprintf ConsoleColor.White str
    
    let printSuccess str = cprintf ConsoleColor.Green str
    
    let printError str = cprintf ConsoleColor.Red str
    
    let printWarning str = cprintf ConsoleColor.DarkYellow str
    