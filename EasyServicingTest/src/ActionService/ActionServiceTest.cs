using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TamanhoFamilia.EasyServicing.ActionService;

namespace TamanhoFamilia.EasyServicingTest.ActionService
{
    #region AbstractActionServiceTest
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
    #endregion

    #region ActionServiceTest
    public class ActionServiceTest : IServiceTest<EasyServicing.ActionService.ActionService>
    {
        public static bool Continue { get; set; }

        public ActionServiceTest() : base(new EasyServicing.ActionService.ActionService(() =>
        {
            while (ActionServiceTest.Continue)
                Thread.Sleep(10);
        })) { }

        protected override void StartDelayedJob()
        {
            ActionServiceTest.Continue = true;
        }

        protected override void StopDelayedJob()
        {
            ActionServiceTest.Continue = false;
        }
    }
    #endregion
}