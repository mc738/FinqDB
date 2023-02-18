namespace FinqDB.Common.V1

open System.IO
open Domain

module Compression =
    
    
    let compressMemoryStream (compressionType: CompressionType) (stream: MemoryStream)  =
        match compressionType with
        | CompressionType.None -> stream
        | CompressionType.GZip -> failwith "GZip compression not implemented yet"
        
    

