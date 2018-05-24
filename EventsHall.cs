using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;


namespace WeddingSeating
{
    public class EventsHall
    {
        private const int k_NumberOfTables = 20;
        private const int k_NumberOfSeatsPerTable = 8;
        private Table[] m_TableArray;
        private List<Guest> m_GuestList;
        private List<List<Guest>> m_ClassifyInstancesReagaringTagHusband;
        private List<List<Guest>> m_ClassifyInstancesReagaringTagBride;
        private List<List<Guest>> m_ClassifyInstancesReagaringTagMixed;
        private int m_LastTableIndex;

        public EventsHall()
        {
            m_LastTableIndex = 0;
            m_ClassifyInstancesReagaringTagHusband = new List<List<Guest>>();
            m_ClassifyInstancesReagaringTagBride = new List<List<Guest>>();
            m_ClassifyInstancesReagaringTagMixed = new List<List<Guest>>();
            initList(m_ClassifyInstancesReagaringTagHusband);
            initList(m_ClassifyInstancesReagaringTagBride);
            initList(m_ClassifyInstancesReagaringTagMixed);
            m_TableArray = new Table[k_NumberOfTables];
            for (int i = 0; i < m_TableArray.Length; i++)
            {
                m_TableArray[i] = new Table(k_NumberOfSeatsPerTable);
            }
            m_GuestList = readExcel.readAndLoadGuestsList();
            classificatyGuestRegardingTags();
            buildTables();
        }


        private void initList(List<List<Guest>> i_List)
        {
            for (int i = 0; i < 9; i++)
            {
                i_List.Add(new List<Guest>());
            }
        }
        private void buildTables()
        {
            perfectMatching(0, 4, m_ClassifyInstancesReagaringTagHusband);
            devideGuestToTables(0, 9, m_ClassifyInstancesReagaringTagHusband, (k_NumberOfTables / 2), Guest.Side.groom);

            perfectMatching(0, 4, m_ClassifyInstancesReagaringTagBride);
            devideGuestToTables(0, 9, m_ClassifyInstancesReagaringTagBride, (k_NumberOfTables / 2), Guest.Side.bride);

            perfectMatching(0, 4, m_ClassifyInstancesReagaringTagMixed);
            devideGuestToTables(0, 9, m_ClassifyInstancesReagaringTagMixed, (k_NumberOfTables / 2), Guest.Side.mixed);
        }
        private void devideGuestToTables(int i_StartingIndex, int i_EndIndex, List<List<Guest>> i_List, int i_MiddleTableIndex, Guest.Side i_Side)
        {

            for (int i = 0; i < i_List.Count; i++)
            {
                i_List[i] = quicksort(i_List[i]);
            }
            Table table;
            int beginingTableIndex = 0;
            int endTableIndex = i_MiddleTableIndex;
            if (i_Side == Guest.Side.bride)
            {
                beginingTableIndex = i_MiddleTableIndex;
                endTableIndex = m_TableArray.Length;

            }
            if (i_Side == Guest.Side.mixed)
            {
                endTableIndex = m_TableArray.Length;

            }
            int maxIndex;
            int minIndex;
            Guest maxGuest = null;
            Guest minGuest = null;
            for (int i = i_StartingIndex; i < i_EndIndex; i++)
            {
                bool wereEqual = false;
                maxIndex = 0;
                minIndex = i_List[i].Count - 1;
                while (!wereEqual)
                {
                    if (maxIndex == minIndex)
                    {
                        wereEqual = true;
                    }
                    if (i_List[i].Count > 0)
                    {
                        maxGuest = i_List[i][maxIndex];
                        minGuest = i_List[i][minIndex];
                        while (maxGuest.AmmountToBeSeated == 0)
                        {
                            maxIndex++;
                            wereEqual = maxIndex == minIndex;
                            if (maxIndex >= i_List[i].Count)
                                break;
                            maxGuest = i_List[i][maxIndex];
                        }
                        while (minGuest.AmmountToBeSeated == 0)
                        {
                            minIndex--;
                            wereEqual = maxIndex == minIndex;
                            if (minIndex < 0)
                            {
                                wereEqual = true;
                                break;
                            }
                            minGuest = i_List[i][minIndex];
                        }

                        for (int j = m_LastTableIndex; j < endTableIndex; j++)
                        {
                            table = m_TableArray[j];
                            int tablePriority = table.TablePriority;

                            if (table.RoomLeft == 0)
                            {
                                continue;
                            }
                            if (table.RoomLeft < minGuest.AmmountToBeSeated)
                            {
                                continue;
                            }
                            if (minIndex == maxIndex)
                            {
                                table.addGuest(maxGuest);
                                table.TablePriority = (int)maxGuest.RealationTag;
                                wereEqual = true;
                                break;
                            }
                            if (maxGuest.AmmountToBeSeated == table.RoomLeft)
                            {
                                table.addGuest(maxGuest);
                                maxIndex++;
                                table.TablePriority = (int)maxGuest.RealationTag;
                                break;
                            }
                            else if (maxGuest.AmmountToBeSeated + minGuest.AmmountToBeSeated > table.RoomLeft)
                            {
                                if (maxGuest.AmmountToBeSeated < table.RoomLeft)
                                {
                                    table.addGuest(maxGuest);
                                    table.TablePriority = (int)maxGuest.RealationTag;
                                    maxIndex++;
                                    break;
                                }
                            }

                            else if (minGuest.AmmountToBeSeated <= table.RoomLeft)
                            {
                                table.addGuest(minGuest);
                                table.TablePriority = (int)minGuest.RealationTag;
                                minIndex--;
                                if (minIndex < 0)
                                {
                                    wereEqual = true;
                                }
                                break;
                            }
                            else if (maxGuest.AmmountToBeSeated + minGuest.AmmountToBeSeated <= table.RoomLeft)
                            {
                                table.addGuest(maxGuest);
                                table.addGuest(minGuest);
                                table.TablePriority = (int)minGuest.RealationTag;
                                minIndex--;
                                maxIndex++;
                                if (minIndex == maxIndex)
                                {
                                    wereEqual = true;
                                }
                                break;
                            }
                            if (m_LastTableIndex < j)
                            {
                                m_LastTableIndex = j;
                            }
                        }


                    }
                    else
                    {
                        wereEqual = true;
                    }


                }

            }
        }


        private void perfectMatching(int staringIndex, int endIndex, List<List<Guest>> i_List)
        {
            for (int i = staringIndex; i < endIndex; i++)
            {
                Guest.Tag tagType = (Guest.Tag)Enum.ToObject(typeof(Guest.Tag), i);
                foreach (Guest first in i_List[i])
                {
                    foreach (Guest second in i_List[i])
                    {
                        if (!first.Equals(second))
                        {
                            foreach (Table table in m_TableArray)
                            {
                                if (first.AmmountToBeSeated == table.RoomLeft)
                                {
                                    table.addGuest(first);
                                    break;
                                }
                                else if (second.AmmountToBeSeated == table.RoomLeft)
                                {
                                    table.addGuest(second);
                                    break;
                                }
                                else if (first.AmmountToBeSeated != 0 && second.AmmountToBeSeated != 0 && first.AmmountToBeSeated + second.AmmountToBeSeated == table.RoomLeft && table.RoomLeft != 0)
                                {
                                    table.addGuest(first);
                                    table.addGuest(second);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        static List<Guest> quicksort(List<Guest> list)
        {
            if (list.Count <= 1)
                return list;
            int pivotPosition = list.Count / 2;
            int pivotValue = list[pivotPosition].NumArrive;
            Guest pivotGuest = list[pivotPosition];
            list.RemoveAt(pivotPosition);
            List<Guest> smaller = new List<Guest>();
            List<Guest> greater = new List<Guest>();
            foreach (Guest item in list)
            {
                if (item.NumArrive > pivotValue)
                {
                    greater.Add(item);
                }
                else
                {
                    smaller.Add(item);
                }
            }
            List<Guest> sorted = quicksort(greater);
            sorted.Add(pivotGuest);
            sorted.AddRange(quicksort(smaller));
            return sorted;
        }

        private void classificatyGuestRegardingTags()
        {
            int numOfEnums = Enum.GetValues(typeof(Guest.Tag)).Length;
            for (int i = 0; i < m_GuestList.Count; i++)
            {
                Guest current = m_GuestList[i];
                if (current.FamilySide == Guest.Side.bride)
                {
                    switch (current.RealationTag)
                    {
                        case Guest.Tag.Family:
                            m_ClassifyInstancesReagaringTagBride[0].Add(current);
                            break;
                        case Guest.Tag.CloseFamily:
                            m_ClassifyInstancesReagaringTagBride[1].Add(current);
                            break;
                        case Guest.Tag.FarFamily:
                            m_ClassifyInstancesReagaringTagBride[2].Add(current);
                            break;
                        case Guest.Tag.CloseFriends:
                            m_ClassifyInstancesReagaringTagBride[3].Add(current);
                            break;
                        case Guest.Tag.FamilyFreinds:
                            m_ClassifyInstancesReagaringTagBride[4].Add(current);
                            break;
                        case Guest.Tag.CloseWorkFreinds:
                            m_ClassifyInstancesReagaringTagBride[5].Add(current);
                            break;
                        case Guest.Tag.FarWorkFreinds:
                            m_ClassifyInstancesReagaringTagBride[6].Add(current);
                            break;
                        case Guest.Tag.FarFriends:
                            m_ClassifyInstancesReagaringTagBride[7].Add(current);
                            break;
                        case Guest.Tag.Other:
                            m_ClassifyInstancesReagaringTagBride[8].Add(current);
                            break;
                        default:
                            m_ClassifyInstancesReagaringTagBride[8].Add(current);
                            break;
                    }
                }
                else if (m_GuestList[i].FamilySide == Guest.Side.groom)
                {
                    switch (current.RealationTag)
                    {
                        case Guest.Tag.Family:
                            m_ClassifyInstancesReagaringTagHusband[0].Add(current);
                            break;
                        case Guest.Tag.CloseFamily:
                            m_ClassifyInstancesReagaringTagHusband[1].Add(current);
                            break;
                        case Guest.Tag.FarFamily:
                            m_ClassifyInstancesReagaringTagHusband[2].Add(current);
                            break;
                        case Guest.Tag.CloseFriends:
                            m_ClassifyInstancesReagaringTagHusband[3].Add(current);
                            break;
                        case Guest.Tag.FamilyFreinds:
                            m_ClassifyInstancesReagaringTagHusband[4].Add(current);
                            break;
                        case Guest.Tag.CloseWorkFreinds:
                            m_ClassifyInstancesReagaringTagHusband[5].Add(current);
                            break;
                        case Guest.Tag.FarWorkFreinds:
                            m_ClassifyInstancesReagaringTagHusband[6].Add(current);
                            break;
                        case Guest.Tag.FarFriends:
                            m_ClassifyInstancesReagaringTagHusband[7].Add(current);
                            break;
                        case Guest.Tag.Other:
                            m_ClassifyInstancesReagaringTagHusband[8].Add(current);
                            break;
                        default:
                            m_ClassifyInstancesReagaringTagHusband[8].Add(current);
                            break;
                    }
                }
                else
                {
                    switch (current.RealationTag)
                    {
                        case Guest.Tag.Family:
                            m_ClassifyInstancesReagaringTagMixed[0].Add(current);
                            break;
                        case Guest.Tag.CloseFamily:
                            m_ClassifyInstancesReagaringTagMixed[1].Add(current);
                            break;
                        case Guest.Tag.FarFamily:
                            m_ClassifyInstancesReagaringTagMixed[2].Add(current);
                            break;
                        case Guest.Tag.CloseFriends:
                            m_ClassifyInstancesReagaringTagMixed[3].Add(current);
                            break;
                        case Guest.Tag.FamilyFreinds:
                            m_ClassifyInstancesReagaringTagMixed[4].Add(current);
                            break;
                        case Guest.Tag.CloseWorkFreinds:
                            m_ClassifyInstancesReagaringTagMixed[5].Add(current);
                            break;
                        case Guest.Tag.FarWorkFreinds:
                            m_ClassifyInstancesReagaringTagMixed[6].Add(current);
                            break;
                        case Guest.Tag.FarFriends:
                            m_ClassifyInstancesReagaringTagMixed[7].Add(current);
                            break;
                        case Guest.Tag.Other:
                            m_ClassifyInstancesReagaringTagMixed[8].Add(current);
                            break;
                        default:
                            m_ClassifyInstancesReagaringTagMixed[8].Add(current);
                            break;
                    }
                }
            }
        }

        /**        public void printTables()
                {
                    for (int i = 0; i < m_TableArray.Length; i++)
                    {
                        StringBuilder s = new StringBuilder();
                        s.Append("Table Id: " + i);
                        s.Append(" Table size = " + m_TableArray[i].TableSize + System.Environment.NewLine);
                        for (int j = 0; j < m_TableArray[i].GuestList.Count; j++)
                        {
                            for (int m = 0; m < m_TableArray[i].GuestList[j].TableList.Count; m++)
                            {
                                if (m_TableArray[i].GuestList[j].TableList[m].TableId == m_TableArray[i].TableId)
                                {
                                    s.Append(m_TableArray[i].GuestList[j].FirstName + " " + m_TableArray[i].GuestList[j].LastName + " " + m_TableArray[i].GuestList[j].NumArrive + " " + m_TableArray[i].GuestList[j].RealationTag + System.Environment.NewLine);
                                }
                                  
                            }
                            
                        }
                        Console.WriteLine(s);
                        s.Clear();
                    }
                }


            } */
        public void TransferXLToTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TableID", typeof(string));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Number", typeof(string));
            dt.Columns.Add("RelationTag", typeof(string));
   

            using (FileStream stream = new FileStream(@"C:\Users\igor sharfman\Desktop\\WeadingSeating.xlsx", FileMode.Create, FileAccess.Write))
            {
                int index = 0;
                IWorkbook wb = new XSSFWorkbook();
                ISheet sheet = wb.CreateSheet("Wedding");
                ICreationHelper cH = wb.GetCreationHelper();
                for (int i = 0; i < m_TableArray.Length; i++)
                {
                    
                    for (int k = 0; k < m_TableArray[i].GuestList.Count; k++)
                    {
                        for (int m = 0; m < m_TableArray[i].GuestList[k].TableList.Count; m++)
                        {
                            IRow row = sheet.CreateRow(index);
                            index++;
                            for (int j = 0; j < 5; j++)
                            {
                                ICell cell = row.CreateCell(j);
                                if (m_TableArray[i].GuestList[k].TableList[m].TableId == m_TableArray[i].TableId)
                                { 
                                    if (j == 0){
                                        cell.SetCellValue(m_TableArray[i].TableId);
                                    }
                                    else if (j == 1)
                                    {
                                        cell.SetCellValue(cH.CreateRichTextString(m_TableArray[i].GuestList[k].FirstName.ToString()));
                                    }
                                   else if (j == 2)
                                    {
                                        cell.SetCellValue(cH.CreateRichTextString(m_TableArray[i].GuestList[k].LastName.ToString()));
                                    }
                                    else if (j == 3)
                                    {
                                        cell.SetCellValue(cH.CreateRichTextString(m_TableArray[i].GuestList[k].NumArrive.ToString()));
                                    }
                                    else if (j == 4)
                                        {
                                            cell.SetCellValue(cH.CreateRichTextString(m_TableArray[i].GuestList[k].RealationTag.ToString()));
                                        }
    

                                }
                            }
                         
                        }
                        
                       
                         

                            {
                            }

                        }
                    }
                    wb.Write(stream);
                }
            }

        }
    }

