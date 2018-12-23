using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TamanhoFamilia.EasyServicing.StrategyService;

namespace TamanhoFamilia.EasyServicingTest.src.StrategyService
{
    public class MyStrategy : IRunStrategy
    {
        public bool Continue { get; set; }

        public void Run()
        {
            while (Continue)
                Thread.Sleep(10);
        }
    }

    public class StrategyServiceTest : IServiceTest<EasyServicing.StrategyService.StrategyService<MyStrategy>>
    {
        public StrategyServiceTest() : base(new EasyServicing.StrategyService.StrategyService<MyStrategy>(new MyStrategy())) { }

        protected override void StartDelayedJob()
        {
            this.Service.RunStrategy.Continue = true;
        }

        protected override void StopDelayedJob()
        {
            this.Service.RunStrategy.Continue = false;
        }
    }
}
