<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Counter</h1>
          <p>This is a simple example of a Vue.js component integrated with Vuex</p>
          <Counter/>
        </v-col> 
      </v-row>
    </v-slide-y-transition>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Counter from '@/components/Counter.vue';
import axios from 'axios';

@Component({
  components: { Counter },
})
export default class OfferVehicle extends Vue {

  // TODO why do we need all these stuff complicating things?

  // data: [
  //   loading: true,
  //   showError: false,
  //   errorMessage: 'Error while loading loading rides.',
  // ]

  // methods: {
  //   async offerVehicle(){
  //     try {
  //       const response = await axios.get<{}>('api/rides');
  //       this.response = response.data;
  //     } catch (e) {
  //       this.showError = true;
  //       this.errorMessage = `Error while loading rides: ${e.message}.`;
  //     }
  //     this.loading = false;
  //   }
  // }
}
</script>