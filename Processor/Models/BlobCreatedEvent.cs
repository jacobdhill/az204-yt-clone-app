using System;

namespace Processor.Models;

public class BlobCreatedEvent
{
    public string Topic { get; set; }
    public string Subject { get; set; }
    public string EventType { get; set; }
    public DateTime EventTime { get; set; }
    public string Id { get; set; }
    public Data Data { get; set; }
    public string DataVersion { get; set; }
    public string MetadataVersion { get; set; }
}

public class Data
{
    public string Api { get; set; }
    public string ClientRequestId { get; set; }
    public string RequestId { get; set; }
    public string ETag { get; set; }
    public string ContentType { get; set; }
    public long ContentLength { get; set; }
    public long ContentOffset { get; set; }
    public string BlobType { get; set; }
    public string Url { get; set; }
    public string Sequencer { get; set; }
    public StorageDiagnostics StorageDiagnostics { get; set; }
}

public class StorageDiagnostics
{
    public string BatchId { get; set; }
}
