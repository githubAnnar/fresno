import { IPerson } from "./iperson.interface";

export interface IGetPersonMessage {
    message: string;
    data: IPerson;
}
