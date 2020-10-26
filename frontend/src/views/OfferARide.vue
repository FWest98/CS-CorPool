<template>
  <v-container fluid>
    
    <h3>Vehicle</h3>
    <input v-model="offer.vehicle.brand" placeholder="brand">
    <input v-model="offer.vehicle.model" placeholder="model">
    <input v-model="offer.vehicle.color" placeholder="color">
    <input v-model.number="offer.vehicle.capacity" type='number' placeholder="capacity">

    <h3>Location</h3>
    <h4>From</h4>
    <input v-model="offer.from.title" placeholder="title">
    <input v-model="offer.from.description" placeholder="description">
    
    <h4>To</h4>
    <input v-model="offer.to.title" placeholder="title">
    <input v-model="offer.to.description" placeholder="description">

    <h3>Arrival Time</h3>
    <datetime v-model="offer.arrivalTime" type="datetime">

    </datetime>

    <v-btn class="ma-2" color="info" @click.prevent="offerVehicle()">Offer Vehicle</v-btn>

    <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>

    <v-alert :value="showSuccess" type="success" v-text="successMessage">
      This is a success alert.
    </v-alert>


  </v-container>
</template>

<script>
import { Datetime } from 'vue-datetime';
import getConfig from '../auth';

export default {
  data() {
    return {
      showError: false,
      showSuccess: false,
      response: {},
      errorMessage: 'Error while loading loading rides.',
      successMessage: '',
      headers: [
        { text: 'Date', value: 'date' },
        { text: 'From', value: 'from' },
        { text: 'To', value: 'to' },
        { text: 'Spots Available', value: 'spotsAvailable' },
      ],
      offer: {
        vehicle: {
          brand: '',
          model: '',
          color: '',
          capacity: 0,
        },
        from: {
          title: '',
          description: '',
        },
        to: {
          title: '',
          description: '',
        },
        arrivalTime: new Date().toISOString(),
      },
    };
  },
  methods: {
    async offerVehicle() {
      try {
        const config = getConfig();
        const response = await axios.post('/offer', this.offer, config);
        this.response = response.data;
        this.showSuccess = true;
        this.successMessage =  `Your offer with ID "${this.response.id}" has been added`;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while adding offer: ${e.message}.`;
      }
    },
  },
};
</script>

<style scoped>
 input {
   width: 100%;
 }
</style>