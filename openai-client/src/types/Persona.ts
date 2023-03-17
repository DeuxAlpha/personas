import {Conversation} from "./Conversation";

export type Persona = {
  persona: string;
  imgUrl?: string;
  id?: number;
}

export type PersonaV2 = {
  persona: string;
  imgUrl?: string;
  id?: string;
  conversations?: Conversation[];
}