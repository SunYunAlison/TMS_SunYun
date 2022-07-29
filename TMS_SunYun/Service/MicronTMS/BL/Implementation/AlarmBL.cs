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
    public class AlarmBL
    {
        private readonly Repository<RTmsAlert> _alarmRepo;

        private ISession _session;
        private readonly Logger _log;

        public AlarmBL()
        {
            _alarmRepo = new Repository<RTmsAlert>();

            _log = new Logger();
        }

        public ActionResult GetAlarmList()
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get Fridge Alert from R_TMS_ALERT", result.TrxName, eventId);

                var sqlString = "SELECT EQP_NAME, * " +
                    "FROM R_TMS_ALERT a, C_TMS_EQPCONFIG b " +
                    "WHERE a.EQP_ID = b.EQP_ID AND COMMENT is null " +
                    "ORDER BY ALARM_TIME ";
                _log.Info("Sql string: " + sqlString, result.TrxName, eventId);

                String connStr = ConfigurationManager.ConnectionStrings["TMS"].ConnectionString.ToString();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                result.Data = JsonConvert.SerializeObject(ds.Tables[0]);

                _log.Info("Fridge alert list count: " + ds.Tables[0].Rows.Count, result.TrxName, eventId);
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

        public ActionResult UpdateAlarmAction(RTmsAlert alermComment)
        {
            var result = new ActionResult();
            var trx = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();
            var lmTime = DateTime.Now;

            try
            {
                _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();
                _session.Transaction.Begin();

                _log.Info("Fridge Alarm Id: " + alermComment.Id, result.TrxName, eventId);

                RTmsAlert AlarmInfoDB = _alarmRepo.GetById(alermComment.Id, _session);

                if (AlarmInfoDB == null)
                    throw new InvalidOperationException("This fridge alarm: " + alermComment.Id + " not exist.");

                AlarmInfoDB.Comment = alermComment.Comment;
                AlarmInfoDB.CommentBy = alermComment.CommentBy;
                AlarmInfoDB.CommentTime = lmTime;
                AlarmInfoDB.UpdateTime = lmTime;
                AlarmInfoDB.UpdateBy = "TMS_AP";

                _session = new NhibernateSessionFactory().GetSessionFactory("TMS").OpenSession();
                _session.Transaction.Begin();

                _alarmRepo.Update(AlarmInfoDB, _session);

                _session.Transaction.Commit();
                _log.Info("End", result.TrxName);
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