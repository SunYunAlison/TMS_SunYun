export class AlarmData {
    EQP_NAME: string = "";
    EQP_ID: string = "";
    ID: number = 0;
    ALARM_STATUS: string = "";
    ALARM_TIME: string = "";
    ALARM_VALUE: number = 0;
    RECOVER_TIME: string="";
    COMMENT: string="";
    COMMENT_BY: string="";
    COMMENT_TIME: string="";
    IS_SEND_ALARM: string="";
    IS_SEND_RECOVER: string="";
    UPDATE_BY: string="";
    ETL: string="";
    UPDATE_TIME: string="";
}

export class AlarmAcion {
    Id: string = "";
    EqpId: string = "";
    AlarmStatus:string = "";
    AlarmTime: string = "";
    AlarmValue:number = 0;
    RecoverTime: string = "";
    Comment: string = "";
    CommentBy: string = "";
    CommentTime: string = "";
    IsSendAlarm: string = "";
    IsSendRecover: string = "";
    UpdateBy: string = "";
    UpdateTime: string = "";
}