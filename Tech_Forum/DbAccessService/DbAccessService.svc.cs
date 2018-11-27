using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DbAccess
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DbAccessService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DbAccessService.svc or DbAccessService.svc.cs at the Solution Explorer and start debugging.
    public class DbAccessService : IDbAccessService
    {
        public List<Domain_Table> GetDomainList()
        {
            using (DbAccessEntity db = new DbAccessEntity())
            {
                List<Domain_Table> DomainList = db.Domain_Table.ToList();
                return DomainList;
            }

        }

        public List<Technology_Table> GetTechforDomain(string domain)
        {
            using (DbAccessEntity db = new DbAccessEntity())
            {
                int selecteddomain = Convert.ToInt32(domain);
                List<Technology_Table> TechnologyList = db.Technology_Table.Where(x => x.did == selecteddomain).ToList();
                return TechnologyList;
            }

        }
        public void DoWork()
        {
        }
    }
}
