namespace FinqDB.Common.V1

open System

module Domain =
    
    
    /// <summary>
    /// A id. This is mainly for internal use allowing a caller to specific if an id should be user defined or generated.
    /// This is only used for inputting new entities, on retrieval all id's are treated as strings.
    /// </summary>
    type Id =
        | UserDefined of string
        | Generated

        /// <summary>
        /// Get the id. Be warned, this will generate a new id is type is Id.Generated each time it is called.
        /// </summary>
        member id.Get() =
            match id with
            | UserDefined str -> str
            | Generated -> Guid.NewGuid().ToString("n")
    
    [<RequireQualifiedAccess>]
    type EncryptionType =
        | None
        | Aes

        static member TryDeserialize(str: string) =
            match str.ToLower() with
            | "none" -> Some EncryptionType.None
            | "aes" -> Some EncryptionType.Aes
            | _ -> Option.None

        static member Deserialize(str: string) =
            EncryptionType.TryDeserialize str |> Option.defaultValue EncryptionType.None

        static member All() =
            [ EncryptionType.None; EncryptionType.Aes ]

        member et.Serialize() =
            match et with
            | EncryptionType.None -> "none"
            | EncryptionType.Aes -> "aes"

    [<RequireQualifiedAccess>]
    type CompressionType =
        | None
        | GZip

        static member TryDeserialize(str: string) =
            match str.ToLower() with
            | "none" -> Some CompressionType.None
            | "gzip" -> Some CompressionType.GZip
            | _ -> Option.None

        static member Deserialize(str: string) =
            EncryptionType.TryDeserialize str |> Option.defaultValue EncryptionType.None

        static member All() =
            [ CompressionType.None; CompressionType.GZip ]

        member ct.Serialize() =
            match ct with
            | CompressionType.None -> "none"
            | CompressionType.GZip -> "gzip"


    [<RequireQualifiedAccess>]
    type FileType =
        // General
        | Binary
        | Text
        // Data
        | Json
        | Xml
        | Csv
        // Documents
        | Pdf
        // Web
        | Html
        | Css
        | JavaScript
        // Audio
        | Mp3
        | Wma
        | RealAudio
        | Wav
        // Images
        | Gif
        | Jpeg
        | Png
        | Tiff
        | Svg
        | WebP
        // Video
        | Mpeg
        | Mp4
        | QuickTime
        | Wmv
        | WebM


        static member TryDeserialize(str: string) =
            match str.ToLower() with
            | "bin"
            | "exe"
            | "binary" -> Some FileType.Binary
            | "text" -> Some FileType.Text
            | "json" -> Some FileType.Json
            | "xml" -> Some FileType.Xml
            | "csv" -> Some FileType.Csv
            | "pdf" -> Some FileType.Pdf
            | "html" -> Some FileType.Html
            | "css" -> Some FileType.Css
            | "js"
            | "javascript" -> Some FileType.JavaScript
            | "mp3" -> Some FileType.Mp3
            | "wma" -> Some FileType.Wma
            | "realaudio" -> Some FileType.RealAudio
            | "wav" -> Some FileType.Wav
            | "gif" -> Some FileType.Gif
            | "jpeg" -> Some FileType.Jpeg
            | "png" -> Some FileType.Png
            | "tiff" -> Some FileType.Tiff
            | "svg" -> Some FileType.Svg
            | "webp" -> Some FileType.WebP
            | "mpeg" -> Some FileType.Mpeg
            | "mp4" -> Some FileType.Mp4
            | "quicktime" -> Some FileType.QuickTime
            | "wmv" -> Some FileType.Wmv
            | "webm" -> Some FileType.WebM
            | _ -> None

        static member Deserialize(str: string) =
            FileType.TryDeserialize str |> Option.defaultValue FileType.Binary

        static member All() =
            [ FileType.Binary
              FileType.Text
              FileType.Json
              FileType.Xml
              FileType.Csv
              FileType.Pdf
              FileType.Html
              FileType.Css
              FileType.JavaScript
              FileType.Mp3
              FileType.Wma
              FileType.RealAudio
              FileType.Wav
              FileType.Gif
              FileType.Jpeg
              FileType.Png
              FileType.Tiff
              FileType.Svg
              FileType.WebP
              FileType.Mpeg
              FileType.Mp4
              FileType.QuickTime
              FileType.Wmv
              FileType.WebM ]

        member ft.Serialize() =
            match ft with
            | Binary -> "binary"
            | Text -> "text"
            | Json -> "json"
            | Xml -> "xml"
            | Csv -> "csv"
            | Pdf -> "pdf"
            | Html -> "html"
            | Css -> "css"
            | JavaScript -> "javascript"
            | Mp3 -> "mp3"     
            | Wma -> "wma"
            | RealAudio -> "realaudio"
            | Wav -> "wav"
            | Gif -> "gif" 
            | Jpeg -> "jpeg"
            | Png -> "png"
            | Tiff -> "tiff"
            | Svg -> "svg"
            | WebP -> "webp"
            | Mpeg -> "mpeg"    
            | Mp4 -> "mp4"
            | QuickTime -> "quicktime"
            | Wmv -> "wmv"
            | WebM -> "webm"

        member ft.GetExtension() =
            match ft with
            | Binary -> ".bin"
            | Text -> ".txt"
            | Json -> ".json"
            | Xml -> ".xml"
            | Csv -> ".csv"
            | Pdf -> ".pdf"
            | Html -> ".html"
            | Css -> ".css"
            | JavaScript -> ".js"
            | Mp3 -> ".mp3"     
            | Wma -> ".wma"
            | RealAudio -> "ra"
            | Wav -> ".wav"
            | Gif -> ".gif" 
            | Jpeg -> ".jpeg"
            | Png -> ".png"
            | Tiff -> ".tiff"
            | Svg -> ".svg"
            | WebP -> ".webp"            
            | Mpeg -> ".mpeg"    
            | Mp4 -> ".mp4"
            | QuickTime -> ".quicktime"
            | Wmv -> ".wmv"
            | WebM -> ".webm"

        member ft.GetContentType() =
            match ft with
            | Binary -> "application/octet-stream"
            | Text -> "text/plain"
            | Json -> "application/json"
            | Xml -> "application/xml"
            | Csv -> "text/csv"
            | Pdf -> "application/pdf"
            | Html -> "text/html"
            | Css -> "text/css"
            | JavaScript -> "application/javascript"
            | Mp3 -> "audio/mpeg"     
            | Wma -> "audio/x-ms-wma"
            | RealAudio -> "audio/vnd.rn-realaudio"
            | Wav -> "audio/x-wav"
            | Gif -> "image/gif" 
            | Jpeg -> "image/jpeg"
            | Png -> "image/png"
            | Tiff -> "image/tiff"
            | Svg -> "image/svg+xml"
            | WebP -> "image/webp"
            | Mpeg -> "video/mpeg"
            | Mp4 -> "video/mp4"
            | QuickTime -> "video/quicktime"
            | Wmv -> "video/x-ms-wmv"
            | WebM -> "video/webm"
    
    type Node = {
        Id: string
        Name: string
        Edges: NodeEdge list option
        CreatedOn: DateTime
        Metadata: Map<string, string>
        Files: File list
        Documents: Document list 
    }
    
    and NodeEdge = {
        Id: string
        ToNode: Node
        Metadata: Map<string, string>
        Files: File list
        Documents: Document list
    }
    
    and File = {
        Id: string
        Name: string
    }
    
    and Document = {
        Id: string
        Name: string
    }