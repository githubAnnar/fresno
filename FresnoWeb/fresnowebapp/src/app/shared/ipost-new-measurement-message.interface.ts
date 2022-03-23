import { IMeasurement } from "./imeasurement.interface";

export interface IPostNewMeasurementMessage {
    message: string;
    data: IMeasurement;
    id: number;
}
