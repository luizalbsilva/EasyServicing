using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TamanhoFamilia.EasyServicing
{
    /// <summary>
    /// Congregador de Serviços
    /// </summary>
    public class ServiceFacade
    {
        /// <summary>
        /// Jobs
        /// </summary>
        private Dictionary<string, IService> _job = new Dictionary<string, IService>();


        /// <summary>
        /// Retorna o IJObService correspondente ao nome
        /// </summary>
        /// <param name="name">Nome do Serviço</param>
        /// <returns></returns>
        private IService _GetJob(string name)
        {
            if (_job.ContainsKey(name))
                return _job[name];
            else
                return DefaultJobService.Instance;
        }

        public int JobServicesCount { get { return _job.Count; } }

        #region Definidos na interface IJobService
        /// <summary>
        /// Retorna a quantidade de trabalhadores
        /// </summary>
        /// <param name="name">Nome do Serviço</param>
        /// <returns></returns>
        public int ActualWorkers(string name)
            => _GetJob(name).ActualWorkers();


        /// <summary>
        /// Adiciona um determinado número de trabalhadores.
        /// </summary>
        /// <param name="workers">Número de trabalhadores. Default: 1</param>
        public void AddWorkers(string name, int workers = 1)
            => _GetJob(name).AddWorkers(workers);


        /// <summary>
        /// Remove uma determinada quantidade de trabalhadores. Default: 1
        /// </summary>
        /// <param name="workers"></param>
        public void RemoveWorkers(string name, int workers = 1)
            => _GetJob(name).RemoveWorkers(workers);


        /// <summary>
        /// Reestarta o serviço
        /// </summary>
        /// <param name="name">Restarta o serviço</param>
        /// <returns></returns>
        public void Restart(string name)
            => _GetJob(name).Restart();


        /// <summary>
        /// Inicia o Serviço
        /// </summary>
        /// <param name="name">Nome do serviço</param>
        /// <param name="workers">Número de trabalhadores</param>
        public void Start(string name, int workers = 1)
            => _GetJob(name).Start(workers);


        /// <summary>
        /// Retorna o estado do serviço
        /// </summary>
        /// <param name="name">Nome do Serviço</param>
        /// <returns></returns>
        public ServiceState State(string name)
            => _GetJob(name).State();


        /// <summary>
        /// Para o serviço
        /// </summary>
        /// <param name="name">Nome do serviço</param>
        public void Stop(string name)
            => _GetJob(name).Stop();
        #endregion


        /// <summary>
        /// ADiciona um novo serviço
        /// </summary>
        /// <param name="name">Identificador do serviço</param>
        /// <param name="jobService"></param>
        public void AddJobService(string name, IService jobService)
            => _job.Add(name, jobService);


        /// <summary>
        /// Lista o nome de todos os serviços
        /// </summary>
        /// <returns></returns>
        public List<string> ListServices()
        {
            return _job.Keys.ToList();
        }


        /// <summary>
        /// Remove o Serviço
        /// </summary>
        /// <param name="name">Nome do Serviço</param>
        public void RemoveJobService(string name)
            => _job.Remove(name);


        /// <summary>
        /// Para todos os serviços
        /// </summary>
        public void StopAll()
            => Parallel.ForEach(_job.Values, a => a.Stop());

        /// <summary>
        /// Classe privada
        /// </summary>
        private class DefaultJobService : IService
        {
            #region Local SingleInstance
            /// <summary>
            /// Construtor privado para garantir singleton
            /// </summary>
            private DefaultJobService() { }


            /// <summary>
            /// Única instância
            /// </summary>
            private static DefaultJobService _instance = new DefaultJobService();


            /// <summary>
            /// Pega única a instância existente
            /// </summary>
            public static DefaultJobService Instance { get { return _instance; } }
            #endregion

            #region Métodos Públicos

            /// <summary>
            /// Número de trabalhadores atual
            /// </summary>
            /// <returns></returns>
            public int ActualWorkers() => 0;

            /// <summary>
            /// Adiciona um determinado número de trabalhadores.
            /// </summary>
            /// <param name="workers">Número de trabalhadores. Default: 1</param>
            public void AddWorkers(int workers = 1) { }

            /// <summary>
            /// Remove uma determinada quantidade de trabalhadores. Default: 1
            /// </summary>
            /// <param name="workers"></param>
            public void RemoveWorkers(int workers = 1) { }

            /// <summary>
            /// Reinicia o Serviço
            /// </summary>
            public void Restart() { }

            /// <summary>
            /// Inicia o Serviço, com um determinado número de trabalhadores
            /// </summary>
            /// <param name="workers"></param>
            public void Start(int workers = 1) { }

            /// <summary>
            /// Retorna o Estado do Serviço
            /// </summary>
            /// <returns></returns>
            public ServiceState State() => ServiceState.IDLE;

            /// <summary>
            /// Para o serviço
            /// </summary>
            public void Stop() { }
            #endregion
        }
    }
}
