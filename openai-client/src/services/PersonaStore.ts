import localForage from "localforage";
import {Persona, PersonaV2} from "../types/Persona";
import {Guid} from "./Guid";
import {ChatMessage} from "../types/ChatMessage";

export class PersonaStore {
  private readonly personaStore: LocalForage;
  private readonly chatStore: LocalForage;

  public constructor() {
    this.personaStore = localForage.createInstance({
      name: "PersonaStore",
      storeName: "PersonaStore",
      description: "PersonaStore",
      driver: localForage.LOCALSTORAGE,
      version: 1.0,
    });
    this.chatStore = localForage.createInstance({
      name: "ChatStore",
      storeName: "ChatStore",
      description: "ChatStore",
      driver: localForage.LOCALSTORAGE,
      version: 1.0,
    })
  }

  public async GetPersona(id: string): Promise<PersonaV2> {
    const result = await this.personaStore.getItem(id);
    if (result) {
      return result as PersonaV2;
    }
    throw new Error('Persona not found')
  }

  public async GetPersonas(): Promise<PersonaV2[]> {
    const result = await this.personaStore.keys();
    if (result) {
      const personas = [];
      for (const key of result) {
        const persona = await this.GetPersona(key);
        personas.push(persona);
      }
      return personas;
    }
    return [];
  }

  public async SetPersona(persona: PersonaV2): Promise<void> {
    if (!persona.id) {
      persona.id = Guid.generate();
    }
    await this.personaStore.setItem(persona.id, persona);
  }

  public async GetConversation(personaId: string): Promise<ChatMessage[]> {
    const result = await this.chatStore.getItem(personaId);
    if (result) {
      return result as ChatMessage[];
    }
    return [];
  }

  public async SetConversation(personaId: string, messages: ChatMessage[]): Promise<void> {
    await this.chatStore.setItem(personaId, messages);
  }
}