import { IMeasurement } from "./imeasurement.interface";

export interface IGetMeasurementMessage {
    message: string;
    data: IMeasurement;
}
