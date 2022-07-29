export class HourlyData {
    Freezer: string = "";
    Type: string = "Hour"
    DateTime!: Array<String>;
    Data!: Array<Number>;
}

export class MinData {
    Freezer: string = "";
    Type: string = "Minute";
    DateTime!: Array<String>;
    Data!: Array<Number>;
}

export class Freezer {
    EqpId: string='';
    EqpName: string='';
    HI:number=0;
    HIHI: number=0;
    LO:number=0;
    LOLO:number=0;
    Offset:number=0;
    UpdateBy:string='';
}