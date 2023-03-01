export type CompletionRequest = {
  Persona: string;
  Prompt: string;
  Prefix: string;
  MaxTokens?: number;
  Temperature?: number;
  Echo?: boolean;
  FrequencyPenalty?: number;
  PresencePenalty?: number;
  EngineId?: string;
  N?: number;
}
