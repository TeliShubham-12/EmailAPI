using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Graph;
using Microsoft.Graph.Models;
using Azure.Identity;
using System.Threading;

namespace emService.AppCode
{
    public class EmailHelperOuth2
    {

        public bool SendMailOuth2(EmailRequest emailRequest)
        {
            bool returnValue = false;
            // emailSendException = null;
            try
            {
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                // using Azure.Identity;
                var options = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                };

                var clientSecretCredential = new ClientSecretCredential(emailRequest.ClientCredentialForSend.TenantId, emailRequest.ClientCredentialForSend.ClientId, emailRequest.ClientCredentialForSend.ClientSecret, options);
                var accessToken = clientSecretCredential.GetTokenAsync(new Azure.Core.TokenRequestContext(scopes) { }).Result;

                //Console.WriteLine(accessToken.Token);

                List<Recipient> to = new List<Recipient>();
                List<Recipient> cc = new List<Recipient>();

                string ToEmailIds = emailRequest.To;

                if (!string.IsNullOrEmpty(ToEmailIds))
                {
                    var Emails = ToEmailIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var emailAddress in Emails)
                    {
                        if (emailAddress != string.Empty)
                        {
                            to.Add(new Recipient { EmailAddress = new EmailAddress { Address = emailAddress, }, });
                        }
                    }
                }

                string CCEmailIds = emailRequest.CC;

                if (emailRequest.CC != null)
                {

                    var ccEmails = CCEmailIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var emailAddress in ccEmails)
                    {
                        if (emailAddress != string.Empty)
                        {
                            cc.Add(new Recipient { EmailAddress = new EmailAddress { Address = emailAddress, }, });
                        }
                    }
                }
                Microsoft.Graph.Models.Message message = new Microsoft.Graph.Models.Message();
                message.Subject = emailRequest.Subject;
                message.Body = new ItemBody { ContentType = emailRequest.IsBodyHtml == true ? BodyType.Html : BodyType.Text, Content = emailRequest.Body, };
                message.ToRecipients = to;

                if (cc != null && cc.Count > 0)
                {
                    message.CcRecipients = cc;
                }

                List<Microsoft.Graph.Models.Attachment> attachments = new List<Microsoft.Graph.Models.Attachment>();

                if (emailRequest.FileNameOuth2 != null && emailRequest.FileNameOuth2 != string.Empty)
                {

                    string fileExtension = System.IO.Path.GetExtension(emailRequest.FileNameOuth2).ToLowerInvariant();
                    string contentType = fileExtension switch
                    {
                        // Text and Documents
                        ".txt" => "text/plain",
                        ".html" or ".htm" => "text/html",
                        ".css" => "text/css",
                        ".csv" => "text/csv",
                        ".json" => "application/json",
                        ".xml" => "application/xml",
                        ".pdf" => "application/pdf",
                        ".doc" => "application/msword",
                        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        ".xls" => "application/vnd.ms-excel",
                        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        ".ppt" => "application/vnd.ms-powerpoint",
                        ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",

                        // Images
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".bmp" => "image/bmp",
                        ".tiff" or ".tif" => "image/tiff",
                        ".svg" => "image/svg+xml",
                        _ => "application/octet-stream"
                    };



                    attachments.Add(new FileAttachment
                    {
                        OdataType = "#microsoft.graph.fileAttachment",
                        Name = emailRequest.FileNameOuth2,
                        ContentType = contentType,
                        ContentBytes = emailRequest.FileArrayOuth2,
                    });

                    message.Attachments = attachments;

                }

                var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
                var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                {
                    Message = message,
                    SaveToSentItems = false,
                };


                graphClient.Users[emailRequest.ClientCredentialForSend.FromEmailId].SendMail.PostAsync(requestBody);
                Thread.Sleep(250);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("mailbox unavailable"))
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;

                }

            }

            return returnValue;
        }



    }
}