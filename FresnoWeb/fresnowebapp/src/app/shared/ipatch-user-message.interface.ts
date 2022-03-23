import { IUser } from "./iuser.interface";

export interface IPatchUserMessage {
    message: string;
    data: IUser;
    changes: any;
}
