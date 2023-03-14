<script setup lang="ts">

import AppV1 from "./components/AppV1.vue";
import AppV2 from "./components/AppV2.vue";

const v = 1;

</script>

<template>
  <Suspense>
    <template #default>
      <template v-if="v === 1">
        <AppV1/>
      </template>
      <template v-else>
        <AppV2 class="w-full h-full"/>
      </template>
    </template>
    <template #fallback>
      Loading...
    </template>
  </Suspense>
</template>

<style lang="scss" scoped>

</style>
