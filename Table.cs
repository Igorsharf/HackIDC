using System;
using System.Collections.Generic;

using System.Text;

namespace WeddingSeating
{
    class Table
    {
        private static int tableId = 0;
        private int m_TableId;
        private int m_tableSize;
        private int m_roomLeft;
        private List<Guest> m_guests = new List<Guest>();
        private Guest.Side tableSide = Guest.Side.empty;
        private int m_TablePriority;
        public Table(int i_tableSize)
        {
            tableId++;
            m_TableId = tableId;
            this.m_tableSize = i_tableSize;
            this.m_roomLeft = i_tableSize;
            m_TablePriority = (int) Guest.Tag.Family;
        }

        public Guest.Side TableSide
        {
            get
            {
                return this.tableSide;
            }
            set
            {
                tableSide = value;
            }
        }

        public int TableId
        {
            get
            {
                return this.m_TableId;
            }
            set
            {
                m_TableId = value;
            }
        }
        public int TablePriority
        {
            get
            {
                return this.m_TablePriority;
            }
            set
            {
                m_TablePriority = value;
            }
        }
        public int TableSize
        {
            get
            {
                return this.m_tableSize;
            }
            set
            {
                m_tableSize = value;
            }
        }

        public int RoomLeft
        {
            get
            {
                return this.m_roomLeft;
            }
            set
            {
                m_roomLeft = value;
            }
        }

        public List<Guest> GuestList
        {
            get
            {
                return this.m_guests;
            }
            set
            {
                this.m_guests = value;
            }
        }

        public Boolean addGuest(Guest i_newGuest)
        {
            bool isSuccess = false;
            if (this.m_roomLeft == 0)
            {
                return false;
            }
            if (i_newGuest.AmmountToBeSeated <= this.m_roomLeft)
            {
                updateTableSide(i_newGuest);
                m_guests.Add(i_newGuest);
                this.m_roomLeft = (this.m_roomLeft - i_newGuest.AmmountToBeSeated);
                i_newGuest.AmmountToBeSeated = i_newGuest.NumArrive - i_newGuest.AmmountToBeSeated;
                i_newGuest.TableList.Add(new MapGuestToTable(m_TableId, i_newGuest.AmmountToBeSeated));
                isSuccess = true;
            }

            return (isSuccess == true);
        }

        // adds only some of the guests from the newGuest inputs
        public Boolean addSomeGuest(Guest i_newGuest, int i_someOfGuest)
        {
            bool isSuccess = false;
            if (i_someOfGuest <= this.m_roomLeft)
            {
                updateTableSide(i_newGuest);
                //update the remaining amount to be seated from this group
                i_newGuest.AmmountToBeSeated -= i_someOfGuest;
                m_guests.Add(i_newGuest);
                this.m_roomLeft = (this.m_tableSize - i_someOfGuest);
                i_newGuest.TableList.Add(new MapGuestToTable(tableId, i_someOfGuest));
                isSuccess = true;

            }
            return isSuccess == true;
        }
        public Boolean Switch(Guest i_newGuest, Guest i_oldGuest)
        {
            bool switchSuccess = false;
            int numNewGuests = i_newGuest.NumArrive;
            int numOldGuests = 0;
            foreach (MapGuestToTable i in i_oldGuest.TableList)
            {
                if (i.TableId == tableId)
                {
                    numOldGuests = i.NumGuestAtTable;
                }
            }
            if (numNewGuests <= (this.m_roomLeft + numOldGuests))
            {
                m_guests.Remove(i_oldGuest);
                m_guests.Add(i_newGuest);
                switchSuccess = true;
            }
            return switchSuccess == true;
        }

        //update the side of the table
        private void updateTableSide(Guest i_newGuest)
        {
            if (i_newGuest.FamilySide != Guest.Side.empty)
            {
                if (i_newGuest.FamilySide != this.tableSide)
                {
                    this.tableSide = Guest.Side.mixed;
                }
            }
            else
            {
                this.tableSide = i_newGuest.FamilySide;
            }
        }
    }
}