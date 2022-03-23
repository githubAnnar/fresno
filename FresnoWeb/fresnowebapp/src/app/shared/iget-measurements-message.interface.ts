import { IMeasurement } from "./imeasurement.interface";

export interface IGetMeasurementsMessage {
    message: string;
    data: IMeasurement[];
}
