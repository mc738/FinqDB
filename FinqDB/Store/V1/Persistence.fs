namespace FinqDB.Store.V1.Persistence

open System
open System.Text.Json.Serialization
open Freql.Core.Common
open Freql.Sqlite

/// Module generated on 13/02/2023 19:29:12 (utc) via Freql.Sqlite.Tools.
[<RequireQualifiedAccess>]
module Records =
    /// A record representing a row in the table `categories`.
    type Category =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE categories (
	name TEXT NOT NULL,
	CONSTRAINT categories_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              categories.`name`
        FROM categories
        """
    
        static member TableName() = "categories"
    
    /// A record representing a row in the table `compression_types`.
    type CompressionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE compression_types (
	name TEXT NOT NULL,
	CONSTRAINT compression_types_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              compression_types.`name`
        FROM compression_types
        """
    
        static member TableName() = "compression_types"
    
    /// A record representing a row in the table `document_metadata`.
    type DocumentMetadataItem =
        { [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { DocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE document_metadata (
	document_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT document_metadata_PK PRIMARY KEY (document_id,item_key),
	CONSTRAINT document_metadata_FK FOREIGN KEY (document_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_metadata.`document_id`,
              document_metadata.`item_key`,
              document_metadata.`item_value`
        FROM document_metadata
        """
    
        static member TableName() = "document_metadata"
    
    /// A record representing a row in the table `document_version_metadata`.
    type DocumentVersionMetadataItem =
        { [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { DocumentVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE document_version_metadata (
	document_version_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT document_version_metadata_PK PRIMARY KEY (document_version_id,item_key),
	CONSTRAINT document_version_metadata_FK FOREIGN KEY (document_version_id) REFERENCES document_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_version_metadata.`document_version_id`,
              document_version_metadata.`item_key`,
              document_version_metadata.`item_value`
        FROM document_version_metadata
        """
    
        static member TableName() = "document_version_metadata"
    
    /// A record representing a row in the table `document_versions`.
    type DocumentVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("documentType")>] DocumentType: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              DocumentId = String.Empty
              Version = 0
              RawBlob = BlobField.Empty()
              Hash = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              DocumentType = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE document_versions (
	id TEXT NOT NULL,
	document_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	raw_blob BLOB NOT NULL,
	hash TEXT NOT NULL,
	encryption_type TEXT NOT NULL,
	compression_type TEXT NOT NULL,
	document_type TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT document_versions_PK PRIMARY KEY (id),
	CONSTRAINT document_versions_UN UNIQUE (document_id,version),
	CONSTRAINT document_versions_FK FOREIGN KEY (document_id) REFERENCES documents(id),
	CONSTRAINT document_versions_FK_1 FOREIGN KEY (encryption_type) REFERENCES encryption_types(name),
	CONSTRAINT document_versions_FK_2 FOREIGN KEY (compression_type) REFERENCES compression_types(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              document_versions.`id`,
              document_versions.`document_id`,
              document_versions.`version`,
              document_versions.`raw_blob`,
              document_versions.`hash`,
              document_versions.`encryption_type`,
              document_versions.`compression_type`,
              document_versions.`document_type`,
              document_versions.`created_on`
        FROM document_versions
        """
    
        static member TableName() = "document_versions"
    
    /// A record representing a row in the table `documents`.
    type Document =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE documents (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT documents_PK PRIMARY KEY (id)
)
        """
    
        static member SelectSql() = """
        SELECT
              documents.`id`,
              documents.`name`,
              documents.`created_on`
        FROM documents
        """
    
        static member TableName() = "documents"
    
    /// A record representing a row in the table `edge_documents`.
    type EdgeDocument =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("documentId")>] DocumentId: string }
    
        static member Blank() =
            { EdgeId = String.Empty
              DocumentId = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE edge_documents (
	edge_id TEXT NOT NULL,
	document_id TEXT NOT NULL,
	CONSTRAINT edge_documents_PK PRIMARY KEY (edge_id,document_id),
	CONSTRAINT edge_documents_FK FOREIGN KEY (edge_id) REFERENCES edges(id),
	CONSTRAINT edge_documents_FK_1 FOREIGN KEY (document_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              edge_documents.`edge_id`,
              edge_documents.`document_id`
        FROM edge_documents
        """
    
        static member TableName() = "edge_documents"
    
    /// A record representing a row in the table `edge_files`.
    type EdgeFile =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("fileId")>] FileId: string }
    
        static member Blank() =
            { EdgeId = String.Empty
              FileId = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE edge_files (
	edge_id TEXT NOT NULL,
	file_id TEXT NOT NULL,
	CONSTRAINT edge_files_PK PRIMARY KEY (edge_id,file_id),
	CONSTRAINT edge_files_FK FOREIGN KEY (edge_id) REFERENCES edges(id),
	CONSTRAINT edge_files_FK_1 FOREIGN KEY (file_id) REFERENCES files(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              edge_files.`edge_id`,
              edge_files.`file_id`
        FROM edge_files
        """
    
        static member TableName() = "edge_files"
    
    /// A record representing a row in the table `edge_metadata`.
    type EdgeMetadataItem =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { EdgeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE edge_metadata (
	edge_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT edge_metadata_PK PRIMARY KEY (edge_id,item_key),
	CONSTRAINT edge_metadata_FK FOREIGN KEY (edge_id) REFERENCES edges(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              edge_metadata.`edge_id`,
              edge_metadata.`item_key`,
              edge_metadata.`item_value`
        FROM edge_metadata
        """
    
        static member TableName() = "edge_metadata"
    
    /// A record representing a row in the table `edge_properties`.
    type EdgeProperties =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("jsonBlob")>] JsonBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { EdgeId = String.Empty
              Version = 0
              JsonBlob = BlobField.Empty()
              Hash = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE edge_properties (
	edge_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	json_blob BLOB NOT NULL,
	hash TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT edge_properties_PK PRIMARY KEY (edge_id,version),
	CONSTRAINT edge_properties_FK FOREIGN KEY (edge_id) REFERENCES edges(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              edge_properties.`edge_id`,
              edge_properties.`version`,
              edge_properties.`json_blob`,
              edge_properties.`hash`,
              edge_properties.`created_on`
        FROM edge_properties
        """
    
        static member TableName() = "edge_properties"
    
    /// A record representing a row in the table `edge_weights`.
    type EdgeWeight =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("category")>] Category: string
          [<JsonPropertyName("weight")>] Weight: decimal }
    
        static member Blank() =
            { EdgeId = String.Empty
              Category = String.Empty
              Weight = 0m }
    
        static member CreateTableSql() = """
        CREATE TABLE edge_weights (
	edge_id TEXT NOT NULL,
	category TEXT NOT NULL,
	weight REAL NOT NULL,
	CONSTRAINT edge_weights_PK PRIMARY KEY (edge_id,category),
	CONSTRAINT edge_weights_FK FOREIGN KEY (edge_id) REFERENCES edges(id),
	CONSTRAINT edge_weights_FK_1 FOREIGN KEY (category) REFERENCES categories(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              edge_weights.`edge_id`,
              edge_weights.`category`,
              edge_weights.`weight`
        FROM edge_weights
        """
    
        static member TableName() = "edge_weights"
    
    /// A record representing a row in the table `edges`.
    type Edge =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("fromNode")>] FromNode: string
          [<JsonPropertyName("toNode")>] ToNode: string
          [<JsonPropertyName("bidirectional")>] Bidirectional: bool
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              FromNode = String.Empty
              ToNode = String.Empty
              Bidirectional = true
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE edges (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	from_node TEXT NOT NULL,
	to_node TEXT NOT NULL,
	bidirectional INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT edges_PK PRIMARY KEY (id),
	CONSTRAINT edges_FK FOREIGN KEY (from_node) REFERENCES nodes(id),
	CONSTRAINT edges_FK_1 FOREIGN KEY (to_node) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              edges.`id`,
              edges.`name`,
              edges.`from_node`,
              edges.`to_node`,
              edges.`bidirectional`,
              edges.`created_on`
        FROM edges
        """
    
        static member TableName() = "edges"
    
    /// A record representing a row in the table `encryption_types`.
    type EncryptionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE encryption_types (
	name TEXT NOT NULL,
	CONSTRAINT encryption_types_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              encryption_types.`name`
        FROM encryption_types
        """
    
        static member TableName() = "encryption_types"
    
    /// A record representing a row in the table `external_edge_documents`.
    type ExternalEdgeDocument =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("documentId")>] DocumentId: string }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              DocumentId = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE external_edge_documents (
	external_edge_id TEXT NOT NULL,
	document_id TEXT NOT NULL,
	CONSTRAINT external_edge_documents_PK PRIMARY KEY (external_edge_id,document_id),
	CONSTRAINT external_edge_documents_FK FOREIGN KEY (external_edge_id) REFERENCES external_edges(id),
	CONSTRAINT external_edge_documents_FK_1 FOREIGN KEY (document_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_edge_documents.`external_edge_id`,
              external_edge_documents.`document_id`
        FROM external_edge_documents
        """
    
        static member TableName() = "external_edge_documents"
    
    /// A record representing a row in the table `external_edge_files`.
    type ExternalEdgeFile =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("fileId")>] FileId: string }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              FileId = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE external_edge_files (
	external_edge_id TEXT NOT NULL,
	file_id TEXT NOT NULL,
	CONSTRAINT external_edge_files_PK PRIMARY KEY (external_edge_id,file_id),
	CONSTRAINT external_edge_files_FK FOREIGN KEY (external_edge_id) REFERENCES external_edges(id),
	CONSTRAINT external_edge_files_FK_1 FOREIGN KEY (file_id) REFERENCES files(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_edge_files.`external_edge_id`,
              external_edge_files.`file_id`
        FROM external_edge_files
        """
    
        static member TableName() = "external_edge_files"
    
    /// A record representing a row in the table `external_edge_metadata`.
    type ExternalEdgeMetadataItem =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE external_edge_metadata (
	external_edge_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT external_edge_metadata_PK PRIMARY KEY (external_edge_id,item_key),
	CONSTRAINT external_edge_metadata_FK FOREIGN KEY (external_edge_id) REFERENCES external_edges(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_edge_metadata.`external_edge_id`,
              external_edge_metadata.`item_key`,
              external_edge_metadata.`item_value`
        FROM external_edge_metadata
        """
    
        static member TableName() = "external_edge_metadata"
    
    /// A record representing a row in the table `external_edge_properties`.
    type ExternalEdgeProperties =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("jsonBlob")>] JsonBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              Version = 0
              JsonBlob = BlobField.Empty()
              Hash = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE external_edge_properties (
	external_edge_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	json_blob BLOB NOT NULL,
	hash TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT external_edge_properties_PK PRIMARY KEY (external_edge_id,version),
	CONSTRAINT external_edge_properties_FK FOREIGN KEY (external_edge_id) REFERENCES external_edges(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_edge_properties.`external_edge_id`,
              external_edge_properties.`version`,
              external_edge_properties.`json_blob`,
              external_edge_properties.`hash`,
              external_edge_properties.`created_on`
        FROM external_edge_properties
        """
    
        static member TableName() = "external_edge_properties"
    
    /// A record representing a row in the table `external_edges`.
    type ExternalEdge =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("bidirectional")>] Bidirectional: bool
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              NodeId = String.Empty
              Bidirectional = true
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE external_edges (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	node_id TEXT NOT NULL,
	bidirectional INTEGER NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT external_edges_PK PRIMARY KEY (id),
	CONSTRAINT external_edges_FK FOREIGN KEY (node_id) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              external_edges.`id`,
              external_edges.`name`,
              external_edges.`node_id`,
              external_edges.`bidirectional`,
              external_edges.`created_on`
        FROM external_edges
        """
    
        static member TableName() = "external_edges"
    
    /// A record representing a row in the table `file_metadata`.
    type FileMetadataItem =
        { [<JsonPropertyName("fileId")>] FileId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { FileId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE file_metadata (
	file_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT file_metadata_PK PRIMARY KEY (item_key,file_id),
	CONSTRAINT file_metadata_FK FOREIGN KEY (file_id) REFERENCES files(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              file_metadata.`file_id`,
              file_metadata.`item_key`,
              file_metadata.`item_value`
        FROM file_metadata
        """
    
        static member TableName() = "file_metadata"
    
    /// A record representing a row in the table `file_types`.
    type FileType =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("extension")>] Extension: string
          [<JsonPropertyName("contentType")>] ContentType: string }
    
        static member Blank() =
            { Name = String.Empty
              Extension = String.Empty
              ContentType = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE file_types (
	name TEXT NOT NULL,
	extension TEXT NOT NULL,
	content_type TEXT NOT NULL,
	CONSTRAINT file_types_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              file_types.`name`,
              file_types.`extension`,
              file_types.`content_type`
        FROM file_types
        """
    
        static member TableName() = "file_types"
    
    /// A record representing a row in the table `file_version_metadata`.
    type FileVersionMetadataItem =
        { [<JsonPropertyName("fileVersionId")>] FileVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { FileVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE file_version_metadata (
	file_version_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT file_version_metadata_PK PRIMARY KEY (file_version_id,item_key),
	CONSTRAINT file_version_metadata_FK FOREIGN KEY (file_version_id) REFERENCES file_versions(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              file_version_metadata.`file_version_id`,
              file_version_metadata.`item_key`,
              file_version_metadata.`item_value`
        FROM file_version_metadata
        """
    
        static member TableName() = "file_version_metadata"
    
    /// A record representing a row in the table `file_versions`.
    type FileVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("fileId")>] FileId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              FileId = String.Empty
              Version = 0
              RawBlob = BlobField.Empty()
              Hash = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE file_versions (
	id TEXT NOT NULL,
	file_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	raw_blob BLOB NOT NULL,
	hash TEXT NOT NULL,
	encryption_type TEXT NOT NULL,
	compression_type TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT file_versions_PK PRIMARY KEY (id),
	CONSTRAINT file_versions_UN UNIQUE (file_id,version),
	CONSTRAINT file_versions_FK FOREIGN KEY (file_id) REFERENCES files(id),
	CONSTRAINT file_versions_FK_1 FOREIGN KEY (encryption_type) REFERENCES encryption_types(name),
	CONSTRAINT file_versions_FK_2 FOREIGN KEY (compression_type) REFERENCES compression_types(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              file_versions.`id`,
              file_versions.`file_id`,
              file_versions.`version`,
              file_versions.`raw_blob`,
              file_versions.`hash`,
              file_versions.`encryption_type`,
              file_versions.`compression_type`,
              file_versions.`created_on`
        FROM file_versions
        """
    
        static member TableName() = "file_versions"
    
    /// A record representing a row in the table `files`.
    type File =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("fileType")>] FileType: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              FileType = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE files (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	file_type TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT files_PK PRIMARY KEY (id),
	CONSTRAINT files_FK FOREIGN KEY (file_type) REFERENCES file_types(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              files.`id`,
              files.`name`,
              files.`file_type`,
              files.`created_on`
        FROM files
        """
    
        static member TableName() = "files"
    
    /// A record representing a row in the table `finq_info`.
    type FinqInfoItem =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("description")>] Description: string
          [<JsonPropertyName("isReadOnly")>] IsReadOnly: bool
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              Description = String.Empty
              IsReadOnly = true
              Version = 0
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE finq_info (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	description TEXT NOT NULL,
	is_read_only INTEGER NOT NULL,
	version INTEGER NOT NULL,
	created_on TEXT NOT NULL
)
        """
    
        static member SelectSql() = """
        SELECT
              finq_info.`id`,
              finq_info.`name`,
              finq_info.`description`,
              finq_info.`is_read_only`,
              finq_info.`version`,
              finq_info.`created_on`
        FROM finq_info
        """
    
        static member TableName() = "finq_info"
    
    /// A record representing a row in the table `labels`.
    type Label =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE labels (
	name TEXT NOT NULL,
	CONSTRAINT labels_PK PRIMARY KEY (name)
)
        """
    
        static member SelectSql() = """
        SELECT
              labels.`name`
        FROM labels
        """
    
        static member TableName() = "labels"
    
    /// A record representing a row in the table `metadata`.
    type MetadataItem =
        { [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE metadata (
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT metadata_PK PRIMARY KEY (item_key)
)
        """
    
        static member SelectSql() = """
        SELECT
              metadata.`item_key`,
              metadata.`item_value`
        FROM metadata
        """
    
        static member TableName() = "metadata"
    
    /// A record representing a row in the table `node_documents`.
    type NodeDocument =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("documentId")>] DocumentId: string }
    
        static member Blank() =
            { NodeId = String.Empty
              DocumentId = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE node_documents (
	node_id TEXT NOT NULL,
	document_id TEXT NOT NULL,
	CONSTRAINT node_documents_PK PRIMARY KEY (node_id,document_id),
	CONSTRAINT node_documents_FK FOREIGN KEY (node_id) REFERENCES nodes(id),
	CONSTRAINT node_documents_FK_1 FOREIGN KEY (document_id) REFERENCES documents(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_documents.`node_id`,
              node_documents.`document_id`
        FROM node_documents
        """
    
        static member TableName() = "node_documents"
    
    /// A record representing a row in the table `node_files`.
    type NodeFile =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("fileId")>] FileId: string }
    
        static member Blank() =
            { NodeId = String.Empty
              FileId = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE node_files (
	node_id TEXT NOT NULL,
	file_id TEXT NOT NULL,
	CONSTRAINT node_files_PK PRIMARY KEY (node_id,file_id),
	CONSTRAINT node_files_FK FOREIGN KEY (node_id) REFERENCES nodes(id),
	CONSTRAINT node_files_FK_1 FOREIGN KEY (file_id) REFERENCES files(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_files.`node_id`,
              node_files.`file_id`
        FROM node_files
        """
    
        static member TableName() = "node_files"
    
    /// A record representing a row in the table `node_labels`.
    type NodeLabel =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("label")>] Label: string }
    
        static member Blank() =
            { NodeId = String.Empty
              Label = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE node_labels (
	node_id TEXT NOT NULL,
	label TEXT NOT NULL,
	CONSTRAINT node_labels_PK PRIMARY KEY (node_id,label),
	CONSTRAINT node_labels_FK FOREIGN KEY (node_id) REFERENCES nodes(id),
	CONSTRAINT node_labels_FK_1 FOREIGN KEY (label) REFERENCES labels(name)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_labels.`node_id`,
              node_labels.`label`
        FROM node_labels
        """
    
        static member TableName() = "node_labels"
    
    /// A record representing a row in the table `node_metadata`.
    type NodeMetadataItem =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { NodeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
        static member CreateTableSql() = """
        CREATE TABLE node_metadata (
	node_id TEXT NOT NULL,
	item_key TEXT NOT NULL,
	item_value TEXT NOT NULL,
	CONSTRAINT node_metadata_PK PRIMARY KEY (node_id,item_key),
	CONSTRAINT node_metadata_FK FOREIGN KEY (node_id) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_metadata.`node_id`,
              node_metadata.`item_key`,
              node_metadata.`item_value`
        FROM node_metadata
        """
    
        static member TableName() = "node_metadata"
    
    /// A record representing a row in the table `node_properties`.
    type NodeProperties =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("jsonBlob")>] JsonBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { NodeId = String.Empty
              Version = 0
              JsonBlob = BlobField.Empty()
              Hash = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE node_properties (
	node_id TEXT NOT NULL,
	version INTEGER NOT NULL,
	json_blob BLOB NOT NULL,
	hash TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT node_properties_PK PRIMARY KEY (node_id,version),
	CONSTRAINT node_properties_FK FOREIGN KEY (node_id) REFERENCES nodes(id)
)
        """
    
        static member SelectSql() = """
        SELECT
              node_properties.`node_id`,
              node_properties.`version`,
              node_properties.`json_blob`,
              node_properties.`hash`,
              node_properties.`created_on`
        FROM node_properties
        """
    
        static member TableName() = "node_properties"
    
    /// A record representing a row in the table `nodes`.
    type Node =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow }
    
        static member CreateTableSql() = """
        CREATE TABLE nodes (
	id TEXT NOT NULL,
	name TEXT NOT NULL,
	created_on TEXT NOT NULL,
	CONSTRAINT nodes_PK PRIMARY KEY (id)
)
        """
    
        static member SelectSql() = """
        SELECT
              nodes.`id`,
              nodes.`name`,
              nodes.`created_on`
        FROM nodes
        """
    
        static member TableName() = "nodes"
    

/// Module generated on 13/02/2023 19:29:12 (utc) via Freql.Tools.
[<RequireQualifiedAccess>]
module Parameters =
    /// A record representing a new row in the table `categories`.
    type NewCategory =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
    
    /// A record representing a new row in the table `compression_types`.
    type NewCompressionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
    
    /// A record representing a new row in the table `document_metadata`.
    type NewDocumentMetadataItem =
        { [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { DocumentId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `document_version_metadata`.
    type NewDocumentVersionMetadataItem =
        { [<JsonPropertyName("documentVersionId")>] DocumentVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { DocumentVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `document_versions`.
    type NewDocumentVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("documentId")>] DocumentId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("documentType")>] DocumentType: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              DocumentId = String.Empty
              Version = 0
              RawBlob = BlobField.Empty()
              Hash = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              DocumentType = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `documents`.
    type NewDocument =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `edge_documents`.
    type NewEdgeDocument =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("documentId")>] DocumentId: string }
    
        static member Blank() =
            { EdgeId = String.Empty
              DocumentId = String.Empty }
    
    
    /// A record representing a new row in the table `edge_files`.
    type NewEdgeFile =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("fileId")>] FileId: string }
    
        static member Blank() =
            { EdgeId = String.Empty
              FileId = String.Empty }
    
    
    /// A record representing a new row in the table `edge_metadata`.
    type NewEdgeMetadataItem =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { EdgeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `edge_properties`.
    type NewEdgeProperties =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("jsonBlob")>] JsonBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { EdgeId = String.Empty
              Version = 0
              JsonBlob = BlobField.Empty()
              Hash = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `edge_weights`.
    type NewEdgeWeight =
        { [<JsonPropertyName("edgeId")>] EdgeId: string
          [<JsonPropertyName("category")>] Category: string
          [<JsonPropertyName("weight")>] Weight: decimal }
    
        static member Blank() =
            { EdgeId = String.Empty
              Category = String.Empty
              Weight = 0m }
    
    
    /// A record representing a new row in the table `edges`.
    type NewEdge =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("fromNode")>] FromNode: string
          [<JsonPropertyName("toNode")>] ToNode: string
          [<JsonPropertyName("bidirectional")>] Bidirectional: bool
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              FromNode = String.Empty
              ToNode = String.Empty
              Bidirectional = true
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `encryption_types`.
    type NewEncryptionType =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
    
    /// A record representing a new row in the table `external_edge_documents`.
    type NewExternalEdgeDocument =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("documentId")>] DocumentId: string }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              DocumentId = String.Empty }
    
    
    /// A record representing a new row in the table `external_edge_files`.
    type NewExternalEdgeFile =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("fileId")>] FileId: string }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              FileId = String.Empty }
    
    
    /// A record representing a new row in the table `external_edge_metadata`.
    type NewExternalEdgeMetadataItem =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `external_edge_properties`.
    type NewExternalEdgeProperties =
        { [<JsonPropertyName("externalEdgeId")>] ExternalEdgeId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("jsonBlob")>] JsonBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { ExternalEdgeId = String.Empty
              Version = 0
              JsonBlob = BlobField.Empty()
              Hash = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `external_edges`.
    type NewExternalEdge =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("bidirectional")>] Bidirectional: bool
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              NodeId = String.Empty
              Bidirectional = true
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `file_metadata`.
    type NewFileMetadataItem =
        { [<JsonPropertyName("fileId")>] FileId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { FileId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `file_types`.
    type NewFileType =
        { [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("extension")>] Extension: string
          [<JsonPropertyName("contentType")>] ContentType: string }
    
        static member Blank() =
            { Name = String.Empty
              Extension = String.Empty
              ContentType = String.Empty }
    
    
    /// A record representing a new row in the table `file_version_metadata`.
    type NewFileVersionMetadataItem =
        { [<JsonPropertyName("fileVersionId")>] FileVersionId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { FileVersionId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `file_versions`.
    type NewFileVersion =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("fileId")>] FileId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("rawBlob")>] RawBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("encryptionType")>] EncryptionType: string
          [<JsonPropertyName("compressionType")>] CompressionType: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              FileId = String.Empty
              Version = 0
              RawBlob = BlobField.Empty()
              Hash = String.Empty
              EncryptionType = String.Empty
              CompressionType = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `files`.
    type NewFile =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("fileType")>] FileType: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              FileType = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `finq_info`.
    type NewFinqInfoItem =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("description")>] Description: string
          [<JsonPropertyName("isReadOnly")>] IsReadOnly: bool
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              Description = String.Empty
              IsReadOnly = true
              Version = 0
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `labels`.
    type NewLabel =
        { [<JsonPropertyName("name")>] Name: string }
    
        static member Blank() =
            { Name = String.Empty }
    
    
    /// A record representing a new row in the table `metadata`.
    type NewMetadataItem =
        { [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `node_documents`.
    type NewNodeDocument =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("documentId")>] DocumentId: string }
    
        static member Blank() =
            { NodeId = String.Empty
              DocumentId = String.Empty }
    
    
    /// A record representing a new row in the table `node_files`.
    type NewNodeFile =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("fileId")>] FileId: string }
    
        static member Blank() =
            { NodeId = String.Empty
              FileId = String.Empty }
    
    
    /// A record representing a new row in the table `node_labels`.
    type NewNodeLabel =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("label")>] Label: string }
    
        static member Blank() =
            { NodeId = String.Empty
              Label = String.Empty }
    
    
    /// A record representing a new row in the table `node_metadata`.
    type NewNodeMetadataItem =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("itemKey")>] ItemKey: string
          [<JsonPropertyName("itemValue")>] ItemValue: string }
    
        static member Blank() =
            { NodeId = String.Empty
              ItemKey = String.Empty
              ItemValue = String.Empty }
    
    
    /// A record representing a new row in the table `node_properties`.
    type NewNodeProperties =
        { [<JsonPropertyName("nodeId")>] NodeId: string
          [<JsonPropertyName("version")>] Version: int
          [<JsonPropertyName("jsonBlob")>] JsonBlob: BlobField
          [<JsonPropertyName("hash")>] Hash: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { NodeId = String.Empty
              Version = 0
              JsonBlob = BlobField.Empty()
              Hash = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
    /// A record representing a new row in the table `nodes`.
    type NewNode =
        { [<JsonPropertyName("id")>] Id: string
          [<JsonPropertyName("name")>] Name: string
          [<JsonPropertyName("createdOn")>] CreatedOn: DateTime }
    
        static member Blank() =
            { Id = String.Empty
              Name = String.Empty
              CreatedOn = DateTime.UtcNow }
    
    
/// Module generated on 13/02/2023 19:29:12 (utc) via Freql.Tools.
[<RequireQualifiedAccess>]
module Operations =

    let buildSql (lines: string list) = lines |> String.concat Environment.NewLine

    /// Select a `Records.Category` from the table `categories`.
    /// Internally this calls `context.SelectSingleAnon<Records.Category>` and uses Records.Category.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectCategoryRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectCategoryRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Category.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Category>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Category>` and uses Records.Category.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectCategoryRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectCategoryRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Category.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Category>(sql, parameters)
    
    let insertCategory (context: SqliteContext) (parameters: Parameters.NewCategory) =
        context.Insert("categories", parameters)
    
    /// Select a `Records.CompressionType` from the table `compression_types`.
    /// Internally this calls `context.SelectSingleAnon<Records.CompressionType>` and uses Records.CompressionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectCompressionTypeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectCompressionTypeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.CompressionType.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.CompressionType>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.CompressionType>` and uses Records.CompressionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectCompressionTypeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectCompressionTypeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.CompressionType.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.CompressionType>(sql, parameters)
    
    let insertCompressionType (context: SqliteContext) (parameters: Parameters.NewCompressionType) =
        context.Insert("compression_types", parameters)
    
    /// Select a `Records.DocumentMetadataItem` from the table `document_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentMetadataItem>` and uses Records.DocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentMetadataItem>` and uses Records.DocumentMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentMetadataItem>(sql, parameters)
    
    let insertDocumentMetadataItem (context: SqliteContext) (parameters: Parameters.NewDocumentMetadataItem) =
        context.Insert("document_metadata", parameters)
    
    /// Select a `Records.DocumentVersionMetadataItem` from the table `document_version_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentVersionMetadataItem>` and uses Records.DocumentVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentVersionMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentVersionMetadataItem>` and uses Records.DocumentVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentVersionMetadataItem>(sql, parameters)
    
    let insertDocumentVersionMetadataItem (context: SqliteContext) (parameters: Parameters.NewDocumentVersionMetadataItem) =
        context.Insert("document_version_metadata", parameters)
    
    /// Select a `Records.DocumentVersion` from the table `document_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.DocumentVersion>` and uses Records.DocumentVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.DocumentVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.DocumentVersion>` and uses Records.DocumentVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.DocumentVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.DocumentVersion>(sql, parameters)
    
    let insertDocumentVersion (context: SqliteContext) (parameters: Parameters.NewDocumentVersion) =
        context.Insert("document_versions", parameters)
    
    /// Select a `Records.Document` from the table `documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.Document>` and uses Records.Document.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Document.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Document>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Document>` and uses Records.Document.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Document.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Document>(sql, parameters)
    
    let insertDocument (context: SqliteContext) (parameters: Parameters.NewDocument) =
        context.Insert("documents", parameters)
    
    /// Select a `Records.EdgeDocument` from the table `edge_documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.EdgeDocument>` and uses Records.EdgeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeDocument.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EdgeDocument>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EdgeDocument>` and uses Records.EdgeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeDocument.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EdgeDocument>(sql, parameters)
    
    let insertEdgeDocument (context: SqliteContext) (parameters: Parameters.NewEdgeDocument) =
        context.Insert("edge_documents", parameters)
    
    /// Select a `Records.EdgeFile` from the table `edge_files`.
    /// Internally this calls `context.SelectSingleAnon<Records.EdgeFile>` and uses Records.EdgeFile.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeFileRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeFileRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeFile.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EdgeFile>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EdgeFile>` and uses Records.EdgeFile.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeFileRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeFileRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeFile.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EdgeFile>(sql, parameters)
    
    let insertEdgeFile (context: SqliteContext) (parameters: Parameters.NewEdgeFile) =
        context.Insert("edge_files", parameters)
    
    /// Select a `Records.EdgeMetadataItem` from the table `edge_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.EdgeMetadataItem>` and uses Records.EdgeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EdgeMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EdgeMetadataItem>` and uses Records.EdgeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EdgeMetadataItem>(sql, parameters)
    
    let insertEdgeMetadataItem (context: SqliteContext) (parameters: Parameters.NewEdgeMetadataItem) =
        context.Insert("edge_metadata", parameters)
    
    /// Select a `Records.EdgeProperties` from the table `edge_properties`.
    /// Internally this calls `context.SelectSingleAnon<Records.EdgeProperties>` and uses Records.EdgeProperties.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgePropertiesRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgePropertiesRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeProperties.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EdgeProperties>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EdgeProperties>` and uses Records.EdgeProperties.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgePropertiesRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgePropertiesRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeProperties.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EdgeProperties>(sql, parameters)
    
    let insertEdgeProperties (context: SqliteContext) (parameters: Parameters.NewEdgeProperties) =
        context.Insert("edge_properties", parameters)
    
    /// Select a `Records.EdgeWeight` from the table `edge_weights`.
    /// Internally this calls `context.SelectSingleAnon<Records.EdgeWeight>` and uses Records.EdgeWeight.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeWeightRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeWeightRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeWeight.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EdgeWeight>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EdgeWeight>` and uses Records.EdgeWeight.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeWeightRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeWeightRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EdgeWeight.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EdgeWeight>(sql, parameters)
    
    let insertEdgeWeight (context: SqliteContext) (parameters: Parameters.NewEdgeWeight) =
        context.Insert("edge_weights", parameters)
    
    /// Select a `Records.Edge` from the table `edges`.
    /// Internally this calls `context.SelectSingleAnon<Records.Edge>` and uses Records.Edge.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Edge.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Edge>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Edge>` and uses Records.Edge.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEdgeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEdgeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Edge.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Edge>(sql, parameters)
    
    let insertEdge (context: SqliteContext) (parameters: Parameters.NewEdge) =
        context.Insert("edges", parameters)
    
    /// Select a `Records.EncryptionType` from the table `encryption_types`.
    /// Internally this calls `context.SelectSingleAnon<Records.EncryptionType>` and uses Records.EncryptionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEncryptionTypeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectEncryptionTypeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EncryptionType.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.EncryptionType>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.EncryptionType>` and uses Records.EncryptionType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectEncryptionTypeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectEncryptionTypeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.EncryptionType.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.EncryptionType>(sql, parameters)
    
    let insertEncryptionType (context: SqliteContext) (parameters: Parameters.NewEncryptionType) =
        context.Insert("encryption_types", parameters)
    
    /// Select a `Records.ExternalEdgeDocument` from the table `external_edge_documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalEdgeDocument>` and uses Records.ExternalEdgeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeDocument.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalEdgeDocument>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalEdgeDocument>` and uses Records.ExternalEdgeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeDocument.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalEdgeDocument>(sql, parameters)
    
    let insertExternalEdgeDocument (context: SqliteContext) (parameters: Parameters.NewExternalEdgeDocument) =
        context.Insert("external_edge_documents", parameters)
    
    /// Select a `Records.ExternalEdgeFile` from the table `external_edge_files`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalEdgeFile>` and uses Records.ExternalEdgeFile.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeFileRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeFileRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeFile.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalEdgeFile>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalEdgeFile>` and uses Records.ExternalEdgeFile.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeFileRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeFileRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeFile.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalEdgeFile>(sql, parameters)
    
    let insertExternalEdgeFile (context: SqliteContext) (parameters: Parameters.NewExternalEdgeFile) =
        context.Insert("external_edge_files", parameters)
    
    /// Select a `Records.ExternalEdgeMetadataItem` from the table `external_edge_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalEdgeMetadataItem>` and uses Records.ExternalEdgeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalEdgeMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalEdgeMetadataItem>` and uses Records.ExternalEdgeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalEdgeMetadataItem>(sql, parameters)
    
    let insertExternalEdgeMetadataItem (context: SqliteContext) (parameters: Parameters.NewExternalEdgeMetadataItem) =
        context.Insert("external_edge_metadata", parameters)
    
    /// Select a `Records.ExternalEdgeProperties` from the table `external_edge_properties`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalEdgeProperties>` and uses Records.ExternalEdgeProperties.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgePropertiesRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgePropertiesRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeProperties.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalEdgeProperties>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalEdgeProperties>` and uses Records.ExternalEdgeProperties.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgePropertiesRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgePropertiesRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdgeProperties.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalEdgeProperties>(sql, parameters)
    
    let insertExternalEdgeProperties (context: SqliteContext) (parameters: Parameters.NewExternalEdgeProperties) =
        context.Insert("external_edge_properties", parameters)
    
    /// Select a `Records.ExternalEdge` from the table `external_edges`.
    /// Internally this calls `context.SelectSingleAnon<Records.ExternalEdge>` and uses Records.ExternalEdge.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdge.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.ExternalEdge>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.ExternalEdge>` and uses Records.ExternalEdge.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectExternalEdgeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectExternalEdgeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.ExternalEdge.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.ExternalEdge>(sql, parameters)
    
    let insertExternalEdge (context: SqliteContext) (parameters: Parameters.NewExternalEdge) =
        context.Insert("external_edges", parameters)
    
    /// Select a `Records.FileMetadataItem` from the table `file_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.FileMetadataItem>` and uses Records.FileMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.FileMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.FileMetadataItem>` and uses Records.FileMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.FileMetadataItem>(sql, parameters)
    
    let insertFileMetadataItem (context: SqliteContext) (parameters: Parameters.NewFileMetadataItem) =
        context.Insert("file_metadata", parameters)
    
    /// Select a `Records.FileType` from the table `file_types`.
    /// Internally this calls `context.SelectSingleAnon<Records.FileType>` and uses Records.FileType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileTypeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileTypeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileType.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.FileType>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.FileType>` and uses Records.FileType.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileTypeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileTypeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileType.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.FileType>(sql, parameters)
    
    let insertFileType (context: SqliteContext) (parameters: Parameters.NewFileType) =
        context.Insert("file_types", parameters)
    
    /// Select a `Records.FileVersionMetadataItem` from the table `file_version_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.FileVersionMetadataItem>` and uses Records.FileVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileVersionMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileVersionMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.FileVersionMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.FileVersionMetadataItem>` and uses Records.FileVersionMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileVersionMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileVersionMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileVersionMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.FileVersionMetadataItem>(sql, parameters)
    
    let insertFileVersionMetadataItem (context: SqliteContext) (parameters: Parameters.NewFileVersionMetadataItem) =
        context.Insert("file_version_metadata", parameters)
    
    /// Select a `Records.FileVersion` from the table `file_versions`.
    /// Internally this calls `context.SelectSingleAnon<Records.FileVersion>` and uses Records.FileVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileVersionRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileVersionRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileVersion.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.FileVersion>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.FileVersion>` and uses Records.FileVersion.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileVersionRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileVersionRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FileVersion.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.FileVersion>(sql, parameters)
    
    let insertFileVersion (context: SqliteContext) (parameters: Parameters.NewFileVersion) =
        context.Insert("file_versions", parameters)
    
    /// Select a `Records.File` from the table `files`.
    /// Internally this calls `context.SelectSingleAnon<Records.File>` and uses Records.File.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.File.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.File>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.File>` and uses Records.File.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFileRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectFileRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.File.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.File>(sql, parameters)
    
    let insertFile (context: SqliteContext) (parameters: Parameters.NewFile) =
        context.Insert("files", parameters)
    
    /// Select a `Records.FinqInfoItem` from the table `finq_info`.
    /// Internally this calls `context.SelectSingleAnon<Records.FinqInfoItem>` and uses Records.FinqInfoItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFinqInfoItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectFinqInfoItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FinqInfoItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.FinqInfoItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.FinqInfoItem>` and uses Records.FinqInfoItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectFinqInfoItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectFinqInfoItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.FinqInfoItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.FinqInfoItem>(sql, parameters)
    
    let insertFinqInfoItem (context: SqliteContext) (parameters: Parameters.NewFinqInfoItem) =
        context.Insert("finq_info", parameters)
    
    /// Select a `Records.Label` from the table `labels`.
    /// Internally this calls `context.SelectSingleAnon<Records.Label>` and uses Records.Label.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectLabelRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectLabelRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Label.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Label>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Label>` and uses Records.Label.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectLabelRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectLabelRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Label.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Label>(sql, parameters)
    
    let insertLabel (context: SqliteContext) (parameters: Parameters.NewLabel) =
        context.Insert("labels", parameters)
    
    /// Select a `Records.MetadataItem` from the table `metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.MetadataItem>` and uses Records.MetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.MetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.MetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.MetadataItem>` and uses Records.MetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.MetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.MetadataItem>(sql, parameters)
    
    let insertMetadataItem (context: SqliteContext) (parameters: Parameters.NewMetadataItem) =
        context.Insert("metadata", parameters)
    
    /// Select a `Records.NodeDocument` from the table `node_documents`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeDocument>` and uses Records.NodeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocument.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeDocument>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeDocument>` and uses Records.NodeDocument.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeDocumentRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeDocumentRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeDocument.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeDocument>(sql, parameters)
    
    let insertNodeDocument (context: SqliteContext) (parameters: Parameters.NewNodeDocument) =
        context.Insert("node_documents", parameters)
    
    /// Select a `Records.NodeFile` from the table `node_files`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeFile>` and uses Records.NodeFile.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeFileRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeFileRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeFile.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeFile>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeFile>` and uses Records.NodeFile.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeFileRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeFileRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeFile.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeFile>(sql, parameters)
    
    let insertNodeFile (context: SqliteContext) (parameters: Parameters.NewNodeFile) =
        context.Insert("node_files", parameters)
    
    /// Select a `Records.NodeLabel` from the table `node_labels`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeLabel>` and uses Records.NodeLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeLabelRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeLabelRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeLabel.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeLabel>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeLabel>` and uses Records.NodeLabel.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeLabelRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeLabelRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeLabel.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeLabel>(sql, parameters)
    
    let insertNodeLabel (context: SqliteContext) (parameters: Parameters.NewNodeLabel) =
        context.Insert("node_labels", parameters)
    
    /// Select a `Records.NodeMetadataItem` from the table `node_metadata`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeMetadataItem>` and uses Records.NodeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeMetadataItemRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeMetadataItemRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeMetadataItem>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeMetadataItem>` and uses Records.NodeMetadataItem.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeMetadataItemRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeMetadataItemRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeMetadataItem.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeMetadataItem>(sql, parameters)
    
    let insertNodeMetadataItem (context: SqliteContext) (parameters: Parameters.NewNodeMetadataItem) =
        context.Insert("node_metadata", parameters)
    
    /// Select a `Records.NodeProperties` from the table `node_properties`.
    /// Internally this calls `context.SelectSingleAnon<Records.NodeProperties>` and uses Records.NodeProperties.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodePropertiesRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodePropertiesRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeProperties.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.NodeProperties>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.NodeProperties>` and uses Records.NodeProperties.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodePropertiesRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodePropertiesRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.NodeProperties.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.NodeProperties>(sql, parameters)
    
    let insertNodeProperties (context: SqliteContext) (parameters: Parameters.NewNodeProperties) =
        context.Insert("node_properties", parameters)
    
    /// Select a `Records.Node` from the table `nodes`.
    /// Internally this calls `context.SelectSingleAnon<Records.Node>` and uses Records.Node.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeRecord ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeRecord (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Node.SelectSql() ] @ query |> buildSql
        context.SelectSingleAnon<Records.Node>(sql, parameters)
    
    /// Internally this calls `context.SelectAnon<Records.Node>` and uses Records.Node.SelectSql().
    /// The caller can provide extra string lines to create a query and boxed parameters.
    /// It is up to the caller to verify the sql and parameters are correct,
    /// this should be considered an internal function (not exposed in public APIs).
    /// Parameters are assigned names based on their order in 0 indexed array. For example: @0,@1,@2...
    /// Example: selectNodeRecords ctx "WHERE `field` = @0" [ box `value` ]
    let selectNodeRecords (context: SqliteContext) (query: string list) (parameters: obj list) =
        let sql = [ Records.Node.SelectSql() ] @ query |> buildSql
        context.SelectAnon<Records.Node>(sql, parameters)
    
    let insertNode (context: SqliteContext) (parameters: Parameters.NewNode) =
        context.Insert("nodes", parameters)
    