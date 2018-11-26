using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TamanhoFamilia.EasyServicing;
using Xunit;

namespace TamanhoFamilia.EasyServicingTest
{
    /// <summary>
    /// Generic tests implementation
    /// </summary>
    /// <typeparam name="T">Implementation been tested</typeparam>
    public abstract class IServiceTest<T> where T : IService
    {
        /// <summary>
        /// Service implementation been tested
        /// </summary>
        protected T Service { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">Instance been tested</param>
        public IServiceTest(T service)
        {
            this.Service = service;
        }

        /// <summary>
        /// Start a job doesn't stops until asked for (#StopDelayedJob())
        /// </summary>
        protected abstract void StartDelayedJob();

        /// <summary>
        /// Stops the job started by #StartDelayedJob() method
        /// </summary>
        protected abstract void StopDelayedJob();

        #region IService Method Testings

        #region ActualWorkers
        [Fact]
        public virtual void ActualWorkers_Started()
        {
            try
            {
                this.Service.Start();
                Assert.Equal(1, this.Service.ActualWorkers());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void ActualWorkers_NotStartedService()
        {
            Assert.Equal(0, this.Service.ActualWorkers());
        }
        #endregion

        #region AddWorkers
        [Fact]
        public virtual void AddWorkers_Started()
        {
            try
            {
                this.Service.Start();
                this.Service.AddWorkers(2);
                Assert.Equal(3, this.Service.ActualWorkers());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void AddWorkers_NotStarted()
        {
            this.Service.AddWorkers(2);
            Assert.Equal(0, this.Service.ActualWorkers());
        }
        #endregion

        #region RemoveWorkers
        [Fact]
        public virtual void RemoveWorkers_StartedServiceWithHighThreadNumber()
        {
            try
            {
                this.Service.Start(5);
                this.Service.RemoveWorkers(2);
                Thread.Sleep(100);
                Assert.Equal(3, this.Service.ActualWorkers());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void RemoveWorkers_StartedServiceLowThreadNumberZerosWorkers()
        {
            try
            {
                this.Service.Start(5);
                this.Service.RemoveWorkers(6);
                Thread.Sleep(100);
                Assert.Equal(0, this.Service.ActualWorkers());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void RemoveWorkers_StartedServiceLowThreadNumberTurnsStarteToIDLE()
        {
            try
            {
                this.Service.Start(5);
                this.Service.RemoveWorkers(6);
                Thread.Sleep(10000);
                Assert.Equal(ServiceState.IDLE, this.Service.State());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void RemoveWorkers_NotStartedServiceWithHighThreadNumber()
        {
            this.Service.RemoveWorkers(2);
            Assert.Equal(0, this.Service.ActualWorkers());
        }

        [Fact]
        public virtual void RemoveWorkers_NotStartedServiceLowThreadNumberZerosWorkers()
        {
            this.Service.RemoveWorkers(6);
            Assert.Equal(0, this.Service.ActualWorkers());
        }

        [Fact]
        public virtual void RemoveWorkers_NotStartedServiceLowThreadNumberTurnsStarteToIDLE()
        {
            this.Service.RemoveWorkers(6);
            Assert.Equal(ServiceState.IDLE, this.Service.State());
        }
        #endregion

        #region Restart
        [Fact]
        public virtual void Restart_StoppedServiceChangesStatus()
        {
            Timeout(200, () => {
                this.Service.Restart();
                Assert.Equal(ServiceState.RUNNING, this.Service.State());
            });
        }

        private async void Timeout(int time, Action action)
        {
            var timeoutTask = Task.Delay(time);
            var actionTask = Task.Run(action);
            var r = await Task.WhenAny(actionTask, timeoutTask)   ;
            Assert.NotEqual(timeoutTask, r);
        }

        [Fact]
        public virtual void Restart_StartedServiceMantainsNumberOfWorkers()
        {
            Timeout(2000, () =>
            {
                try
                {
                    this.Service.Start(4);
                    this.Service.Restart();
                    Assert.Equal(4, this.Service.ActualWorkers());
                }
                finally
                {
                    this.Service.Stop();
                }
            });
        }

        [Fact]
        public virtual void Restart_StartedServiceAsksForStoppingService()
        {
            Timeout(2000, () =>
            {
                try
                {
                    this.Service.Start(4);
                    this.StartDelayedJob();
                    new Thread(() => this.Service.Restart()).Start();
                    Thread.Sleep(10);
                    Assert.Equal(ServiceState.STOPPING, this.Service.State());
                    this.StopDelayedJob();
            }
            finally
                {
                    this.Service.Stop();
                }
            });
        }

        [Fact]
        public virtual void Restart_StartedServiceCheckFinalStatus()
        {
            Timeout(2000, () => 
            {
                int c = 0;
                if (c == 0)
                    throw new Exception("Dados");
                try
                {
                    this.Service.Start(4);
                    this.Service.Restart();
                    Assert.Equal(ServiceState.RUNNING, this.Service.State());
                }
                finally
                {
                    this.Service.Stop();
                }
            });
        }

        #endregion

        #region State
        [Fact]
        public virtual void InitialStatus()
        {
            Assert.Equal(ServiceState.IDLE, this.Service.State());
        }
        #endregion

        #region Start
        [Fact]
        public virtual void Start_ChangesStatusToRunning()
        {
            try
            {
                this.Service.Start();
                Assert.Equal(ServiceState.RUNNING, this.Service.State());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void Start_CreatesNumberOfWorkers()
        {
            try
            {
                this.Service.Start(5);
                Assert.Equal(5, this.Service.ActualWorkers());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void Start_AlreadyStartedDontChangeNumberOfWorkers()
        {
            try
            {
                this.Service.Start(5);
                this.Service.Start(5);
                Assert.Equal(5, this.Service.ActualWorkers());
            }
            finally
            {
                this.Service.Stop();
            }
        }

        [Fact]
        public virtual void Start_AlreadyStartedKeepsStatusToRunning()
        {
            try
            {
                this.Service.Start();
                this.Service.Start();
                Assert.Equal(ServiceState.RUNNING, this.Service.State());
            }
            finally
            {
                this.Service.Stop();
            }
        }
        #endregion

        #region Stop
        [Fact]
        public virtual void Stop_ChangesRunningStatsToStopping()
        {
            this.Service.Start();
            try
            {
                StartDelayedJob();
                this.Service.Stop();
                Assert.Equal(ServiceState.STOPPING, this.Service.State());
            }
            finally
            {
                StopDelayedJob();
            }
        }

        [Fact]
        public virtual void Stop_ChangesRunningStatsToIDLEWhenJobsAreDone()
        {
            this.Service.Start();
            try
            {
                StartDelayedJob();
                this.Service.Stop();
            }
            finally
            {
                StopDelayedJob();
                Thread.Sleep(10);
            }
            Assert.Equal(ServiceState.IDLE, this.Service.State());
        }

        [Fact]
        public virtual void Stop_IDLEService()
        {
            this.Service.Stop();
            StopDelayedJob();
            Assert.Equal(ServiceState.IDLE, this.Service.State());
        }
        #endregion

        #endregion
    }

    public class IServiceTestTest : IServiceTest<IService>
    {
        public IServiceTestTest() : base(Substitute.For<IService>())
        { }

        protected override void StartDelayedJob()
        {
        }

        protected override void StopDelayedJob()
        {
        }

        public override void ActualWorkers_Started()
        {
            base.Service.ActualWorkers().Returns(1);

            base.ActualWorkers_Started();
        }

        public override void AddWorkers_Started()
        {
            base.Service.ActualWorkers().Returns(3);

            base.AddWorkers_Started();
        }

        public override void RemoveWorkers_StartedServiceWithHighThreadNumber()
        {
            base.Service.ActualWorkers().Returns(3);

            base.RemoveWorkers_StartedServiceWithHighThreadNumber();
        }

        public override void Restart_StartedServiceAsksForStoppingService()
        {
            this.Service.State().Returns(ServiceState.STOPPING);

            base.Restart_StartedServiceAsksForStoppingService();
        }

        public override void Restart_StartedServiceCheckFinalStatus()
        {
            this.Service.State().Returns(ServiceState.RUNNING);

            base.Restart_StartedServiceCheckFinalStatus();
        }

        public override void Restart_StartedServiceMantainsNumberOfWorkers()
        {
            this.Service.ActualWorkers().Returns(4);

            base.Restart_StartedServiceMantainsNumberOfWorkers();
        }

        public override void Start_AlreadyStartedDontChangeNumberOfWorkers()
        {
            this.Service.ActualWorkers().Returns(5);
            base.Start_AlreadyStartedDontChangeNumberOfWorkers();
        }

        public override void Start_AlreadyStartedKeepsStatusToRunning()
        {
            this.Service.State().Returns(ServiceState.RUNNING);
            base.Start_AlreadyStartedKeepsStatusToRunning();
        }

        public override void Start_ChangesStatusToRunning()
        {
            this.Service.State().Returns(ServiceState.RUNNING);
            base.Start_ChangesStatusToRunning();
        }

        public override void Start_CreatesNumberOfWorkers()
        {
            this.Service.ActualWorkers().Returns(5);
            base.Start_CreatesNumberOfWorkers();
        }

        public override void Stop_ChangesRunningStatsToStopping()
        {
            this.Service.State().Returns(ServiceState.STOPPING);
            base.Stop_ChangesRunningStatsToStopping();
        }

    }
}
