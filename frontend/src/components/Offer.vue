<template>
  <div>
    <h3>Vehicle</h3>
    <input v-model="vehicleBrand" placeholder="brand">
    <input v-model="vehicleModel" placeholder="model">
    <input v-model="vehicleColor" placeholder="color">

    <h3>Location</h3>
    <h4>From</h4>
    <input v-model="locationFromTitle" placeholder="title">
    <input v-model="locationFromDescription" placeholder="description">
    
    <h4>To</h4>
    <input v-model="locationToTitle" placeholder="title">
    <input v-model="locationToDescription" placeholder="description">

    <h3>Vehicle</h3>
    <input v-model="vehicleBrand" placeholder="brand">
    <input v-model="vehicleModel" placeholder="model">
    <input v-model="vehicleColor" placeholder="color">

    <h3>Arrival Time</h3>
    <datetime v-model="arrivalTime" type="datetime">
      <label for="startDate" slot="before">Label, click to the right</label>
    </datetime>

    <v-btn class="ma-2" color="info" @click.prevent="offerVehicle()">Send that shit to the backend</v-btn>

    <!-- <v-btn class="ma-2" color="info" @click.prevent="increment">Increment</v-btn> -->
    <!-- <v-btn class="ma-2" color="info" @click.prevent="reset">Reset</v-btn> -->
  </div>
</template>

<script lang="ts">
import { Action, Getter } from 'vuex-class';
import { Component, Vue } from 'vue-property-decorator';
import { Datetime } from 'vue-datetime';
import axios from 'axios';

const namespace: string = 'offer';

@Component
export default class Offer extends Vue {
  public vehicleBrand: string = '';
  public vehicleModel: string = '';
  public vehicleColor: string = '';

  public locationFromTitle: string = '';
  public locationFromDescription: string = '';

  public locationToTitle: string = '';
  public locationToDescription: string = '';

  public arrivalTime: string = '';

  @Getter('currentCount', { namespace })
  private currentCount!: number;
  @Action('increment', { namespace })
  private incrementCounter: any;
  @Action('reset', { namespace })
  private resetCounter: any;

  public async offerVehicle() {
    try {
      // TODO send fields of form
      const response = await axios.get<{}>('/vehicles');
      // this.vehicles = response.data;
    } catch (e) {
      // this.showError = true;
      // this.errorMessage = `Error while loading rides: ${e.message}.`;
    }
    // this.loading = false;
  }

  public mounted() {
    this.arrivalTime = Date();
  }

  private increment() {
    this.incrementCounter();
  }
  private reset() {
    this.resetCounter();
  }

}
</script>

<style scoped>
 input {
   width: 100%;
 }
</style>