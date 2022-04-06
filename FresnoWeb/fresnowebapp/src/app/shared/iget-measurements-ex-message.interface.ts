import { IMeasurementEx } from "./imeasurement-ex.interface";

export interface IGetMeasurementsExMessage {
    message: string;
    data: IMeasurementEx[];
}
