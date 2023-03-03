import {ChatMessage} from "./ChatMessage";

export type ChatRequest = {
  model: string;
  messages: ChatMessage[];
}

