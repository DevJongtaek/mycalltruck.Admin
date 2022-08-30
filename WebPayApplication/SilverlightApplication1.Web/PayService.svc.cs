using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SilverlightApplication1.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PayService
    {
        [OperationContract]
        public void DoWork()
        {
            // 여기에 작업 구현을 추가합니다.
            return;
        }

        // 여기에 작업을 추가하고 해당 작업을 [OperationContract]로 표시합니다.
    }
}
