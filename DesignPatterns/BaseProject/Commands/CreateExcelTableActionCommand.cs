using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Commands
{
    //Execute metodunun nasıl çalışağı ile ilgili instance class'ımız
    public class CreateExcelTableActionCommand<TModel> : ITableActionCommand
    {
        //Bu arkadaş DI ile gelmeyecek, kimin geleceğine isteğe göre karar vereceğiz
        //Burada Strategy Design Pattern da uygulanabilr, bağımlılık olmaması açısından
        private readonly ExcelFile<TModel> _excelFile;

        public CreateExcelTableActionCommand(ExcelFile<TModel> excelFile)
        {
            _excelFile = excelFile;
        }

        public IActionResult Execute()
        {
            var excelMemoryStream = _excelFile.Create();
            return new FileContentResult(excelMemoryStream.ToArray(), _excelFile.FileType) { FileDownloadName = _excelFile.FileName };
        }
    }
}
