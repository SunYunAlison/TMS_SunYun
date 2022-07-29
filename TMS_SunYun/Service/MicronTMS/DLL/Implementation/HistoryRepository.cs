using NHibernate;
using MicronTMS.DLL.Entities;

namespace MicronTMS.DLL.Implementation
{
    public class HistoryRepository 
    {
        public void WriteInUserInfoHis(ISession session, string eventId, string trxName, CUserInfo cUserInfo)
        {
            var hUserInfoTrn = new HUserInfo
            {
                EventId = eventId,
                TrxName = trxName,
                UserId = cUserInfo.UserId,
                UserName = cUserInfo.UserName,
                Email = cUserInfo.Email,
                ContactNo = cUserInfo.ContactNo,
                LmUser = cUserInfo.LmUser,
                LmTime = cUserInfo.LmTime,
            };

            session.Save(hUserInfoTrn);
        }

        public void WriteInStatusInfoHis(ISession session, REqpStatus rStatusInfo)
        {
            var hStatusInfoTrn = new HEqpStatus
            {
                EqpId = rStatusInfo.EqpId,
                Status = rStatusInfo.Status,
                SubStatus = rStatusInfo.SubStatus,
                Temp = rStatusInfo.Temp,
                IsOpen = rStatusInfo.IsOpen,
                DoorOpenTime = rStatusInfo.DoorOpenTime,
                UpdateBy = rStatusInfo.UpdateBy,
                UpdateTime = rStatusInfo.UpdateTime,
                //Offset = rStatusInfo.Offset
            };

            session.Save(hStatusInfoTrn);
        }
    }
}
