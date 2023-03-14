<template>
  <div class="side-panel p-2 bg-black shadow-sm shadow-amber-100 h-screen flex flex-col fixed left-0 w-80 overflow-y-auto">
    <transition-group name="slide" tag="div">
      <div class="hover:underline" v-for="(person, idx) of persona" :key="person.persona"
           @click="onPersonaClicked(person)"
           @click.right="onRightClick(person)" oncontextmenu="return false">
        <div class="side-panel__header flex flex-row space-x-2 my-2 cursor-pointer">
          <img alt="Profile Picture" v-if="persona[idx].imgUrl" :src="persona[idx].imgUrl" class="w-16 h-16 rounded-full"/>
          <h4 class="w-full h-12 overflow-y-auto resize-y">{{ person.persona }}</h4>
          <div class="w-16 flex flex-col" id="action-menu">
            <div @click="onSavePersonaClicked(idx)" id="save-persona"
                 class="w-16 h-8 bg-amber-100 hover:bg-amber-200 rounded-full flex justify-center items-center">
              <svg v-if="savingPersona === idx" class="animate-spin h-5 w-5 text-amber-500"
                   xmlns="http://www.w3.org/2000/svg"
                   fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8H4z"></path>
              </svg>
              <svg v-else xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24"
                   stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"
                      class="stroke-black"/>
              </svg>
            </div>
            <div @click="onDeletePersonaClicked(person)" id="delete-persona"
                 class="w-16 h-8 bg-red-100 hover:bg-red-200 rounded-full flex justify-center items-center">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24"
                   stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"
                      class="stroke-black"/>
              </svg>
            </div>
          </div>
        </div>
      </div>
    </transition-group>
    <div class="w-full h-12 flex flex-row justify-center items-center my-4">
      <button @click="onAddPersona" class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md">
        <template v-if="loading">
          ...
        </template>
        <template v-else>
          +
        </template>
      </button>
    </div>
    <div id="horizontal-line" class="w-full h-1 bg-amber-300 my-4"></div>
    <div class="flex flex-col flex-grow h-full justify-end">
      <div v-if="loadingPermanentPersonas"
           class="w-full h-12 flex flex-col space-y-2 justify-center items-center my-4 group" @click="loadPermanentPersonas">
        <svg class="animate-spin h-5 w-5 text-amber-500" xmlns="http://www.w3.org/2000/svg" fill="none"
             viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8H4z"></path>
        </svg>
        <h1 class="invisible group-hover:visible text-amber-300 opacity-50 drop-shadow">Load personas...</h1>
      </div>
      <div class="flex flex-col" v-if="permanentPersonas.length <= 0 && permPersonaCount > 0">
        <button @click="loadPermanentPersonas" class="bg-amber-300 text-black font-bold py-2 px-4 rounded-full hover:shadow-md">
          Load permanent personas ({{ permPersonaCount }})
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import {onMounted, PropType, Ref, ref} from "vue";
import {ApiClient} from "../clients/ApiClient";
import {PersonaResponse} from "../types/PersonaResponse";
import {PersonaStore} from "../services/PersonaStore";
import {Guid} from "../services/Guid";
import {PersonaV2} from "../types/Persona";

const personaStore = new PersonaStore();
const permPersonaCount = ref(0);

onMounted(async () => {
  permPersonaCount.value = await personaStore.GetPersonaCount();
});

const props = defineProps({
  persona: {
    type: Array as PropType<PersonaV2[]>,
    required: true
  },
  selectedPersona: {
    type: String,
    required: true
  }
})

const loading = ref(false);

const emit = defineEmits(['add', 'selected', 'edit', 'delete', 'selected-perm', 'saved', 'loaded']);

const client = new ApiClient();

// const loading = ref(false);
const onAddPersona = async () => {
  // loading.value = true;
  // const newPersona = await client.GetRandomPersona().finally(() => {
  //   loading.value = false;
  // });
  // console.log('new persona', newPersona);
  emit('add');
}

const onPermanentPersonaClicked = (persona: PersonaResponse) => {
  emit('selected-perm', persona);
}

const onPersonaClicked = (persona?: PersonaV2) => {
  if (!persona) {
    return;
  }
  console.log('selected', persona);
  emit('selected', persona);
}

const onRightClick = (persona?: PersonaV2) => {
  if (!persona) {
    return;
  }
  console.log('edit', persona);
  emit('edit', persona);
}

const savingPersona = ref(-1);
const onSavePersonaClicked = async (personaIdx: number) => {
  savingPersona.value = personaIdx
  const persona = props.persona[personaIdx].persona;
  const personaImg = props.persona[personaIdx].imgUrl;
  // const newPersona = await client.SavePersona({persona: persona, imgUrl: personaImg}).finally(() => {
  //   setTimeout(() => savingPersona.value = -1, 600);
  // });
  const newPersona = await personaStore.SetPersona({
    persona: persona,
    imgUrl: personaImg,
    id: Guid.generate(),
  });

  emit('saved', newPersona, personaIdx)
}

const loadingPermanentPersonas = ref(false);
const permanentPersonas = ref([]) as Ref<PersonaResponse[]>;
const loadPermanentPersonas = async () => {
  loadingPermanentPersonas.value = true;
  const personas = await personaStore.GetPersonas().finally(() => {
    loadingPermanentPersonas.value = false;
  });
  permanentPersonas.value = personas.map(p => {
    return {
      description: p.persona,
      id: p.id?.toString() ?? '',
      imgUrl: p.imgUrl
    }
  });
  emit('loaded', permanentPersonas.value);
}

const onDeletePersonaClicked = async (persona?: PersonaV2) => {
  if (!persona) {
    return;
  }
  console.log('deleted', persona);
  emit('delete', persona);
}

const deletePersona = ref(-1);
const onDeletePermanentPersonaClicked = async (persona: PersonaResponse, idx: number) => {
  deletePersona.value = idx;
  await client.DeletePersona(persona.id).finally(() => deletePersona.value = -1);
  permanentPersonas.value = permanentPersonas.value.filter(p => p.id !== persona.id);
}

const updatePermanentPersona = ref(-1);
const onUpdatePermanentPersonaClicked = async (persona: PersonaV2, idx: number) => {
  // updatePermanentPersona.value = idx;
  // permanentPersonas.value[idx] = await client.UpdatePersona({
  //   persona: persona.persona,
  //   imgUrl: persona.imgUrl,
  //   id: persona.id ?? ''
  // }).finally(() => {
  //   updatePermanentPersona.value = -1;
  // });
}

</script>

<style>
.side-panel {
  background-color: #fafafa;
}

.slide-leave-active, .slide-enter-active {
  transition: all 0.5s ease;
}

.slide-enter {
  transform: translate(100%, 0);
  opacity: 100;
}
.slide-enter-from,
.slide-leave-to {
  transform: translate(-100%, 0);
  opacity: 0;
}
</style>
