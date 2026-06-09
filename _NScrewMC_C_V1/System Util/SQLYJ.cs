
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace _NScrewMC_C_V1
{
    public class SQLiteYJ
    {
        static Stopwatch sw;
#if CHECKTIME
        static Stopwatch sw;
#endif
        public SQLiteConnection _Myconnection;
        public SQLiteConnection _MyconnectionIDV;
        public string TBName = "";
        public string _filePath = "";
        public SQLiteYJ(string _TbName, string FilePath)
        {
            TBName = _TbName;
            _filePath = FilePath;
        }
        ~SQLiteYJ()
        {
            CloseConnection();
            _Myconnection = null;
            _MyconnectionIDV = null;
        }
        public void OpenConnection()
        {
            if (_Myconnection.State != System.Data.ConnectionState.Open)
            {
                _Myconnection.Open();
                _MyconnectionIDV.Open();
            }
        }
        public void CloseConnection()
        {
            if (_Myconnection != null)
            {
                //// _Myconnection.Close();
                // _Myconnection = null;
            }
        }
        /***********************************************************************/

        public void CreateTB()
        {
            if (!Directory.Exists(_filePath)) { Directory.CreateDirectory(_filePath); }
            _Myconnection = new SQLiteConnection($"Data Source = {_filePath}{TBName}.sqlite3");
            if (!File.Exists($"{_filePath}{TBName}.sqlite3"))
            {
                #region< string command update all parameter model>
                string strcommand = "CREATE TABLE " + TBName + "(" +
                                    "NUMERROR           TEXT NOT NULL," +
                                    "DATE               TEXT NOT NULL," +
                                    "CONTENS            TEXT NOT NULL);";
                using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
                {
                    using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                    {
                        _Myconnection.Open();
                        mycommand.ExecuteNonQuery();
                        _Myconnection.Close();
                    }
                }
                #endregion

                #region <Create Index>
                string strcmdIndex = "CREATE INDEX SN_INDEX on " + TBName + "(NUMERROR)";
                using (SQLiteCommand mycommand = new SQLiteCommand(strcmdIndex, _Myconnection))
                {
                    using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                    {
                        _Myconnection.Open();
                        mycommand.ExecuteNonQuery();
                        _Myconnection.Close();
                    }
                }
                #endregion
            }
        }
        /***********************************************************************/
        public void InsertData(string[] Data)
        {
            #region< string command update all parameter model>
            string strcommand = "INSERT INTO " + TBName + " VALUES ( '" + Data[0] + "','" + Data[1] + "','" + Data[2] + "')";
            _Myconnection = new SQLiteConnection($"Data Source = {_filePath}{TBName}.sqlite3");
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    try 
                    {
                        _Myconnection.Open();
                        mycommand.ExecuteNonQuery();
                        _Myconnection.Close();
                    }
                    catch(Exception ex) 
                    {
                        ex.ToString();
                    }
                }
            }
        }
        public void DeleteAllData(string _tbName, string _SN)
        {
            #region //Command Update data
            string strcommand = "DELETE FROM " + _tbName + "  WHERE  SN != " + "'" + _SN + "'";
            _Myconnection = new SQLiteConnection("Data Source = Data/Database/" + _tbName + ".sqlite3");
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    mycommand.ExecuteNonQuery();
                    _Myconnection.Close();
                }
            }
        }
        public DataTable SearchData(string _tbName, string _Datacheck)
        {
#if CHECKTIME

            sw = Stopwatch.StartNew();
#endif
            #region< string command update all parameter model>
            string strcommand = "SELECT * FROM " + _tbName + " WHERE SN = '" + _Datacheck + "'";
            _Myconnection = new SQLiteConnection("Data Source = Data/Database/" + _tbName + ".sqlite3");
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    var Result = mycommand.ExecuteNonQuery();
                    DataTable Tb = new DataTable();
                    Db.Fill(Tb);
#if CHECKTIME
            Debug.WriteLine("Time Poll : " + sw.ElapsedMilliseconds);
#endif
                    _Myconnection.Close();
                    return Tb;
                }
            }
        }
        public DataTable SearhLotNo(string _tbName, string _lotno)
        {
#if CHECKTIME
            sw = Stopwatch.StartNew();
#endif
            #region< string command update all parameter model>
            string strcommand = "SELECT * FROM " + _tbName + " WHERE LOT_NO = '" + _lotno + "'";
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    var Result = mycommand.ExecuteNonQuery();
                    DataTable Tb = new DataTable();
                    Db.Fill(Tb);
#if CHECKTIME
            Debug.WriteLine("Time Poll : " + sw.ElapsedMilliseconds);
#endif
                    _Myconnection.Close();
                    return Tb;
                }
            }
        }
        public DataTable SearchTbSetting(string _tbName, string _Suplier)
        {
#if CHECKTIME
            sw = Stopwatch.StartNew();
#endif
            #region< string command update all parameter model>
            string strcommand = "SELECT * FROM " + _tbName + " WHERE SUPPLIER = '" + _Suplier + "'" + " AND " + "INVENTORY >= '1'";
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    var Result = mycommand.ExecuteNonQuery();
                    DataTable Tb = new DataTable();
                    Db.Fill(Tb);
#if CHECKTIME
            Debug.WriteLine("Time Poll : " + sw.ElapsedMilliseconds);
#endif
                    _Myconnection.Close();
                    return Tb;
                }
            }
        }
        public void UpdateData(string _tbName, string _NameChane, string _NameCheck, string _condition, string _data)
        {
            #region //Command Update data
            string strcommand = "UPDATE " + _tbName + " SET " + _NameChane + " = '" + _data + "' WHERE " + _NameCheck + "='" + _condition + "'";
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    mycommand.ExecuteNonQuery();
                    _Myconnection.Close();
                }
            }
        }
        public void DeleteData(string _tbName, string _dataDelete)
        {
            #region //Command Update data
            _Myconnection = new SQLiteConnection("Data Source = Data/Database/" + _tbName + ".sqlite3");
            string strcommand = "DELETE FROM " + _tbName + "  WHERE  SN = " + "'" + _dataDelete + "'";
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    mycommand.ExecuteNonQuery();
                    _Myconnection.Close();
                }
            }
        }
        public DataTable GetDataInDate(string _tbName, string Data)
        {
#if CHECKTIME
            sw = Stopwatch.StartNew();
#endif
            #region< string command update all parameter model>
            string strcommand = "SELECT * FROM " + _tbName + " WHERE DATE = '" + Data + "'";
            #endregion
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    var Result = mycommand.ExecuteNonQuery();
                    DataTable Tb = new DataTable();
                    Db.Fill(Tb);
#if CHECKTIME
            Debug.WriteLine("Time Poll : " + sw.ElapsedMilliseconds);
#endif
                    _Myconnection.Close();
                    return Tb;
                }
            }
        }
        public void InsertDataTable(string _tbName, DataTable _tbData)
        {
            #region //Check file Exist Affter Update
            if (!Directory.Exists("Data/Database/")) { Directory.CreateDirectory("Data/Database/"); }
            _Myconnection = new SQLiteConnection("Data Source = Data/Database/" + _tbName + ".sqlite3");
            if (!File.Exists("Data/Database/" + _tbName + ".sqlite3"))
            {
                #region< string command update all parameter model>
                string strcommandtb = "CREATE TABLE " + _tbName + "(" +
                                    "TIME           TEXT NOT NULL," +
                                    "SN             TEXT NOT NULL);";
                using (SQLiteCommand mycommand = new SQLiteCommand(strcommandtb, _Myconnection))
                {
                    using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                    {
                        _Myconnection.Open();
                        mycommand.ExecuteNonQuery();
                        _Myconnection.Close();
                    }
                }
                #endregion

                #region <Create Index>
                string strcmdIndex = "CREATE INDEX SN_INDEX on " + _tbName + "(SN)";
                using (SQLiteCommand mycommand = new SQLiteCommand(strcmdIndex, _Myconnection))
                {
                    using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                    {
                        _Myconnection.Open();
                        mycommand.ExecuteNonQuery();
                        _Myconnection.Close();
                    }
                }
                #endregion
            }
            #endregion

            #region< string command update all parameter model>

            _Myconnection = new SQLiteConnection("Data Source = Data/Database/" + _tbName + ".sqlite3");

            string strcommand = "INSERT INTO " + _tbName + " VALUES";
            foreach (DataRow _dr in _tbData.Rows)
            {
                strcommand += " ( '" + _dr[0] + "','" + _dr[1] + "'),";
            }
            strcommand = strcommand.Substring(0, strcommand.Length - 1) + ";";
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    mycommand.ExecuteNonQuery();
                    _Myconnection.Close();
                }
            }
            #endregion
        }
        public DataTable GetAllDataTable(string _tbName)
        {
            _Myconnection = new SQLiteConnection("Data Source = Data/Database/" + _tbName + ".sqlite3");
            string strcommand = "SELECT * FROM " + _tbName;
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    mycommand.ExecuteNonQuery();
                    DataTable Tb = new DataTable();
                    Db.Fill(Tb);
                    _Myconnection.Close();
                    return Tb;
                }
            }
        }

        #region //HINGER 
        public DataTable GetDataSearchRange(string _conditent1, string _conditent2)
        {
            //SELECT * FROM HingerDataLogError WHERE DATE BETWEEN '20221214_00:00:00' AND '20221214_23:59:59'
            #region< string command update all parameter model>
            string strcommand = $"SELECT * FROM {TBName} WHERE DATE BETWEEN '{_conditent1}' AND '{_conditent2}'";
            #endregion
            //long test = 0;
            // sw = Stopwatch.StartNew();
            using (SQLiteCommand mycommand = new SQLiteCommand(strcommand, _Myconnection))
            {
                using (SQLiteDataAdapter Db = new SQLiteDataAdapter(mycommand))
                {
                    _Myconnection.Open();
                    var Result = mycommand.ExecuteNonQuery();
                    DataTable Tb = new DataTable();
                    Db.Fill(Tb);
                    _Myconnection.Close();

                    //test = sw.ElapsedMilliseconds;
                    return Tb;
                }
            }
        }
        #endregion
    }

}
