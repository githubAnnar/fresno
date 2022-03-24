import { IPerson } from "./iperson.interface";

export interface IPostNewPersonMessage {
    message: string;
    data: IPerson;
    id: number;
}
