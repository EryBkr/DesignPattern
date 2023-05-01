using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BaseProject.ChainOfResponsibility
{
    //Zincirin ilk halkası
    public class ExcelProcessHandler<TEntity> : ProcessHandler
    {
        //Tip bağımsız DataTable oluşturacak
        private DataTable GetTable(Object o)
        {
            var table = new DataTable();
            var type = typeof(TEntity);

            type.GetProperties().ToList().ForEach(i => table.Columns.Add(i.Name, i.PropertyType));
            var list = o as List<TEntity>;

            list.ForEach(i =>
            {
                var values = type.GetProperties().Select(propertInfo => propertInfo.GetValue(i, null)).ToArray();
                table.Rows.Add(values);
            });

            return table;
        }

        public override object Handle(Object o)
        {
            var workBookSheet = new XLWorkbook();
            var dataSet = new DataSet();

            dataSet.Tables.Add(GetTable(o));

            workBookSheet.Worksheets.Add(dataSet);

            var memoryStream = new MemoryStream();

            workBookSheet.SaveAs(memoryStream);

            //Zincirin bi sonraki halkası için excel memory stream'i oluşturup tekrar handle metoduna gönderiyoruz
            return base.Handle(memoryStream);
        }
    }
}
