import {ChatChoice} from "./ChatChoice";
import {ChatUsage} from "./ChatUsage";

export type ChatResponse = {
  id: string;
  object: string;
  created: number;
  choices: ChatChoice[];
  usage: ChatUsage;
}