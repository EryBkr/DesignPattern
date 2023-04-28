using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BaseProject.Commands
{
    //Invoker nihayi olarak çağıracağımız bir yapıdır,CommandConcrete'lerın hepsi bunun aracılığıyla işletilir
    //Bu arkadaş hangi metod instance'na ihtiyacımız varsa onu set edecek
    //Command Design Pattern'ın amacı method düzeyinde kapsülleme yapmaktır
    //Invoker olmadan da command'ları çalıştırabilir Invoker iş yapmıyor gibi görünebilir fakat farklı farklı çalışacak olan metotları organize eder,burada toplu olarak komutları biriktirip çağırabiliyor mesela Example:CreateFiles metodu
    public class FileCreateInvoker
    {
        private ITableActionCommand _tableActionCommand;

        //Belki bir den fazla türde işlem yapacağız yani hem Excel hem de PDF çıktısı istiyoruz,olabilir
        private List<ITableActionCommand> tableActionCommands = new List<ITableActionCommand>();

        public void SetCommand(ITableActionCommand command)
        {
            _tableActionCommand = command;
        }

        public void AddCommand(ITableActionCommand command)
        {
            tableActionCommands.Add(command);
        }

        //Tek dosya için
        public IActionResult CreateFile()
        {
            return _tableActionCommand.Execute();
        }

        //Birden fazla dosya için
        public List<IActionResult> CreateFiles()
        {
            return tableActionCommands.Select(i => i.Execute()).ToList();
        }
    }
}
