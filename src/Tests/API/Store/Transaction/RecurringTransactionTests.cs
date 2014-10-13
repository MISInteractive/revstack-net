using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.API.Store.Transaction
{
    [TestClass]
    public class RecurringTransactionTests
    {
        [TestMethod]
        public void Subscribe()
        {
            /*var document = {
                "currency": "usd",
                "payment_type": "recurring",
                "payment_method": "credit_card",
                "transaction_type": "subscribe",
                "amount": 55.00,
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
                },
                "recurring": {
                    "name": "RevStack Premium",
                    "description": "",
                    "billing_cycle": 1,
                    "billing_interval": "months",
                    "total_occurrences": 9999,
                    "start_date": start_date,
                    "trial_occurrences": null,
                    "trial_amount": null
                }
            };
            */
        }

        [TestMethod]
        public void UpdateSubscription()
        {
            /*var document = {
                "currency": "usd",
                "payment_type": "recurring",
                "payment_method": "credit_card",
                "transaction_type": "update_subscription",
                "amount": 45.00,
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
                },
                "recurring": {
                    "name": "RevStack Premium",
                    "description": "updated...",
                    "trial_occurrences": null,
                    "trial_amount": null
                },
                "transaction_id": "2224208"
            };
            */
        }

        [TestMethod]
        public void CancelSubscription()
        {
            /*var document = {
                "payment_type": "recurring",
                "transaction_type": "cancel_subscription",
                "payment_gateway": {
                    "code": "",
                    "api_login": "",
                    "api_key": "",
                    "live_mode": false
                },
                "transaction_id": "2224208"
            };*/
        }
    }
}
