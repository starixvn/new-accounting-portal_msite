namespace Fanex.Maintenance.Repository.Interface.ServiceMonitoring
{
    using System.Collections.Generic;
    using System.ServiceProcess;
    using Fanex.ServiceManagement.Model;

    public interface IServiceManagementRepository
    {
        bool TestConnection();

        List<ServiceViewModel> GetServices();

        ServiceController GetServiceByName(string serviceName);

        void StartService(ServiceController service);

        void StopService(ServiceController service);

        void RestartService(ServiceController service);

        List<string> GetServiceConfigs(string serviceName);

        string GetServiceConfigFile(string serviceName, string fileName);

        bool SaveServiceConfigFile(string serviceName, string fileName, string fileText);

        string GetEtag(List<ServiceViewModel> servicesList);
    }
}
