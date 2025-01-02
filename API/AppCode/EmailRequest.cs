
namespace emService.AppCode
{
    public class EmailRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string DomainForMessageID { get; set; }
        public string FileNameOuth2 { get; set; }
        public byte[] FileArrayOuth2 { get; set; }
        //public string FileContentTypeOuth2 { get; set; }

         // Moved outside the nested class

        public ClientCredential ClientCredentialForSend { get; set; }
        
    }

    public class ClientCredential
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string TenantId { get; set; }
            public string FromEmailId { get; set; }
        }

}