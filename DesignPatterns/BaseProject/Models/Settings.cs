namespace BaseProject.Models
{
    //Biz kullanıcıların hangi db'yi kullanacaklarını Claim'lere kaydediyoruz,bu class ile Claim bilgilerini bir model ile wrap'leyerek okuyacağız
    public class Settings
    {
        public static string ClaimDatabaseType = "databasetype";
        public DatabaseTypeEnum DatabaseType { get; set; }
        public DatabaseTypeEnum DefaultDatabase = DatabaseTypeEnum.SqlServer;
    }
}
