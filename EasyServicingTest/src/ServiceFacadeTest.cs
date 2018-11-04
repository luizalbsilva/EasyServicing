using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using TamanhoFamilia.EasyServicing;
using Xunit;

namespace TamanhoFamilia.EasyServicingTest
{
    public class ServiceFacadeTest
    {
        #region Inicialization
        private ServiceFacade facade = new ServiceFacade();
        private IService[] services = new IService[] { Substitute.For<IService>(), Substitute.For<IService>() };
        public ServiceFacadeTest()
        {
            for (int x = 0; x < services.Length; x++)
                facade.AddJobService($"{x}", services[x]);
        }
        #endregion

        #region Definidos na interface IJobService

        #region ActualWorkers()
        [Fact]
        public void ActualWorkers_RegistroExistente()
        {
            facade.ActualWorkers("0").Returns(10);

            Assert.Equal(10, facade.ActualWorkers("0"));

            services[0].Received().ActualWorkers();
            services[1].DidNotReceive().ActualWorkers();
        }


        [Fact]
        public void ActualWorkers_RegistroInexistente()
        {
            facade.ActualWorkers("10");

            services[0].DidNotReceive().ActualWorkers();
            services[1].DidNotReceive().ActualWorkers();
        }


        [Fact]
        public void ActualWorkers_FacadeVazio()
        {
            facade = new ServiceFacade();
            facade.ActualWorkers("0");
        }
        #endregion

        #region AddWorkers
        [Fact]
        public void AddWorkers_existing_index()
        {
            facade.AddWorkers("0", 2);

            services[0].Received().AddWorkers(2);
            services[1].DidNotReceive().AddWorkers(Arg.Any<int>());
        }

        [Fact]
        public void AddWorkers_inexisting_index()
        {
            facade.AddWorkers("10", 2);

            foreach (var js in services)
                js.DidNotReceive().AddWorkers(Arg.Any<int>());
        }
        #endregion

        #region RemoveWorkers()
        [Fact]
        public void RemoveWorkers_existing_index()
        {
            facade.RemoveWorkers("0");
            services[0].Received().RemoveWorkers(1);
        }

        [Fact]
        public void RemoveWorkers_inexisting_index()
        {
            facade.RemoveWorkers("10");

            foreach (var js in services)
                js.DidNotReceive().RemoveWorkers(Arg.Any<int>());
        }
        #endregion

        #region Restart()
        [Fact]
        public void Restart_RegistroExistente()
        {
            facade.Restart("0");

            services[0].Received().Restart();
            services[1].DidNotReceive().Restart();
        }

        [Fact]
        public void Restart_RegistroInexistente()
        {
            facade.Restart("10");

            services[0].DidNotReceive().Restart();
            services[1].DidNotReceive().Restart();
        }

        [Fact]
        public void Restart_FacadeVazio()
        {
            facade = new ServiceFacade();
            facade.Restart("0");
        }
        #endregion

        #region Metodo Start()
        [Fact]
        public void Start_RegistroExistente()
        {
            facade.Start("0");

            services[0].Received().Start(1);
            services[1].DidNotReceive().Start(Arg.Any<int>());
        }

        [Fact]
        public void Start_RegistroInexistente()
        {
            facade.Start("10");

            services[0].DidNotReceive().Start(Arg.Any<int>());
            services[1].DidNotReceive().Start(Arg.Any<int>());
        }

        [Fact]
        public void Start_FacadeVazio()
        {
            facade = new ServiceFacade();
            facade.Start("0");
        }
        #endregion

        #region State()
        [Fact]
        public void State_RegistroExistente()
        {
            facade.State("0").Returns(ServiceState.RUNNING);

            Assert.Equal(ServiceState.RUNNING, facade.State("0"));

            services[0].Received().State();
            services[1].DidNotReceive().State();
        }


        [Fact]
        public void State_RegistroInexistente()
        {
            facade.State("10");

            services[0].DidNotReceive().State();
            services[1].DidNotReceive().State();
        }

        [Fact]
        public void State_FacadeVazio()
        {
            facade = new ServiceFacade();
            facade.State("0");
        }
        #endregion

        #region Stop()
        [Fact]
        public void Stop_RegistroExistente()
        {

            facade.Stop("0");

            services[0].Received().Stop();
            services[1].DidNotReceive().Stop();
        }


        [Fact]
        public void Stop_RegistroInexistente()
        {
            facade.Stop("10");

            services[0].DidNotReceive().Stop();
            services[1].DidNotReceive().Stop();
        }

        [Fact]
        public void Stop_FacadeVazio()
        {
            facade = new ServiceFacade();
            facade.Stop("0");
        }
        #endregion

        #endregion

        #region AddJobService
        [Fact]
        public void AddJobService()
        {
            facade = new ServiceFacade();
            Assert.Empty(facade.ListServices());
            string key = "Zero:Zero:Zero";
            facade.AddJobService(key, Substitute.For<IService>());

            Assert.Single(facade.ListServices());
            Assert.Equal(key, facade.ListServices()[0]);
        }
        #endregion

        #region ListServices
        [Fact]
        public void ListServices_RetornoDaListaDeServicos()
        {
            for (int a = 0; a < services.Length; a++)
                Assert.Contains(Convert.ToString(a), facade.ListServices());
            Assert.Equal(services.Length, facade.ListServices().Count);
        }
        #endregion

        #region RemoveJobService()
        [Fact]
        public void RemoveJobService_ServicoExistente()
        {
            facade.RemoveJobService("0");

            Assert.Equal(1, facade.JobServicesCount);
        }
        #endregion

        #region StopAll()
        [Fact]
        public void StopAll_RegistrosExistentes()
        {

            facade.StopAll();

            services[0].Received().Stop();
            services[1].Received().Stop();
        }


        [Fact]
        public void StopAll_FacadeVazio()
        {
            facade = new ServiceFacade();
            facade.StopAll();
        }
        #endregion

        #region JobServicesCount()
        [Fact]
        public void JobServicesCount_ServicosExistente()
        {
            Assert.Equal(services.Length, facade.JobServicesCount);
        }

        [Fact]
        public void JobServicesCount_Vazio()
        {
            facade = new ServiceFacade();
            Assert.Equal(0, facade.JobServicesCount);
        }
        #endregion
    }
}
