import {ChatMessage} from "./ChatMessage";

export type ChatChoice = {
  index: number;
  message: ChatMessage
  finish_reason: string;
}