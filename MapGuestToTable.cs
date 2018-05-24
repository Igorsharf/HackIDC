using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingSeating
{
    public class MapGuestToTable
    {
        private int m_TableId = -1;
        private int m_NumGuestAtTable = -1;

        public MapGuestToTable(int i_TableId, int i_NumGuestAtTable)
        {
            this.m_TableId = i_TableId;
            this.m_NumGuestAtTable = i_NumGuestAtTable;
        }

        public int TableId
        {
            get
            {
                return m_TableId;
            }
            set
            {
                m_TableId = value;
            }
        }

        public int NumGuestAtTable
        {
            get
            {
                return m_NumGuestAtTable;
            }
            set
            {
                m_NumGuestAtTable = value;
            }
        }
    }
}

