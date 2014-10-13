using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Types
{
    public abstract class RSBaseRecord
    {
        private string _rsClassName;

        #region Orient record specific properties

        public RSID RID { get; set; }

        public int RSVersion { get; set; }

        public short RSClassId { get; set; }

        public string RSClassName
        {
            get
            {
                if (string.IsNullOrEmpty(_rsClassName))
                {
                    return GetType().Name;
                }

                return _rsClassName;
            }

            set
            {
                _rsClassName = value;
            }
        }

        #endregion

        public RSDocument ToDocument()
        {
            return RSDocument.ToDocument(this);
        }
    }
}
