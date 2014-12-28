using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using RevStack.Client.API.Datastore;
using RevStack.Client.API.Query;
using System.Collections;

namespace Tests.API.Datastore
{
    public class OStorage_File 
    {
        [DataMember(Name = "@rid")]
        public string @rid { get; set; }
        [DataMember(Name = "@version")]
        public string @version { get; set; }
        [DataMember(Name = "@created")]
        public string @created { get; set; }
        [DataMember(Name = "@class")]
        public string @class { get; set; }
        [DataMember(Name = "@modified")]
        public string @modified { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string path { get; set; }
    }

    //******* datacontract required to keep revstack naming convention
    [DataContract]
    public class Test
    {
        [DataMember(Name = "@rid")]
        public string @rid { get; set; }
        [DataMember(Name = "@version")]
        public string @version { get; set; }
        [DataMember(Name = "@created")]
        public string @created { get; set; }
        [DataMember(Name = "@modified")]
        public string @modified { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string path { get; set; }
    }
    //**********

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
            OStorage_File file = request.Datastore.Get<OStorage_File>(id);
            //delete
            request.Datastore.Delete(id);
        }

        [TestMethod]
        public void GetById()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            string id = "13:3";
            Test test = request.Datastore.Get<Test>(id);
            Console.WriteLine(test.id);
        }

        [TestMethod]
        public void Query_Where_Test()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            //works
            Query<Test> query = request.Datastore.CreateQuery<Test>(null);
            IQueryable<Test> paths = query.Where(c => c.id == "38611375");

            Console.WriteLine("Query:\n{0}\n", paths);

            var list = paths.ToList();
            foreach (var item in list)
            {
                Console.WriteLine("Path: {0}", item.path);
            }
        }

        [TestMethod]
        public void Query_Single_Test()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            RevStack.Client.API.Query.Query<Test> query = request.Datastore.CreateQuery<Test>(null);
            Test single = query.Single<Test>(c => c.id == "38611375");
            Console.WriteLine("Path: {0}", single.path);
        }

        [TestMethod]
        public void Query_Select_Test()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            RevStack.Client.API.Query.Query<Test> query = request.Datastore.CreateQuery<Test>(null);
            IQueryable<Test> results = query.Where(c => c.id == "38611375"); 
            IEnumerable<Test> list = results.ToList();
            IEnumerable<string> paths = list.Select(e => e.path);
            string path = paths.FirstOrDefault();
            Console.WriteLine("Path: {0}", path);
        }

        [TestMethod]
        public void Query_Lamda_Test()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            var ids = from c in request.Datastore.CreateQuery<Test>(null)
                               where c.id == "38611375"
                               select new Test { rid = c.rid };

            IEnumerable results = ids.ToArray();

            Console.WriteLine("Path: {0}", results);
        }

        [TestMethod]
        public void PaginateTests()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            PagedDataSource<Test> pagedDatasource = request.Datastore.Paginate<Test>("select from test", 1, -1, null);
            List<Test> results = pagedDatasource.Data.ToList();

            Console.WriteLine(results.Count);
        }

        [TestMethod]
        public void CreateTestRecord()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;

            Test test = new Test();
            test.path = "/path/and/one";
            test = request.Datastore.Create<Test>(test);
        }

        [TestMethod]
        public void DeleteRecord()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);

            IAccessToken token = request.User.Login();
            credentials.AccessToken = token;
            string id = "38611375";
            //delete
            request.Datastore.Delete(id);
        }

        //[TestMethod]
        //public void GetAndUpdateRecord()
        //{
        //    //ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
        //    //RevStackRequest request = RevStackClient.CreateRequest(credentials);

        //    //IAccessToken token = request.User.Login();
        //    //credentials.AccessToken = token;

        //    //string id = "9:2";
        //    //JObject document = request.Datastore.Get(id);
        //    //if (document == null)
        //    //    throw new NullReferenceException();

        //    //document["title"] = "updated...";
        //    //document.Add("title2", "added...");

        //    //request.Datastore.Update(document); 

        //    //promo giveaway
        //    string appId = "3be1897b-0fc0-47dc-83ab-9fb652fad5ac";
        //    string username = "host";
        //    string password = "host";
        //    ICredentials credentials = new Credentials(appId, username, password, null);
        //    RevStackRequest RevStackRequest = RevStackClient.CreateRequest(credentials);
        //    credentials.AccessToken = RevStackRequest.User.Login();
            
        //    string email = "dsheets@live.com";
        //    string firstname = "Jenna";
        //    string lastname = "Jones";
        //    JArray r = RevStackRequest.Datastore.Lookup("select from promo_vip_referral_giveaway_recipients where email = '" + email + "'", -1, -1);
        //    if (r.Count > 0)
        //    {
        //        foreach (JObject obj in r)
        //        {
        //            int i = int.Parse(obj["number_of_items_purchased"].ToString());
        //            i = i + 1;
        //            obj["number_of_items_purchased"] = i.ToString();
        //            RevStackRequest.Datastore.Update(obj);

        //            //
        //            JObject sender = (JObject) obj["sender"];
        //            string sender_email = sender["email"].ToString();
        //            int num_purchase = int.Parse(sender["number_of_items_purchased"].ToString());
        //            num_purchase = num_purchase + 1;
        //            sender["number_of_items_purchased"] = num_purchase.ToString();
        //            RevStackRequest.Datastore.Update(sender);

        //            //sender email
        //            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        //            client.Send(new System.Net.Mail.MailMessage("noreply@julielopezshoes.com", sender_email, "VIP Giveaway Order Notification", "This is a notification that your friend " + firstname + " " + lastname + " has placed an order at Julie Lopez Shoes. To date you have a total of " + num_purchase + " order(s) from friends."));

        //            //referral email
        //            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage("noreply@julielopezshoes.com", email, "VIP Referral Giveaway", "<a href='https://julielopezshoes.com/shop/Promo/VIPReferralGiveaway'><img src='http://media.r3vstack.com/assets/julielopezshoes/email.jpg' width='' height='' alt='' title='' align='left' border='0' /></a>");
        //            message.IsBodyHtml = true;
        //            client.Send(message);

        //        }
        //    }
        //    //end promo giveaway
        //}

        //[TestMethod]
        //public void QueryReviews()
        //{
        //    ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
        //    RevStackRequest request = RevStackClient.CreateRequest(credentials);
        //    IAccessToken token = request.User.Login();
        //    credentials.AccessToken = token;

        //    string query = "select from reviews";
        //    JArray reviews = request.Datastore.Lookup(query, -1, -1);
        //}
    }
}
