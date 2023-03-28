import axios, {AxiosInstance} from "axios";
import {AiModel} from "../types/AiModel";
import {CompletionRequest} from "../types/CompletionRequest";
import {PromptRequest} from "../types/PromptRequest";
import {PersonaRequest} from "../types/PersonaRequest";
import axiosRetry from "axios-retry";
import {PersonaRedefinitionRequest} from "../types/PersonaRedefinitionRequest";
import {Persona} from "../types/Persona";
import {PersonaResponse} from "../types/PersonaResponse";
import {ChatRequest} from "../types/ChatRequest";
import {ChatResponse} from "../types/ChatResponse";

export class ApiClient {
  private client: AxiosInstance;
  constructor() {
    this.client = axios.create({
      baseURL: 'https://localhost:7155',
    });
    axiosRetry(this.client, {retries: 8, retryDelay: axiosRetry.exponentialDelay});
  }

  public async GetAiModels(): Promise<AiModel[]> {
    const models = await this.client.get<AiModel[]>('/api/openai-models');

    if (models.status !== 200) {
      console.error(models.statusText);
      throw new Error('Failed to get models');
    }

    console.log('models', models);
    return models.data;
  }



  public async GetRandomPersona(): Promise<string> {
    const persona = await this.client.post('/api/persona/random');

    if (persona.status !== 200) {
      console.error(persona.statusText);
      throw new Error('Failed to get persona');
    }

    console.log('persona data choices', persona.data);

    const dataChoice = persona.data.choices[0].text;
    return dataChoice as string;
  }

  public async GetPrompt(req: PromptRequest): Promise<string> {
    console.log('req', req);
    const prompt = await this.client.post('/api/completions/prompt', req);

    if (prompt.status !== 200) {
      console.error(prompt.statusText);
      throw new Error('Failed to get prompt');
    }

    const dataChoice = prompt.data.choices[0].text;
    return dataChoice as string;
  }

  public async GetChat(chatRequest: ChatRequest): Promise<string> {
    const chat = await this.client.post<ChatResponse>('/api/chat', chatRequest);

    if (chat.status !== 200) {
      console.error(chat.statusText);
      throw new Error('Failed to get chat');
    }

    console.log('chat data choices', chat.data.choices);
    return chat.data.choices[0].message.content;
  }

  public async GetCompletion(completionRequest: CompletionRequest): Promise<string> {
    const completion = await this.client.post('/api/completions', completionRequest);

    if (completion.status !== 200) {
      console.error(completion.statusText);
      throw new Error('Failed to get completion');
    }

    const dataChoice = completion.data.choices[0].text;
    return dataChoice as string;
  }

  public async GetImage(prompt: string): Promise<string> {
    const img = await this.client.post('/api/images', {prompt});

    if (img.status !== 200) {
      console.error(img.statusText);
      throw new Error('Failed to get image');
    }

    return img.data;
  }

  public async GetNewPersona(request: PersonaRequest): Promise<string> {
    const persona = await this.client.post('/api/persona', request);

    if (persona.status !== 200) {
      console.error(persona.statusText);
      throw new Error('Failed to get persona');
    }

    const dataChoice = persona.data.choices[0].text;
    return dataChoice as string;
  }

  public async GetRedefinedPersona(request: PersonaRedefinitionRequest): Promise<string> {
    const redefined = await this.client.post('/api/persona/redefine', request);

    if (redefined.status !== 200) {
      console.error(redefined.statusText);
      throw new Error('Failed to get redefined persona');
    }

    const dataChoice = redefined.data.choices[0].text;
    return dataChoice as string;
  }

  public async GetPersonas(): Promise<PersonaResponse[]> {
    const resp = await this.client.get<PersonaResponse[]>('/api/persona');
    if (resp.status !== 200) {
      console.error(resp.statusText);
      throw new Error('Failed to get personas');
    }

    console.log('personas', resp.data);
    return resp.data;
  }

  public async SavePersona(persona: Persona): Promise<PersonaResponse> {
    const resp = await this.client.post<PersonaResponse>('/api/persona/save-persona', {
      persona: persona.persona,
      imgUrl: persona.imgUrl,
    });
    if (resp.status !== 201) {
      console.error(resp.statusText);
      throw new Error('Failed to save persona');
    }

    return resp.data;
  }

  public async UpdatePersona(persona: Persona): Promise<PersonaResponse> {
    if (!persona.id) throw new Error('Persona id is required');
    const resp = await this.client.put<PersonaResponse>(`/api/persona/${persona.id}`, {
      id: persona.id,
      description: persona.persona,
      imgUrl: persona.imgUrl,
    });

    if (resp.status !== 200) {
      console.error(resp.statusText);
      throw new Error('Failed to update persona');
    }
    return resp.data;
  }

  public async DeletePersona(personaId: string): Promise<void> {
    if (!personaId) throw new Error('Persona id is required');
    const resp = await this.client.delete(`/api/persona/${personaId}`);

    if (resp.status !== 200) {
      console.error(resp.statusText);
      throw new Error('Failed to delete persona');
    }
  }

  public async GetModels(): Promise<string[]> {
    const models = await this.client.get<{id: string}[]>('/api/models');

    if (models.status !== 200) {
      console.error(models.statusText);
      throw new Error('Failed to get models');
    }

    return models.data.map(m => m.id);
  }
}