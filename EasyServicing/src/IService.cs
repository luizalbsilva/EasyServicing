using System;
using System.Collections.Generic;
using System.Text;

namespace TamanhoFamilia.EasyServicing
{
    /// <summary>
    /// Service States
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        /// Not Running
        /// </summary>
        IDLE,
        /// <summary>
        /// Running
        /// </summary>
        RUNNING,
        /// <summary>
        /// Stopping all worked, when it`s done will be turned to IDLE
        /// </summary>
        STOPPING
    }

    /// <summary>
    /// Service
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Actual workers number.
        /// </summary>
        /// <returns>Actual workers number.</returns>
        int ActualWorkers();

        /// <summary>
        /// Add a number of workers
        /// </summary>
        /// <param name="workers">Add a number of workers. Default: 1</param>
        void AddWorkers(int workers = 1);

        /// <summary>
        /// Remove a number of workers. If turns 0, turns it state to IDLE. Default: 1
        /// </summary>
        /// <param name="workers"></param>
        void RemoveWorkers(int workers = 1);

        /// <summary>
        /// Restart a service.
        /// </summary>
        void Restart();

        /// <summary>
        /// Starts the service with a initial number of workers.
        /// </summary>
        /// <param name="workers">Initial number of workers. Default 1.</param>
        void Start(int initialWorkers = 1);

        /// <summary>
        /// Actual service state.
        /// </summary>
        /// <returns>Actual Service State</returns>
        ServiceState State();

        /// <summary>
        /// Stop the service.
        /// </summary>
        void Stop();
    }
}
