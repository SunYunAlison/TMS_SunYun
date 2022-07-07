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