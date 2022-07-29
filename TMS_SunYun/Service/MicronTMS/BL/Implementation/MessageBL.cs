using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using NHibernate;
using MicronTMS.Helper;
using MicronTMS.DLL.Implementation;
using MicronTMS.DLL.Entities;
using MicronTMS.DLL.Models;


namespace MicronTMS.BL.Implementation
{
    public class MessageBL
    {
        private readonly Repository<HTeleMessage> _histTelgRepo;
        private readonly Repository<HSMSMessage> _histSMSRepo;
        private readonly Repository<CMcVersion> _versCMCRepo;
        private readonly Repository<RCtlEtl> _etlRepo;
        private readonly Repository<CUserInfo> _userInfoRepo;
        private readonly Logger _log;
        private ISession _session;

        public MessageBL()
        {
            _histTelgRepo = new Repository<HTeleMessage>();
            _versCMCRepo = new Repository<CMcVersion>();
            _histSMSRepo = new Repository<HSMSMessage>();
            _etlRepo = new Repository<RCtlEtl>();
            _userInfoRepo = new Repository<CUserInfo>();
            _log = new Logger();
        }

        private async Task<string> sendTelegramAPI(string TelegramAPI, string chatid, string datacont, string chatgrp)
        {
            System.Net.ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12 |
            SecurityProtocolType.Tls11 |
            SecurityProtocolType.Tls;

            //foreach (var chatid in chatids)+
            string result = string.Empty;
            string WebAPI = TelegramAPI + chatid + datacont;
            Uri u = new Uri(WebAPI);

            HttpContent c = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            using (HttpClient client = new System.Net.Http.HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = u,
                    Content = c
                };

                try
                {
                    response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        result = jsonString;
                    }

                    System.Threading.Thread.Sleep(1000);



                }
                catch (HttpRequestException e)
                {
                    _log.Error("Send Telegram Error: " + e.Message);

                }
            }
            return result; //response

        }

        private async void sendTelegram(List<String> chatids, string datas, string chatgrp, string lmuser, DateTime Lmtime, ISession _session)
        {
            string telegramflag = ConfigurationManager.AppSettings["TELEGRAM_ENABLE"];
            string result = "NOTOK";
            string eventId = Guid.NewGuid().ToString();
            List<String> Okchats = new List<String>();
            if ((telegramflag == "Y"))
            {
                _log.Info("Enter Telegram:");
                HTeleMessage teldatas = new HTeleMessage();

                string TelegramAPI = ConfigurationManager.AppSettings["TELEGRAM_API"];

                string TelegramToken = ConfigurationManager.AppSettings["BOT_TOKEN"];
                string TelegramAPI2 = TelegramAPI.Replace("bot_token", TelegramToken);
                string datacont = "&text=" + datas;
                foreach (var chatid in chatids)
                {
                    var t = Task.Run(() => sendTelegramAPI(TelegramAPI2, chatid, datacont, chatgrp));
                    t.Wait();
                    _log.Info("Sending Telegram :" + chatid + " STATUS CODE:" + t.Result);
                    if (t.Result.Length > 0)
                    {
                        Okchats.Add(chatid);
                        result = "OK";
                    }
                }
                teldatas.EventId = eventId;
                teldatas.MessageCode = result;
                teldatas.MessageBody = datas;
                teldatas.ChatId = string.Join(",", Okchats);
                teldatas.ChatGroup = chatgrp;
                teldatas.LmTime = Lmtime;
                teldatas.LmUser = lmuser;
                teldatas.ReportTime = Lmtime;
                _histTelgRepo.Create(teldatas, _session);
                _session.Transaction.Commit();
                _log.Info("Save Telegram History");
            }
            else
            {
                _log.Info("Telegram is not enabled!!");
                //result= false;
            }


        }

        private void sendEmail(List<string> emails, List<string> emailnames, string datas)
        {

            string SmtpPort = ConfigurationManager.AppSettings["SMTP_PORT"];
            string SmtpHost = ConfigurationManager.AppSettings["SMTP_HOST"];
            string SmtpUser = ConfigurationManager.AppSettings["SMTP_USER"];
            string SmtpPass = ConfigurationManager.AppSettings["SMTP_PASS"];
            string SmtpFrom = ConfigurationManager.AppSettings["SMTP_FROM"];
            string emailSubject = ConfigurationManager.AppSettings["EMAIL_SUBJECT"];
            try
            {
                int portsm = int.Parse(SmtpPort);
                if ((emails.Count <= 0))
                {
                    _log.Info("No email send");
                }
                else
                {
                    string emaillist = string.Join(";", emails);
                    string emaillistnames = string.Join(";", emailnames);
                    // create message
                    if (SmtpHost.Contains("gmail"))
                    {
                        bool sendReply = GmailSend(emaillist, emaillistnames, datas, SmtpUser, SmtpPass, SmtpHost, SmtpPort, emailSubject);
                        _log.Info("Send Gmail Suceess ", sendReply.ToString());
                    }
                    else
                    {

                        //Normal email
                        _log.Info("Enter Email function");
                        using (SmtpClient client = new SmtpClient(SmtpHost, portsm))
                        {
                            // Configure the client  
                            client.EnableSsl = true;
                            client.Credentials = new NetworkCredential(SmtpUser, SmtpPass);
                            // client.UseDefaultCredentials = true;  

                            // A client has been created, now you need to create a MailMessage object  
                            MailMessage message = new MailMessage(
                                                     SmtpFrom, // From field  
                                                     emaillist, // Recipient field  
                                                     emailSubject, // Subject of the email message  
                                                     datas // Email message body  
                                                  );

                            // Send the message  
                            client.Send(message);
                        }
                        _log.Info("Send Normal mail Suceess ");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                _log.Error("Message :{0} ", e.Message);
            }



        }

        private bool GmailSend(string receiverEmail, string ReceiverName, string body, string SmtpUser, string SmtpPass, string SmtpHost, string SmtpPort, string emailSubject)
        {
            _log.Info("Enter Gmail function");
            MailMessage mailMessage = new MailMessage();
            MailAddress mailAddress = new MailAddress(SmtpUser, "Sender Name"); // abc@gmail.com = input Sender Email Address 
            string[] recvmails;
            string name = string.Empty;
            mailMessage.From = mailAddress;
            if (receiverEmail.IndexOf(";") > 0)
            {
                recvmails = receiverEmail.Split(';');
            }
            else
            {
                recvmails = new string[0];
                recvmails[0] = receiverEmail;
            }

            for (var i = 0; i < recvmails.Length; i++)
            {
                var address = recvmails[i];
                if (recvmails.Length > 0)
                {
                    name = ReceiverName.Split(';')[i];
                }
                else
                {
                    name = ReceiverName;
                }
                //Check if empty email address and name
                if ((address.Length > 0) && (name.Length) > 0)
                {
                    mailMessage.To.Add(new MailAddress(address, name));
                }
            }

            mailMessage.Subject = emailSubject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            int portsm = int.Parse(SmtpPort);
            SmtpClient mailSender = new SmtpClient(SmtpHost, portsm)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(SmtpUser, SmtpPass)   // abc@gmail.com = input sender email address  
                                                                          //pass = sender email password
            };

            try
            {
                mailSender.Send(mailMessage);
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                // Write the exception to a Log file.
                _log.Error("SmtpFailedRecipientException :{0} ", ex.Message);
            }
            catch (SmtpException ex)
            {
                // Write the exception to a Log file.
                _log.Error("SmtpException  :{0} ", ex.Message);
            }
            finally
            {
                mailSender = null;
                mailMessage.Dispose();
            }
            return false;
        }

        public string GetCMCCurrentVersion()
        {
            string version = String.Empty;
            _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
            version = _versCMCRepo.GetAll(_session).OrderBy(x => x.Id).Max(x => x.VersionNumber);

            return version;
        }

        public ActionResult GetMessageRecords()
        {
            var result = new ActionResult();
            var trx = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString();

            try
            {
                _log.Info("Start", result.TrxName);
                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
                _session.Transaction.Begin();

                var histTelegrams = _histTelgRepo.GetAll(_session).OrderByDescending(x => x.ReportTime);

                result.Data = JsonConvert.SerializeObject(histTelegrams);

                _log.Info("Total Mesages Count Query: " + histTelegrams.Count(), result.TrxName, eventId);
                result.Result = "Success";
                return result;
            }
            catch (Exception ex)
            {
                if (ex is NHibernate.ADOException)
                {
                    result.Error = "Database Exception: " + ex.Message.ToString();
                }
                else
                {
                    result.Error = "Logical Exception: " + ex.Message.ToString();
                }
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName);
                return result;
            }
            finally
            {
                if (_session.IsOpen)
                {
                    _session.Close();
                    _session.Dispose();
                }
            }
        }

        public ActionResult sendMessages(SendWebAPIModel messageData)
        {
            var result = new ActionResult();
            var trx = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString();

            List<String> phones = new List<String>();
            List<String> chatids = new List<String>();
            List<String> emails = new List<String>();
            List<String> emailnames = new List<String>();
            HSMSMessage smsdatas = new HSMSMessage();

            try
            {
                _log.Info("Start", result.TrxName);
                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
                _session.Transaction.Begin();
                var Lmtime = DateTime.Now;
                var scene = messageData.scene;
                var picurl = messageData.picUrl;
                var tag = messageData.tag;
                var messageContent = messageData.desc;
                var userid = messageData.userId;

                _log.Info("Start", result.TrxName);
                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
                _session.Transaction.Begin();
                var etldata = _etlRepo.GetAll(_session).FirstOrDefault();
                var activeflag = etldata.ActiveFlag;
                if ((activeflag == "N") || (activeflag == null))
                {
                    _log.Info("Disable Active Flag");
                    result.Data = "";
                    result.Result = "Success";
                    return result;
                }

                var userInfos = _userInfoRepo.GetAll(_session);
                _log.Info("Get UserInfos Count :" + userInfos.Count().ToString());
                foreach (var userInfo in userInfos)
                {
                    if (userInfo.UserId.Length < 14)
                    {
                        chatids.Add(userInfo.UserId);
                    }

                    phones.Add(userInfo.ContactNo);
                    if ((String.IsNullOrEmpty(userInfo.Email) == false) && (String.IsNullOrEmpty(userInfo.UserName) == false))
                    {
                        emails.Add(userInfo.Email);
                        emailnames.Add(userInfo.UserName);
                        _log.Info("Get All UserInfos" + userInfo.Email);
                    }
                }

                var smspassword = etldata.SMSPassword;
                var smsuser = etldata.SMSUser;
                var chatgrp = etldata.ChatGroup;

                sendTelegram(chatids, messageContent, chatgrp, userid, Lmtime, _session);

                sendEmail(emails, emailnames, messageContent);

                result.Data = "";
                result.Result = "Success";
                return result;
            }
            catch (Exception ex)
            {
                if (ex is NHibernate.ADOException)
                {
                    result.Error = "Database Exception: " + ex.Message.ToString();
                }
                else
                {
                    result.Error = "Logical Exception: " + ex.Message.ToString();
                }
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName);
                return result;
            }
            finally
            {
                if (_session.IsOpen)
                {
                    _session.Close();
                    _session.Dispose();
                }
            }
        }
    }
}