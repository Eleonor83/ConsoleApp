using ConsoleApp.Enums;

namespace ConsoleApp.Interfaces
{
    public interface IServiceResult
    {
        object Result { get; set; }
        ServiceStatus Status { get; set; }
    }
}