import { IPerson } from ".";

export interface IGetPersonsMessage {
    message: string;
    data: IPerson[];
}
