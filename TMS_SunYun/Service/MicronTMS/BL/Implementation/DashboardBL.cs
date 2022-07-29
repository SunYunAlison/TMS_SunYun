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
using System.Collections.Generic;

namespace MicronTMS.BL.Implementation
{
    public class DashboardBL
    {
        private readonly Repository<CTmsEqpConfig> _fridgeCfgRepo;

        private ISession _session;
        private readonly Logger _log;

        public DashboardBL()
        {
            _fridgeCfgRepo = new Repository<CTmsEqpConfig>();

            _log = new Logger();
        }

        public ActionResult GetRealTimeTemp()
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge Real Time Data", result.TrxName, eventId);

                var sqlString = "SELECT cfg.Eqp_Id, Eqp_Name, datetime, temperature, HIHI_Limit, LOLO_Limit, HI_Limit, LO_Limit, Status, Sub_Status, Is_Open " +
                    "FROM " +
                    "(SELECT Signal_Index, TDate as datetime, tValue as temperature, RANK() OVER(PARTITION BY Signal_Index ORDER BY TDate DESC) as index_Num " +
                    "FROM " +
                    "(SELECT Signal_Index, tdate, tvalue, replace(tdates, 'sample_tdate_', '') idon, replace(tvalues, 'sample_value_', '') idod " +
                    "FROM " +
                    "(SELECT a.*, sample_tdate_1, sample_value_1, sample_tdate_2, sample_value_2, sample_tdate_3, sample_value_3, sample_tdate_4, sample_value_4, " +
                    "sample_tdate_5, sample_value_5, sample_tdate_6, sample_value_6, sample_tdate_7, sample_value_7, sample_tdate_8, sample_value_8, sample_tdate_9, sample_value_9, + " +
                    "sample_tdate_10, sample_value_10, sample_tdate_11, sample_value_11, sample_tdate_12, sample_value_12, sample_tdate_13, sample_value_13, sample_tdate_14, sample_value_14, " +
                    "sample_tdate_15, sample_value_15, sample_tdate_16, sample_value_16, sample_tdate_17, sample_value_17, sample_tdate_18, sample_value_18, sample_tdate_19, sample_value_19, " +
                    "sample_tdate_20, sample_value_20, sample_tdate_21, sample_value_21, sample_tdate_22, sample_value_22, sample_tdate_23, sample_value_23, sample_tdate_24, sample_value_24, " +
                    "sample_tdate_25, sample_value_25, sample_tdate_26, sample_value_26, sample_tdate_27, sample_value_27, sample_tdate_28, sample_value_28, sample_tdate_29, sample_value_29, " +
                    "sample_tdate_30, sample_value_30, sample_tdate_31, sample_value_31, sample_tdate_32, sample_value_32, sample_tdate_33, sample_value_33, sample_tdate_34, sample_value_34, " +
                    "sample_tdate_35, sample_value_35, sample_tdate_36, sample_value_36 " +
                    "FROM (SELECT Signal_Index, MAX(Latest_TDate) Max_Time FROM Fridge GROUP BY Signal_Index) a, Fridge b WHERE a.Signal_Index = b.Signal_Index AND a.Max_Time = b.Latest_TDate) a " +
                    "UNPIVOT (tdate for tdates in " +
                    "(sample_tdate_1, sample_tdate_2, sample_tdate_3, sample_tdate_4, sample_tdate_5, sample_tdate_6, sample_tdate_7, sample_tdate_8, sample_tdate_9, sample_tdate_10, " +
                    "sample_tdate_11, sample_tdate_12, sample_tdate_13, sample_tdate_14, sample_tdate_15, sample_tdate_16, sample_tdate_17, sample_tdate_18, sample_tdate_19, sample_tdate_20, " +
                    "sample_tdate_21, sample_tdate_22, sample_tdate_23, sample_tdate_24, sample_tdate_25, sample_tdate_26, sample_tdate_27, sample_tdate_28, sample_tdate_29, sample_tdate_30, " +
                    "sample_tdate_31, sample_tdate_32, sample_tdate_33, sample_tdate_34, sample_tdate_35, sample_tdate_36)) tdate " +
                    "UNPIVOT (tvalue for tvalues in " +
                    "(sample_value_1, sample_value_2, sample_value_3, sample_value_4, sample_value_5, sample_value_6, sample_value_7, sample_value_8, sample_value_9, sample_value_10, " +
                    "sample_value_11, sample_value_12, sample_value_13, sample_value_14, sample_value_15, sample_value_16, sample_value_17, sample_value_18, sample_value_19, sample_value_20, " +
                    "sample_value_21, sample_value_22, sample_value_23, sample_value_24, sample_value_25, sample_value_26, sample_value_27, sample_value_28, sample_value_29, sample_value_30, " +
                    "sample_value_31, sample_value_32, sample_value_33, sample_value_34, sample_value_35, sample_value_36)) tvalue) x WHERE idod = idon) temp, " +
                    "(SELECT Eqp_Id, Eqp_Name, HIHI_Limit, LOLO_Limit, HI_Limit, LO_Limit FROM C_TMS_EQPCONFIG) cfg, " +
                    "(SELECT Eqp_Id, Status, Sub_Status, Is_Open FROM R_TMS_EQPSTATUS) s " +
                    "WHERE temp.Signal_Index = cfg.Eqp_Id AND cfg.Eqp_Id = s.Eqp_Id AND index_Num = 1 " +
                    "ORDER BY Eqp_Id";
                _log.Info("Sql string: " + sqlString, result.TrxName, eventId);

                String connStr = ConfigurationManager.ConnectionStrings["TMS"].ConnectionString.ToString();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                result.Data = JsonConvert.SerializeObject(ds.Tables[0]);

                _log.Info("Fridge Real Time Data Count: " + ds.Tables[0].Rows.Count, result.TrxName, eventId);
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

        public ActionResult UpdateTempbyLimit(List<CTmsEqpConfig> fridgeInfoList)
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();
                _session.Transaction.Begin();

                foreach (CTmsEqpConfig fridgeInfo in fridgeInfoList)
                {
                    _log.Info("Fridge Id: " + fridgeInfo.EqpId, result.TrxName, eventId);

                    CTmsEqpConfig fridgeInfoDB = _fridgeCfgRepo.GetById(fridgeInfo.EqpId, _session);
                    _session.Transaction.Commit();

                    if (fridgeInfoDB == null)
                        throw new InvalidOperationException("This fridge: " + fridgeInfo.EqpId + " not exist.");

                    fridgeInfoDB.HIHI = fridgeInfo.HIHI;
                    fridgeInfoDB.HI = fridgeInfo.HI;
                    fridgeInfoDB.LOLO = fridgeInfo.LOLO;
                    fridgeInfoDB.LO = fridgeInfo.LO;

                    _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();
                    _session.Transaction.Begin();

                    _fridgeCfgRepo.Update(fridgeInfoDB, _session);
                }
                
                _session.Transaction.Commit();
                _log.Info("End", result.TrxName, eventId);
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
                _session.Transaction.Rollback();
                result.Result = "Fail";
                _log.Error(result.Error, result.TrxName);
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
    }
}