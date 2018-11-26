using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TamanhoFamilia.EasyServicing.ActionService
{
    public delegate void ExceptionEventHandler<T>(T origin, Exception exception, out bool willContinueRunning);

    public abstract class AbstractActionService : IService
    {
        #region Protected Properties
        protected int Workers { get; set; }
        protected ServiceState ServiceState { get; set; }
        #endregion

        #region Event Handlers
        public event EventHandler OnChangeState;
        public event ExceptionEventHandler<Thread> OnException;
        #endregion

        public int ActualWorkers()
        {
            return Workers;
        }

        public void AddWorkers(int workers = 1)
        {
            if (this.State() == ServiceState.RUNNING)
                this.Workers += workers;
            ReorganizeWorkers();
        }

        public void RemoveWorkers(int workers = 1)
        {
            Workers = Math.Max(0, Workers - workers);
            if (Workers == 0 && this.ServiceState != ServiceState.IDLE)
                this.ServiceState = ServiceState.STOPPING;
        }

        public void Restart()
        {
            Stop();
            while (State() != ServiceState.IDLE)
                Thread.Sleep(10);
            Start();
        }

        public void Start(int initialWorkers = 1)
        {
            Workers = initialWorkers;
            this.ServiceState = ServiceState.RUNNING;
            ReorganizeWorkers();
        }

        public ServiceState State()
            => this.ServiceState;

        public void Stop()
        {
            if (this.ServiceState != ServiceState.IDLE)
                this.ServiceState = ServiceState.STOPPING;
        }


        private List<Thread> threads = new List<Thread>();
        protected abstract void MyAction();
        private void ReorganizeWorkers()
        {
            while (threads.Count < this.Workers)
            {
                lock (threads)
                {
                    var t = new Thread(() =>
                    {
                        while (this.ServiceState == ServiceState.RUNNING)
                            try
                            {
                                MyAction();
                            }
                            catch (Exception ex)
                            {
                                if (ex is ThreadAbortException)
                                    throw;
                                bool breakIt;
                                OnException(Thread.CurrentThread, ex, out breakIt);
                                if (breakIt)
                                    break;
                            }
                        TellMeYourNotHere(Thread.CurrentThread);
                    });

                    threads.Add(t);
                    t.Start();
                }
            }
        }
        private void TellMeYourNotHere(Thread thread)
        {
            lock (threads)
            {
                threads.Remove(thread);
                if (threads.Count == 0)
                    this.ServiceState = ServiceState.IDLE;
            }
        }
    }

    public class ActionService : AbstractActionService
    {
        private Action Action { get; set; }
        public ActionService(Action action)
        {
            Action = action;
        }

        protected override void MyAction()
            => Action();
    }
}
