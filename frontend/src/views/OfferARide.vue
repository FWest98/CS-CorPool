<template>
  <v-container fluid>
    
    <Offer/>

    <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>


  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Offer from '@/components/Offer.vue';
import axios from 'axios';

@Component({
  components: { Offer },
})
export default class OfferVehicle extends Vue {

  loading: boolean = true;
  showError: boolean = false;
  errorMessage: string = 'Error while loading loading rides.';
  vehicles!:{};
  headers!: [
    { text: 'Date', value: 'date' },
    { text: 'From', value: 'from' },
    { text: 'To', value: 'to' },
    { text: 'Spots Available', value: 'spotsAvailable' },
  ];


  async offerVehicle() {
    try {
      const response = await axios.get<{}>('api/vehicles');
      this.vehicles = response.data;
    } catch (e) {
      this.showError = true;
      this.errorMessage = `Error while loading rides: ${e.message}.`;
    }
    this.loading = false;
  }

}
</script>