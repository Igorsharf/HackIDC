using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WeddingSeating
{
    class readExcel
    {
        public static List<Guest> readAndLoadGuestsList() {

            XSSFWorkbook xssfwb;
            
            using (FileStream file = new FileStream(@"C: \Users\igor sharfman\Desktop\\wedding.xlsx", FileMode.Open, FileAccess.Read))
            //using (FileStream file = new FileStream(@"C:\IGOR\wedding.xlsx", FileMode.Open, FileAccess.Read))
            //using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }
            string firstName ="";
            string lastName = "";
            int numberOfGuestes = 0;
            Guest.Tag tag = Guest.Tag.Other;
            Guest.Side side = Guest.Side.empty;

            ISheet sheet = xssfwb.GetSheetAt(0);
            List<Guest> guestList = new List<Guest>();

            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {
                    for (int column = 0; column <= 4; column++)
                    {
                        if (column == 2)
                        {
                            //Console.WriteLine("Row {0} = {1}", row, sheet.GetRow(row).GetCell(column).NumericCellValue);
                            numberOfGuestes = (int) sheet.GetRow(row).GetCell(column).NumericCellValue;
                        }
                        else
                        {
                           // Console.WriteLine("Row {0} = {1}", row, sheet.GetRow(row).GetCell(column).StringCellValue);
                            if (column == 0)
                            {
                                firstName = sheet.GetRow(row).GetCell(column).StringCellValue;
                            }
                            else if (column == 1)
                            {
                                lastName = sheet.GetRow(row).GetCell(column).StringCellValue;
                            }
                            else if (column == 3)
                            {
                                side = ParseEnum<Guest.Side>(sheet.GetRow(row).GetCell(column).StringCellValue);
                            }
                            else
                            {
                                tag = ParseEnum<Guest.Tag>(sheet.GetRow(row).GetCell(column).StringCellValue);
                            }
                        }
                        
                       
                    }
                    guestList.Add(new Guest(firstName, lastName, numberOfGuestes, side, tag));
                }
            }
            return guestList;
        }
            public static T ParseEnum<T>(string value)
            {
            return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
            }


        
    }
}