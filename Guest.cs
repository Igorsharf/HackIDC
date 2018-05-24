using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingSeating
{
    class Guest
    {

        public enum Side
        {
            bride,
            groom,
            mixed,
            empty,
        }

        public enum Tag
        {
            Family = 1,
            CloseFamily = 2,
            FarFamily = 3,
            CloseFriends = 4,
            FamilyFreinds = 5,
            CloseWorkFreinds = 6,
            FarWorkFreinds = 7,
            FarFriends = 8,
            Other = 9,
        }


        private string m_FirstName;
        private string m_LastName;
        private int m_NumArrive;
        private int m_AmmountToBeSeated;
        private Side m_Side;
        private Tag m_Tag;
        private List<MapGuestToTable> m_TableList;

        public Guest(string i_FirstName, string i_LastName, int i_NumArrive, Side i_Side, Tag i_Tag)
        {
            this.m_FirstName = i_FirstName;
            this.m_LastName = i_LastName;
            this.m_NumArrive = i_NumArrive;
            this.m_AmmountToBeSeated = i_NumArrive;
            this.m_Side = i_Side;
            this.m_Tag = i_Tag;
            m_TableList = new List<MapGuestToTable>();
        }

        public string LastName
        {
            get
            {
                return m_LastName;
            }
            set
            {
                m_LastName = value;
            }
        }

        public string FirstName
        {
            get
            {
                return m_FirstName;
            }
            set
            {
                m_FirstName = value;
            }
        }

        public int NumArrive
        {
            get
            {
                return m_NumArrive;
            }
            set
            {
                m_NumArrive = value;
            }
        }

        public int AmmountToBeSeated
        {
            get
            {
                return m_AmmountToBeSeated;
            }
            set
            {
                m_AmmountToBeSeated = value;
            }
        }

        public Side FamilySide
        {
            get
            {
                return m_Side;
            }
            set
            {
                m_Side = value;
            }
        }

        public Tag RealationTag
        {
            get
            {
                return m_Tag;
            }
            set
            {
                m_Tag = value;
            }
        }

        public List<MapGuestToTable> TableList
        {
            get
            {
                return m_TableList;
            }
            set
            {
                m_TableList = value;
            }
        }

    }
}



