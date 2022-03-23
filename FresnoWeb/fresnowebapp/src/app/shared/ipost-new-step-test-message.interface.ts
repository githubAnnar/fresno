import { IStepTest } from "./istep-test.interface";

export interface IPostNewStepTestMessage {
    message: string;
    data: IStepTest;
    id: number;
}
