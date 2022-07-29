using System;
using System.Linq;
using Newtonsoft.Json;
using MicronTMS.Helper;
using MicronTMS.DLL.Implementation;
using MicronTMS.DLL.Entities;
using NHibernate;

namespace MicronTMS.BL.Implementation
{
    public class AuthenticationBL
    {
        private readonly Repository<CUserInfo> _loginCUserRepo;
        private readonly HistoryRepository _repositoryForHis;

        private ISession _session;
        private readonly Logger _log;

        public AuthenticationBL()
        {
            _loginCUserRepo = new Repository<CUserInfo>();
            _repositoryForHis = new HistoryRepository();

            _log = new Logger();
        }

        public ActionResult NewAccount(CUserInfo userInfo)
        {
            var result = new ActionResult();
            var trx = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();
            var lmTime = DateTime.Now;

            try
            {
                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
                _session.Transaction.Begin();

                _log.Info("User Id: " + userInfo.UserId, result.TrxName);

                if (userInfo.UserId != "NA")
                {
                    var userData = _loginCUserRepo.GetById(userInfo.UserId, _session);

                    if (userData != null)
                        throw new InvalidOperationException("This account: " + userInfo.UserId + " already exist.");
                }
                else
                {
                    userInfo.UserId = lmTime.ToString("yyyyMMddHHmmss");
                }

                userInfo.LmUser = userInfo.UserId;
                userInfo.LmTime = lmTime;

                _loginCUserRepo.Create(userInfo, _session);
                _repositoryForHis.WriteInUserInfoHis(_session, eventId, trx, userInfo);

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

        public ActionResult UpdateAccountInfo(CUserInfo userInfo)
        {
            var result = new ActionResult();
            var trx = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();
            var lmTime = DateTime.Now;

            try
            {
                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
                _session.Transaction.Begin();

                _log.Info("User Id: " + userInfo.UserId, result.TrxName);

                CUserInfo userInfoDB = _loginCUserRepo.GetById(userInfo.UserId, _session);
                _session.Transaction.Commit();

                if (userInfoDB == null)
                    throw new InvalidOperationException("This account: " + userInfo.UserId + " not exist.");

                userInfoDB.Email = userInfo.Email;
                userInfoDB.ContactNo = userInfo.ContactNo;
                userInfoDB.LmTime = lmTime;
                userInfoDB.LmUser = userInfo.UserId;

                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
                _session.Transaction.Begin();

                _loginCUserRepo.Update(userInfoDB, _session);
                _repositoryForHis.WriteInUserInfoHis(_session, eventId, trx, userInfoDB);

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

        public ActionResult GetAllUsers()
        {
            var result = new ActionResult();
            result.TrxName = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();

            try
            {
                _log.Info("Get User List from C_USER_INFO", result.TrxName, eventId);
                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();

                var userList = _loginCUserRepo.GetAll(_session).ToList();

                result.Data = JsonConvert.SerializeObject(userList);

                _log.Info("User list count: " + userList.Count(), result.TrxName, eventId);
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

        public ActionResult DeleteAccount(CUserInfo userInfo)
        {
            var result = new ActionResult();
            var trx = this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            string eventId = Guid.NewGuid().ToString("N").ToUpper();
            var lmTime = DateTime.Now;

            try
            {
                _session = new NhibernateSessionFactory().GetSessionFactory("CMC").OpenSession();
                _session.Transaction.Begin();

                _log.Info("User Id: " + userInfo.UserId, result.TrxName);

                var userData = _loginCUserRepo.GetById(userInfo.UserId, _session);

                if (userData == null)
                    throw new InvalidOperationException("This account: " + userInfo.UserId + " not exist.");

                userInfo.LmUser = userInfo.UserId;
                userInfo.LmTime = lmTime;

                _loginCUserRepo.Delete (userData.UserId, _session);
                _repositoryForHis.WriteInUserInfoHis(_session, eventId, trx, userInfo);

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