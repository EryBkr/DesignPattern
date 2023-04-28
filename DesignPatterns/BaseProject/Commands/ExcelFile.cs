using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BaseProject.Commands
{
    //Bu arkadaş bir Receiver'dir
    //Excel işlemlerini üstlenen sınıf
    public class ExcelFile<TModel>
    {
        public readonly List<TModel> _list;

        //İsim modele göre oluşacak
        public string FileName => $"{typeof(TModel).Name}.xlsx";

        //Excel Mime Type
        public string FileType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelFile(List<TModel> list)
        {
            _list = list;
        }

        //Excel File'ı oluşturuyoruz
        public MemoryStream Create()
        {
            var wb = new XLWorkbook();
            var ds = new DataSet();

            //DataSet'e reflection ile oluşturduğumuz Tablo yu veriyoruz
            ds.Tables.Add(GetTable());

            //Worksheet excel deki tablardır aslında
            wb.Worksheets.Add(ds);

            var excelMemory = new MemoryStream();

            //Excel'e dönüşecek data memory'e aktarıldı
            wb.SaveAs(excelMemory);

            //Excel Memory'imiz hazır ;)
            return excelMemory;
        }

        //Temelde excel için Table oluşturuyorum
        private DataTable GetTable()
        {
            var table = new DataTable();

            var type = typeof(TModel);

            //Gelen modelin property'leri Kolon olarak kullanılacak
            type.GetProperties().ToList().ForEach(i =>
            {
                table.Columns.Add(i.Name, i.PropertyType);
            });

            //Bize gelen listenin value'larını alıyorum 
            _list.ForEach(i =>
            {
                var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(i, null)).ToArray();

                //Gelen value'ları Memoryde ki tabloya veriyorum
                table.Rows.Add(values);
            });

            return table;
        }
    }
}
