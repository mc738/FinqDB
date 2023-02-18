namespace FinqDB.Common


[<AutoOpen>]
module Extensions =

    open System
    open System.IO
    open System.Text
    open System.Security.Cryptography
    open ToolBox.Core
        
    type MemoryStream with

        member ms.GetSHA256Hash() = Hashing.hashStream (SHA256.Create()) ms
        

    type String with
        
        member str.ToUtf8Bytes() =
            Encoding.UTF8.GetBytes str