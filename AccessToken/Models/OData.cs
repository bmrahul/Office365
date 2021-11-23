using Newtonsoft.Json;
using System;

namespace AccessToken.Models
{
    public class OData
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public Value[] value { get; set; }
        [JsonProperty("@odata.nextLink")]
        public string odatanextLink { get; set; }
    }

    public class Value
    {
        [JsonProperty("@odata.etag")]
        public string odataetag { get; set; }
        public string id { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string changeKey { get; set; }
        public object[] categories { get; set; }
        public DateTime receivedDateTime { get; set; }
        public DateTime sentDateTime { get; set; }
        public bool hasAttachments { get; set; }
        public string internetMessageId { get; set; }
        public string subject { get; set; }
        public string bodyPreview { get; set; }
        public string importance { get; set; }
        public string parentFolderId { get; set; }
        public string conversationId { get; set; }
        public string conversationIndex { get; set; }
        //public bool isDeliveryReceiptRequested { get; set; }
        public bool isReadReceiptRequested { get; set; }
        public bool isRead { get; set; }
        public bool isDraft { get; set; }
        public string webLink { get; set; }
        public string inferenceClassification { get; set; }
        public Body body { get; set; }
        public Sender sender { get; set; }
        public From from { get; set; }
        public Torecipient[] toRecipients { get; set; }
        public object[] ccRecipients { get; set; }
        public object[] bccRecipients { get; set; }
        public object[] replyTo { get; set; }
        public Flag flag { get; set; }
    }

    public class Body
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class Sender
    {
        public Emailaddress emailAddress { get; set; }
    }

    public class Emailaddress
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class From
    {
        public Emailaddress emailAddress { get; set; }
    }

    public class Flag
    {
        public string flagStatus { get; set; }
    }

    public class Torecipient
    {
        public Emailaddress emailAddress { get; set; }
    }
}