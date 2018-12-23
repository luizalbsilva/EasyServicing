using System;
using System.Collections.Generic;
using System.Text;
using TamanhoFamilia.EasyServicing.ActionService;

namespace TamanhoFamilia.EasyServicing.StrategyService
{
    public class StrategyService<T> : AbstractActionService where T : IRunStrategy
    {
        public T RunStrategy { get; private set; }
        public StrategyService(T runStrategy)
        {
            this.RunStrategy = runStrategy;
        }

        protected sealed override void MyAction()
        {
            this.RunStrategy.Run();
        }
    }
}
