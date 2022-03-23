import { IUser } from "./iuser.interface";

export interface IPostNewUserMessage {
    message: string;
    data: IUser;
    id: number;
}
