export type AiModel = {
  id: string;
  owned_by: string;
  createdTime: Date
  object: string,
  successful: boolean,
  root: string,
  error: OpenAiError,
}

type OpenAiError = {
  code: string,
  message: string,
  param: string,
  type: string
}