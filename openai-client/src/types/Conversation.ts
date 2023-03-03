export type Conversation = {
  origin: 'user' | 'system' | 'assistant';
  text: string;
  prefix?: boolean;
}