using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;
using Newtonsoft.Json.Linq;

namespace Tests.API.Datastore
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void CreateGetAndDeleteRecord()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            JObject document = new JObject();
            document.Add("@class", "reviews");
            document.Add("title", "Love It!");
            //create document
            JObject json = request.Datastore.Create(document);
            //retrieve by id
            string id = json["@rid"].ToString().Replace("#", "");
            json = request.Datastore.Get(id);
            //delete
            request.Datastore.Delete(id);
        }

        [TestMethod]
        public void CreateAndDeleteRecord()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            JObject document = new JObject();
            document.Add("@class", "reviews");
            document.Add("title", "Love It!");
            //create document
            JObject json = request.Datastore.Create(document);
            

        }

        [TestMethod]
        public void DeleteRecord()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;
            string id = "9:3";
            //delete
            request.Datastore.Delete(id);
        }

        [TestMethod]
        public void GetAndUpdateRecord()
        {
            //ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            //RevStackRequest request = RevStackClient.CreateRequest(credentials);

            //IAccessToken token = request.User.Login();
            //credentials.AccessToken = token;

            //string id = "9:2";
            //JObject document = request.Datastore.Get(id);
            //if (document == null)
            //    throw new NullReferenceException();

            //document["title"] = "updated...";
            //document.Add("title2", "added...");

            //request.Datastore.Update(document); 

            //promo giveaway
            string appId = "3be1897b-0fc0-47dc-83ab-9fb652fad5ac";
            string username = "host";
            string password = "host";
            ICredentials credentials = new Credentials(appId, username, password, null);
            RevStackRequest RevStackRequest = RevStackClient.CreateRequest(credentials);
            credentials.AccessToken = RevStackRequest.User.Login();
            
            string email = "dsheets@live.com";
            string firstname = "Jenna";
            string lastname = "Jones";
            JArray r = RevStackRequest.Datastore.Lookup("select from promo_vip_referral_giveaway_recipients where email = '" + email + "'", -1, -1);
            if (r.Count > 0)
            {
                foreach (JObject obj in r)
                {
                    int i = int.Parse(obj["number_of_items_purchased"].ToString());
                    i = i + 1;
                    obj["number_of_items_purchased"] = i.ToString();
                    RevStackRequest.Datastore.Update(obj);

                    //
                    JObject sender = (JObject) obj["sender"];
                    string sender_email = sender["email"].ToString();
                    int num_purchase = int.Parse(sender["number_of_items_purchased"].ToString());
                    num_purchase = num_purchase + 1;
                    sender["number_of_items_purchased"] = num_purchase.ToString();
                    RevStackRequest.Datastore.Update(sender);

                    //sender email
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                    client.Send(new System.Net.Mail.MailMessage("noreply@julielopezshoes.com", sender_email, "VIP Giveaway Order Notification", "This is a notification that your friend " + firstname + " " + lastname + " has placed an order at Julie Lopez Shoes. To date you have a total of " + num_purchase + " order(s) from friends."));

                    //referral email
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage("noreply@julielopezshoes.com", email, "VIP Referral Giveaway", "<a href='https://julielopezshoes.com/shop/Promo/VIPReferralGiveaway'><img src='http://media.r3vstack.com/assets/julielopezshoes/email.jpg' width='' height='' alt='' title='' align='left' border='0' /></a>");
                    message.IsBodyHtml = true;
                    client.Send(message);

                }
            }
            //end promo giveaway
        }

        [TestMethod]
        public void QueryReviews()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            string query = "select from reviews";
            JArray reviews = request.Datastore.Lookup(query, -1, -1);
        }
    }
}
