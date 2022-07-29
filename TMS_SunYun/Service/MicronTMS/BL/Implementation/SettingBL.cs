using System;
using System.Linq;
using Newtonsoft.Json;
using MicronTMS.Helper;
using MicronTMS.DLL.Implementation;
using MicronTMS.DLL.Entities;
using MicronTMS.DLL.Models;
using NHibernate;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace MicronTMS.BL.Implementation
{
    public class SettingBL
    {
        private readonly Repository<REqpStatus> _statusRepo;
        private readonly Repository<CTmsEqpConfig> _configRepo;
        private readonly HistoryRepository _repositoryForHis;

        private ISession _session;
        private readonly Logger _log;

        public SettingBL()
        {
            _statusRepo = new Repository<REqpStatus>();
            _configRepo = new Repository<CTmsEqpConfig>();
            _repositoryForHis = new HistoryRepository();

            _log = new Logger();
        }

        public ActionResult GetRealTimeStatusAndOffset()
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge Status from R_TMS_EQPSTATUS", result.TrxName, eventId);
                var sqlString = "SELECT EQP_NAME, b.EQP_ID, STATUS, OFFSET " +
                    "FROM R_TMS_EQPSTATUS a, C_TMS_EQPCONFIG b " +
                    "WHERE a.EQP_ID = b.EQP_ID " +
                    "ORDER BY b.EQP_NAME";
                _log.Info("Sql string: " + sqlString, result.TrxName, eventId);

                String connStr = ConfigurationManager.ConnectionStrings["TMS"].ConnectionString.ToString();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                result.Data = JsonConvert.SerializeObject(ds.Tables[0]);

                _log.Info("Fridge status list count: " + ds.Tables[0].Rows.Count, result.TrxName, eventId);
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

        public ActionResult UpdateStatusAndOffset(List<StatusOffsetModel> statusOffsetList)
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();
            var lmTime = DateTime.Now;

            try
            {
                _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();
                _session.Transaction.Begin();

                foreach (StatusOffsetModel statusOffsetInfo in statusOffsetList)
                {
                    _log.Info("Fridge Id: " + statusOffsetInfo.EqpId, result.TrxName, eventId);
                    var statusFlag = "N";
                    var offsetFlag = "N";

                    REqpStatus statusInfoDB = _statusRepo.GetById(statusOffsetInfo.EqpId, _session);
                    _session.Transaction.Commit();

                    if (statusInfoDB == null)
                        throw new InvalidOperationException("This fridge: " + statusOffsetInfo.EqpId + " not exist.");

                    if (statusInfoDB.Status != statusOffsetInfo.Status)
                    {
                        statusInfoDB.Status = statusOffsetInfo.Status;
                        statusInfoDB.UpdateTime = lmTime;
                        statusInfoDB.UpdateBy = "TMS_AP";
                        statusFlag = "Y";
                    }

                    _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();
                    _session.Transaction.Begin();

                    CTmsEqpConfig offsetInfoDB = _configRepo.GetById(statusOffsetInfo.EqpId, _session);
                    _session.Transaction.Commit();

                    if (offsetInfoDB == null)
                        throw new InvalidOperationException("This fridge: " + statusOffsetInfo.EqpId + " not exist.");
                    
                    {
                        offsetInfoDB.Offset = statusOffsetInfo.Offset;
                        offsetInfoDB.UpdateTime = lmTime;
                        offsetInfoDB.UpdateBy = "TMS_AP";
                        offsetFlag = "Y";
                    }

                    _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();
                    _session.Transaction.Begin();
                    if (statusFlag == "Y")
                    {
                        _statusRepo.Update(statusInfoDB, _session);
                        _repositoryForHis.WriteInStatusInfoHis(_session, statusInfoDB);
                    }

                    if (offsetFlag == "Y")
                        _configRepo.Update(offsetInfoDB, _session);
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