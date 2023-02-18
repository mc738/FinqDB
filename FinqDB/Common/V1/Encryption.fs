namespace FinqDB.Common.V1

open System.IO
open FinqDB.Common.V1.Domain

module Encryption =
    
    let encryptMemoryStream (encryptionType: EncryptionType)  (stream: MemoryStream) =
        match encryptionType with
        | EncryptionType.None -> stream
        | EncryptionType.Aes -> failwith "Aes encryption not yet implemented."
        
