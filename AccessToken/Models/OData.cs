using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessToken.Models
{
    public class OData
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public List<Value> Value { get; set; }
    }

    public class emailAddress
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class body
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class Value
    {
        public string id { get; set; }
        public string createdDateTime { get; set; }
        public string lastModifiedDateTime { get; set; }
        public string changeKey { get; set; }
        public List<string> categories { get; set; }
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
        public bool isDeliveryReceiptRequested { get; set; }
        public bool isReadReceiptRequested { get; set; }
        public bool isRead { get; set; }
        public bool isDraft { get; set; }
        public string webLink { get; set; }
        public string inferenceClassification { get; set; }
        public List<body> body { get; set; }
        public virtual emailAddress sender { get; set; }
        public virtual emailAddress from { get; set; }
        public virtual List<emailAddress> toRecipients { get; set; }
        public List<string> ccRecipients { get; set; }
        public List<string> bccRecipients { get; set; }
        public List<string> replyTo { get; set; }
    }
}