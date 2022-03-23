import { IStepTest } from "./istep-test.interface";

export interface IPatchStepTestMessage {
    message: string;
    data: IStepTest;
    changes: any;
}
