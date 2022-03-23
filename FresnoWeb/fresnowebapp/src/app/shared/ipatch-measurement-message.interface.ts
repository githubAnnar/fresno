import { IMeasurement } from "./imeasurement.interface";

export interface IPatchMeasurementMessage {
    messsage: string;
    data: IMeasurement;
    changes: any;
}
