export type Conversation = {
  origin: 'user' | 'bot';
  text: string;
  prefix?: boolean;
}