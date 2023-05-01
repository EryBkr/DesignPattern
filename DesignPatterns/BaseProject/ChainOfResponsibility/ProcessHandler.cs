using System;

namespace BaseProject.ChainOfResponsibility
{
    public abstract class ProcessHandler : IProcessHandler
    {
        private IProcessHandler nextProcessHandler;

        //Çalışacak metot
        public virtual object Handle(Object objet)
        {
            //Bir sonraki adımda çalışması gereken bir iş var mı,varsa çalıştır
            if (nextProcessHandler != null)
                return nextProcessHandler.Handle(objet);

            return null;
        }

        //Bir sonraki işin tanımı
        public IProcessHandler SetNext(IProcessHandler processHandler)
        {
            nextProcessHandler = processHandler;
            return nextProcessHandler;
        }
    }
}
