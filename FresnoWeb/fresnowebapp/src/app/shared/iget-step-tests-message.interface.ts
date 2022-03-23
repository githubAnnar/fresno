import { IStepTest } from "./istep-test.interface";

export interface IGetStepTestsMessage {
    message: string;
    data: IStepTest[];
}
