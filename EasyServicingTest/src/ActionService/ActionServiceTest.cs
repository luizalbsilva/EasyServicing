using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TamanhoFamilia.EasyServicing.ActionService;

namespace TamanhoFamilia.EasyServicingTest.ActionService
{
    public class LocalAbstractActionService : AbstractActionService
    {
        public bool Continue { get; set; }

        protected override void MyAction()
        {
            while(Continue)
                Thread.Sleep(10);
        }
    }


    public class AbstractActionServiceTest : IServiceTest<LocalAbstractActionService>
    {
        public AbstractActionServiceTest() : base(new LocalAbstractActionService()) { }

        protected override void StartDelayedJob()
        {
            Service.Continue = true;
        }

        protected override void StopDelayedJob()
        {
            Service.Continue = false;
        }
    }
}
