<template>
  <div class="flex flex-row w-full min-h-screen bg-gray-900 text-white">
    <VueModal v-model="openEditModal" modal-class="bg-gray-900 rounded shadow shadow-gray-700">
      <div class="flex flex-col">
        <textarea class="bg-gray-800 text-white" rows="5" cols="33" v-model="selectedPersona"/>
        <div class="flex flex-row justify-between">
          <button @click="onEditPersonaSubmit"
                  class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-start mt-2">
            Submit
            <template v-if="editSubmitLoading">...</template>
          </button>
          <button @click="onRedefinePersona"
                  class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md mt-2">
            Expand
            <template v-if="redefineSubmitLoading">...</template>
          </button>
        </div>
      </div>
    </VueModal>
    <VueModal v-model="createNewPersona" modal-class="bg-gray-900 rounded shadow shadow-gray-700 w-11/12">
      <div class="flex flex-col space-y-1">
        <div class="flex flex-row space-x-0.5">
          <span class="text-white">a</span> <select v-model="ai[0]">
          <option value="generous">generous</option>
          <option value="greedy">greedy</option>
          <option value="honest">honest</option>
          <option value="dishonest">dishonest</option>
          <option value="kind">kind</option>
          <option value="mean">mean</option>
          <option value="loyal">loyal</option>
          <option value="crazy">crazy</option>
          <option value="insane">insane</option>
          <option value="disloyal">disloyal</option>
          <option value="brave">brave</option>
          <option value="cowardly">cowardly</option>
          <option value="proud">proud</option>
          <option value="humble">humble</option>
          <option value="patient">patient</option>
          <option value="impatient">impatient</option>
        </select> <span class="text-white">character, with a</span> <select v-model="ai[1]">
          <option value="dangerous">dangerous</option>
          <option value="peaceful">peaceful</option>
          <option value="powerful">powerful</option>
          <option value="weak">weak</option>
          <option value="rich">rich</option>
          <option value="poor">poor</option>
          <option value="smart">smart</option>
          <option value="stupid">stupid</option>
          <option value="beautiful">beautiful</option>
          <option value="ugly">ugly</option>
          <option value="strong">strong</option>
          <option value="weak">weak</option>
        </select>
          <span class="text-white">background in</span> <select v-model="ai[2]">
          <option value="the military">the military</option>
          <option value="the arts">the arts</option>
          <option value="the sciences">the sciences</option>
          <option value="the law">the law</option>
          <option value="the clergy">the clergy</option>
          <option value="the government">the government</option>
          <option value="the business world">the business world</option>
          <option value="the underworld">the underworld</option>
          <option value="the medical field">the medical field</option>
          <option value="the culinary arts">the culinary arts</option>
          <option value="the entertainment industry">the entertainment industry</option>
          <option value="the sports world">the sports world</option>
        </select>
        </div>
        <button class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-start"
                @click="submitGenerativeAi">Submit
        </button>
      </div>
      <div class="flex flex-row space-x-2">
        <div class="flex flex-col w-1/2 relative">
          <label class="text-gray-100 absolute top-0 left-0 text-xs">Description</label>
          <textarea class="pt-3 bg-gray-800 text-white" rows="5" cols="28" v-model="desc"/>
        </div>
        <div class="flex flex-col w-1/2 relative">
          <label class="text-gray-100 absolute top-0 left-0 text-xs">Pretext</label>
          <textarea class="pt-3 bg-gray-800 text-white" rows="5" cols="28" v-model="pretext"/>
        </div>
      </div>
      <button @click="onNewPersonaSubmit"
              class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-start mt-2">
        Submit
        <template v-if="newPersonaSubmitLoading">...</template>
      </button>
      <div class="flex flex-col w-full">
        <button @click="onRandomPersonaSubmit"
                class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-start mt-2">
          Random
          <template v-if="randomPersonaLoading">...</template>
        </button>
      </div>
    </VueModal>
    <div class="w-1/3 resize-x">
      <SidePanel :persona="persona" @add="onPersonaAdded" @selected="onPersonaSelected"
                 @selected-perm="permPersona = $event"
                 :selected-persona="selectedPersona" @edit="onPersonaEdit"
                 @delete="onPersonaDeleted" @saved="onPersonaSaved" @loaded="onPermanentPersonasLoaded"/>
    </div>
    <div class="flex flex-col w-2/3">
      <div class="flex flex-col justify-center items-center">
        <h1 class="text-3xl font-bold underline text-amber-300">
          Selected Persona:
        </h1>
        <h3 class="text-amber-400">
          <!--          <template v-if="permPersona">-->
          <!--            {{ permPersona }}-->
          <!--          </template>-->
          <!--          <template v-else>-->
          {{ selectedPersona }}
          <!--          </template>-->
        </h3>
      </div>
      <div class="flex flex-col">
        <div class="w-full flex flex-row">
          <textarea rows="4" cols="42" class="bg-gray-600" v-model="prompt"></textarea>
          <div class="flex flex-col justify-end">
            <button @click="onOpenAiModelsClicked"
                    class="bg-amber-300 text-black py-2 px-4 rounded-sm hover:shadow-md self-start mt-2 text-sm">
              Load OpenAI Models
              <template v-if="openAiModelsLoading">...</template>
            </button>
            <label for="mode" class="text-gray-100 text-sm">Mode</label>
            <select class="bg-gray-700 self-baseline" v-model="mode">
              <option value="chat">Chat</option>
              <option value="completion">Completion</option>
            </select>
            <div class="flex flex-row" v-if="mode === 'chat'">
              <label for="filter" class="text-gray-100 text-sm">Filter chat?</label>
              <input type="checkbox" v-model="filterChat"/>
            </div>
            <select class="bg-gray-700 self-baseline" v-model="model">
              <option value="text-davinci-003">text-davinci-003</option>
              <option value="text-curie-001">text-curie-001</option>
              <option value="text-babbage-001">text-babbage-001</option>
              <option value="text-ada-001">text-ada-001</option>
              <option value="code-davinci-002">code-davinci-002</option>
              <option value="gpt-3.5-turbo">gpt-3.5-turbo</option>
              <option disabled id="separator">
                --------------------
              </option>
              <option :value="openAiModel.id" v-for="openAiModel in openAiModels" :key="openAiModel.id">
                {{ openAiModel.id }}
              </option>
            </select>
            <button v-show="convo.filter(c => c.prefix).length > 0" @click="clearPrefix"
                    class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-baseline">
              Clear Prefix
            </button>
          </div>
          <div class="flex flex-col justify-end">
            <button @click="onGenerateClicked"
                    class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-start mt-2">
              Generate prompt
              <template v-if="generateLoading">...</template>
            </button>
            <button v-if="mode === 'completion' || model !== 'gpt-3.5-turbo'" @click="onSwitchChatClicked"
                    class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-start mt-2">
              Switch to chat
            </button>
            <button @click="onClearClicked"
                    class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md self-start mt-2">
              Clear
            </button>
          </div>
          <div class="flex flex-col h-full space-y-2">
            <div class="flex justify-center flex-1 relative">
              <label class="absolute top-0 left-0 text-gray-100 text-xs">Temperature</label>
              <input type="range" min="0" max="1" step="0.1" v-model="temperature"/>
            </div>
            <div class="flex justify-center flex-1 relative">
              <label class="absolute top-0 left-0 text-gray-100 text-xs">Presence Penalty</label>
              <input type="range" min="-2" max="2" step="0.1" v-model="presencePenalty"/>
            </div>
            <div class="flex justify-center flex-1 relative">
              <label class="absolute top-0 left-0 text-gray-100 text-xs">Frequency Penalty</label>
              <input type="range" min="-2" max="2" step="0.1" v-model="frequencyPenalty"/>
            </div>
          </div>
          <div class="mx-auto"/>
          <img @click="onGenerateImgClicked" :alt="imgLoading ? '...' : 'Profile Picture'"
               :src="permPersona ? permPersona.imgUrl : persona[selectedPersonaIndex].imgUrl"
               class="w-32 h-32 cursor-pointer rounded p-1"/>
        </div>
        <button class="bg-amber-300 text-black text-sm py-2" @click="getPrompt">
          <template v-if="promptLoading">
            ...
          </template>
          <template v-else>
            ?
          </template>
        </button>
        <button class="bg-green-300 text-black text-sm py-2" @click="submit">
          <template v-if="submitLoading">
            ...
          </template>
          <template v-else>
            !
          </template>
        </button>
      </div>
      <div class="flex flex-col">
        <div class="w-full rounded py-2 px-1 relative" :class="convClass(conv.origin)" v-for="conv in convo"
             :key="conv.text">
          <Chatbox :convo="conv" @prefix-update="onPrefixUpdate(conv)"/>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup lang="ts">

import SidePanel from "../components/PersonaSidePanel.vue";
import {Ref, ref} from "vue";
import {ApiClient} from "../clients/ApiClient";
import {Conversation} from "../types/Conversation";
import VueModal from "../components/Modal.vue";
import {AiModel} from "../types/AiModel";
import {PersonaResponse} from "../types/PersonaResponse";
import {ChatMessage} from "../types/ChatMessage";
import {marked} from 'marked';
import hljs from "highlight.js/lib/core";
import gsap from "gsap";
import Chatbox from "./Chatbox.vue";
import {PersonaStore} from "../services/PersonaStore";
import {PersonaV2} from "../types/Persona";

const ai = ref(['', '', '']);
const generatedAi = () => {
  return `a ${ai.value[0]} character, with a ${ai.value[1]} background in ${ai.value[2]}`
}

const persona: Ref<PersonaV2[]> = ref([{
  persona: 'John, He\'s a nice guy',
}, {
  persona: 'Jane, she\'s a nice girl',
}]);
const model = ref('text-davinci-003');

const selectedPersona = ref(persona.value[0].persona)
const selectedPersonaIndex = ref(0);

const prompt = ref('Hello World!')

const apiClient = new ApiClient();

const submitLoading = ref(false);

const convo = ref([]) as Ref<Conversation[]>;

const submit = async () => {
  if (permPersona.value !== undefined) {
    console.log('permPersona', permPersona.value)
    submitLoading.value = true;
    if (mode.value === "completion") {
      console.log('completion', permPersona.value?.description)
      const response = await apiClient.GetCompletion({
        Persona: permPersona.value?.description ?? '',
        Prompt: prompt.value,
        Prefix: getPrefix(),
        EngineId: model.value,
        Temperature: temperature.value,
        FrequencyPenalty: frequencyPenalty.value,
        PresencePenalty: presencePenalty.value,
      }).finally(() => submitLoading.value = false);
      convo.value.unshift({
        text: marked(prompt.value, {
          highlight(code: string, lang: string): string | void {
            if (lang && hljs.getLanguage(lang)) {
              return hljs.highlight(code, {language: lang}).value;
            }
            return hljs.highlightAuto(code).value;
          }
        }), origin: 'user'
      });
      convo.value.unshift({
        text: marked(response, {
          highlight(code: string, lang: string, callback?: (error: any, code?: string) => void): string | void {
            if (lang && hljs.getLanguage(lang)) {
              return hljs.highlight(code, {language: lang}).value;
            }
            return hljs.highlightAuto(code).value;
          }
        }), origin: 'assistant'
      });
    } else if (mode.value === 'chat') {
      console.log('chat', permPersona.value?.description)
      const chat: ChatMessage[] = [{
        role: 'system',
        content: `Context: You are playing a persona. Act accordingly: ${permPersona.value?.description || 'nothing'}`
      }, {
        role: 'user',
        content: prompt.value
      }];
      for (let c of convo.value) {
        if (filterChat.value && c.prefix === false) {
          continue;
        }
        chat.unshift({role: c.origin, content: c.text});
      }
      const response = await apiClient.GetChat({
        model: model.value,
        messages: chat
      }).finally(() => submitLoading.value = false);
      console.log('response', response)
      convo.value.unshift({
        text: marked(prompt.value, {
          highlight(code: string, lang: string): string | void {
            if (lang && hljs.getLanguage(lang)) {
              return hljs.highlight(code, {language: lang}).value;
            }
            return hljs.highlightAuto(code).value;
          }
        }), origin: 'user'
      });
      convo.value.unshift({
        text: marked(response, {
          highlight(code: string, lang: string): string | void {
            if (lang && hljs.getLanguage(lang)) {
              return hljs.highlight(code, {language: lang}).value;
            }
            return hljs.highlightAuto(code).value;
          }
        }), origin: 'assistant'
      });
    }
  } else {
    console.log('selectedPersona', selectedPersona.value)
    submitLoading.value = true;
    if (mode.value === "completion") {
      console.log('completion', selectedPersona.value)
      const newText = await apiClient.GetCompletion({
        Persona: selectedPersona.value,
        Prompt: prompt.value,
        Prefix: getPrefix(),
        EngineId: model.value,
        Temperature: temperature.value,
        FrequencyPenalty: frequencyPenalty.value,
        PresencePenalty: presencePenalty.value,
      }).finally(() => submitLoading.value = false);
      convo.value.unshift({text: marked(prompt.value), origin: 'user'});
      convo.value.unshift({text: marked(newText), origin: 'assistant'});
    } else if (mode.value === 'chat') {
      console.log('chat', selectedPersona.value)
      const chat: ChatMessage[] = [{
        role: 'system',
        content: `Context: You are playing a persona. Act maccordingly: ${selectedPersona.value}`
      }, {
        role: 'user',
        content: prompt.value
      }];
      for (let c of convo.value) {
        console.log('conv', c);
        if (filterChat.value === true && c.prefix !== true) {
          continue;
        }
        chat.unshift({role: c.origin, content: c.text});
      }
      console.log('request', chat);
      const response = await apiClient.GetChat({
        model: model.value,
        messages: chat
      }).finally(() => submitLoading.value = false);
      console.log('response', response)
      convo.value.unshift({text: marked(prompt.value), origin: 'user'});
      convo.value.unshift({text: marked(response), origin: 'assistant'});
    }
  }
}

function getPrefix() {
  return convo.value.filter(c => c.prefix).map(c => c.text).join(' ');
}

const onPersonaAdded = () => {
  // persona.value.push(newP);
  createNewPersona.value = true;
}

const onPersonaSelected = (selected: PersonaV2) => {
  permPersona.value = undefined;
  selectedPersona.value = selected.persona;
  selectedPersonaIndex.value = persona.value.indexOf(selected);
}

const promptLoading = ref(false);
const getPrompt = async () => {
  promptLoading.value = true;
  if (permPersona.value !== undefined) {
    prompt.value = await apiClient.GetPrompt({
      persona: permPersona.value?.description || '',
      prefix: getPrefix(),
      model: model.value
    }).finally(() => promptLoading.value = false);
  } else {
    const newPrompt = await apiClient.GetPrompt({
      persona: selectedPersona.value,
      prefix: getPrefix(),
      model: model.value
    })
        .finally(() => promptLoading.value = false);
    console.log('prompt', newPrompt);
    prompt.value = newPrompt;
  }
}

const convClass = (origin: 'assistant' | 'user' | 'system') => {
  return origin === 'assistant' ? {'bg-gray-600': true} : {'bg-blue-600': true, 'text-right': true};
}
const convOriginClass = (origin: 'assistant' | 'user' | 'system') => {
  return origin === 'assistant' ? {'left-0': true} : {'right-0': true};
}

const openEditModal = ref(false);

const onPersonaEdit = (event: PersonaV2) => {
  selectedPersona.value = event.persona;
  selectedPersonaIndex.value = persona.value.indexOf(event);
  openEditModal.value = true;
}

const editSubmitLoading = ref(false);
const onEditPersonaSubmit = () => {
  editSubmitLoading.value = true;
  openEditModal.value = false;
  persona.value[selectedPersonaIndex.value].persona = selectedPersona.value;
  editSubmitLoading.value = false;
}

const imgLoading = ref(false);

const onGenerateImgClicked = async () => {
  imgLoading.value = true;
  const img = await apiClient.GetImage(selectedPersona.value)
      .catch(err => {
        console.error(err);
        return '';
      })
      .finally(() => imgLoading.value = false);
  console.log('img', img);
  persona.value[selectedPersonaIndex.value].imgUrl = img;
}

const clearPrefix = () => {
  for (let conv of convo.value) {
    conv.prefix = false;
  }
}

const createNewPersona = ref(false);

const randomPersonaLoading = ref(false);
const onRandomPersonaSubmit = async () => {
  randomPersonaLoading.value = true;
  const newPersona = await apiClient.GetRandomPersona().finally(() => randomPersonaLoading.value = false);
  persona.value.push({
    persona: newPersona
  });
  createNewPersona.value = false;
}
const desc = ref('');
const pretext = ref('');

const newPersonaSubmitLoading = ref(false);

const submitGenerativeAi = async () => {
  newPersonaSubmitLoading.value = true;
  const newPersona = await apiClient.GetNewPersona({
    description: `${generatedAi()} ${desc.value}`,
    pretext: pretext.value
  }).finally(() => {
    newPersonaSubmitLoading.value = false;
  });
  persona.value.push({
    persona: newPersona
  });
  createNewPersona.value = false;
}

const onNewPersonaSubmit = async () => {
  newPersonaSubmitLoading.value = true;
  const newPersona = await apiClient.GetNewPersona({
    description: desc.value,
    pretext: pretext.value
  }).finally(() => {
    newPersonaSubmitLoading.value = false;
  });
  persona.value.push({
    persona: newPersona
  });
  createNewPersona.value = false;
}

const redefineSubmitLoading = ref(false);
const onRedefinePersona = async () => {
  redefineSubmitLoading.value = true;
  const updatedPersona = await apiClient.GetRedefinedPersona({
    base: selectedPersona.value
  }).finally(() => redefineSubmitLoading.value = false);
  persona.value[selectedPersonaIndex.value].persona += updatedPersona;
  selectedPersona.value += updatedPersona;
  openEditModal.value = false
}

const personaStore = new PersonaStore();

const onPersonaDeleted = async (toDelete: PersonaV2) => {
  const found = persona.value.find(p => p === toDelete);
  if (!found) return;
  persona.value.splice(persona.value.indexOf(found), 1);
  const permDelete = await personaStore.GetPersonaBySelf(toDelete.persona);
  if (permDelete) {
    await personaStore.DeletePersona(permDelete.id || '');
  }
}

const temperature = ref(0);
const frequencyPenalty = ref(0);
const presencePenalty = ref(0);

const openAiModels: Ref<AiModel[]> = ref([]);
const openAiModelsLoading = ref(false);
const onOpenAiModelsClicked = async () => {
  openAiModelsLoading.value = true;
  openAiModels.value = await apiClient.GetAiModels().finally(() => openAiModelsLoading.value = false);
}

const permPersona: Ref<PersonaResponse | undefined> = ref(undefined);

const mode = ref('completion') as Ref<'chat' | 'completion'>
const filterChat = ref(false);

const generateLoading = ref(false);

const onGenerateClicked = async () => {
  generateLoading.value = true;
  if (permPersona.value !== undefined) {
    prompt.value = await apiClient.GetPrompt({
      persona: permPersona.value?.description || '',
      prefix: getPrefix(),
      model: 'text-davinci-003'
    }).finally(() => generateLoading.value = false);
  } else {
    const newPrompt = await apiClient.GetPrompt({
      persona: selectedPersona.value,
      prefix: getPrefix(),
      model: 'text-davinci-003'
    }).finally(() => generateLoading.value = false);
    console.log('prompt', newPrompt);
    prompt.value = newPrompt;
  }
}

const onClearClicked = () => {
  convo.value = [];
}

const chatLoading = ref(false);

const onChatClicked = async () => {
  chatLoading.value = true;

}

const onSwitchChatClicked = () => {
  mode.value = 'chat';
  model.value = 'gpt-3.5-turbo';
}

const onConvLoaded = (conv: Conversation) => {
  console.log('Hello', conv.origin);
}

const onPersonaSaved = (newPersona: PersonaResponse, personaIdx: number) => {
  persona.value[personaIdx].persona = newPersona.description;
}

const onPermanentPersonasLoaded = (personas?: PersonaResponse[]) => {
  if (!personas) return;
  for (let permPersona of personas) {
    persona.value.push({
      persona: permPersona.description,
      imgUrl: permPersona.imgUrl,
      id: permPersona.id
    });
  }
}

const onPrefixUpdate = (conv: Conversation) => {
  if (conv.prefix === true) {
    conv.prefix = false;
  } else if (conv.prefix === false) {
    conv.prefix = true;
  } else {
    conv.prefix = true;
  }
}

</script>

<style scoped lang="scss">
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
}

.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}

.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>