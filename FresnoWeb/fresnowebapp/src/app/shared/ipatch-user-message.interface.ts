import { IPerson } from "./iperson.interface";

export interface IPatchUserMessage {
    message: string;
    data: IPerson;
    changes: any;
}
