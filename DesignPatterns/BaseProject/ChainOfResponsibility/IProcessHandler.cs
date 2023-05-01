using System;

namespace BaseProject.ChainOfResponsibility
{
    //CoR design pattern'nin amacı ardışık işleri sıraya eklemek ve ilk çağırıldığı yerden soyutlamaktır
    public interface IProcessHandler
    {
        //İş bitimi yapılacak bir sonraki çağrımı temsil ediyor
        IProcessHandler SetNext(IProcessHandler processHandler);

        //Execute işlemi
        object Handle(Object objet);
    }
}
