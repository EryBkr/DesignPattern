using System;
using System.IO;
using System.IO.Compression;

namespace BaseProject.ChainOfResponsibility
{
    //Zincirin ikinci halkası
    public class ZipFileProcessHandlar<TEntity> : ProcessHandler
    {

        public override object Handle(Object o)
        {
            //Bize bir memorystream datası geleceğini biliyoruz (excel datası geliyor)
            var excelMemoryStream = o as MemoryStream;

            excelMemoryStream.Position = 0;

            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create,true))
                {
                    var zipFile = archive.CreateEntry($"{typeof(TEntity).Name}.xlsx");

                    using (var zipEntry = zipFile.Open())
                    {
                        excelMemoryStream.CopyTo(zipEntry);
                    }
                }

                //Burada da Zip dosyasını tanımlıyoruz,bu da zincirin bir sonraki halkasına gidecek
                return base.Handle(zipStream);
            }
        }
    }
}
