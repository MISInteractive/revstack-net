using RevStack.Client.API.Account;
using RevStack.Client.API.App;
using RevStack.Client.API.Datastore;
using RevStack.Client.API.Membership;
using RevStack.Client.API.Role;
using RevStack.Client.API.Storage;
using RevStack.Client.API.Store.Transaction;
using RevStack.Client.API.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API
{
    public class RevStackRequest
    {
        #region Private members and constructors
        private ClientType ClientType { get; set; }
        private ICredentials Credentials { get; set; }
        private string Host { get; set; }
        private int Version { get; set; }

        private TransactionService _transaction;
        public TransactionService Transaction
        {
            get
            {
                return _transaction;
            }
        }

        private DatastoreService _datastore;
        public DatastoreService Datastore
        { 
            get 
            {
                return _datastore;
            } 
        }

        private AccountService _account;
        public AccountService Account
        {
            get
            {
                return _account;
            }
        }

        private AppService _app;
        public AppService App
        {
            get
            {
                return _app;
            }
        }

        private UserService _user;
        public UserService User
        {
            get
            {
                return _user;
            }
        }

        private RoleService _role;
        public RoleService Role
        {
            get
            {
                return _role;
            }
        }

        private MembershipService _membership;
        public MembershipService Membership
        {
            get
            {
                return _membership;
            }
        }

        private StorageService _storage;
        public StorageService Storage
        {
            get
            {
                return _storage;
            }
        }
        #endregion

        internal RevStackRequest(ICredentials credentials, ClientType clientType)
        {
            this.Init("", 0, credentials, clientType);
        }

        internal RevStackRequest(string host, int version, ICredentials credentials, ClientType clientType) 
        {
            this.Init(host, version, credentials, clientType);
        }

        private void Init(string host, int version, ICredentials credentials, ClientType clientType) 
        {
            if (credentials == null)
                throw new ArgumentNullException("Credentials are required.");
            
            this.ClientType = clientType;
            this.Credentials = credentials;
            this.Host = host;
            this.Version = version;

            //load Services
            switch (clientType) {
                case ClientType.Http:
                    //set http defaults
                    if (string.IsNullOrEmpty(this.Host))
                        this.Host = RevStack.Client.Http.Constants.API_HOST_URL;
                    if (this.Version == 0 || this.Version == -1)
                        this.Version = int.Parse(RevStack.Client.Http.Constants.API_VERSION);
                    //account
                    Http.Account.Account account = new Http.Account.Account(this.Host, this.Version, this.Credentials);
                    _account = new AccountService(account);
                    //app
                    Http.App.App app = new Http.App.App(this.Host, this.Version, this.Credentials);
                    _app = new AppService(app);
                    //datastore
                    Http.Datastore.Datastore datastore = new Http.Datastore.Datastore(this.Host, this.Version, this.Credentials);
                    _datastore = new DatastoreService(datastore);
                    //user
                    Http.User.User user = new Http.User.User(this.Host, this.Version, this.Credentials);
                    _user = new User.UserService(user);
                    //role
                    Http.Role.Role role = new Http.Role.Role(this.Host, this.Version, this.Credentials);
                    _role = new RoleService(role);
                    //membership
                    Http.Membership.Membership membership = new Http.Membership.Membership(this.Host, this.Version, this.Credentials);
                    _membership = new MembershipService(membership);
                    //storage
                    Http.Storage.Storage storage = new Http.Storage.Storage(this.Host, this.Version, this.Credentials);
                    _storage = new Storage.StorageService(storage);
                    //transaction
                    Http.Store.Transaction.Transaction transaction = new Http.Store.Transaction.Transaction(this.Host, this.Version, this.Credentials);
                    _transaction = new Store.Transaction.TransactionService(transaction);
                break;
            }
        }


    }
}
