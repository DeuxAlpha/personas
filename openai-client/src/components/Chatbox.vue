<template>
  <span ref="chatbox" class="chatbox overflow-x-auto"></span>
  <span class="absolute text-xs bottom-0" :class="convOriginClass(convo.origin)">{{ convo.origin }}</span>
  <input type="checkbox" class="absolute top-0" :class="convOriginClass(convo.origin)" :value="convo.prefix"
         @change="onPrefixUpdate">
</template>

<script setup lang="ts">
import gsap from "gsap";
import {onMounted, Ref, ref} from "vue";
import hljs from "highlight.js/lib/core";

const chatbox = ref(null) as Ref<HTMLElement | null>;

const props = defineProps<{
  convo: { text: string, origin: 'assistant' | 'user' | 'system', prefix: boolean };
}>();

onMounted(() => {
  // Select all the code elements and apply highlight.js to them
  console.log('highlighting');

  const tl = gsap.timeline();
  tl.to(chatbox.value, {text: props.convo.text, opacity: 1, duration: 1.5, stagger: 0.33, onComplete: () => {
      console.log('highlighting');
      hljs.highlightAll();
    }});
});

const emits = defineEmits(['prefix-update']);

const onPrefixUpdate = (e: Event) => {
  emits('prefix-update', e);
}

const convClass = (origin: 'assistant' | 'user' | 'system') => {
  return origin === 'assistant' ? {'bg-gray-600': true} : {'bg-blue-600': true, 'text-right': true};
}
const convOriginClass = (origin: 'assistant' | 'user' | 'system') => {
  return origin === 'assistant' ? {'left-0': true} : {'right-0': true};
}

</script>

<style scoped lang="scss">
.chatbox {
  padding: 10px;
  color: white;
  font-family: monospace;
}
</style>