import { IUser } from ".";

export interface IGetUsersMessage {
    message: string;
    data: IUser[];
}
