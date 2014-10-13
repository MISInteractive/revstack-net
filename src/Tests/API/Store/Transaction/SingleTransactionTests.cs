using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevStack.Client.API;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace Tests.API.Store.Transaction
{
    [TestClass]
    public class SingleTransactionTests
    {
        [TestMethod]
        public void AuthorizeAndCapture()
        {
            ICredentials credentials = new Credentials(Constants.APP_ID, Constants.USERNAME, Constants.PASSWORD, null);
            RevStackRequest request = RevStackClient.CreateRequest(credentials);
            IAccessToken token = request.Account.Login();
            credentials.AccessToken = token; //store token

            /*
             var document = {
                "currency": "usd",					
                "payment_type": "single",				
                "payment_method": "credit_card",			
                "transaction_type": "charge",				
                "amount":  45.00, 
                "credit_card": {					
                    "number": "4007000000027",
                    "type": "visa",					
                    "exp_month": 01,					
                    "exp_year": 2016,					
                    "cvv": "327",
                    "country": "US",
                    "name": "Bob Banks",				
                    "address": null,
                    "address_2": null,
                    "city": null,
                    "state": null,
                    "zip": null
               },
               "payment_gateway": {				
                   "code": "",				
                   "api_login": "", 
                   "api_key": "",
                   "live_mode": false
                }
            };
             */

            string gateway = Constants.GATEWAY;
            string api_login = Constants.API_LOGIN;
            string api_key = Constants.API_KEY;

            string json = "{ 'currency': 'usd', 'payment_type': 'single', 'payment_method': 'credit_card', 'transaction_type': 'charge', 'amount':  45.00, 'credit_card': { 'number': '4007000000027', 'type': 'visa', 'exp_month': 01, 'exp_year': 2016, 'cvv': '327', 'country': 'US', 'name': 'Bob Banks', 'address': null, 'address_2': null, 'city': null, 'state': null, 'zip': null }, 'payment_gateway': { 'code': '" + gateway + "', 'api_login': '" + api_login + "', 'api_key': '" + api_key + "', 'live_mode': false } }";
            JObject trans = JObject.Parse(json);
            JObject transaction = request.Transaction.Create(trans);
        }

        [TestMethod]
        public void Authorize()
        {
            /*
             var document = {
                "currency": "usd",					
                "payment_type": "single",				
                "payment_method": "credit_card",			
                "transaction_type": "authorize",				
                "amount":  55.00, 
                "credit_card": {					
                    "number": "4007000000027",
                    "type": "visa",					
                    "exp_month": 01,					
                    "exp_year": 2016,					
                    "cvv": "327",
                    "country": "US",
                    "name": "Bob Banks",				
                    "address": null,
                    "address_2": null,
                    "city": null,
                    "state": null,
                    "zip": null
               },
               "payment_gateway": {				
                   "code": "",				
                   "api_login": "", 
                   "api_key": "",
                   "live_mode": false
                }
            };
             */
        }

        [TestMethod]
        public void Capture()
        {
            /*var document = {
                "payment_type": "single",
                "payment_method": "credit_card",
                "transaction_id": "2219131482",
                "transaction_type": "capture",
                "amount": 10.00,
                "payment_gateway": {
                    "code": "",
                    "api_login": "",
                    "api_key": "",
                    "test_mode": true
                }
            };
            */
        }

        [TestMethod]
        public void Refund()
        {
            /*var document = {
                "payment_type": "single",
                "payment_method": "credit_card",
                "transaction_id": "2217930700",
                "transaction_type": "refund",
                "amount": 8.00,
                "credit_card_number": "4007000000027",
                "payment_gateway": {
                    "code": "",
                    "api_login": "",
                    "api_key": "",
                    "test_mode": true
                }
            };
            */
        }

        [TestMethod]
        public void Void()
        {
            /*var document = {
                "payment_type": "single",
                "payment_method": "credit_card",
                "transaction_id": "2219132620",
                "transaction_type": "void",
                "payment_gateway": {
                    "code": "",
                    "api_login": "",
                    "api_key": "",
                    "test_mode": true
                }
            };
            */
        }
    }
}
