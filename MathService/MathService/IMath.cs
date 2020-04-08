using System.ServiceModel;

namespace MathService
{
    [ServiceContract]
    public interface IMath
    {
        [OperationContract]
        bool Prime(int value);
    }
}
