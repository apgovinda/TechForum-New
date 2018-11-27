using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DbAccess
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDbAccessService" in both code and config file together.
    [ServiceContract]
    public interface IDbAccessService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        List<Domain_Table> GetDomainList();

        [OperationContract]
        List<Technology_Table> GetTechforDomain(string domain);
    }

    [DataContract]
    public partial class Domain_Table
    {

    }

    [DataContract]
    public partial class Technology_Table
    {

    }
    [DataContract]
    public partial class Admin_Table
    {

    }
    [DataContract]
    public partial class Question_Bank_Table
    {


    }
    [DataContract]
    public partial class Test_Table
    {


    }
    [DataContract]
    public partial class Subscriber_Table
    {


    }
    [DataContract]
    public partial class Post_Table
    {


    }
}
