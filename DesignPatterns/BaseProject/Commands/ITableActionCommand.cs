using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Commands
{
    //Çalışacak olan metodun soyut halini verdik, Command Design Pattern metod düzeyinde Run Time'da değişikliğe adaptasyon sağlar
    public interface ITableActionCommand
    {
        IActionResult Execute();
    }
}
