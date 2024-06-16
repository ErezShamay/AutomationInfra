namespace Splitit.Automation.NG.Backend.Services.MerchantPortal.Report.Responses;

public class PostGenerateFileResponse
{
    public class ColumnHeader
    {
        public string PropertyKey { get; set; }
        public string Text { get; set; }
    }

    public class Component
    {
        public string Site { get; set; }
    }

    public class Component2
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Container
    {
        public List<Component> Components { get; set; }
    }

    public class Data
    {
        public List<Metadata> Metadata { get; set; }
        public List<DataHeader> DataHeaders { get; set; }
        public Data data { get; set; }
        public PagingResponse PagingResponse { get; set; }
        public Site Site { get; set; }
        public Container Container { get; set; }
        public bool DesignMode { get; set; }
        public string RemotingFormat { get; set; }
        public string SchemaSerializationMode { get; set; }
        public bool CaseSensitive { get; set; }
        public List<DefaultViewManager> DefaultViewManager { get; set; }
        public bool EnforceConstraints { get; set; }
        public string DataSetName { get; set; }
        public string Namespace { get; set; }
        public string Prefix { get; set; }
        public ExtendedProperties ExtendedProperties { get; set; }
        public bool HasErrors { get; set; }
        public bool IsInitialized { get; set; }
        public string Locale { get; set; }
        public List<Relation> Relations { get; set; }
        public List<Table> Tables { get; set; }
    }

    public class DataHeader
    {
        public string TableName { get; set; }
        public List<ColumnHeader> ColumnHeaders { get; set; }
    }

    public class DefaultViewManager
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public class ExtendedProperties
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Metadata
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class PagingResponse
    {
        public int TotalResults { get; set; }
    }

    public class Relation
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class Root
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Data Data { get; set; }
    }

    public class Site
    {
        public Component Component { get; set; }
        public Container Container { get; set; }
        public bool DesignMode { get; set; }
        public string Name { get; set; }
    }

    public class Table
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }
}