using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Types
{
    public class RSID
    {
        public short ClusterId { get; set; }
        public long ClusterPosition { get; set; }
        public string RID
        {
            get
            {
                return "#" + ClusterId + ":" + ClusterPosition;
            }

            set
            {
                string[] split = value.Split(':');

                ClusterId = short.Parse(split[0].Substring(1));
                ClusterPosition = long.Parse(split[1]);
            }
        }

        public RSID()
        {

        }

        public RSID(short clusterId, long clusterPosition)
        {
            ClusterId = clusterId;
            ClusterPosition = clusterPosition;
        }

        public RSID(string orid)
        {
            string[] split = orid.Split(':');

            ClusterId = short.Parse(split[0].Substring(1));
            ClusterPosition = long.Parse(split[1]);
        }

        public override string ToString()
        {
            return RID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // if parameter cannot be cast to ORID return false.
            RSID orid = obj as RSID;

            if ((System.Object)orid == null)
            {
                return false;
            }

            return (this.ToString() == orid.ToString());
        }

        public override int GetHashCode()
        {
            return (int)ClusterId + (int)ClusterPosition;
        }
    }
}
