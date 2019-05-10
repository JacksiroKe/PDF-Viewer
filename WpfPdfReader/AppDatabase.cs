using System;
using System.Data;
using System.Data.SQLite;

namespace WpfPdfReader
{

    public class AppDatabase
    {
        SQLiteConnection sConn;
        SQLiteCommand sCmd;
        SQLiteDataAdapter sAdapter = null;
        DataSet dS = null;
        DataTable dT = new DataTable();
        SQLiteDataReader reader;
        DataRowCollection dRowCol;

        public AppDatabase()
        {
            sConn = new SQLiteConnection("Data Source=kicd_content\\kec.db;New=False;Version=3");
            sConn.Open();
        }

        public DataRowCollection getList(string CommandText)
        {
            dS = new DataSet();
            sAdapter = new SQLiteDataAdapter(CommandText, sConn);
            sAdapter.Fill(dS);
            dRowCol = dS.Tables[0].Rows;
            sqlClose();
            return dRowCol;
        }

        public SQLiteDataReader getSingle(string CommandText)
        {
            sCmd = new SQLiteCommand(CommandText, sConn);
            reader = sCmd.ExecuteReader();
            return reader;
        }
        
        public string todate()
        {
            return DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
        }

        public void sqlClose()
        {
            sConn.Close();
        }

    }
}
