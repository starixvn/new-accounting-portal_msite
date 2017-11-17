namespace Fanex.Maintenance.Service.Interface.ServiceMonitoring
{
    using System.Collections.Generic;
    using System.ServiceProcess;
    using Fanex.ServiceManagement.Model;

    public interface IServiceManagement
    {
        bool TestConnection();

        List<ServiceViewModel> GetServices(string serviceName, int status);

        ServiceController GetServiceByName(string serviceName);

        void StartService(string serviceName);

        void StopService(string serviceName);

        void RestartService(string serviceName);

        List<string> GetServiceConfigs(string serviceName);

        string GetServiceConfigFile(string serviceName, string fileName);

        bool SaveServiceConfigFile(string serviceName, string fileName, string fileText);

        string GetEtag(List<ServiceViewModel> servicesList);
    }
}
