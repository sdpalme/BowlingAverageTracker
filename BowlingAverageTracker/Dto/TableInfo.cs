using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingAverageTracker.Dto
{
    public class TableInfo : Object
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int notnull { get; set; }
        public string dflt_value { get; set; }
        public int pk { get; set; }

        override public bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            TableInfo t = obj as TableInfo;
            if ((Object)t == null)
            {
                return false;
            }
            return cid == t.cid &&
                ((name == null && t.name == null) || name.Equals(t.name)) &&
                ((type == null && t.name == null) || type.Equals(t.type)) &&
                notnull == t.notnull &&
                ((dflt_value == null && t.dflt_value == null) || dflt_value.Equals(t.dflt_value)) &&
                pk == t.pk;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + cid + notnull + pk + name.GetHashCode() +
                type.GetHashCode() + dflt_value.GetHashCode();
        }
    }
}
