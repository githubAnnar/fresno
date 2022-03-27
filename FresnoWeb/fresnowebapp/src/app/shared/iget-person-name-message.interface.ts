import { IPersonName } from "./iperson.interface";

export interface IGetPersonNameMessage {
    message: string;
    data: IPersonName;
}
