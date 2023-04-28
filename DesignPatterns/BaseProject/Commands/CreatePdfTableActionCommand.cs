using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Commands
{
    //Execute metodunun nasıl çalışağı ile ilgili instance class'ımız
    public class CreatePdfTableActionCommand<TModel> : ITableActionCommand
    {
        //Bu arkadaş DI ile gelmeyecek, kimin geleceğine isteğe göre karar vereceğiz
        //Burada Strategy Design Pattern da uygulanabilr, bağımlılık olmaması açısından
        private readonly PdfFile<TModel> _pdfFile;

        public CreatePdfTableActionCommand(PdfFile<TModel> pdfFile)
        {
            _pdfFile = pdfFile;
        }

        public IActionResult Execute()
        {
            var pdfMemoryStream = _pdfFile.Create();
            return new FileContentResult(pdfMemoryStream.ToArray(), _pdfFile.FileType) { FileDownloadName = _pdfFile.FileName };
        }
    }
}
