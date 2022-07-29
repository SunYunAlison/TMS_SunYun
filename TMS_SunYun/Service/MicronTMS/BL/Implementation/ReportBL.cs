using System;
using System.Linq;
using Newtonsoft.Json;
using MicronTMS.Helper;
using MicronTMS.DLL.Implementation;
using MicronTMS.DLL.Entities;
using NHibernate;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MicronTMS.BL.Implementation
{
    public class ReportBL
    {
        private readonly Repository<CTmsEqpConfig> _fridgeCfgRepo;

        private ISession _session;
        private readonly Logger _log;

        public ReportBL()
        {
            _fridgeCfgRepo = new Repository<CTmsEqpConfig>();

            _log = new Logger();
        }

        public ActionResult GetFridgeList()
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge List from C_TMS_EQPCONFIG", result.TrxName, eventId);
                _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();

                var fridgeList = _fridgeCfgRepo.GetAll(_session).OrderBy(x => x.EqpId).ToList();

                result.Data = JsonConvert.SerializeObject(fridgeList);

                _log.Info("Fridge list count: " + fridgeList.Count(), result.TrxName, eventId);
                result.Result = "Success";
                return result;
            }
            catch (Exception ex)
            {
                if (ex is NHibernate.ADOException)
                {
                    result.Error = "Database Exception: " + ex.Message.ToString();
                }
                else
                {
                    result.Error = "Logical Exception: " + ex.Message.ToString();
                }
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName, eventId);
                return result;
            }
            finally
            {
                if (_session.IsOpen)
                {
                    _session.Close();
                    _session.Dispose();
                }
            }
        }

        public ActionResult GetTempbyMin(string fridgeId, string startTime, string endTime)
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge Temperature by minute", result.TrxName, eventId);
                var startTimeStr = startTime.Replace("T", " ");
                var endTimeStr = endTime.Replace("T", " ");
                var fridgeStr = fridgeId.Trim();

                var sqlString = "select Eqp_name, 'DATE', CONVERT(VARCHAR(20),[1],20), CONVERT(VARCHAR(20),[2],20), CONVERT(VARCHAR(20),[3],20), CONVERT(VARCHAR(20),[4],20), CONVERT(VARCHAR(20),[5],20),  " +
                    "CONVERT(VARCHAR(20),[6], 20), CONVERT(VARCHAR(20),[7], 20), CONVERT(VARCHAR(20),[8], 20), CONVERT(VARCHAR(20),[9], 20), CONVERT(VARCHAR(20),[10], 20), " +
                    "CONVERT(VARCHAR(20),[11], 20), CONVERT(VARCHAR(20),[12], 20), CONVERT(VARCHAR(20),[13], 20), CONVERT(VARCHAR(20),[14], 20), CONVERT(VARCHAR(20),[15], 20), " +
                    "CONVERT(VARCHAR(20),[16], 20), CONVERT(VARCHAR(20),[17], 20), CONVERT(VARCHAR(20),[18], 20), CONVERT(VARCHAR(20),[19], 20), CONVERT(VARCHAR(20),[20], 20), " +
                    "CONVERT(VARCHAR(20),[21], 20), CONVERT(VARCHAR(20),[22], 20), CONVERT(VARCHAR(20),[23], 20), CONVERT(VARCHAR(20),[24], 20), CONVERT(VARCHAR(20),[25], 20), " +
                    "CONVERT(VARCHAR(20),[26], 20), CONVERT(VARCHAR(20),[27], 20), CONVERT(VARCHAR(20),[28], 20), CONVERT(VARCHAR(20),[29], 20), CONVERT(VARCHAR(20),[30], 20), " +
                    "CONVERT(VARCHAR(20),[31], 20), CONVERT(VARCHAR(20),[32], 20), CONVERT(VARCHAR(20),[33], 20), CONVERT(VARCHAR(20),[34], 20), CONVERT(VARCHAR(20),[35], 20),  " +
                    "CONVERT(VARCHAR(20),[36], 20) " +
                    "from (select Eqp_Name, Earliest_TDate, tdate, replace(tdates, 'sample_tdate_', '') idon " +
                    "from " +
                    "(select Eqp_Name, Earliest_TDate, sample_tdate_1, sample_value_1, sample_tdate_2, sample_value_2, sample_tdate_3, sample_value_3, sample_tdate_4, sample_value_4, sample_tdate_5, sample_value_5, " +
                    "sample_tdate_6, sample_value_6, sample_tdate_7, sample_value_7, sample_tdate_8, sample_value_8, sample_tdate_9, sample_value_9, sample_tdate_10, sample_value_10, sample_tdate_11, " +
                    "sample_value_11, sample_tdate_12, sample_value_12, sample_tdate_13, sample_value_13, sample_tdate_14, sample_value_14, sample_tdate_15, sample_value_15, sample_tdate_16, " +
                    "sample_value_16, sample_tdate_17, sample_value_17, sample_tdate_18, sample_value_18, sample_tdate_19, sample_value_19, sample_tdate_20, sample_value_20, sample_tdate_21, " +
                    "sample_value_21, sample_tdate_22, sample_value_22, sample_tdate_23, sample_value_23, sample_tdate_24, sample_value_24, sample_tdate_25, sample_value_25, sample_tdate_26, " +
                    "sample_value_26, sample_tdate_27, sample_value_27, sample_tdate_28, sample_value_28, sample_tdate_29, sample_value_29, sample_tdate_30, sample_value_30, sample_tdate_31, " +
                    "sample_value_31, sample_tdate_32, sample_value_32, sample_tdate_33, sample_value_33, sample_tdate_34, sample_value_34, sample_tdate_35, sample_value_35, sample_tdate_36, " +
                    "sample_value_36 " +
                    "from fridge a, C_TMS_EQPCONFIG c " +
                    "where a.signal_index = c.Eqp_Id and c.Eqp_Name = '" + fridgeStr + "' and Earliest_TDate >= '" + startTimeStr + "') as fd " +
                    "unpivot(tdate for tdates in " +
                    "(sample_tdate_1, sample_tdate_2, sample_tdate_3, sample_tdate_4, sample_tdate_5, sample_tdate_6, sample_tdate_7, sample_tdate_8, sample_tdate_9, sample_tdate_10, " +
                    "sample_tdate_11, sample_tdate_12, sample_tdate_13, sample_tdate_14, sample_tdate_15, sample_tdate_16, sample_tdate_17, sample_tdate_18, sample_tdate_19, sample_tdate_20, " +
                    "sample_tdate_21, sample_tdate_22, sample_tdate_23, sample_tdate_24, sample_tdate_25, sample_tdate_26, sample_tdate_27, sample_tdate_28, sample_tdate_29, sample_tdate_30, " +
                    "sample_tdate_31, sample_tdate_32, sample_tdate_33, sample_tdate_34, sample_tdate_35, sample_tdate_36)) tdate " +
                    "where TDate <= '" + endTimeStr + "') up " +
                    "PIVOT (MAX(TDate) FOR idon IN([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], " +
                    "[21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36])) AS PivotTable " +
                    "UNION ALL " +
                    "select Eqp_name, 'VALUE', CONVERT(VARCHAR(20),[1], 20), CONVERT(VARCHAR(20),[2], 20), CONVERT(VARCHAR(20),[3], 20), CONVERT(VARCHAR(20),[4], 20), CONVERT(VARCHAR(20),[5], 20), " +
                    "CONVERT(VARCHAR(20),[6], 20), CONVERT(VARCHAR(20),[7], 20), CONVERT(VARCHAR(20),[8], 20), CONVERT(VARCHAR(20),[9], 20), CONVERT(VARCHAR(20),[10], 20), " +
                    "CONVERT(VARCHAR(20),[11], 20), CONVERT(VARCHAR(20),[12], 20), CONVERT(VARCHAR(20),[13], 20), CONVERT(VARCHAR(20),[14], 20), CONVERT(VARCHAR(20),[15], 20), " +
                    "CONVERT(VARCHAR(20),[16], 20), CONVERT(VARCHAR(20),[17], 20), CONVERT(VARCHAR(20),[18], 20), CONVERT(VARCHAR(20),[19], 20), CONVERT(VARCHAR(20),[20], 20), " +
                    "CONVERT(VARCHAR(20),[21], 20), CONVERT(VARCHAR(20),[22], 20), CONVERT(VARCHAR(20),[23], 20), CONVERT(VARCHAR(20),[24], 20), CONVERT(VARCHAR(20),[25], 20), " +
                    "CONVERT(VARCHAR(20),[26], 20), CONVERT(VARCHAR(20),[27], 20), CONVERT(VARCHAR(20),[28], 20), CONVERT(VARCHAR(20),[29], 20), CONVERT(VARCHAR(20),[30], 20), " +
                    "CONVERT(VARCHAR(20),[31], 20), CONVERT(VARCHAR(20),[32], 20), CONVERT(VARCHAR(20),[33], 20), CONVERT(VARCHAR(20),[34], 20), CONVERT(VARCHAR(20),[35], 20), " +
                    "CONVERT(VARCHAR(20),[36], 20) " +
                    "from " +
                    "(select Eqp_Name, Earliest_TDate, tValue, idod " +
                    "from (select Eqp_Name, Earliest_TDate, tdate, tvalue, replace(tdates, 'sample_tdate_', '') idon, replace(tvalues, 'sample_value_', '') idod " +
                    "from " +
                    "(select Eqp_Name, Earliest_TDate, sample_tdate_1, sample_value_1, sample_tdate_2, sample_value_2, sample_tdate_3, sample_value_3, sample_tdate_4, sample_value_4, sample_tdate_5, sample_value_5, " +
                    "sample_tdate_6, sample_value_6, sample_tdate_7, sample_value_7, sample_tdate_8, sample_value_8, sample_tdate_9, sample_value_9, sample_tdate_10, sample_value_10, sample_tdate_11, " +
                    "sample_value_11, sample_tdate_12, sample_value_12, sample_tdate_13, sample_value_13, sample_tdate_14, sample_value_14, sample_tdate_15, sample_value_15, sample_tdate_16, " +
                    "sample_value_16, sample_tdate_17, sample_value_17, sample_tdate_18, sample_value_18, sample_tdate_19, sample_value_19, sample_tdate_20, sample_value_20, sample_tdate_21, " +
                    "sample_value_21, sample_tdate_22, sample_value_22, sample_tdate_23, sample_value_23, sample_tdate_24, sample_value_24, sample_tdate_25, sample_value_25, sample_tdate_26, " +
                    "sample_value_26, sample_tdate_27, sample_value_27, sample_tdate_28, sample_value_28, sample_tdate_29, sample_value_29, sample_tdate_30, sample_value_30, sample_tdate_31, " +
                    "sample_value_31, sample_tdate_32, sample_value_32, sample_tdate_33, sample_value_33, sample_tdate_34, sample_value_34, sample_tdate_35, sample_value_35, sample_tdate_36, " +
                    "sample_value_36 " +
                    "from fridge a, C_TMS_EQPCONFIG c where a.signal_index = c.Eqp_Id and c.Eqp_Name = '" + fridgeStr + "' and Earliest_TDate >= '" + startTimeStr + "') as fd " +
                    "unpivot(tdate for tdates in " +
                    "(sample_tdate_1, sample_tdate_2, sample_tdate_3, sample_tdate_4, sample_tdate_5, sample_tdate_6, sample_tdate_7, sample_tdate_8, sample_tdate_9, sample_tdate_10, " +
                    "sample_tdate_11, sample_tdate_12, sample_tdate_13, sample_tdate_14, sample_tdate_15, sample_tdate_16, sample_tdate_17, sample_tdate_18, sample_tdate_19, sample_tdate_20, " +
                    "sample_tdate_21, sample_tdate_22, sample_tdate_23, sample_tdate_24, sample_tdate_25, sample_tdate_26, sample_tdate_27, sample_tdate_28, sample_tdate_29, sample_tdate_30, " +
                    "sample_tdate_31, sample_tdate_32, sample_tdate_33, sample_tdate_34, sample_tdate_35, sample_tdate_36)) tdate " +
                    "unpivot(tvalue for tvalues in " +
                    "(sample_value_1, sample_value_2, sample_value_3, sample_value_4, sample_value_5, sample_value_6, sample_value_7, sample_value_8, sample_value_9, sample_value_10, " +
                    "sample_value_11, sample_value_12, sample_value_13, sample_value_14, sample_value_15, sample_value_16, sample_value_17, sample_value_18, sample_value_19, sample_value_20, " +
                    "sample_value_21, sample_value_22, sample_value_23, sample_value_24, sample_value_25, sample_value_26, sample_value_27, sample_value_28, sample_value_29, sample_value_30, " +
                    "sample_value_31, sample_value_32, sample_value_33, sample_value_34, sample_value_35, sample_value_36)) tvalue ) x " +
                    "where idod = idon and TDate <= '" + endTimeStr + "') up " +
                    "PIVOT(MAX(tValue) FOR idod IN([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], " +
                    "[21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36])) AS PivotTable";
                _log.Info("Sql string of temp: " + sqlString, result.TrxName, eventId);

                String connStr = ConfigurationManager.ConnectionStrings["TMS"].ConnectionString.ToString();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();

                var dateString = "";
                var valueString = "";
                var dataString = "";
                foreach (DataRow datarow in ds.Tables[0].Rows)
                {
                    if (datarow.ItemArray[1].Equals("DATE"))
                    {
                        dateString = dateString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else
                    {
                        valueString = valueString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                }
                dateString = dateString.Replace("],[", "");
                valueString = valueString.Replace("],[", "");

                var sqlString1 = "SELECT 'HIHI', HIHI_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "' " +
                    "UNION ALL " +
                    "SELECT 'HI', HI_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "' " +
                    "UNION ALL " +
                    "SELECT 'LOLO', LOLO_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "' " +
                    "UNION ALL " +
                    "SELECT 'LO', LO_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "'";
                _log.Info("Sql string of spec: " + sqlString, result.TrxName, eventId);

                conn.Open();
                SqlCommand cmd1 = new SqlCommand(sqlString1, conn);
                DataSet ds1 = new DataSet();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1);
                conn.Close();

                var HIHIString = "";
                var HIString = "";
                var LOLOString = "";
                var LOString = "";
                foreach (DataRow datarow in ds1.Tables[0].Rows)
                {
                    if (datarow.ItemArray[0].Equals("HIHI"))
                    {
                        HIHIString = HIHIString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else if (datarow.ItemArray[0].Equals("HI"))
                    {
                        HIString = HIString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else if (datarow.ItemArray[0].Equals("LOLO"))
                    {
                        LOLOString = LOLOString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else
                    {
                        LOString = LOString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                }
                HIHIString = HIHIString.Replace("],[", "");
                HIString = HIString.Replace("],[", "");
                LOLOString = LOLOString.Replace("],[", "");
                LOString = LOString.Replace("],[", "");

                dataString = HIHIString + HIString + LOLOString + LOString + dateString + valueString;

                result.Data = JsonConvert.SerializeObject(dataString);

                _log.Info("Fridge Temperature by minute: " + ds.Tables[0].Rows.Count, result.TrxName, eventId);
                result.Result = "Success";
                return result;
            }
            catch (Exception ex)
            {
                if (ex is NHibernate.ADOException)
                {
                    result.Error = "Database Exception: " + ex.Message.ToString();
                }
                else
                {
                    result.Error = "Logical Exception: " + ex.Message.ToString();
                }
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName, eventId);
                return result;
            }
        }

        public ActionResult GetTempbyHour(string fridgeId, string startTime, string endTime)
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge Temperature by hour", result.TrxName, eventId);
                var startTimeStr = startTime.Replace("T", " ");
                var endTimeStr = endTime.Replace("T", " ");
                var fridgeStr = fridgeId.Trim();

                var sqlString = "select Eqp_name, 'DATE', CONVERT(VARCHAR(20),[1],20), CONVERT(VARCHAR(20),[2],20), CONVERT(VARCHAR(20),[3],20), CONVERT(VARCHAR(20),[4],20), CONVERT(VARCHAR(20),[5],20),  " +
                    "CONVERT(VARCHAR(20),[6], 20), CONVERT(VARCHAR(20),[7], 20), CONVERT(VARCHAR(20),[8], 20), CONVERT(VARCHAR(20),[9], 20), CONVERT(VARCHAR(20),[10], 20), " +
                    "CONVERT(VARCHAR(20),[11], 20), CONVERT(VARCHAR(20),[12], 20), CONVERT(VARCHAR(20),[13], 20), CONVERT(VARCHAR(20),[14], 20), CONVERT(VARCHAR(20),[15], 20), " +
                    "CONVERT(VARCHAR(20),[16], 20), CONVERT(VARCHAR(20),[17], 20), CONVERT(VARCHAR(20),[18], 20), CONVERT(VARCHAR(20),[19], 20), CONVERT(VARCHAR(20),[20], 20), " +
                    "CONVERT(VARCHAR(20),[21], 20), CONVERT(VARCHAR(20),[22], 20), CONVERT(VARCHAR(20),[23], 20), CONVERT(VARCHAR(20),[24], 20), CONVERT(VARCHAR(20),[25], 20), " +
                    "CONVERT(VARCHAR(20),[26], 20), CONVERT(VARCHAR(20),[27], 20), CONVERT(VARCHAR(20),[28], 20), CONVERT(VARCHAR(20),[29], 20), CONVERT(VARCHAR(20),[30], 20), " +
                    "CONVERT(VARCHAR(20),[31], 20), CONVERT(VARCHAR(20),[32], 20), CONVERT(VARCHAR(20),[33], 20), CONVERT(VARCHAR(20),[34], 20), CONVERT(VARCHAR(20),[35], 20),  " +
                    "CONVERT(VARCHAR(20),[36], 20) " +
                    "from (select Eqp_Name, Earliest_TDate, tdate, replace(tdates, 'sample_tdate_', '') idon " +
                    "from " +
                    "(select Eqp_Name, Earliest_TDate, sample_tdate_1, sample_value_1, sample_tdate_2, sample_value_2, sample_tdate_3, sample_value_3, sample_tdate_4, sample_value_4, sample_tdate_5, sample_value_5, " +
                    "sample_tdate_6, sample_value_6, sample_tdate_7, sample_value_7, sample_tdate_8, sample_value_8, sample_tdate_9, sample_value_9, sample_tdate_10, sample_value_10, sample_tdate_11, " +
                    "sample_value_11, sample_tdate_12, sample_value_12, sample_tdate_13, sample_value_13, sample_tdate_14, sample_value_14, sample_tdate_15, sample_value_15, sample_tdate_16, " +
                    "sample_value_16, sample_tdate_17, sample_value_17, sample_tdate_18, sample_value_18, sample_tdate_19, sample_value_19, sample_tdate_20, sample_value_20, sample_tdate_21, " +
                    "sample_value_21, sample_tdate_22, sample_value_22, sample_tdate_23, sample_value_23, sample_tdate_24, sample_value_24, sample_tdate_25, sample_value_25, sample_tdate_26, " +
                    "sample_value_26, sample_tdate_27, sample_value_27, sample_tdate_28, sample_value_28, sample_tdate_29, sample_value_29, sample_tdate_30, sample_value_30, sample_tdate_31, " +
                    "sample_value_31, sample_tdate_32, sample_value_32, sample_tdate_33, sample_value_33, sample_tdate_34, sample_value_34, sample_tdate_35, sample_value_35, sample_tdate_36, " +
                    "sample_value_36 " +
                    "from hourly a, C_TMS_EQPCONFIG c " +
                    "where a.signal_index = c.Eqp_Id and c.Eqp_Name = '" + fridgeStr + "' and Earliest_TDate >= '" + startTimeStr + "') as fd " +
                    "unpivot(tdate for tdates in " +
                    "(sample_tdate_1, sample_tdate_2, sample_tdate_3, sample_tdate_4, sample_tdate_5, sample_tdate_6, sample_tdate_7, sample_tdate_8, sample_tdate_9, sample_tdate_10, " +
                    "sample_tdate_11, sample_tdate_12, sample_tdate_13, sample_tdate_14, sample_tdate_15, sample_tdate_16, sample_tdate_17, sample_tdate_18, sample_tdate_19, sample_tdate_20, " +
                    "sample_tdate_21, sample_tdate_22, sample_tdate_23, sample_tdate_24, sample_tdate_25, sample_tdate_26, sample_tdate_27, sample_tdate_28, sample_tdate_29, sample_tdate_30, " +
                    "sample_tdate_31, sample_tdate_32, sample_tdate_33, sample_tdate_34, sample_tdate_35, sample_tdate_36)) tdate " +
                    "where TDate <= '" + endTimeStr + "') up " +
                    "PIVOT (MAX(TDate) FOR idon IN([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], " +
                    "[21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36])) AS PivotTable " +
                    "UNION ALL " +
                    "select Eqp_name, 'VALUE', CONVERT(VARCHAR(20),[1], 20), CONVERT(VARCHAR(20),[2], 20), CONVERT(VARCHAR(20),[3], 20), CONVERT(VARCHAR(20),[4], 20), CONVERT(VARCHAR(20),[5], 20), " + 
                    "CONVERT(VARCHAR(20),[6], 20), CONVERT(VARCHAR(20),[7], 20), CONVERT(VARCHAR(20),[8], 20), CONVERT(VARCHAR(20),[9], 20), CONVERT(VARCHAR(20),[10], 20), " +
                    "CONVERT(VARCHAR(20),[11], 20), CONVERT(VARCHAR(20),[12], 20), CONVERT(VARCHAR(20),[13], 20), CONVERT(VARCHAR(20),[14], 20), CONVERT(VARCHAR(20),[15], 20), " +
                    "CONVERT(VARCHAR(20),[16], 20), CONVERT(VARCHAR(20),[17], 20), CONVERT(VARCHAR(20),[18], 20), CONVERT(VARCHAR(20),[19], 20), CONVERT(VARCHAR(20),[20], 20), " +
                    "CONVERT(VARCHAR(20),[21], 20), CONVERT(VARCHAR(20),[22], 20), CONVERT(VARCHAR(20),[23], 20), CONVERT(VARCHAR(20),[24], 20), CONVERT(VARCHAR(20),[25], 20), " +
                    "CONVERT(VARCHAR(20),[26], 20), CONVERT(VARCHAR(20),[27], 20), CONVERT(VARCHAR(20),[28], 20), CONVERT(VARCHAR(20),[29], 20), CONVERT(VARCHAR(20),[30], 20), " +
                    "CONVERT(VARCHAR(20),[31], 20), CONVERT(VARCHAR(20),[32], 20), CONVERT(VARCHAR(20),[33], 20), CONVERT(VARCHAR(20),[34], 20), CONVERT(VARCHAR(20),[35], 20), " +
                    "CONVERT(VARCHAR(20),[36], 20) " +
                    "from " +
                    "(select Eqp_Name, Earliest_TDate, tValue, idod " +
                    "from (select Eqp_Name, Earliest_TDate, tdate, tvalue, replace(tdates, 'sample_tdate_', '') idon, replace(tvalues, 'sample_value_', '') idod " +
                    "from " +
                    "(select Eqp_Name, Earliest_TDate, sample_tdate_1, sample_value_1, sample_tdate_2, sample_value_2, sample_tdate_3, sample_value_3, sample_tdate_4, sample_value_4, sample_tdate_5, sample_value_5, " +
                    "sample_tdate_6, sample_value_6, sample_tdate_7, sample_value_7, sample_tdate_8, sample_value_8, sample_tdate_9, sample_value_9, sample_tdate_10, sample_value_10, sample_tdate_11, " +
                    "sample_value_11, sample_tdate_12, sample_value_12, sample_tdate_13, sample_value_13, sample_tdate_14, sample_value_14, sample_tdate_15, sample_value_15, sample_tdate_16, " +
                    "sample_value_16, sample_tdate_17, sample_value_17, sample_tdate_18, sample_value_18, sample_tdate_19, sample_value_19, sample_tdate_20, sample_value_20, sample_tdate_21, " +
                    "sample_value_21, sample_tdate_22, sample_value_22, sample_tdate_23, sample_value_23, sample_tdate_24, sample_value_24, sample_tdate_25, sample_value_25, sample_tdate_26, " +
                    "sample_value_26, sample_tdate_27, sample_value_27, sample_tdate_28, sample_value_28, sample_tdate_29, sample_value_29, sample_tdate_30, sample_value_30, sample_tdate_31, " +
                    "sample_value_31, sample_tdate_32, sample_value_32, sample_tdate_33, sample_value_33, sample_tdate_34, sample_value_34, sample_tdate_35, sample_value_35, sample_tdate_36, " +
                    "sample_value_36 " +
                    "from hourly a, C_TMS_EQPCONFIG c where a.signal_index = c.Eqp_Id and c.Eqp_Name = '" + fridgeStr + "' and Earliest_TDate >= '" + startTimeStr + "') as fd " +
                    "unpivot(tdate for tdates in " +
                    "(sample_tdate_1, sample_tdate_2, sample_tdate_3, sample_tdate_4, sample_tdate_5, sample_tdate_6, sample_tdate_7, sample_tdate_8, sample_tdate_9, sample_tdate_10, " +
                    "sample_tdate_11, sample_tdate_12, sample_tdate_13, sample_tdate_14, sample_tdate_15, sample_tdate_16, sample_tdate_17, sample_tdate_18, sample_tdate_19, sample_tdate_20, " +
                    "sample_tdate_21, sample_tdate_22, sample_tdate_23, sample_tdate_24, sample_tdate_25, sample_tdate_26, sample_tdate_27, sample_tdate_28, sample_tdate_29, sample_tdate_30, " +
                    "sample_tdate_31, sample_tdate_32, sample_tdate_33, sample_tdate_34, sample_tdate_35, sample_tdate_36)) tdate " +
                    "unpivot(tvalue for tvalues in " +
                    "(sample_value_1, sample_value_2, sample_value_3, sample_value_4, sample_value_5, sample_value_6, sample_value_7, sample_value_8, sample_value_9, sample_value_10, " +
                    "sample_value_11, sample_value_12, sample_value_13, sample_value_14, sample_value_15, sample_value_16, sample_value_17, sample_value_18, sample_value_19, sample_value_20, " +
                    "sample_value_21, sample_value_22, sample_value_23, sample_value_24, sample_value_25, sample_value_26, sample_value_27, sample_value_28, sample_value_29, sample_value_30, " +
                    "sample_value_31, sample_value_32, sample_value_33, sample_value_34, sample_value_35, sample_value_36)) tvalue ) x " +
                    "where idod = idon and TDate <= '" + endTimeStr + "') up " +
                    "PIVOT(MAX(tValue) FOR idod IN([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], " +
                    "[21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36])) AS PivotTable";
                _log.Info("Sql string of temp: " + sqlString, result.TrxName, eventId);

                String connStr = ConfigurationManager.ConnectionStrings["TMS"].ConnectionString.ToString();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();

                var dateString = "";
                var valueString = "";
                var dataString = "";
                foreach (DataRow datarow in ds.Tables[0].Rows)
                {
                    if (datarow.ItemArray[1].Equals("DATE"))
                    {
                        dateString = dateString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else
                    {
                        valueString = valueString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                }
                dateString = dateString.Replace("],[", "");
                valueString = valueString.Replace("],[", "");

                var sqlString1 = "SELECT 'HIHI', HIHI_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "' " +
                    "UNION ALL " +
                    "SELECT 'HI', HI_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "' " +
                    "UNION ALL " +
                    "SELECT 'LOLO', LOLO_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "' " +
                    "UNION ALL " +
                    "SELECT 'LO', LO_Limit FROM C_TMS_EQPCONFIG WHERE Eqp_Name = '" + fridgeId + "'";
                _log.Info("Sql string of spec: " + sqlString1, result.TrxName, eventId);

                conn.Open();
                SqlCommand cmd1 = new SqlCommand(sqlString1, conn);
                DataSet ds1 = new DataSet();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1);
                conn.Close();

                var HIHIString = "";
                var HIString = "";
                var LOLOString = "";
                var LOString = "";
                foreach (DataRow datarow in ds1.Tables[0].Rows)
                {
                    if (datarow.ItemArray[0].Equals("HIHI"))
                    {
                        HIHIString = HIHIString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else if (datarow.ItemArray[0].Equals("HI"))
                    {
                        HIString = HIString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else if (datarow.ItemArray[0].Equals("LOLO"))
                    {
                        LOLOString = LOLOString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                    else
                    {
                        LOString = LOString + JsonConvert.SerializeObject(datarow.ItemArray) + ",";
                    }
                }
                HIHIString = HIHIString.Replace("],[", "");
                HIString = HIString.Replace("],[", "");
                LOLOString = LOLOString.Replace("],[", "");
                LOString = LOString.Replace("],[", "");

                dataString = HIHIString + HIString + LOLOString + LOString + dateString + valueString;

                result.Data = JsonConvert.SerializeObject(dataString);

                _log.Info("Fridge Temperature by hour: " + ds.Tables[0].Rows.Count, result.TrxName, eventId);
                result.Result = "Success";
                return result;
            }
            catch (Exception ex)
            {
                if (ex is NHibernate.ADOException)
                {
                    result.Error = "Database Exception: " + ex.Message.ToString();
                }
                else
                {
                    result.Error = "Logical Exception: " + ex.Message.ToString();
                }
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName, eventId);
                return result;
            }
        }

        public ActionResult DownloadTempbyMin(string fridgeId, string startTime, string endTime)
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge Temperature by minute for exporting", result.TrxName, eventId);
                var startTimeStr = startTime.Replace("T", " ");
                var endTimeStr = endTime.Replace("T", " ");
                var fridgeStr = fridgeId.Trim();

                var sqlString = "select Eqp_Name as freezer, TDate as datetime, tValue as temperature " +
                    "from " +
                    "(select Eqp_Name, tdate, tvalue, replace(tdates, 'sample_tdate_', '') idon, replace(tvalues, 'sample_value_', '') idod " +
                    "from " +
                    "(select Eqp_Name, sample_tdate_1, sample_value_1, sample_tdate_2, sample_value_2, sample_tdate_3, sample_value_3, sample_tdate_4, sample_value_4, " +
                    "sample_tdate_5, sample_value_5, sample_tdate_6, sample_value_6, sample_tdate_7, sample_value_7, sample_tdate_8, sample_value_8, sample_tdate_9, sample_value_9, " +
                    "sample_tdate_10, sample_value_10, sample_tdate_11, sample_value_11, sample_tdate_12, sample_value_12, sample_tdate_13, sample_value_13, sample_tdate_14, sample_value_14, " +
                    "sample_tdate_15, sample_value_15, sample_tdate_16, sample_value_16, sample_tdate_17, sample_value_17, sample_tdate_18, sample_value_18, sample_tdate_19, sample_value_19, " +
                    "sample_tdate_20, sample_value_20, sample_tdate_21, sample_value_21, sample_tdate_22, sample_value_22, sample_tdate_23, sample_value_23, sample_tdate_24, sample_value_24, " +
                    "sample_tdate_25, sample_value_25, sample_tdate_26, sample_value_26, sample_tdate_27, sample_value_27, sample_tdate_28, sample_value_28, sample_tdate_29, sample_value_29, " +
                    "sample_tdate_30, sample_value_30, sample_tdate_31, sample_value_31, sample_tdate_32, sample_value_32, sample_tdate_33, sample_value_33, sample_tdate_34, sample_value_34, " +
                    "sample_tdate_35, sample_value_35, sample_tdate_36, sample_value_36 " +
                    "from fridge a, C_TMS_EQPCONFIG c " +
                    "where a.signal_index = c.Eqp_Id " +
                    "and c.Eqp_Name = '" + fridgeStr + "' " +
                    "and Earliest_TDate >= '" + startTimeStr + "') as fd " +
                    "unpivot( " +
                    "   tdate for tdates in " +
                    "   (sample_tdate_1, sample_tdate_2, sample_tdate_3, sample_tdate_4, sample_tdate_5, sample_tdate_6, sample_tdate_7, sample_tdate_8,sample_tdate_9, sample_tdate_10, " +
                    "   sample_tdate_11, sample_tdate_12, sample_tdate_13, sample_tdate_14, sample_tdate_15, sample_tdate_16, sample_tdate_17, sample_tdate_18,sample_tdate_19, sample_tdate_20, " +
                    "   sample_tdate_21, sample_tdate_22, sample_tdate_23, sample_tdate_24, sample_tdate_25, sample_tdate_26, sample_tdate_27, sample_tdate_28,sample_tdate_29, sample_tdate_30, " +
                    "   sample_tdate_31, sample_tdate_32, sample_tdate_33, sample_tdate_34, sample_tdate_35, sample_tdate_36)) tdate " +
                    "unpivot " +
                    "   (tvalue for tvalues in " +
                    "   (sample_value_1, sample_value_2, sample_value_3, sample_value_4, sample_value_5, sample_value_6, sample_value_7, sample_value_8, sample_value_9, sample_value_10, " +
                    "   sample_value_11, sample_value_12, sample_value_13, sample_value_14, sample_value_15, sample_value_16, sample_value_17, sample_value_18, sample_value_19, sample_value_20, " +
                    "   sample_value_21, sample_value_22, sample_value_23, sample_value_24, sample_value_25, sample_value_26, sample_value_27, sample_value_28, sample_value_29, sample_value_30, " +
                    "   sample_value_31, sample_value_32, sample_value_33, sample_value_34, sample_value_35, sample_value_36)) tvalue " +
                    ") x where idod = idon and TDate <= '" + endTimeStr + "' " +
                    "order by datetime";
                _log.Info("Sql string: " + sqlString, result.TrxName, eventId);

                String connStr = ConfigurationManager.ConnectionStrings["TMS"].ConnectionString.ToString();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                result.Data = JsonConvert.SerializeObject(ds.Tables[0]);

                _log.Info("Fridge Temperature by minute: " + ds.Tables[0].Rows.Count, result.TrxName, eventId);
                result.Result = "Success";
                return result;
            }
            catch (Exception ex)
            {
                if (ex is NHibernate.ADOException)
                {
                    result.Error = "Database Exception: " + ex.Message.ToString();
                }
                else
                {
                    result.Error = "Logical Exception: " + ex.Message.ToString();
                }
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName, eventId);
                return result;
            }
        }

        public ActionResult DownloadTempbyHour(string fridgeId, string startTime, string endTime)
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge Temperature by hour for exporting", result.TrxName, eventId);
                var startTimeStr = startTime.Replace("T", " ");
                var endTimeStr = endTime.Replace("T", " ");
                var fridgeStr = fridgeId.Trim();

                var sqlString = "select Eqp_Name as freezer, TDate as datetime, tValue as temperature " +
                    "from " +
                    "(select Eqp_Name, tdate, tvalue, replace(tdates, 'sample_tdate_', '') idon, replace(tvalues, 'sample_value_', '') idod " +
                    "from " +
                    "(select Eqp_Name, sample_tdate_1, sample_value_1, sample_tdate_2, sample_value_2, sample_tdate_3, sample_value_3, sample_tdate_4, sample_value_4, " +
                    "sample_tdate_5, sample_value_5, sample_tdate_6, sample_value_6, sample_tdate_7, sample_value_7, sample_tdate_8, sample_value_8, sample_tdate_9, sample_value_9, " +
                    "sample_tdate_10, sample_value_10, sample_tdate_11, sample_value_11, sample_tdate_12, sample_value_12, sample_tdate_13, sample_value_13, sample_tdate_14, sample_value_14, " +
                    "sample_tdate_15, sample_value_15, sample_tdate_16, sample_value_16, sample_tdate_17, sample_value_17, sample_tdate_18, sample_value_18, sample_tdate_19, sample_value_19, " +
                    "sample_tdate_20, sample_value_20, sample_tdate_21, sample_value_21, sample_tdate_22, sample_value_22, sample_tdate_23, sample_value_23, sample_tdate_24, sample_value_24, " +
                    "sample_tdate_25, sample_value_25, sample_tdate_26, sample_value_26, sample_tdate_27, sample_value_27, sample_tdate_28, sample_value_28, sample_tdate_29, sample_value_29, " +
                    "sample_tdate_30, sample_value_30, sample_tdate_31, sample_value_31, sample_tdate_32, sample_value_32, sample_tdate_33, sample_value_33, sample_tdate_34, sample_value_34, " +
                    "sample_tdate_35, sample_value_35, sample_tdate_36, sample_value_36 " +
                    "from hourly a, C_TMS_EQPCONFIG c " +
                    "where a.signal_index = c.Eqp_Id " +
                    "and c.Eqp_Name = '" + fridgeStr + "' " +
                    "and Earliest_TDate >= '" + startTimeStr + "') as fd " +
                    "unpivot( " +
                    "   tdate for tdates in " +
                    "   (sample_tdate_1, sample_tdate_2, sample_tdate_3, sample_tdate_4, sample_tdate_5, sample_tdate_6, sample_tdate_7, sample_tdate_8,sample_tdate_9, sample_tdate_10, " +
                    "   sample_tdate_11, sample_tdate_12, sample_tdate_13, sample_tdate_14, sample_tdate_15, sample_tdate_16, sample_tdate_17, sample_tdate_18,sample_tdate_19, sample_tdate_20, " +
                    "   sample_tdate_21, sample_tdate_22, sample_tdate_23, sample_tdate_24, sample_tdate_25, sample_tdate_26, sample_tdate_27, sample_tdate_28,sample_tdate_29, sample_tdate_30, " +
                    "   sample_tdate_31, sample_tdate_32, sample_tdate_33, sample_tdate_34, sample_tdate_35, sample_tdate_36)) tdate " +
                    "unpivot " +
                    "   (tvalue for tvalues in " +
                    "   (sample_value_1, sample_value_2, sample_value_3, sample_value_4, sample_value_5, sample_value_6, sample_value_7, sample_value_8, sample_value_9, sample_value_10, " +
                    "   sample_value_11, sample_value_12, sample_value_13, sample_value_14, sample_value_15, sample_value_16, sample_value_17, sample_value_18, sample_value_19, sample_value_20, " +
                    "   sample_value_21, sample_value_22, sample_value_23, sample_value_24, sample_value_25, sample_value_26, sample_value_27, sample_value_28, sample_value_29, sample_value_30, " +
                    "   sample_value_31, sample_value_32, sample_value_33, sample_value_34, sample_value_35, sample_value_36)) tvalue " +
                    ") x where idod = idon and TDate <= '" + endTimeStr + "' " +
                    "order by datetime";
                _log.Info("Sql string: " + sqlString, result.TrxName, eventId);

                String connStr = ConfigurationManager.ConnectionStrings["TMS"].ConnectionString.ToString();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                result.Data = JsonConvert.SerializeObject(ds.Tables[0]);

                _log.Info("Fridge Temperature by hour: " + ds.Tables[0].Rows.Count, result.TrxName, eventId);
                result.Result = "Success";
                return result;
            }
            catch (Exception ex)
            {
                if (ex is NHibernate.ADOException)
                {
                    result.Error = "Database Exception: " + ex.Message.ToString();
                }
                else
                {
                    result.Error = "Logical Exception: " + ex.Message.ToString();
                }
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName, eventId);
                return result;
            }
        }
    }
}